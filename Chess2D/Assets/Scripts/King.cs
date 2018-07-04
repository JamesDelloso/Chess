using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {

    private bool hasMoved;

    public King(Colour colour, Board board) : base(colour, board)
    {
        value = 100;
        hasMoved = false;
    }
    public override Piece deepCopy(Board b)
    {
        return new King(getColour(), b);
    }
    public override void updatePossibleMoves()
    {
        possibleMoves = new List<Square>();
        protectingSquares = new List<Square>();
        int column1 = getBoard().findSquareWithPiece(this).Column();
        int row1 = getBoard().findSquareWithPiece(this).Row();
        foreach (Square square in getBoard().getSquaresOnBoard())
        {
            int column2 = square.Column();
            int row2 = square.Row();
            int colDiff = column2 - column1;
            int rowDiff = row2 - row1;
            if (colDiff >= -1 && colDiff <= 1 && rowDiff >= -1 && rowDiff <= 1)
            {
                if (square.isEmpty() == false)
                {
                    if (square.Piece().getColour() != getColour())
                    {
                        possibleMoves.Add(square);
                    }
                    else
                    {
                        protectingSquares.Add(square);
                    }
                }
                else
                {
                    possibleMoves.Add(square);
                }
                if(isCheck(square))
                {
                    possibleMoves.Remove(square);
                }
            }
        }
        if (hasMoved == false && getBoard().getSquare("A1").isEmpty() == false && getBoard().getSquare("B1").isEmpty() && getBoard().getSquare("C1").isEmpty() && getBoard().getSquare("D1").isEmpty())
        {
            if (getBoard().getSquare("A1").Piece().GetType().Equals(typeof(Rook)))
            {
                if ((getBoard().getSquare("A1").Piece() as Rook).HasMoved() == false)
                {
                    possibleMoves.Add(getBoard().getSquare("C1"));
                    if (isCheck(getBoard().getSquare("D1")))
                    {
                        possibleMoves.Remove(getBoard().getSquare("D1"));
                        possibleMoves.Remove(getBoard().getSquare("C1"));
                    }
                }
            }
        }
        if (hasMoved == false && getBoard().getSquare("H1").isEmpty() == false && getBoard().getSquare("F1").isEmpty() && getBoard().getSquare("G1").isEmpty())
        {
            if (getBoard().getSquare("H1").Piece().GetType().Equals(typeof(Rook)))
            {
                if ((getBoard().getSquare("H1").Piece() as Rook).HasMoved() == false)
                {
                    possibleMoves.Add(getBoard().getSquare("G1"));
                    if (isCheck(getBoard().getSquare("F1")))
                    {
                        possibleMoves.Remove(getBoard().getSquare("F1"));
                        possibleMoves.Remove(getBoard().getSquare("G1"));
                    }
                }
            }
        }
    }

    public bool canCastle(Square to)
    {
        //Board oldBoard = new Board(getBoard());
        if (hasMoved == false && to.Equals(getBoard().getSquare("C1")) && possibleMoves.Contains(getBoard().getSquare("D1")))
        {
            return true;
        }
        else if (hasMoved == false && to.Equals(getBoard().getSquare("G1")) && possibleMoves.Contains(getBoard().getSquare("F1")))
        {
            return true;
        }
        hasMoved = true;
        return false;
    }

    public bool isCheck(Square square)
    {
        foreach (Piece piece in square.getAttackingPieces(getColour()))
        {
            if (piece.getColour() != getColour())
            {
                //Debug.Log("Check");
                //if(square.Name().Equals("D1"))
                //Debug.Log(square+":" + piece);
                return true;
            }
        }
        return false;
    }
}
