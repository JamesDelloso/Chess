using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    public Knight(Colour colour) : base(colour)
    {
        value = 3;
    }

    public override List<Vector2Int> generatePossibleMoves(Board board)
    {
        possibleMoves = new List<Vector2Int>();
        int file = board.getPosition(this).x;
        int rank = board.getPosition(this).y;

        checkSquare(board, file - 2, rank - 1);
        checkSquare(board, file - 1, rank - 2);
        checkSquare(board, file + 2, rank - 1);
        checkSquare(board, file + 1, rank - 2);
        checkSquare(board, file - 2, rank + 1);
        checkSquare(board, file - 1, rank + 2);
        checkSquare(board, file + 2, rank + 1);
        checkSquare(board, file + 1, rank + 2);

        removePossibleChecks(board);
        return possibleMoves;
    }

    public override float getMobilityValue(int file, int rank)
    {
        List<double[]> list = new List<double[]>();
        list.Add(new double[] { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 });
        list.Add(new double[] { -4.0, -2.0, 0.0, 0.0, 0.0, 0.0, -2.0, -4.0 });
        list.Add(new double[] { -3.0, 0.0, 1.0, 1.5, 1.5, 1.0, 0.0, -3.0 });
        list.Add(new double[] { -3.0, 0.5, 1.5, 2.0, 2.0, 1.5, 0.5, -3.0 });
        list.Add(new double[] { -3.0, 0.0, 1.5, 2.0, 2.0, 1.5, 0.0, -3.0 });
        list.Add(new double[] { -3.0, 0.5, 1.0, 1.5, 1.5, 1.0, 0.5, -3.0 });
        list.Add(new double[] { -4.0, -2.0, 0.0, 0.5, 0.5, 0.0, -2.0, -4.0 });
        list.Add(new double[] { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 });
        if (colour == Colour.Black)
        {
            list.Reverse();
        }
        return (float)list[file][rank];
        //int value = 8;
        //if (file - 2 < 0 || rank - 1 < 0) value--;
        //if (file - 1 < 0 || rank - 2 < 0) value--;
        //if (file + 2 > 7 || rank - 1 < 0) value--;
        //if (file + 1 > 7 || rank - 2 < 0) value--;
        //if (file - 2 < 0 || rank + 1 > 7) value--;
        //if (file - 1 < 0 || rank + 2 > 7) value--;
        //if (file + 2 > 7 || rank + 1 > 7) value--;
        //if (file + 1 > 7 || rank + 2 > 7) value--;
        //return value;
    }
}
