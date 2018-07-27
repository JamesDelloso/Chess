using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    public Knight(Colour colour) : base(colour)
    {

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

        return possibleMoves;
    }
}
