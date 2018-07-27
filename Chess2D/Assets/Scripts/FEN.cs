using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FEN {


    public static string generate(Board board)
    {
        string fen = "";
        int num = 0;
        Piece[,] squares = board.squares;
        for (int i = 7; i >= 0; i--)
        {
            for (int j = 0; j < 8; j++)
            {
                if (squares[j, i] == null)
                {
                    num++;
                }
                else
                {
                    string letter = squares[j, i].GetType().ToString().Substring(0, 1);
                    if (squares[j, i].GetType().Equals(typeof(Knight)))
                    {
                        letter = "N";
                    }
                    if (squares[j, i].colour == Colour.Black)
                    {
                        letter = letter.ToLower();
                    }
                    if (num > 0)
                    {
                        fen += num;
                    }
                    fen += letter;
                    num = 0;
                }
            }
            if (num > 0)
            {
                fen += num;
            }
            fen += "/";
            num = 0;
        }
        fen = fen.Remove(fen.Length - 1);
        if (board.whitesTurn) fen += " w ";
        else fen += " b ";
        if (board.wkCastle) fen += "K";
        if (board.wqCastle) fen += "Q";
        if (board.bkCastle) fen += "k";
        if (board.bqCastle) fen += "q";
        if (board.enPassant == null) fen += " - ";
        else fen += " " + convertPosToString(board.enPassant) + " ";
        fen += board.halfMove + " " + (int)board.fullMove;
        return fen;
    }

    public static string convertPosToString(Vector2Int? pos)
    {
        string letters = "abcdefgh";
        return letters.Substring(pos.Value.x, 1) + (pos.Value.y+1);
    }

    public static Piece[,] readBoard(string fen)
    {
        Piece[,] squares = new Piece[8, 8];
        string board = fen.Split(' ')[0];
        int rank = 7;
        int file = -1;
        foreach (char c in board)
        {
            int number;
            bool isNumber = int.TryParse(c.ToString(), out number);
            file++;
            if (c.Equals('/'))
            {
                rank--;
                file = -1;
            }
            else if (c.Equals('r'))
            {
                squares[file, rank] = new Rook(Colour.Black);
            }
            else if (c.Equals('n'))
            {
                squares[file, rank] = new Knight(Colour.Black);
            }
            else if (c.Equals('b'))
            {
                squares[file, rank] = new Bishop(Colour.Black);
            }
            else if (c.Equals('q'))
            {
                squares[file, rank] = new Queen(Colour.Black);
            }
            else if (c.Equals('k'))
            {
                squares[file, rank] = new King(Colour.Black);
            }
            else if (c.Equals('p'))
            {
                squares[file, rank] = new Pawn(Colour.Black);
            }
            else if (c.Equals('R'))
            {
                squares[file, rank] = new Rook(Colour.White);
            }
            else if (c.Equals('N'))
            {
                squares[file, rank] = new Knight(Colour.White);
            }
            else if (c.Equals('B'))
            {
                squares[file, rank] = new Bishop(Colour.White);
            }
            else if (c.Equals('Q'))
            {
                squares[file, rank] = new Queen(Colour.White);
            }
            else if (c.Equals('K'))
            {
                squares[file, rank] = new King(Colour.White);
            }
            else if (c.Equals('P'))
            {
                squares[file, rank] = new Pawn(Colour.White);
            }
            else if (isNumber)
            {
                if (number > 0 && number < 9)
                {
                    file = file + number - 1;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        return squares;
    }
}
