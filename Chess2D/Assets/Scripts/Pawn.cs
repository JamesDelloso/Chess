using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {

    private int file;

    public Pawn(Colour colour) : base(colour)
    {

    }

    public override List<Vector2Int> generatePossibleMoves(Board board)
    {
        possibleMoves = new List<Vector2Int>();
        file = board.getPosition(this).x;
        int rank = board.getPosition(this).y;

        int dir = 1;
        if (colour == Colour.Black)
        {
            dir = -1;
        }

        checkPawnSquare(board, file - 1, rank + dir);
        checkPawnSquare(board, file + 1, rank + dir);

        bool check = checkPawnSquare(board, file, rank + dir);

        if (colour == Colour.White && rank == 1 && check == true)
        {
            checkPawnSquare(board, file, rank + dir + dir);
        }
        else if(colour == Colour.Black && rank == 6 && check == true)
        {
            checkPawnSquare(board, file, rank + dir + dir);
        }
        removePossibleChecks(board);
        return possibleMoves;
    }

    private bool checkPawnSquare(Board board, int f, int r)
    {
        if (f < 0 || f > 7 || r < 0 || r > 7)
        {
            return false;
        }
        if (board.getPiece(f, r) == null && f == file)
        {
            possibleMoves.Add(new Vector2Int(f,r));
            return true;
        }
        if (f != file && board.getPiece(f, r) != null && board.getPiece(f, r).colour != colour || (board.enPassant.Equals(new Vector2Int(f, r))))
        {
            possibleMoves.Add(new Vector2Int(f, r));
        }
        return false;
    }
}
