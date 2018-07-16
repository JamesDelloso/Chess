using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {

    public Bishop(Colour colour, Board board, int c, int r) : base(colour, board, c, r)
    {
        value = 3;
    }

    public override List<Square> getPossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = board.getSquare(this).getColumn();
        int row = board.getSquare(this).getRow();

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
        return possibleMoves;
    }
}
