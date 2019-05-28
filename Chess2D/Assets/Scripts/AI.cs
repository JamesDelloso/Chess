using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public static class AI
{
    private static Colour aiColour = Colour.Black;
    private static Colour playerColour = Colour.White;

    private static bool nextMoveFound = false;
    private static List<string> boardPos = new List<string>();
    private static List<string> boardPosNextMove = new List<string>();

    public static IEnumerator makeMove(Board currentBoard, MovePieceUI ui)
    {
        float time = Time.time;
        yield return null;
        //Profiler.BeginSample("AI move search");
        if (currentBoard.whitesTurn == true)
        {
            aiColour = Colour.White;
            playerColour = Colour.Black;
        }
        Board board = new Board(currentBoard.getFen());
        int x1 = 0, y1 = 0, x2 = 0, y2 = 0;
        Vector4 move = new Vector4();
        float maxValue = -10000000;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.squares[i, j] != null && board.squares[i, j].colour == aiColour)
                {
                    foreach (Vector2Int pos in board.squares[i, j].possibleMoves)
                    {
                        string fen = board.getFen();
                        Piece piece1 = board.getPiece(i, j);
                        Piece piece2 = board.getPiece(pos.x, pos.y);
                        board.movePiece(i, j, pos.x, pos.y);
                        //float value = board.getValue(Colour.Black);
                        float value = min(board, 2, -10000, 10000);
                        if (value >= maxValue)
                        {
                            maxValue = value;
                            x1 = i;
                            y1 = j;
                            x2 = pos.x;
                            y2 = pos.y;
                        }
                        board = new Board(fen);
                        yield return null;
                    }
                }
            }
        }
        Debug.Log(x1 + "," + y1 + "-" + x2 + "," + y2);
        Debug.Log(Time.time - time);
        ui.move(x1, y1, x2, y2);
        //Profiler.EndSample();
    }

    private static float min(Board board, int depth, float alpha, float beta)
    {
        if (depth <= 0)
        {
            return board.getValue(aiColour);
        }
        float minValue = 10000000;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.squares[i, j] != null && board.squares[i, j].colour == playerColour)
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
                        if (beta <= alpha)
                        {
                            return minValue;
                        }
                        board = new Board(fen);
                    }
                }
            }
        }
        return minValue;
    }

    private static float max(Board board, int depth, float alpha, float beta)
    {
        if (depth <= 0)
        {
            return board.getValue(aiColour);
        }
        float maxValue = -10000000;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.squares[i, j] != null && board.squares[i, j].colour == aiColour)
                {
                    foreach (Vector2Int pos in board.squares[i, j].possibleMoves)
                    {
                        string fen = board.getFen();
                        Piece piece1 = board.getPiece(i, j);
                        Piece piece2 = board.getPiece(pos.x, pos.y);
                        board.movePiece(i, j, pos.x, pos.y);
                        maxValue = Mathf.Max(maxValue, min(board, depth - 1, alpha, beta));
                        alpha = Mathf.Max(alpha, maxValue);
                        if (beta <= alpha)
                        {
                            return maxValue;
                        }
                        board = new Board(fen);
                    }
                }
            }
        }
        return maxValue;
    }

    public static IEnumerator prepareNextMove()
    {
        yield return null;
        Board board = Game.board;
        float maxValue = -10000000;
        string nextMoveFen = "";
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (board.squares[i, j] != null && board.squares[i, j].colour == playerColour)
                {
                    foreach (Vector2Int pos in board.squares[i, j].possibleMoves)
                    {
                        string fen = board.getFen();
                        boardPos.Add(fen);
                        board.movePiece(i, j, pos.x, pos.y);
                        float value = min(board, 2, -10000, 10000);
                        if (value > maxValue)
                        {
                            maxValue = value;
                            nextMoveFen = board.getFen();
                        }
                        board = new Board(fen);
                    }
                    yield return null;
                    boardPosNextMove.Add(nextMoveFen);
                    maxValue = -10000000;
                }
            }
        }
        yield return null;
    }
}
