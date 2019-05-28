using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using System.IO;
using Photon.Pun;
using ExitGames.Client.Photon;

public class MovePieceUI : MonoBehaviourPun
{
    private Piece selectedPiece;
    private GameObject selectedGO;
    private bool promotingPawn = false;

    private GameObject[] prevSquares = new GameObject[0];
    private Material[] prevMaterials = new Material[0];
    private Material prevMaterial;

    private GameObject prevSquare1;
    private GameObject prevSquare2;

    [SerializeField]
    private GameObject pawnPromotion;
    [SerializeField]
    private GameObject gameEnd;

    // Use this for initialization
    void Start()
    {
        name = "White Board";
        if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            name = "Black Board";
            transform.eulerAngles = new Vector3(0, 0, 180);
        }
        else if(PhotonNetwork.IsConnected == false)
        {
            PhotonNetwork.OfflineMode = true;
            PhotonNetwork.CreateRoom(null);
        }
        loadPieces();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void squareSelected(GameObject squareGo)
    {
        if(pawnPromotion.transform.GetChild(0).gameObject.activeSelf == false && pawnPromotion.transform.GetChild(1).gameObject.activeSelf == false)
        {
            int file = int.Parse(squareGo.name.Substring(0, 1));
            int rank = int.Parse(squareGo.name.Substring(2, 1));
            Image image = squareGo.GetComponent<Image>();
            for (int i = 0; i < prevSquares.Length; i++)
            {
                Destroy(prevSquares[i]);
            }
            if (prevMaterial != null && selectedGO != null)
            {
                selectedGO.transform.parent.GetComponent<Image>().material = prevMaterial;
            }
            if (selectedPiece != null && selectedPiece.generatePossibleMoves(Game.board).Contains(new Vector2Int(file, rank)))
            {
                int x = Game.board.getPosition(selectedPiece).x;
                int y = Game.board.getPosition(selectedPiece).y;
                if (selectedPiece.GetType().Equals(typeof(Pawn)) && rank == 7 || selectedPiece.GetType().Equals(typeof(Pawn)) && rank == 0)
                {
                    if (rank == 7)
                    {
                        pawnPromotion.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        pawnPromotion.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    GameObject square = GameObject.Find(file.ToString() + "," + rank.ToString());
                    if (square.transform.childCount == 1)
                    {
                        Destroy(square.transform.GetChild(0).gameObject);
                    }
                    selectedGO.transform.SetParent(square.transform);
                    selectedGO.transform.position = square.transform.position;
                }
                else
                {
                    //StopCoroutine(AI.prepareNextMove());
                    photonView.RPC("move", RpcTarget.All, x, y, file, rank);
                    Game.board.history.Add(Game.board.getFen());

                    //using (StreamWriter sw = new StreamWriter("Assets/GameStatus.txt", false))
                    //{
                    //    sw.WriteLine(FEN.generate(Game.board));
                    //}

                    selectedPiece = null;
                    selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    selectedGO = null;
                    if (PhotonNetwork.OfflineMode == true && gameEnd.activeSelf == false)
                    {
                        StartCoroutine(AI.makeMove(Game.board, this));
                        //StartCoroutine(AI.prepareNextMove());
                    }
                }
            }
            else if (selectedPiece != null && !selectedPiece.generatePossibleMoves(Game.board).Contains(new Vector2Int(file, rank)))
            {
                selectedGO.transform.localPosition = Vector3.zero;
                selectedPiece = null;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                selectedGO = null;
            }
            if (Game.board.getPiece(file, rank) != null && ((Game.board.getPiece(file, rank).colour == Colour.White && Game.board.whitesTurn && name == "White Board") || (Game.board.getPiece(file, rank).colour == Colour.Black && !Game.board.whitesTurn && name == "Black Board")))
            {
                List<Vector2Int> possibleMoves = Game.board.getPiece(file, rank).generatePossibleMoves(Game.board);
                prevMaterials = new Material[possibleMoves.Count + 1];
                prevSquares = new GameObject[possibleMoves.Count + 1];
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    Vector2Int sq = possibleMoves[i];
                    GameObject dot = new GameObject();
                    dot.name = "dot";
                    dot.transform.SetParent(GameObject.Find("Canvas").transform);
                    dot.transform.position = GameObject.Find(sq.x.ToString() + "," + sq.y.ToString()).transform.position;
                    dot.AddComponent<Image>();
                    Sprite sprite = Resources.Load<Sprite>("Materials/Dot");
                    dot.GetComponent<Image>().sprite = sprite;
                    dot.GetComponent<Image>().color = new Color32(0, 0, 0, 75);
                    dot.GetComponent<Image>().raycastTarget = false;
                    dot.GetComponent<RectTransform>().sizeDelta = new Vector2(70, 70);
                    dot.GetComponent<RectTransform>().localScale = Vector3.one;
                    dot.gameObject.tag = "dot";
                    prevSquares[i] = dot;
                }
                prevMaterial = image.material;
                image.material = (Material)Resources.Load("Materials/SelectedSquare");
                selectedPiece = Game.board.getPiece(file, rank);
                selectedGO = GameObject.Find(file + "," + rank).transform.GetChild(0).gameObject;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }

    public void onDragPiece()
    {
        if (selectedPiece != null && pawnPromotion.transform.GetChild(0).gameObject.activeSelf == false && pawnPromotion.transform.GetChild(1).gameObject.activeSelf == false)
        {
            Vector3 v = Input.mousePosition;
            v.z = 100;
            v = Camera.main.ScreenToWorldPoint(v);
            if (Input.GetMouseButtonUp(0))
            {
                squareSelected(selectedGO);
            }
            else
            {
                v.x = Mathf.Clamp(v.x, -50, 50);
                v.y = Mathf.Clamp(v.y, -50, 50);
                selectedGO.transform.position = v;
            }
        }
    }

    [PunRPC]
    public void move(int a, int b, int c, int d)
    {
        Piece rookToCastle = Game.board.isCastling(a, b, c, d);
        if (rookToCastle != null)
        {
            string pos = Game.board.getPosition(rookToCastle).x.ToString() + "," + Game.board.getPosition(rookToCastle).y.ToString();
            GameObject rook = GameObject.Find(pos).transform.GetChild(0).gameObject;
            pos = ((a + c) / 2).ToString() + "," + Game.board.getPosition(rookToCastle).y.ToString();
            GameObject square = GameObject.Find(pos).gameObject;
            rook.transform.SetParent(square.transform);
            rook.transform.position = square.transform.position;
        }
        else if (Game.board.enPassant == new Vector2Int(c, d))
        {
            if (Game.board.squares[a, b].colour == Colour.White)
            {
                pieceTaken(GameObject.Find(c.ToString() + "," + (d - 1).ToString()).transform.GetChild(0).gameObject);
            }
            else
            {
                pieceTaken(GameObject.Find(c.ToString() + "," + (d + 1).ToString()).transform.GetChild(0).gameObject);
            }
        }
        try
        {
            pieceTaken(GameObject.Find(c.ToString() + "," + d.ToString()).transform.GetChild(0).gameObject);
        }
        catch { }
        GameObject piece = GameObject.Find(a.ToString() + "," + b.ToString()).transform.GetChild(0).gameObject;
        GameObject parent = GameObject.Find(c.ToString() + "," + d.ToString()).gameObject;
        piece.transform.SetParent(parent.transform);
        piece.transform.position = parent.transform.position;
        if (prevSquare1 != null)
        {
            int file = int.Parse(prevSquare1.name.Substring(0, 1));
            int rank = int.Parse(prevSquare1.name.Substring(2, 1));
            prevSquare1.GetComponent<Image>().material = (Material)Resources.Load("Materials/LightSquare");
            if ((file + rank) % 2 == 0)
            {
                prevSquare1.GetComponent<Image>().material = (Material)Resources.Load("Materials/DarkSquare");
            }
            file = int.Parse(prevSquare2.name.Substring(0, 1));
            rank = int.Parse(prevSquare2.name.Substring(2, 1));
            prevSquare2.GetComponent<Image>().material = (Material)Resources.Load("Materials/LightSquare");
            if ((file + rank) % 2 == 0)
            {
                prevSquare2.GetComponent<Image>().material = (Material)Resources.Load("Materials/DarkSquare");
            }
        }
        prevSquare1 = GameObject.Find(a.ToString() + "," + b.ToString());
        prevSquare2 = GameObject.Find(c.ToString() + "," + d.ToString());
        prevSquare1.GetComponent<Image>().material = (Material)Resources.Load("Materials/PrevSquare");
        prevSquare2.GetComponent<Image>().material = (Material)Resources.Load("Materials/PrevSquare");

        Game.board.movePiece(a, b, c, d);
        seeIfCheckOrStaleMate();
    }

    private void pieceTaken(GameObject piece)
    {
        if (piece.name.Contains("Black"))
        {
            if (!Game.whitePiecesTaken.Contains(piece.name))
            {
                Game.whitePiecesTaken.Add(piece.name);
                piece.transform.SetParent(GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.Count - 1));
                piece.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.Count - 1).transform.position;
            }
            else
            {
                piece.transform.SetParent(GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)));
                piece.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)).transform.position;
                piece.transform.parent.GetComponent<TextMesh>().text = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)).childCount.ToString();
            }
        }
        else
        {
            if (!Game.blackPiecesTaken.Contains(piece.name))
            {
                Game.blackPiecesTaken.Add(piece.name);
                piece.transform.SetParent(GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.Count - 1));
                piece.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.Count - 1).transform.position;
            }
            else
            {
                piece.transform.SetParent(GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)));
                piece.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)).transform.position;
                piece.transform.parent.GetComponent<TextMesh>().text = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)).childCount.ToString();
            }
        }
        piece.transform.localScale = new Vector3(100, 100, 100);
    }

    private void seeIfCheckOrStaleMate()
    {
        if (Game.board.isCheckMate(Game.board.wKing) == true)
        {
            gameEnd.SetActive(true);
            gameEnd.transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nBlack Wins";
        }
        else if (Game.board.isCheckMate(Game.board.bKing) == true)
        {
            gameEnd.SetActive(true);
            gameEnd.transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nWhite Wins";
        }
        else if (Game.board.isStaleMate(Game.board.wKing) == true)
        {
            gameEnd.SetActive(true);
            gameEnd.transform.GetChild(0).GetComponent<Text>().text = "Stalemate!\n\nDraw";
        }
        else if (Game.board.isStaleMate(Game.board.bKing) == true)
        {
            gameEnd.SetActive(true);
            gameEnd.transform.GetChild(0).GetComponent<Text>().text = "Stalemate!\n\nDraw";
        }
    }

    private void loadPieces()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject.Find(i.ToString() + "," + j.ToString()).GetComponent<Image>().material = (Material)Resources.Load("Materials/LightSquare");
                if ((i + j) % 2 == 0)
                {
                    GameObject.Find(i.ToString() + "," + j.ToString()).GetComponent<Image>().material = (Material)Resources.Load("Materials/DarkSquare");
                }
                if (GameObject.Find(i.ToString() + "," + j.ToString()).transform.childCount > 0)
                {
                    Destroy(GameObject.Find(i.ToString() + "," + j.ToString()).transform.GetChild(0).gameObject);
                }
                if (Game.board.squares[i, j] != null)
                {
                    GameObject go = (GameObject)Instantiate(Resources.Load(Game.board.squares[i, j].ToString().Replace(" ", string.Empty)));
                    go.name = Game.board.squares[i, j].ToString();
                    go.transform.SetParent(GameObject.Find(i.ToString() + "," + j.ToString()).transform);
                    go.transform.localPosition = Vector3.zero;
                }
            }
        }
        foreach (GameObject dot in GameObject.FindGameObjectsWithTag("dot"))
        {
            Destroy(dot);
        }
    }

    public void selectPromotion(string piece)
    {
        Vector2Int from = Game.board.getPosition(selectedPiece);
        Vector2Int to = new Vector2Int(int.Parse(selectedGO.transform.parent.name.Split(',')[0]), int.Parse(selectedGO.transform.parent.name.Split(',')[1]));
        photonView.RPC("promotePawn", RpcTarget.All, piece, from.x, from.y, to.x, to.y);

        if (PhotonNetwork.OfflineMode == true && gameEnd.activeSelf == false)
        {
            StartCoroutine(AI.makeMove(Game.board, this));
        }

        selectedPiece = null;
        selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
        selectedGO = null;
    }

    [PunRPC]
    public void promotePawn(string piece, int fromX, int fromY, int toX, int toY)
    {
        pawnPromotion.transform.GetChild(0).gameObject.SetActive(false);
        pawnPromotion.transform.GetChild(1).gameObject.SetActive(false);

        string pieceType = piece.Split(' ')[1];
        Colour colour = Colour.White;
        if (piece.Split(' ')[0] == "Black")
        {
            colour = Colour.Black;
        }
        if (pieceType == "Queen")
        {
            Game.board.squares[fromX, fromY] = new Queen(colour);
        }
        else if (pieceType == "Rook")
        {
            Game.board.squares[fromX, fromY] = new Rook(colour);
        }
        else if (pieceType == "Bishop")
        {
            Game.board.squares[fromX, fromY] = new Bishop(colour);
        }
        else if (pieceType == "Knight")
        {
            Game.board.squares[fromX, fromY] = new Knight(colour);
        }

        Game.board.movePiece(fromX, fromY, toX, toY);

        if(GameObject.Find(toX.ToString() + "," + toY.ToString()).transform.childCount == 1)
        {
            Destroy(GameObject.Find(toX.ToString() + "," + toY.ToString()).transform.GetChild(0).gameObject);
        }
        if(GameObject.Find(fromX.ToString() + "," + fromY.ToString()).transform.childCount == 1)
        {
            Destroy(GameObject.Find(fromX.ToString() + "," + fromY.ToString()).transform.GetChild(0).gameObject);
        }

        GameObject go = (GameObject)Instantiate(Resources.Load(piece.Replace(" ", string.Empty)));
        go.transform.SetParent(GameObject.Find(toX.ToString() + "," + toY.ToString()).transform);
        go.transform.localPosition = Vector3.zero;

        prevSquare1 = GameObject.Find(fromX.ToString() + "," + fromY.ToString());
        prevSquare2 = GameObject.Find(toX.ToString() + "," + toY.ToString());
        prevSquare1.GetComponent<Image>().material = (Material)Resources.Load("Materials/PrevSquare");
        prevSquare2.GetComponent<Image>().material = (Material)Resources.Load("Materials/PrevSquare");

        seeIfCheckOrStaleMate();
    }
}
