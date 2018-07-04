using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece {

    protected List<Square> possibleMoves;
    protected List<Square> protectingSquares;
    private Colour colour;
    protected Board board;
    protected int value; 

    public Piece() { }

	public Piece(Colour colour, Board board) {
        this.colour = colour;
        possibleMoves = new List<Square>();
        protectingSquares = new List<Square>();
        this.board = board;
        //updatePossibleMoves();
    }

    public virtual Piece deepCopy(Board b)
    {
        Piece other = new Piece(colour, b);
        return other;
    }

    public Colour getColour()
    {
        return colour;
    }

    public int getValue()
    {
        return value;
    }

    public Board getBoard()
    {
        return board;
    }

    public virtual void updatePossibleMoves()
    {

    }

    public List<Square> getPossibleMoves()
    {
        Square[] s = possibleMoves.ToArray();
        foreach (Square sq in s)
        {
            //Square square = getBoard().findSquareWithPiece(this);
            //getBoard().movePiece(this, sq);
            //if (getBoard().isCheck())
            //{
            //    Debug.Log("Check in Pawn Class");
            //    //possibleMoves.Remove(sq);
            //}
            //getBoard().movePiece(this, square);
        }
        return possibleMoves;
    }

    public List<Square> getProtectingSquares()
    {
        return protectingSquares;
    }

    public virtual void move(Square to)
    {
        //to.addPiece(this);
        //foreach (Square square in allPossibleMoves())
        //{
        //    if (square.isEmpty() == false)
        //    {
        //        square.addAttackingPieces(this);
        //    }
        //}
        //Board b = new Board(Game.getBoard());
        //b.simulateMove(this, to);
    }

    public override string ToString()
    {
        return getColour() + " " + GetType();
    }
}
