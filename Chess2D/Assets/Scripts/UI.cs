using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    private Board board;
    private Piece selectedPiece;
    private GameObject selectedGO;
    private bool promotingPawn = false;
    private bool mouseDown = false;

    private GameObject[] prevSquares = new GameObject[0];
    private Material[] prevMaterials = new Material[0];

    // Use this for initialization
    void Start()
    {
        using (StreamReader sr = new StreamReader("Assets/GameStatus.txt"))
        {
            //board = new Board(sr.ReadLine());
            board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }
        updatePieces();
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
                board.movePiece(board.getPosition(selectedPiece).x, board.getPosition(selectedPiece).y, file, rank);
                if (selectedPiece.GetType().Equals(typeof(Pawn)) && rank == 7)
                {
                    GameObject.Find("Promotion").GetComponent<Canvas>().enabled = true;
                    promotingPawn = true;
                }
                else
                {
                    selectedPiece = null;
                    selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    selectedGO = null;
                }
                updatePieces();
            }
            else if (selectedPiece != null && !selectedPiece.generatePossibleMoves(board).Contains(new Vector2Int(file, rank)))
            {
                selectedGO.transform.localPosition = Vector3.zero;
                selectedPiece = null;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                selectedGO = null;
            }
            if (samePiece == false && board.getPiece(file, rank) != null && ((board.getPiece(file, rank).colour == Colour.White && board.whitesTurn) || (board.getPiece(file, rank).colour == Colour.Black && !board.whitesTurn)))
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
        GameObject.Find("Promotion").GetComponent<Canvas>().enabled = false;
        Vector2Int square = board.getPosition(selectedPiece);
        Destroy(GameObject.Find(square.x.ToString() + "," + square.y.ToString()).transform.GetChild(0).gameObject);
        if (image.name == "Queen")
        {
            board.squares[square.x, square.y] = new Queen(selectedPiece.colour);
        }
        else if (image.name == "Rook")
        {
            board.squares[square.x, square.y] = new Rook(selectedPiece.colour);
        }
        else if (image.name == "Bishop")
        {
            board.squares[square.x, square.y] = new Bishop(selectedPiece.colour);
        }
        else if (image.name == "Knight")
        {
            board.squares[square.x, square.y] = new Knight(selectedPiece.colour);
        }
        selectedGO = (GameObject)Instantiate(Resources.Load(board.getPiece(square.x, square.y).ToString().Replace(" ", string.Empty)));
        selectedGO.name = board.getPiece(square.x, square.y).ToString();
        selectedGO.transform.parent = GameObject.Find(square.x.ToString() + "," + square.y.ToString()).transform;
        selectedGO.transform.position = GameObject.Find(square.x.ToString() + "," + square.y.ToString()).transform.position;
        promotingPawn = false;
        //selectedPiece = null;
    }

    public void updatePieces()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (GameObject.Find(i.ToString() + "," + j.ToString()).transform.childCount != 0)
                {
                    Destroy(GameObject.Find(i.ToString() + "," + j.ToString()).transform.GetChild(0).gameObject);
                }
                if (board.squares[i,j] != null)
                {
                    GameObject go = (GameObject)Instantiate(Resources.Load(board.squares[i, j].ToString().Replace(" ", string.Empty)));
                    go.name = board.squares[i, j].ToString();
                    go.transform.parent = GameObject.Find(i.ToString() + "," + j.ToString()).transform;
                    go.transform.position = GameObject.Find(i.ToString() + "," + j.ToString()).transform.position;
                }
            }
        }
        using (StreamWriter sw = new StreamWriter("Assets/GameStatus.txt", false))
        {
            sw.WriteLine(FEN.generate(board));
        }
        updateMoves();
    }

    public void updateMoves()
    {
        //GameObject.Find("Moves").GetComponent<Text>().text += board.moves[board.moves.Count-1];
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
        updatePieces();
    }
}
