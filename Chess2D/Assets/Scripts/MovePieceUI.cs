using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using System.IO;
using Photon.Pun;
using ExitGames.Client.Photon;
using Unity.Jobs;

public class MovePieceUI : MonoBehaviourPun
{
    private GameObject selectedPiece;
    private bool promotingPawn = false;
    private string promotePawnFrom = "";
    private string promotePawnTo = "";
    private bool isWhite = true;

    private List<GameObject> legalMoves = new List<GameObject>();
    private Material[] prevMaterials = new Material[0];
    private Material prevMaterial;

    private GameObject prevSquare1;
    private GameObject prevSquare2;

    [SerializeField]
    private GameObject pawnPromotion;
    [SerializeField]
    private GameObject gameEnd;
    [SerializeField]
    private GameObject legalMoveDotPrefab;
    [SerializeField]
    private GameObject legalMoveBorderPrefab;
    [SerializeField]
    private Color32 lightColour;
    [SerializeField]
    private Color32 darkColour;
    [SerializeField]
    private GameObject pieceHighlight;
    [SerializeField]
    private GameObject lastMoveHighlight1;
    [SerializeField]
    private GameObject lastMoveHighlight2;

    // Use this for initialization
    void Start()
    {
        if (PhotonNetwork.IsConnected && PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            isWhite = false;
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
        if (Input.GetMouseButtonUp(0) && selectedPiece != null)
        {
            selectedPiece.transform.localPosition = Vector3.zero;
            selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
    }

    public void squareSelected(GameObject squareGo)
    {
        if (pawnPromotion.transform.GetChild(0).gameObject.activeSelf == false && pawnPromotion.transform.GetChild(1).gameObject.activeSelf == false)
        {
            lastMoveHighlight1.transform.localScale = Vector3.one;
            lastMoveHighlight2.transform.localScale = Vector3.one;
            for (int i = 0; i < legalMoves.Count; i++)
            {
                Destroy(legalMoves[i]);
            }
            if (selectedPiece != null)
            {
                selectedPiece.transform.localPosition = Vector3.zero;
            }
            if (selectedPiece != null && squareGo.transform.childCount != 0 && squareGo.transform.GetChild(0).name == "legalMove")
            {
                if ((selectedPiece.name == "WhitePawn" && squareGo.name[1] == '8') || (selectedPiece.name == "BlackPawn" && squareGo.name[1] == '1'))
                {
                    promotePawnFrom = selectedPiece.transform.parent.name;
                    promotePawnTo = squareGo.name;
                    if (squareGo.name[1] == '8')
                    {
                        pawnPromotion.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    else
                    {
                        pawnPromotion.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    selectedPiece.transform.SetParent(squareGo.transform);
                    if (squareGo.transform.childCount > 1)
                    {
                        Destroy(squareGo.transform.GetChild(0).gameObject);
                        if (squareGo.transform.childCount > 1)
                        {
                            Destroy(squareGo.transform.GetChild(1).gameObject);
                        }
                    }
                }
                else
                {
                    photonView.RPC("move", RpcTarget.All, selectedPiece.transform.parent.name, squareGo.name);
                    selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    selectedPiece = null;
                    if (PhotonNetwork.OfflineMode == true)
                    {
                        StartCoroutine(AI.getMove(Game.board, this));
                    }
                }
                pieceHighlight.transform.localScale = Vector3.zero;
            }
            else if (squareGo.transform.childCount != 0 && squareGo.transform.GetChild(0).name != "legalMove" && ((Game.board.isWhitesTurn() && char.IsUpper(Game.board.getPiece(squareGo.name)) && isWhite) || (!Game.board.isWhitesTurn() && char.IsLower(Game.board.getPiece(squareGo.name)) && !isWhite)))
            //else if (squareGo.transform.childCount != 0 && squareGo.transform.GetChild(0).name != "legalMove")
            {
                legalMoves = new List<GameObject>();
                foreach (string move in PossibleMoves.search(Game.board, squareGo.name))
                {
                    GameObject prefab = legalMoveDotPrefab;
                    GameObject square = GameObject.Find(move.Substring(0,2));
                    GameObject legalMove;
                    Transform parentTransform = square.transform;
                    if (parentTransform.childCount >= 1 && parentTransform.GetChild(0).name == "legalMove")
                    {
                        continue;
                    }
                    if ((parentTransform.childCount == 1 && parentTransform.GetChild(0).name != "legalMove") || parentTransform.childCount == 2)
                    {
                        prefab = legalMoveBorderPrefab;
                    }
                    legalMove = GameObject.Instantiate<GameObject>(prefab, parentTransform);
                    legalMove.name = "legalMove";
                    legalMove.transform.localPosition = Vector3.zero;
                    legalMove.transform.SetAsFirstSibling();
                    legalMoves.Add(legalMove);
                    if (lastMoveHighlight1.transform.position == square.transform.position)
                    {
                        lastMoveHighlight1.transform.localScale = Vector3.zero;
                    }
                    else if (lastMoveHighlight2.transform.position == square.transform.position)
                    {
                        lastMoveHighlight2.transform.localScale = Vector3.zero;
                    }
                }
                selectedPiece = squareGo.transform.GetChild(0).gameObject;
                selectedPiece.GetComponent<SpriteRenderer>().sortingOrder = 2;
                pieceHighlight.transform.position = squareGo.transform.position;
                pieceHighlight.transform.localScale = Vector3.one;
            }
            else
            {
                selectedPiece = null;
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
            v.x = Mathf.Clamp(v.x, -50, 50);
            v.y = Mathf.Clamp(v.y, -50, 50);
            selectedPiece.transform.position = v;
        }
    }

    [PunRPC]
    public void move(string from, string to)
    {
        Game.board.movePiece(from, to);
        lastMoveHighlight1.transform.position = GameObject.Find(from).transform.position;
        lastMoveHighlight2.transform.position = GameObject.Find(to.Substring(0,2)).transform.position;
        loadPieces();
        seeIfCheckOrStaleMate();
    }

    private void pieceTaken(GameObject piece)
    {
        //if (piece.name.Contains("Black"))
        //{
        //    if (!Game.whitePiecesTaken.Contains(piece.name))
        //    {
        //        Game.whitePiecesTaken.Add(piece.name);
        //        piece.transform.SetParent(GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.Count - 1));
        //        piece.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.Count - 1).transform.position;
        //    }
        //    else
        //    {
        //        piece.transform.SetParent(GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)));
        //        piece.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)).transform.position;
        //        piece.transform.parent.GetComponent<TextMesh>().text = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)).childCount.ToString();
        //    }
        //}
        //else
        //{
        //    if (!Game.blackPiecesTaken.Contains(piece.name))
        //    {
        //        Game.blackPiecesTaken.Add(piece.name);
        //        piece.transform.SetParent(GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.Count - 1));
        //        piece.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.Count - 1).transform.position;
        //    }
        //    else
        //    {
        //        piece.transform.SetParent(GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)));
        //        piece.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)).transform.position;
        //        piece.transform.parent.GetComponent<TextMesh>().text = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)).childCount.ToString();
        //    }
        //}
        //piece.transform.localScale = new Vector3(100, 100, 100);
    }

    private void seeIfCheckOrStaleMate()
    {
        if (Game.board.isCheckMate('K'))
        {
            gameEnd.SetActive(true);
            gameEnd.transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nBlack Wins";
        }
        else if (Game.board.isCheckMate('k'))
        {
            gameEnd.SetActive(true);
            gameEnd.transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nWhite Wins";
        }
        else if (Game.board.isStalemate('K') || Game.board.isStalemate('k'))
        {
            gameEnd.SetActive(true);
            gameEnd.transform.GetChild(0).GetComponent<Text>().text = "Stalemate!\n\nDraw";
        }
    }

    private void loadPieces()
    {
        string fen = Game.board.fen;
        string[] board = fen.Remove(fen.IndexOf(' ')).Split('/');
        string boardString = "";
        int index = 0;
        string prefabName = "";
        foreach (string s in board)
        {
            boardString = s + boardString;
        }
        for(int i=0;i<64;i++)
        {
            if (transform.GetChild(i).transform.childCount != 0)
            {
                Destroy(transform.GetChild(i).GetChild(0).gameObject);
                if (transform.GetChild(i).childCount == 2)
                {
                    Destroy(transform.GetChild(i).GetChild(1).gameObject);
                }
            }
        }
        int number;
        foreach (char c in boardString)
        {
            prefabName = "";
            if (int.TryParse(c.ToString(), out number) == true)
            {
                index += number;
            }
            else
            {
                if (char.IsLower(c))
                {
                    prefabName = "Black";
                }
                else if (char.IsUpper(c))
                {
                    prefabName = "White";
                }
                char piece = char.ToLower(c);
                if (piece == 'r')
                {
                    prefabName += "Rook";
                }
                else if (piece == 'n')
                {
                    prefabName += "Knight";
                }
                else if (piece == 'b')
                {
                    prefabName += "Bishop";
                }
                else if (piece == 'q')
                {
                    prefabName += "Queen";
                }
                else if (piece == 'k')
                {
                    prefabName += "King";
                }
                else if (piece == 'p')
                {
                    prefabName += "Pawn";
                }
                if(prefabName != "")
                {
                    GameObject pieceGo = (GameObject)Instantiate(Resources.Load(prefabName));
                    pieceGo.name = prefabName;
                    pieceGo.transform.SetParent(transform.GetChild(index).transform);
                    pieceGo.transform.localPosition = Vector3.zero;
                    index++;
                }
            }
        }
    }

    public void selectPromotion(string piece)
    {
        pawnPromotion.transform.GetChild(0).gameObject.SetActive(false);
        pawnPromotion.transform.GetChild(1).gameObject.SetActive(false);

        photonView.RPC("promotePawn", RpcTarget.All, piece, promotePawnFrom, promotePawnTo);
        if (PhotonNetwork.OfflineMode == true && gameEnd.activeSelf == false)
        {
            StartCoroutine(AI.getMove(Game.board, this));
        }
    }

    [PunRPC]
    public void promotePawn(string newPiece, string from, string to)
    {
        lastMoveHighlight1.transform.position = GameObject.Find(from).transform.position;
        lastMoveHighlight2.transform.position = GameObject.Find(to).transform.position;
        string pieceColour = newPiece.Split(' ')[0];
        char piece = newPiece.Split(' ')[1][0];
        if(newPiece.Split(' ')[1] == "Knight")
        {
            piece = 'N';
        }
        if(pieceColour == "Black")
        {
            piece = char.ToLower(piece);
        }
        Game.board.promotePawn(piece, from, to);
        loadPieces();
        seeIfCheckOrStaleMate();
    }
}
