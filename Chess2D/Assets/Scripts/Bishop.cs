using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {

    public Bishop(Colour colour, Board board) : base(colour, board)
    {
        value = 3;
    }
    public override Piece deepCopy(Board b)
    {
        Piece other = new Bishop(getColour(), b);
        return new Bishop(getColour(), b);
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
            if (System.Math.Abs(colDiff) == System.Math.Abs(rowDiff) && getBoard().isDiagonalBlocked(getBoard().findSquareWithPiece(this), square) == false)
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
