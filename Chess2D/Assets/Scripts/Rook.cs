﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece {

    public Rook(Colour colour) : base(colour)
    {

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
        return possibleMoves;
    }
}
