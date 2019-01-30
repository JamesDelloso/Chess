using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class AI {

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

    public IEnumerator getMove(Board board)
    {
        yield return null;
        Profiler.BeginSample("AI move search");
        //Debug.Log("==================================================================================================");
        //Debug.Log(board.getFen());
        Board bo = new Board(board.getFen());
        List<Vector4> moves = new List<Vector4>();
        //a = 1;
        //b = 7;
        //c = 2;
        //d = 5;
        float maxValue = -10000000;
        for(int i=0;i<8;i++)
        {
            for(int j=0;j<8;j++)
            {
                if(bo.squares[i,j] != null && bo.squares[i, j].colour == Colour.Black)
                {
                    foreach(Vector2Int pos in bo.squares[i, j].possibleMoves)
                    {
                        //Debug.Log(board.getFen());
                        string fen = bo.getFen();
                        Piece piece1 = bo.getPiece(i, j);
                        Piece piece2 = bo.getPiece(pos.x, pos.y);
                        bo.movePiece(i, j, pos.x, pos.y);
                        //int value = board.getValue(Colour.Black);
                        float value = min(bo, 2, -10000, 10000);
                        //Debug.Log(value);
                        if (value >= maxValue)
                        {
                            if(value > maxValue)
                            {
                                moves = new List<Vector4>();
                            }
                            maxValue = value;
                            moves.Add(new Vector4(i, j, pos.x, pos.y));
                            //a = i;
                            //b = j;
                            //c = pos.x;
                            //d = pos.y;
                        }
                        //board.undo(fen, piece1, piece2, new Vector2Int(i, j), new Vector2Int(pos.x, pos.y));
                        bo = new Board(fen);
                        yield return null;
                        //Debug.Log(board.getFen());
                    }
                }
            }
        }
        Vector4 move = moves[Random.Range(0, moves.Count - 1)];
        //Debug.Log(move);
        int a = (int)move.x;
        int b = (int)move.y;
        int c = (int)move.z;
        int d = (int)move.w;
        Debug.Log(move.x + "," + move.y + "," + move.z + "," + move.w);
        Game.currentPlayer.move(a, b, c, d);
        Game.board.movePiece(a, b, c, d);
        Game.currentPlayer.seeIfCheckOrStaleMate();
        Game.board.history.Add(Game.board.getFen());
        Profiler.EndSample();
    }

    private float min(Board board, int depth, float alpha, float beta)
    {
        if (depth <= 0)
        {
            return board.getValue(Colour.Black);
        }
        float minValue = 10000000;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.squares[i, j] != null && board.squares[i, j].colour == Colour.White)
                {
                    board.squares[i, j].generatePossibleMoves(board);
                    foreach (Vector2Int pos in board.squares[i, j].possibleMoves)
                    {
                        string fen = board.getFen();
                        Piece piece1 = board.getPiece(i, j);
                        Piece piece2 = board.getPiece(pos.x, pos.y);
                        board.movePiece(i, j, pos.x, pos.y);
                        minValue = Mathf.Min(minValue, max(board, depth - 1, alpha, beta));
                        beta = Mathf.Min(beta, minValue);
                        if(beta <= alpha)
                        {
                            return minValue;
                        }
                        //int value = max(board, depth - 1);
                        //Debug.Log(board.getFen() + " = " + value);
                        //if (value < minValue)
                        //{
                        //    minValue = value;
                        //}
                        //board.undo(fen, piece1, piece2, new Vector2Int(i, j), new Vector2Int(pos.x, pos.y));
                        board = new Board(fen);
                        //Debug.Log(board.getFen());
                    }
                }
            }
        }
        return minValue;
    }

    private float max(Board board, int depth, float alpha, float beta)
    {
        if (depth <= 0)
        {
            return board.getValue(Colour.Black);
        }
        float maxValue = -10000000;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.squares[i, j] != null && board.squares[i, j].colour == Colour.Black)
                {
                    foreach (Vector2Int pos in board.squares[i, j].possibleMoves)
                    {
                        string fen = board.getFen();
                        Piece piece1 = board.getPiece(i, j);
                        Piece piece2 = board.getPiece(pos.x, pos.y);
                        //Debug.Log(board.getFen());
                        board.movePiece(i, j, pos.x, pos.y);
                        maxValue = Mathf.Max(maxValue, min(board, depth - 1, alpha, beta));
                        alpha = Mathf.Max(alpha, maxValue);
                        if(beta <= alpha)
                        {
                            return maxValue;
                        }
                        //int value = min(board, depth - 1);
                        ////Debug.Log(board.getFen() + " = " + value);
                        //if (value > maxValue)
                        //{
                        //    maxValue = value;
                        //}
                        //board.undo(fen, piece1, piece2, new Vector2Int(i, j), new Vector2Int(pos.x, pos.y));
                        board = new Board(fen);
                        //Debug.Log(board.getFen());
                    }
                }
            }
        }
        return maxValue;
    }
}
