using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    //StreamWriter writer = new StreamWriter("Assets/GameStatus.txt", false);

    private Piece selectedPiece;
    private GameObject[] prevSquares = new GameObject[0];
    private Material[] prevMaterials = new Material[0];
    private Player player;
    private Computer computer;
    private Game game;
    private bool ableToMove = true;
    private Square sq;

    // Use this for initialization
    void Start()
    {
        using(StreamReader sr = new StreamReader("Assets/GameStatus.txt"))
        {
            //game = new Game(sr.ReadLine());
            game = new Game();
        }
        player = game.getPlayer();
        computer = game.getComputer();
        updateBoard();
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void squareClicked(Image image)
    {
        Square square = Game.getBoard().getSquare(image.gameObject.name);
        
        if(pieceSelected(image) == false)
        {
            selectedPiece = null;
            showMovesReset();
            //ableToMove = true;
            pieceSelected(image);
        }
    }

    private bool pieceSelected(Image image)
    {
        sq = Game.getBoard().getSquare(image.gameObject.name);
        Square square = Game.getBoard().getSquare(image.gameObject.name);
        if (square.getPiece() != null && selectedPiece == null && ableToMove == true) //clicked piece to move
        {
            Piece piece = square.getPiece();
            piece.updatePossibleMoves();
            //if (square.Piece().getColour().Equals(player.getColour()))
            //{
                prevMaterials = new Material[piece.getPossibleMoves().Count + 1];
                prevSquares = new GameObject[piece.getPossibleMoves().Count + 1];
                for (int i = 0; i < piece.getPossibleMoves().Count; i++)
                {
                    Square sq = piece.getPossibleMoves()[i];
                    prevMaterials[i] = GameObject.Find(sq.ToString()).GetComponent<Image>().material;
                    prevSquares[i] = GameObject.Find(sq.ToString());
                    GameObject.Find(sq.ToString()).GetComponent<Image>().material = (Material)Resources.Load("Materials/SelectedSquare");
                }
                prevMaterials[prevMaterials.Length - 1] = image.material;
                prevSquares[prevSquares.Length - 1] = GameObject.Find(square.ToString());
                image.material = (Material)Resources.Load("Materials/SelectedSquare");
                selectedPiece = piece;
            //}
            return true;
        }
        else if (selectedPiece != null && Game.getBoard().findSquareWithPiece(selectedPiece).getPiece().getPossibleMoves().Contains(square) && ableToMove == true) //move piece
        {
            game.movePiece(selectedPiece, Game.getBoard().findSquareWithPiece(selectedPiece), square);
            updateBoard();
            Game.getBoard().updateBoard();
            if (square.getRow() == 8 && selectedPiece.GetType().Equals(typeof(Pawn)))
            {
                GameObject.Find("Promotion").GetComponent<Canvas>().enabled = true;
                ableToMove = false;
            }
            else
            {
                selectedPiece = null;
                ableToMove = true;
            }
            showMovesReset();
            return true;
        }
        return false;
    }

    private void showMovesReset()
    {
        for (int i = 0; i < prevSquares.Length; i++)
        {
            prevSquares[i].GetComponent<Image>().material = prevMaterials[i];
        }
    }

    public void selectPromotion(Image image)
    {
        GameObject.Find("Promotion").GetComponent<Canvas>().enabled = false;
        Square square = Game.getBoard().findSquareWithPiece(selectedPiece);
        Destroy(GameObject.Find(square.ToString()).transform.GetChild(0).gameObject);
        if (image.name == "Queen")
        {
            (selectedPiece as Pawn).promote(square, new Queen(player.getColour()));
            
        }
        else if (image.name == "Rook")
        {
            (selectedPiece as Pawn).promote(square, new Rook(player.getColour()));
        }
        else if (image.name == "Bishop")
        {
            (selectedPiece as Pawn).promote(square, new Bishop(player.getColour()));
        }
        else if (image.name == "Knight")
        {
            (selectedPiece as Pawn).promote(square, new Knight(player.getColour()));
        }
        GameObject go = (GameObject)Instantiate(Resources.Load(square.getPiece().getColour() + square.getPiece().GetType().ToString()));
        go.name = square.getPiece().getColour() + square.getPiece().GetType().ToString();
        go.transform.parent = GameObject.Find(square.ToString()).transform;
        go.transform.position = GameObject.Find(square.ToString()).transform.position;
        selectedPiece = null;
        ableToMove = true;
    }

    public void updateBoard()
    {
        foreach (Square square in Game.getBoard().getSquaresOnBoard())
        {
            if (GameObject.Find(square.ToString()).transform.childCount != 0)
            {
                Destroy(GameObject.Find(square.ToString()).transform.GetChild(0).gameObject);
            }
        }
        print(game.getPGN());
        print(game.getFen());
        //StreamWriter writer = new StreamWriter("Assets/GameStatus.txt", false);
        //writer.WriteLine(game.getFen());
        //writer.Flush();
        //writer.Close();
        using (StreamWriter sw = new StreamWriter("Assets/GameStatus.txt", false))
        {
            sw.WriteLine(game.getFen());
        }
        //print(Game.getBoard());
        string board = Game.getBoard().ToString();
        string alphabet = "ABCDEFGH";
        int rank = 8;
        int file = 0;
        foreach(char c in board)
        {
            int number;
            bool isNumber = int.TryParse(c.ToString(), out number);
            file++;
            if (c.Equals('/'))
            {
                rank--;
                file = 0;
            }
            else if (c.Equals('r'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("BlackRook"));
                go.name = "Black Rook";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('n'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("BlackKnight"));
                go.name = "Black Knight";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('b'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("BlackBishop"));
                go.name = "Black Bishop";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('q'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("BlackQueen"));
                go.name = "Black Queen";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('k'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("BlackKing"));
                go.name = "Black King";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('p'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("BlackPawn"));
                go.name = "Black Pawn";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('R'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("WhiteRook"));
                go.name = "White Rook";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('N'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("WhiteKnight"));
                go.name = "White Knight";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('B'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("WhiteBishop"));
                go.name = "White Bishop";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('Q'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("WhiteQueen"));
                go.name = "White Queen";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('K'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("WhiteKing"));
                go.name = "White King";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (c.Equals('P'))
            {
                GameObject go = (GameObject)Instantiate(Resources.Load("WhitePawn"));
                go.name = "White Pawn";
                go.transform.parent = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform;
                go.transform.position = GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.position;
            }
            else if (isNumber)
            {
                for (int i = file; i < file + number; i++)
                {
                    if(GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.childCount != 0)
                    {
                        Destroy(GameObject.Find(alphabet.Substring(file - 1, 1) + rank).transform.GetChild(0).gameObject);
                    }
                }
                file = file + number - 1;;
            }
            updateMoves();
        }
    }

    public void updateMoves()
    {
        //GameObject.Find("Moves").GetComponent<Text>().text = game.getPGN();
    }
}
