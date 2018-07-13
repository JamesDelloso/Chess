using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {

     public Queen(Colour colour) : base(colour)
    {
        value = 9;
    }

    public override void updatePossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = Game.getBoard().findSquareWithPiece(this).getColumn();
        int row = Game.getBoard().findSquareWithPiece(this).getRow();

        for (int i = column - 1; i > 0; i--)
        {
            if (checkSquare(i, row) == false)
            {
                break;
            }
        }
        for (int i = column + 1; i < 9; i++)
        {
            if (checkSquare(i, row) == false)
            {
                break;
            }
        }
        for (int i = row - 1; i > 0; i--)
        {
            if (checkSquare(column, i) == false)
            {
                break;
            }
        }
        for (int i = row + 1; i < 9; i++)
        {
            if (checkSquare(column, i) == false)
            {
                break;
            }
        }

        int r = row - 1;
        for (int c = column - 1; c > 0 && r > 0; c--, r--)
        {
            if (checkSquare(c, r) == false)
            {
                break;
            }
        }
        r = row + 1;
        for (int c = column + 1; c < 9 && r < 9; c++, r++)
        {
            if (checkSquare(c, r) == false)
            {
                break;
            }
        }
        r = row + 1;
        for (int c = column - 1; c > 0 && r < 9; c--, r++)
        {
            if (checkSquare(c, r) == false)
            {
                break;
            }
        }
        r = row - 1;
        for (int c = column + 1; c < 9 && r > 0; c++, r--)
        {
            if (checkSquare(c, r) == false)
            {
                break;
            }
        }
    }
}
