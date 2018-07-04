using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece {

    private bool hasMoved;

    public Rook(Colour colour, Board board) : base(colour, board)
    {
        hasMoved = false;
        value = 5;
    }

    public void move()
    {
        hasMoved = true;
    }

    public bool HasMoved()
    {
        return hasMoved;
    }
    public override Piece deepCopy(Board b)
    {
        return new Rook(getColour(), b);
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
            if (colDiff == 0 && getBoard().isColumnBlocked(getBoard().findSquareWithPiece(this), square) == false)
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
            }
            else if (rowDiff == 0 && getBoard().isRowBlocked(getBoard().findSquareWithPiece(this), square) == false)
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
            }
        }
    }
}
