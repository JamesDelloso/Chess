using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

public class Board {

    private List<Square> squares;
    private string fen;

    public Board(Board oldBoard)
    {

    }

    public Board(string fen)
    {
        squares = new List<Square>();
        //fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        //fen = "8/1k3n2/2BQ4/8/4b3/8/2K3p1/3R4 w - - 0 1";
        //fen = "8/1k3n2/2B5/8/8/8/2K3p1/3R4 w - - 0 1";
        string board = fen.Split(' ')[0];

        string alphabet = "ABCDEFGH";
        int rank = 8;
        int file = 0;
        foreach (char c in board)
        {
            int number;
            bool isNumber = int.TryParse(c.ToString(), out number);
            file++;
            if (c.Equals('/'))
            {
                rank--;
                file = 0;
            }
            else if (c.Equals('r'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Rook(Colour.Black)));
            }
            else if (c.Equals('n'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Knight(Colour.Black)));
            }
            else if (c.Equals('b'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Bishop(Colour.Black)));
            }
            else if (c.Equals('q'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Queen(Colour.Black)));
            }
            else if (c.Equals('k'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new King(Colour.Black)));
            }
            else if (c.Equals('p'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Pawn(Colour.Black)));
            }
            else if (c.Equals('R'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Rook(Colour.White)));
            }
            else if (c.Equals('N'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Knight(Colour.White)));
            }
            else if (c.Equals('B'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Bishop(Colour.White)));
            }
            else if (c.Equals('Q'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Queen(Colour.White)));
            }
            else if (c.Equals('K'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new King(Colour.White)));
            }
            else if (c.Equals('P'))
            {
                squares.Add(new Square(alphabet.Substring(file - 1, 1) + rank, new Pawn(Colour.White)));
            }
            else if (isNumber)
            {
                for (int i=file;i<file+number;i++)
                {
                    squares.Add(new Square(alphabet.Substring(i - 1, 1) + rank, null));
                }
                file = file + number - 1;
            }
        }
    }

    public void setPiece(Piece piece, Square to)
    {
        to.addPiece(piece);
    }

    public void removePiece(Square square)
    {
        square.removePiece();
    }

    public void updateBoard()
    {
        //Square[] s = squares.ToArray();
        //foreach (Square square in s)
        //{
        //    if (square.isEmpty() == false)
        //    {
        //        square.Piece().updatePossibleMoves();
        //    }
        //    square.updateAttackingPieces();
        //}
        //foreach (Square square in s)
        //{
        //    if (square.isEmpty() == false)
        //    {
        //        square.Piece().updatePossibleMoves();
        //    }
        //    square.updateAttackingPieces();
        //}
    }

    public List<Square> getSquaresOnBoard()
    {
        return squares;
    }

    public List<Piece> findSquareWithPiece(string name)
    {
        List<Piece> list = new List<Piece>();
        foreach (Square square in squares)
        {
            if(square.isEmpty() == false && square.getPiece().ToString() == name)
            {
                list.Add(square.getPiece());
            }
        }
        return list;
    }

    public Square findSquareWithPiece(Piece piece)
    {
        Square[] s = squares.ToArray();
        foreach (Square square in s)
        {
            if(square.isEmpty() == false)
            {
                if (square.getPiece().Equals(piece))
                {
                    return square;
                }
            }
        }
        return null;
    }

    public Square getSquare(string name)
    {
        foreach (Square square in squares)
        {
            if (square.ToString() == name)
            {
                return square;
            }
        }
        return null;
    }

    public Square getSquare(int column, int row)
    {
        foreach (Square square in squares)
        {
            if (square.getColumn() == column && square.getRow() == row)
            {
                return square;
            }
        }
        return null;
    }

    public override string ToString()
    {
        //  rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq -1 2
        string fen = "";
        int file = 0;
        int num = 0;
        squares.Reverse<Square>();
        for(int i=0;i<squares.Count;i++)
        {
            file++;
            if(squares[i].isEmpty())
            {
                num++;
            }
            else
            {
                string letter = squares[i].getPiece().GetType().ToString().Substring(0, 1);
                if (squares[i].getPiece().GetType().Equals(typeof(Knight)))
                {
                    letter = "N";
                }
                if (squares[i].getPiece().getColour() == Colour.Black)
                {
                    letter = letter.ToLower();
                }
                if(num > 0)
                {
                    fen += num;
                }
                fen += letter;
                num = 0;
            }
            if(file >= 8)
            {
                if(num > 0)
                {
                    fen += num;
                }
                fen += "/";
                file = 0;
                num = 0;
            }
        }
        fen = fen.Remove(fen.Length - 1);
        return fen;
    }
}
