using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI {

    private int count = 0;
    public Colour colour;
    public List<Piece> pieces = new List<Piece>();

    public AI(Colour colour)
    {
        this.colour = colour;
        foreach (Piece p in Game.board.squares)
        {
            if (p != null && p.colour == colour)
            {
                pieces.Add(p);
            }
        }
    }

    public void getMove(Board board, out int a, out int b, out int c, out int d)
    {
        string fen = board.getFen();
        a = 1;
        b = 7;
        c = 2;
        d = 5;
        int maxValue = -1000000;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = board.squares[i, j];
                if (piece != null && piece.colour == Colour.Black)
                {
                    Debug.Log("\n" + piece + "(" + board.getPosition(piece).x + "," + board.getPosition(piece).y + ")");
                    List<Vector2Int> moves = piece.possibleMoves;
                    foreach (Vector2Int pos in moves)
                    {
                        Debug.Log("~" + pos);
                        //string fen = board.getFen();
                        board.movePiece(i, j, pos.x, pos.y);
                        //Piece p = board.squares[pos.x, pos.y];
                        //board.squares[pos.x, pos.y] = piece;
                        //board.squares[i, j] = null;
                        int value = board.getValue(Colour.Black);
                        //int value = max(board, 3);
                        if (value > maxValue)
                        {
                            a = i;
                            b = j;
                            c = pos.x;
                            d = pos.y;
                        }
                        //board.squares[i, j] = piece;
                        //board.squares[pos.x, pos.y] = p;
                        //board = new Board(fen);
                        board.undo();
                    }
                }
            }
        }
        //foreach (Piece piece in board.squares)
        //{
        //    if (piece != null && piece.colour == Colour.Black)
        //    {
        //        Debug.Log("\n" + piece + "(" + board.getPosition(piece).x + "," + board.getPosition(piece).y + ")");
        //        foreach (Vector2Int pos in piece.possibleMoves)
        //        {
        //            Debug.Log("~" + pos);
        //            int x = board.getPosition(piece).x;
        //            int y = board.getPosition(piece).y;
        //            Piece piece1 = board.squares[x, y];
        //            Piece piece2 = board.squares[pos.x, pos.y];
        //            board.movePiece(board.getPosition(piece).x, board.getPosition(piece).y, pos.x, pos.y);
        //            //board.squares[pos.x, pos.y] = piece;
        //            //board.squares[x, y] = null;
        //            int value = max(board, 3);
        //            //int value = board.getValue(Colour.Black);
        //            if (value > maxValue)
        //            {
        //                a = x;
        //                b = y;
        //                c = pos.x;
        //                d = pos.y;
        //            }
        //            //board.movePiece(pos.x, pos.y, x, y);
        //            //board.squares[x, y] = piece;
        //            //board.squares[pos.x, pos.y] = piece2;
        //            board.undo();
        //        }
        //    }
        //}
    }

    private int min(Board board, int depth)
    {
        if(depth == 0)
        {
            return board.getValue(Colour.Black);
        }
        int minValue = 1000000;
        //foreach (Piece piece in board.squares)
        //{
        //    if (piece != null && piece.colour == Colour.White)
        //    {
        //        //Debug.Log("\n" + piece + "(" + board.getPosition(piece).x + "," + board.getPosition(piece).y + ")");
        //        foreach (Vector2Int pos in piece.possibleMoves)
        //        {
        //            //Debug.Log("~" + pos);
        //            int x = board.getPosition(piece).x;
        //            int y = board.getPosition(piece).y;
        //            board.movePiece(board.getPosition(piece).x, board.getPosition(piece).y, pos.x, pos.y);
        //            int value = max(board, depth - 1);
        //            if (value < minValue)
        //            {
        //                minValue = value;
        //            }
        //            board.undo();
        //        }
        //    }
        //}
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = board.squares[i, j];
                if (piece != null && piece.colour == Colour.White)
                {
                    //Debug.Log("\n" + piece + "(" + board.getPosition(piece).x + "," + board.getPosition(piece).y + ")");
                    List<Vector2Int> moves = piece.possibleMoves;
                    foreach (Vector2Int pos in moves)
                    {
                        //Debug.Log("~" + pos);
                        //board.movePiece(i, j, pos.x, pos.y);
                        Piece p = board.squares[pos.x, pos.y];
                        board.squares[pos.x, pos.y] = piece;
                        board.squares[i, j] = null;
                        //int value = board.getValue(Colour.Black);
                        int value = max(board, depth - 1);
                        if (value < minValue)
                        {
                            minValue = value;
                        }
                        //board.undo();
                        board.squares[i, j] = piece;
                        board.squares[pos.x, pos.y] = p;
                    }
                }
            }
        }
        return minValue;
    }

    private int max(Board board, int depth)
    {
        if (depth == 0)
        {
            return board.getValue(Colour.Black);
        }
        int maxValue = -1000000;
        //foreach (Piece piece in board.squares)
        //{
        //    if (piece != null && piece.colour == Colour.White)
        //    {
        //        //Debug.Log("\n" + piece + "(" + board.getPosition(piece).x + "," + board.getPosition(piece).y + ")");
        //        foreach (Vector2Int pos in piece.possibleMoves)
        //        {
        //            //Debug.Log("~" + pos);
        //            int x = board.getPosition(piece).x;
        //            int y = board.getPosition(piece).y;
        //            board.movePiece(board.getPosition(piece).x, board.getPosition(piece).y, pos.x, pos.y);
        //            int value = min(board, depth - 1);
        //            if (value > maxValue)
        //            {
        //                maxValue = value;
        //            }
        //            board.undo();
        //        }
        //    }
        //}
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Piece piece = board.squares[i, j];
                if (piece != null && piece.colour == Colour.Black)
                {
                    //Debug.Log("\n" + piece + "(" + board.getPosition(piece).x + "," + board.getPosition(piece).y + ")");
                    List<Vector2Int> moves = piece.possibleMoves;
                    foreach (Vector2Int pos in moves)
                    {
                        //Debug.Log("~" + pos);
                        //board.movePiece(i, j, pos.x, pos.y);
                        Piece p = board.squares[pos.x, pos.y];
                        board.squares[pos.x, pos.y] = piece;
                        board.squares[i, j] = null;
                        //int value = board.getValue(Colour.Black);
                        int value = min(board, depth - 1);
                        if (value > maxValue)
                        {
                            maxValue = value;
                        }
                        //board.undo();
                        board.squares[i, j] = piece;
                        board.squares[pos.x, pos.y] = p;
                    }
                }
            }
        }
        return maxValue;
    }
}
