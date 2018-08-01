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

        if (colour == Colour.White && board.wqCastle == true && board.squares[1,0] == null && board.squares[2, 0] == null && board.squares[3, 0] == null && board.squares[0,0] != null && board.squares[0,0].ToString() == "White Rook")
        {
            possibleMoves.Add(new Vector2Int(2, 0));
        }
        if (colour == Colour.White && board.wkCastle == true && board.squares[5, 0] == null && board.squares[6, 0] == null && board.squares[7, 0] != null && board.squares[7, 0].ToString() == "White Rook")
        {
            possibleMoves.Add(new Vector2Int(6, 0));
        }
        else if (colour == Colour.Black && board.bqCastle == true && board.squares[1, 7] == null && board.squares[2, 7] == null && board.squares[3, 7] == null && board.squares[0, 7] != null && board.squares[0, 7].ToString() == "Black Rook")
        {
            possibleMoves.Add(new Vector2Int(2, 7));
        }
        if (colour == Colour.Black && board.bkCastle == true && board.squares[5, 7] == null && board.squares[6, 7] == null && board.squares[7, 7] != null && board.squares[7, 7].ToString() == "Black Rook")
        {
            possibleMoves.Add(new Vector2Int(6, 7));
        }
        removePossibleChecks(board);
        return possibleMoves;
    }

    public bool isCheck(Board board)
    {
        int file = board.getPosition(this).x;
        int rank = board.getPosition(this).y;

        for (int i = file - 1; i >= 0; i--)
        {
            if (checkForPiece(board, i, rank) == true)
            {
                if (board.getPiece(i, rank).colour != colour && (board.getPiece(i, rank).GetType() == typeof(Queen) || board.getPiece(i, rank).GetType() == typeof(Rook)))
                {
                    return true;
                }
                break;
            }
        }
        for (int i = file + 1; i <= 7; i++)
        {
            if (checkForPiece(board, i, rank) == true)
            {
                if (board.getPiece(i, rank).colour != colour && (board.getPiece(i, rank).GetType() == typeof(Queen) || board.getPiece(i, rank).GetType() == typeof(Rook)))
                {
                    return true;
                }
                break;
            }
        }
        for (int i = rank - 1; i >= 0; i--)
        {
            if (checkForPiece(board, file, i) == true)
            {
                if (board.getPiece(file, i).colour != colour && (board.getPiece(file, i).GetType() == typeof(Queen) || board.getPiece(file, i).GetType() == typeof(Rook)))
                {
                    return true;
                }
                break;
            }
        }
        for (int i = rank + 1; i <= 7; i++)
        {
            if (checkForPiece(board, file, i) == true)
            {
                if (board.getPiece(file, i).colour != colour && (board.getPiece(file, i).GetType() == typeof(Queen) || board.getPiece(file, i).GetType() == typeof(Rook)))
                {
                    return true;
                }
                break;
            }
        }
        int r = rank - 1;
        for (int f = file - 1; f >= 0 && r >= 0; f--, r--)
        {
            if (checkForPiece(board, f, r) == true)
            {
                if (board.getPiece(f, r).colour != colour && (board.getPiece(f, r).GetType() == typeof(Queen) || board.getPiece(f, r).GetType() == typeof(Bishop)))
                {
                    return true;
                }
                break;
            }
        }
        r = rank + 1;
        for (int f = file + 1; f <= 7 && r <= 7; f++, r++)
        {
            if (checkForPiece(board, f, r) == true)
            {
                if (board.getPiece(f, r).colour != colour && (board.getPiece(f, r).GetType() == typeof(Queen) || board.getPiece(f, r).GetType() == typeof(Bishop)))
                {
                    return true;
                }
                break;
            }
        }
        r = rank + 1;
        for (int f = file - 1; f >= 0 && r <= 7; f--, r++)
        {
            if (checkForPiece(board, f, r) == true)
            {
                if (board.getPiece(f, r).colour != colour && (board.getPiece(f, r).GetType() == typeof(Queen) || board.getPiece(f, r).GetType() == typeof(Bishop)))
                {
                    return true;
                }
                break;
            }
        }
        r = rank - 1;
        for (int f = file + 1; f <= 7 && r >= 0; f++, r--)
        {
            if (checkForPiece(board, f, r) == true)
            {
                if (board.getPiece(f, r).colour != colour && (board.getPiece(f, r).GetType() == typeof(Queen) || board.getPiece(f, r).GetType() == typeof(Bishop)))
                {
                    return true;
                }
                break;
            }
        }
        int dir = 1;
        if (colour == Colour.Black)
        {
            dir = -1;
        }
        if (checkForPiece(board, file - 1, rank + dir) == true && board.getPiece(file - 1, rank + dir).colour != colour && board.getPiece(file - 1, rank + dir).GetType() == typeof(Pawn))
        {
            return true;
        }
        if (checkForPiece(board, file + 1, rank + dir) == true && board.getPiece(file + 1, rank + dir).colour != colour && board.getPiece(file + 1, rank + dir).GetType() == typeof(Pawn))
        {
            return true;
        }
        if (checkForPiece(board, file - 2, rank - 1) == true)
        {
            if (board.getPiece(file - 2, rank - 1).colour != colour && board.getPiece(file - 2, rank - 1).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        if (checkForPiece(board, file - 1, rank - 2) == true)
        {
            if (board.getPiece(file - 1, rank - 2).colour != colour && board.getPiece(file - 1, rank - 2).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        if (checkForPiece(board, file + 2, rank - 1) == true)
        {
            if (board.getPiece(file + 2, rank - 1).colour != colour && board.getPiece(file + 2, rank - 1).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        if (checkForPiece(board, file + 1, rank - 2) == true)
        {
            if (board.getPiece(file + 1, rank - 2).colour != colour && board.getPiece(file + 1, rank - 2).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        if (checkForPiece(board, file - 2, rank + 1) == true)
        {
            if (board.getPiece(file - 2, rank + 1).colour != colour && board.getPiece(file - 2, rank + 1).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        if (checkForPiece(board, file - 1, rank + 2) == true)
        {
            if (board.getPiece(file - 1, rank + 2).colour != colour && board.getPiece(file - 1, rank + 2).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        if (checkForPiece(board, file + 2, rank + 1) == true)
        {
            if (board.getPiece(file + 2, rank + 1).colour != colour && board.getPiece(file + 2, rank + 1).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        if (checkForPiece(board, file + 1, rank + 2) == true)
        {
            if (board.getPiece(file + 1, rank + 2).colour != colour && board.getPiece(file + 1, rank + 2).GetType() == typeof(Knight))
            {
                return true;
            }
        }
        return false;
    }

    protected bool checkForPiece(Board board, int file, int rank)
    {
        if (file < 0 || file > 7 || rank < 0 || rank > 7)
        {
            return false;
        }
        if (board.getPiece(file, rank) == null)
        {
            return false;
        }
        else if (board.getPiece(file, rank) != null)
        {
            return true;
        }
        return false;
    }
}
