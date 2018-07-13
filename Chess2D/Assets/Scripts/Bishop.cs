using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {

    public Bishop(Colour colour) : base(colour)
    {
        value = 3;
    }

    public override void updatePossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = Game.getBoard().findSquareWithPiece(this).getColumn();
        int row = Game.getBoard().findSquareWithPiece(this).getRow();

        int r = row-1;
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
