using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece {

    public Colour colour;
    public List<Vector2Int> possibleMoves;
    public int value;

    public Piece(Colour colour)
    {
        this.colour = colour;
    }

    public abstract List<Vector2Int> generatePossibleMoves(Board board);

    public abstract int getMobilityValue(int file, int rank);

    protected bool checkSquare(Board board, int file, int rank)
    {
        if (file < 0 || file > 7 || rank < 0 || rank > 7)
        {
            return false;
        }
        if (board.getPiece(file, rank) == null)
        {
            possibleMoves.Add(new Vector2Int(file, rank));
            return true;
        }
        else if (board.getPiece(file, rank).colour != colour)
        {
            possibleMoves.Add(new Vector2Int(file, rank));
        }
        return false;
    }

    protected void removePossibleChecks(Board board)
    {
        List<Vector2Int> toRemove = new List<Vector2Int>();
        for (int i = 0; i < possibleMoves.Count; i++)
        {
            int tempX = board.getPosition(this).x;
            int tempY = board.getPosition(this).y;
            Piece p = board.squares[possibleMoves[i].x, possibleMoves[i].y];
            board.squares[possibleMoves[i].x, possibleMoves[i].y] = this;
            board.squares[tempX, tempY] = null;
            bool isCheck = false;
            if (colour == Colour.White)
            {
                if(board.wKing != null)
                isCheck = board.wKing.isCheck(board);
            }
            else
            {
                if(board.bKing != null)
                isCheck = board.bKing.isCheck(board);
            }
            board.squares[possibleMoves[i].x, possibleMoves[i].y] = p;
            board.squares[tempX, tempY] = this;
            if (isCheck == true)
            {
                toRemove.Add(possibleMoves[i]);
            }
        }
        foreach (Vector2Int v in toRemove)
        {
            possibleMoves.Remove(v);
        }
    }

    public override string ToString()
    {
        return colour + " " + base.ToString();
    }
}
