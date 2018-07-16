using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece {

    public Queen(Player player, Board board, int c, int r) : base(player, board, c, r)
    {
        value = 9;
    }

    public override List<Square> getPossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = board.getSquare(this).getColumn();
        int row = board.getSquare(this).getRow();

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

        return possibleMoves;
    }
}
