using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public static class AI
{
    private static string letters = "ABCDEFGH";
    private static bool isWhite = false;

    public static IEnumerator<string[]> getMove(Board board, MovePieceUI ui)
    {
        float time = Time.time;
        yield return null;
        if (board.isWhitesTurn())
        {
            isWhite = true;
        }

        float maxValue = -10000000;
        string from = "";
        string to = "";
        string pieces = board.getPieces();
        for(int i=0;i<pieces.Length;i++)
        {
            if ((board.isWhitesTurn() && char.IsUpper(pieces[i])) || (!board.isWhitesTurn() && char.IsLower(pieces[i])))
            {
                string square = letters[i % 9] + "" + (8 - i / 9);
                foreach (string move in PossibleMoves.search(board, square))
                {
                    string fen = board.fen;
                    board.movePiece(square, move);
                    float value = min(board, 2, -10000, 10000);
                    if (value >= maxValue)
                    {
                        maxValue = value;
                        from = square;
                        to = move;
                    }
                    board.fen = fen;
                    yield return null;
                }
            }
        }
        ui.move(from, to);
        Debug.Log(Time.time - time);
        //return new string[] { from, to };
    }

    private static float min(Board board, int depth, float alpha, float beta)
    {
        if (depth <= 0)
        {
            return board.getValue(isWhite);
        }
        float minValue = 10000000;
        string pieces = board.getPieces();
        for (int i = 0; i < pieces.Length; i++)
        {
            if ((board.isWhitesTurn() && char.IsUpper(pieces[i])) || (!board.isWhitesTurn() && char.IsLower(pieces[i])))
            {
                string square = letters[i % 9] + "" + (8 - i / 9);
                foreach (string move in PossibleMoves.search(board, square))
                {
                    string fen = board.fen;
                    board.movePiece(square, move);
                    minValue = Mathf.Min(minValue, max(board, depth - 1, alpha, beta));
                    beta = Mathf.Min(beta, minValue);
                    if (beta <= alpha)
                    {
                        return minValue;
                    }
                    board.fen = fen;
                }
            }
        }
        return minValue;
    }

    private static float max(Board board, int depth, float alpha, float beta)
    {
        if (depth <= 0)
        {
            return board.getValue(isWhite);
        }
        float maxValue = -10000000;
        string pieces = board.getPieces();
        for (int i = 0; i < pieces.Length; i++)
        {
            if ((board.isWhitesTurn() && char.IsUpper(pieces[i])) || (!board.isWhitesTurn() && char.IsLower(pieces[i])))
            {
                string square = letters[i % 9] + "" + (8 - i / 9);
                foreach (string move in PossibleMoves.search(board, square))
                {
                    string fen = board.fen;
                    board.movePiece(square, move);
                    maxValue = Mathf.Max(maxValue, max(board, depth - 1, alpha, beta));
                    alpha = Mathf.Max(alpha, maxValue);
                    if (beta <= alpha)
                    {
                        return maxValue;
                    }
                    board.fen = fen;
                }
            }
        }
        return maxValue;
    }
}
