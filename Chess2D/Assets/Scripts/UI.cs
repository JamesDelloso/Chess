using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

public class UI : NetworkBehaviour {

    public static Board board;
    public static Piece selectedPiece;
    public static GameObject selectedGO;
    public static bool promotingPawn = false;
    public static Vector2Int promotingPos;

    private GameObject[] prevSquares = new GameObject[0];
    private Material[] prevMaterials = new Material[0];

    private ArrayList whitePiecesTaken = new ArrayList();
    private ArrayList blackPiecesTaken = new ArrayList();

    public static Player player;

    // Use this for initialization
    void Start()
    {
        using (StreamReader sr = new StreamReader("Assets/GameStatus.txt"))
        {
            //board = new Board(sr.ReadLine());
            //board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            //board = new Board("rnbqkbnr/pppp1ppp/8/4p3/3PP3/8/PPP2PPP/RNBQKBNR b KQkq e3 0 2");
            //board = new Board("rnbqkbnr/pppp1ppp/8/4pQ2/2BPP3/8/PPP2PPP/RNB1K1NR w KQkq - 7 5");
        }
        board = Game.board;
        //updatePieces();
    }

	// Update is called once per frame
	void Update () {
        
    }

    public void squareSelected(Image image)
    {
        if(promotingPawn == false)
        {
            int file = int.Parse(image.name.Substring(0, 1));
            int rank = int.Parse(image.name.Substring(2, 1));
            bool samePiece = false;
            if (board.getPosition(selectedPiece) == new Vector2Int(file, rank))
            {
                samePiece = true;
            }
            for (int i = 0; i < prevSquares.Length; i++)
            {
                prevSquares[i].GetComponent<Image>().material = prevMaterials[i];
            }
            if (selectedPiece != null && selectedPiece.generatePossibleMoves(board).Contains(new Vector2Int(file, rank)))
            {
                int x = board.getPosition(selectedPiece).x;
                int y = board.getPosition(selectedPiece).y;
                if (selectedPiece.GetType().Equals(typeof(Pawn)) && rank == 7)
                {
                    GameObject.Find("White Promotion").GetComponent<Canvas>().enabled = true;
                    promotingPawn = true;
                    promotingPos = new Vector2Int(file, rank);
                    //try
                    //{
                    //    Destroy(GameObject.Find(file.ToString() + "," + rank.ToString()).transform.GetChild(0).gameObject);
                    //}
                    //catch { }
                    //selectedGO.transform.SetParent(GameObject.Find(file.ToString() + "," + rank.ToString()).transform);
                    //selectedGO.transform.position = GameObject.Find(file.ToString() + "," + rank.ToString()).transform.position;
                }
                else if (selectedPiece.GetType().Equals(typeof(Pawn)) && rank == 0)
                {
                    GameObject.Find("Black Promotion").GetComponent<Canvas>().enabled = true;
                    promotingPawn = true;
                    promotingPos = new Vector2Int(file, rank);
                    //try
                    //{
                    //    Destroy(GameObject.Find(file.ToString() + "," + rank.ToString()).transform.GetChild(0).gameObject);
                    //}
                    //catch { }
                    //selectedGO.transform.SetParent(GameObject.Find(file.ToString() + "," + rank.ToString()).transform);
                    //selectedGO.transform.position = GameObject.Find(file.ToString() + "," + rank.ToString()).transform.position;
                }
                else
                {
                    player.CmdMove(x, y, file, rank);
                    selectedPiece = null;
                    selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    selectedGO = null;
                }
                //else
                //{
                //    selectedPiece = null;
                //    selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                //    selectedGO = null;
                //}
                //if (promotingPawn == false)
                //{
                //    player.CmdMove(x, y, file, rank);
                //    selectedPiece = null;
                //    selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                //    selectedGO = null;
                //}
                //try
                //{
                //    pieceTaken(GameObject.Find(file.ToString() + "," + rank.ToString()).transform.GetChild(0).gameObject);
                //}
                //catch { }
            }
            else if (selectedPiece != null && !selectedPiece.generatePossibleMoves(board).Contains(new Vector2Int(file, rank)))
            {
                selectedGO.transform.localPosition = Vector3.zero;
                selectedPiece = null;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                selectedGO = null;
            }
            if (samePiece == false && board.getPiece(file, rank) != null && ((board.getPiece(file, rank).colour == Colour.White && board.whitesTurn && name == "White Board") || (board.getPiece(file, rank).colour == Colour.Black && !board.whitesTurn && name == "Black Board")))
            {
                List<Vector2Int> possibleMoves = board.getPiece(file, rank).generatePossibleMoves(board);
                prevMaterials = new Material[possibleMoves.Count + 1];
                prevSquares = new GameObject[possibleMoves.Count + 1];
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    Vector2Int sq = possibleMoves[i];
                    prevMaterials[i] = GameObject.Find(sq.x.ToString() + "," + sq.y.ToString()).GetComponent<Image>().material;
                    prevSquares[i] = GameObject.Find(sq.x.ToString() + "," + sq.y.ToString());
                    GameObject.Find(sq.x.ToString() + "," + sq.y.ToString()).GetComponent<Image>().material = (Material)Resources.Load("Materials/SelectedSquare");
                }
                prevMaterials[prevMaterials.Length - 1] = image.material;
                prevSquares[prevSquares.Length - 1] = GameObject.Find(image.gameObject.name);
                image.material = (Material)Resources.Load("Materials/SelectedSquare");
                selectedPiece = board.getPiece(file, rank);
                selectedGO = GameObject.Find(file + "," + rank).transform.GetChild(0).gameObject;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
    }

    public void onDragPiece()
    {
        if(selectedPiece != null && promotingPawn == false)
        {
            Vector3 v = Input.mousePosition;
            v.z = 100;
            v = Camera.main.ScreenToWorldPoint(v);
            if (Input.GetMouseButtonUp(0))
            {
                squareSelected(selectedGO.transform.parent.GetComponent<Image>());
            }
            else
            {
                v.x = Mathf.Clamp(v.x, -50, 50);
                v.y = Mathf.Clamp(v.y, -50, 50);
                selectedGO.transform.position = v;
            }
        }
    }

    public void selectPromotion(Image image)
    {

        GameObject.Find("White Promotion").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Black Promotion").GetComponent<Canvas>().enabled = false;
        Vector2Int square = board.getPosition(selectedPiece);
        string colour = "White";
        if(selectedPiece.colour == Colour.Black)
        {
            colour = "Black";
        }
        player.CmdpromotePawn(image.name, colour, square.x, square.y, promotingPos.x, promotingPos.y);
        selectedPiece = null;
        selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
        selectedGO = null;
        promotingPawn = false;
    }

    public void updateMoves()
    {
        string whiteMoves = "";
        string blackMoves = "";
        for (int i = 0; i < board.moves.Count; i++)
        {
            if (i % 2 == 0)
            {
                whiteMoves += (i/2+1) + ". " + board.moves[i] + "\n";
            }
            else
            {
                blackMoves += board.moves[i] + "\n";
            }
        }
        GameObject.Find("White Moves").GetComponent<Text>().text = whiteMoves;
        GameObject.Find("Black Moves").GetComponent<Text>().text = blackMoves;
    }

    public void seeIfCheckOrStaleMate()
    {
        if (board.isCheckMate(board.wKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nBlack Wins";
        }
        else if (board.isCheckMate(board.bKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nWhite Wins";
        }
        else if (board.isStaleMate(board.wKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Stalemate!\n\nDraw";
        }
        else if (board.isStaleMate(board.bKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Stalemate!\n\nDraw";
        }
    }

    public void pieceTaken(GameObject go)
    {
        if(selectedPiece.colour == Colour.Black)
        {
            if(!whitePiecesTaken.Contains(go.name))
            {
                whitePiecesTaken.Add(go.name);
                go.transform.parent = GameObject.Find("White Taken Pieces").transform.GetChild(whitePiecesTaken.Count - 1);
                go.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(whitePiecesTaken.Count - 1).transform.position;
            }
            else
            {
                go.transform.parent = GameObject.Find("White Taken Pieces").transform.GetChild(whitePiecesTaken.IndexOf(go.name));
                go.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(whitePiecesTaken.IndexOf(go.name)).transform.position;
                go.transform.parent.GetComponent<TextMesh>().text = "x" + GameObject.Find("White Taken Pieces").transform.GetChild(whitePiecesTaken.IndexOf(go.name)).childCount.ToString();
            }
        }
        else
        {
            if (!blackPiecesTaken.Contains(go.name))
            {
                blackPiecesTaken.Add(go.name);
                go.transform.parent = GameObject.Find("Black Taken Pieces").transform.GetChild(blackPiecesTaken.Count - 1);
                go.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(blackPiecesTaken.Count - 1).transform.position;
            }
            else
            {
                go.transform.parent = GameObject.Find("Black Taken Pieces").transform.GetChild(blackPiecesTaken.IndexOf(go.name));
                go.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(blackPiecesTaken.IndexOf(go.name)).transform.position;
                go.transform.parent.GetComponent<TextMesh>().text = "x" + GameObject.Find("Black Taken Pieces").transform.GetChild(blackPiecesTaken.IndexOf(go.name)).childCount.ToString();
            }
        }
    }

    public void fenEntered(InputField fenInput)
    {
        print(fenInput.text);
        try
        {
            board = new Board(fenInput.text);
        }
        catch
        {
        }
        fenInput.text = "";
    }

    public void copyFEN()
    {
        TextEditor te = new TextEditor();
        te.text = FEN.generate(board);
        te.SelectAll();
        te.Copy();
    }

    public void newGame()
    {
        GameObject.Find("Game End").GetComponent<Canvas>().enabled = false;
        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }
}
