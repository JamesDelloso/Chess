using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece {
    
    public Bishop(Colour colour) : base(colour)
    {

    }

    public override List<Vector2Int> generatePossibleMoves(Board board)
    {
        possibleMoves = new List<Vector2Int>();
        int file = board.getPosition(this).x;
        int rank = board.getPosition(this).y;

        int r = rank - 1;
        for (int f = file - 1; f >= 0 && r >= 0; f--, r--)
        {
            if (checkSquare(board, f, r) == false)
            {
                break;
            }
        }
        r = rank + 1;
        for (int f = file + 1; f <= 7 && r <= 7; f++, r++)
        {
            if (checkSquare(board, f, r) == false)
            {
                break;
            }
        }
        r = rank + 1;
        for (int f = file - 1; f >= 0 && r <= 7; f--, r++)
        {
            if (checkSquare(board, f, r) == false)
            {
                break;
            }
        }
        r = rank - 1;
        for (int f = file + 1; f <= 7 && r >= 0; f++, r--)
        {
            if (checkSquare(board, f, r) == false)
            {
                break;
            }
        }
        removePossibleChecks(board);
        return possibleMoves;
    }
}
