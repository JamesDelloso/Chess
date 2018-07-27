using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PGN {

    public static string convert(Board board, Piece piece, int fromX, int toX, int toY)
    {
        string pgn = "";
        bool castled = false;
        //if (piece.getPlayer() == player1)
        //{
        //    pgn += (int)fullMove + ".  ";
        //}
        if (piece.GetType().Equals(typeof(Rook)))
        {
            pgn += "R";
        }
        else if (piece.GetType().Equals(typeof(Bishop)))
        {
            pgn += "B";
        }
        else if (piece.GetType().Equals(typeof(Knight)))
        {
            pgn += "N";
        }
        else if (piece.GetType().Equals(typeof(Queen)))
        {
            pgn += "Q";
        }
        else if (piece.GetType().Equals(typeof(King)))
        {
            if (fromX == 4 && toX == 3)
            {
                pgn += "O-O-O";
                castled = true;
            }
            else if (fromX == 4 && toX == 7)
            {
                pgn += "O-O";
                castled = true;
            }
            else
            {
                pgn += "K";
            }
        }
        if (board.squares[toX, toY] != null)
        {
            pgn += "x";
        }
        if (castled == false)
        {
            pgn += FEN.convertPosToString(new Vector2Int(toX, toY));
        }
        return pgn;
    }
}
