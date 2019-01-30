using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece {

    public Rook(Colour colour) : base(colour)
    {
        value = 5;
    }

    public override List<Vector2Int> generatePossibleMoves(Board board)
    {
        possibleMoves = new List<Vector2Int>();
        int file = board.getPosition(this).x;
        int rank = board.getPosition(this).y;

        for (int i = file - 1; i >= 0; i--)
        {
            if (checkSquare(board, i, rank) == false)
            {
                break;
            }
        }
        for (int i = file + 1; i <= 7; i++)
        {
            if (checkSquare(board, i, rank) == false)
            {
                break;
            }
        }
        for (int i = rank - 1; i >= 0; i--)
        {
            if (checkSquare(board, file, i) == false)
            {
                break;
            }
        }
        for (int i = rank + 1; i <= 7; i++)
        {
            if (checkSquare(board, file, i) == false)
            {
                break;
            }
        }
        removePossibleChecks(board);
        return possibleMoves;
    }

    public override float getMobilityValue(int file, int rank)
    {
        List<double[]> list = new List<double[]>();
        list.Add(new double[] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 });
        list.Add(new double[] { 0.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.5 });
        list.Add(new double[] { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 });
        list.Add(new double[] { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 });
        list.Add(new double[] { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 });
        list.Add(new double[] { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 });
        list.Add(new double[] { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 });
        list.Add(new double[] { 0.0, 0.0, 0.0, 0.5, 0.5, 0.0, 0.0, 0.0 });
        if(colour == Colour.Black)
        {
            list.Reverse();
        }
        return (float)list[file][rank];
        //return 14;
    }
}
