using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Rook : Piece {

    public Rook(Colour colour, Board board, int c, int r) : base(colour, board, c, r)
    {
        value = 5;
    }

    public override List<Square> getPossibleMoves()
    {
        Profiler.BeginSample("Rook Possible Moves");
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
        for (int i = column+1; i < 9; i++)
        {
            if(checkSquare(i, row) == false)
            {
                break;
            }
        }
        for (int i = row-1; i > 0; i--)
        {
            if (checkSquare(column, i) == false)
            {
                break;
            }
        }
        for (int i = row+1; i < 9; i++)
        {
            if (checkSquare(column, i) == false)
            {
                break;
            }
        }
        Profiler.EndSample();
        return possibleMoves;
    }
}
