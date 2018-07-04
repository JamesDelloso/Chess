using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    private Piece selectedPiece;
    private GameObject[] prevSquares = new GameObject[0];
    private Material[] prevMaterials = new Material[0];
    private Player player;
    private Computer computer;
    private Game game;
    private bool ableToMove = true;

    // Use this for initialization
    void Start()
    {
        //new Game.getBoard()(player.getColour(), computer.getColour());
        game = new Game();
        player = game.getPlayer();
        computer = game.getComputer();
        updateBoard();

        //print(Game.getBoard());
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
        Square square = Game.getBoard().getSquare(image.gameObject.name);
        if (square.Piece() != null && selectedPiece == null && ableToMove == true) //clicked piece to move
        {
            Piece piece = square.Piece();
            if (square.Piece().getColour().Equals(player.getColour()))
            {
                prevMaterials = new Material[piece.getPossibleMoves().Count + 1];
                prevSquares = new GameObject[piece.getPossibleMoves().Count + 1];
                for (int i = 0; i < piece.getPossibleMoves().Count; i++)
                {
                    Square sq = piece.getPossibleMoves()[i];
                    prevMaterials[i] = GameObject.Find(sq.Name()).GetComponent<Image>().material;
                    prevSquares[i] = GameObject.Find(sq.Name());
                    GameObject.Find(sq.Name()).GetComponent<Image>().material = (Material)Resources.Load("Materials/SelectedSquare");
                }
                prevMaterials[prevMaterials.Length - 1] = image.material;
                prevSquares[prevSquares.Length - 1] = GameObject.Find(square.Name());
                image.material = (Material)Resources.Load("Materials/SelectedSquare");
                selectedPiece = piece;
            }
            return true;
        }
        else if (selectedPiece != null && Game.getBoard().findSquareWithPiece(selectedPiece).Piece().getPossibleMoves().Contains(square) && ableToMove == true) //move piece
        {
            //selectedPiece.move(square);
            Game.getBoard().movePiece(selectedPiece, square);
            computer.play();
            updateBoard();
            Game.getBoard().updateBoard();
            if (square.Row() == 8 && selectedPiece.GetType().Equals(typeof(Pawn)))
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
        Destroy(GameObject.Find(square.Name()).transform.GetChild(0).gameObject);
        if (image.name == "Queen")
        {
            (selectedPiece as Pawn).promote(square, new Queen(player.getColour(), Game.getBoard()));
            
        }
        else if (image.name == "Rook")
        {
            (selectedPiece as Pawn).promote(square, new Rook(player.getColour(), Game.getBoard()));
        }
        else if (image.name == "Bishop")
        {
            (selectedPiece as Pawn).promote(square, new Bishop(player.getColour(), Game.getBoard()));
        }
        else if (image.name == "Knight")
        {
            (selectedPiece as Pawn).promote(square, new Knight(player.getColour(), Game.getBoard()));
        }
        GameObject go = (GameObject)Instantiate(Resources.Load(square.Piece().getColour() + square.Piece().GetType().ToString()));
        go.name = square.Piece().getColour() + square.Piece().GetType().ToString();
        go.transform.parent = GameObject.Find(square.Name()).transform;
        go.transform.position = GameObject.Find(square.Name()).transform.position;
        selectedPiece = null;
        ableToMove = true;
    }

    public void updateBoard()
    {
        foreach (Square square in Game.getBoard().getSquaresOnBoard())
        {
            if (GameObject.Find(square.Name()).transform.childCount != 0)
            {
                Destroy(GameObject.Find(square.Name()).transform.GetChild(0).gameObject);
            }
            if (square.isEmpty() == false)
            {
                GameObject go = (GameObject)Instantiate(Resources.Load(square.Piece().getColour() + square.Piece().GetType().ToString()));
                go.name = square.Piece().getColour() + square.Piece().GetType().ToString();
                go.transform.parent = GameObject.Find(square.Name()).transform;
                go.transform.position = GameObject.Find(square.Name()).transform.position;
            }
            //GameObject textGo = new GameObject(square.Name()+" value");
            //textGo.AddComponent<Text>();
            //textGo.GetComponent<Text>().text = square.getValue(computer.getColour()).ToString();
            //textGo.transform.parent = GameObject.Find(square.Name()).transform;
        }
        print(Game.getBoard());
    }
}
