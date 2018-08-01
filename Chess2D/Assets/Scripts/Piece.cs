using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece {

    public Colour colour;
    public List<Vector2Int> possibleMoves;

    public Piece(Colour colour)
    {
        this.colour = colour;
    }

    public abstract List<Vector2Int> generatePossibleMoves(Board board);

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

    public override string ToString()
    {
        return colour + " " + base.ToString();
    }
}
