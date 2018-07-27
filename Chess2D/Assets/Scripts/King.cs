using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {

    public King(Colour colour) : base(colour)
    {

    }

    public override List<Vector2Int> generatePossibleMoves(Board board)
    {
        possibleMoves = new List<Vector2Int>();
        int file = board.getPosition(this).x;
        int rank = board.getPosition(this).y;

        checkSquare(board, file - 1, rank + 1);
        checkSquare(board, file, rank + 1);
        checkSquare(board, file + 1, rank + 1);
        checkSquare(board, file - 1, rank);
        checkSquare(board, file, rank);
        checkSquare(board, file + 1, rank);
        checkSquare(board, file - 1, rank - 1);
        checkSquare(board, file, rank - 1);
        checkSquare(board, file + 1, rank - 1);

        if (colour == Colour.White && board.wqCastle == true && board.squares[1,0] == null && board.squares[2, 0] == null && board.squares[3, 0] == null)
        {
            possibleMoves.Add(new Vector2Int(2, 0));
        }
        if (colour == Colour.White && board.wkCastle == true && board.squares[5, 0] == null && board.squares[6, 0] == null)
        {
            possibleMoves.Add(new Vector2Int(6, 0));
        }
        else if (colour == Colour.Black && board.bqCastle == true && board.squares[1, 7] == null && board.squares[2, 7] == null && board.squares[3, 7] == null)
        {
            possibleMoves.Add(new Vector2Int(2, 7));
        }
        if (colour == Colour.Black && board.bkCastle == true && board.squares[5, 7] == null && board.squares[6, 7] == null)
        {
            possibleMoves.Add(new Vector2Int(6, 7));
        }

        return possibleMoves;
    }

    public bool isCheck(Board board)
    {
        foreach(Piece p in board.squares)
        {
            if (p != null)
            {
                if (p != null && p.possibleMoves.Contains(board.getPosition(this)))
                {
                    Debug.Log("Check!");
                    return true;
                }
            }
        }
        return false;
    }
}
