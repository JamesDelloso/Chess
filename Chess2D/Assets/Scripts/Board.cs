using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

[Serializable]
public class Board {

    //private string[] state;
    //[NonSerialized]
    //[SerializeField]
    private List<Square> squares;
    private Piece king;
    //private List<Piece> pieces;

    public Board(Board oldBoard)
    {
        squares = new List<Square>();
        for(int i=0;i<64;i++)
        {
            if(oldBoard.squares[i].isEmpty())
            {
                squares.Add(new Square(oldBoard.squares[i].Name(), this));
            }
            else
            {
                squares.Add(new Square(oldBoard.squares[i].Name(), oldBoard.squares[i].Piece().deepCopy(this), this));
            }
        }
        king = getSquare("E1").Piece();
        updateBoard();
    }

    public Board(Colour pc, Colour cc)
    {
        //Debug.Log(c);
        //state = new string[] {  "r", "n", "b", "q", "k", "b", "n", "r",
        //                        "p", "p", "p", "p", "p", "p", "p", "p",
        //                        " ", " ", " ", " ", " ", " ", " ", " ",
        //                        " ", " ", " ", " ", " ", " ", " ", " ",
        //                        " ", " ", " ", " ", " ", " ", " ", " ",
        //                        " ", " ", " ", " ", " ", " ", " ", " ",
        //                        "P", "P", "P", "P", "P", "P", "P", "P",
        //                        "R", "N", "B", "Q", "K", "B", "N", "R"};
        squares = new List<Square>();
        //Colour pc = Player.getColour();
        //Colour cc = Computer.getColour();
        squares.Add(new Square("A1", new Rook(pc, this), this));
        squares.Add(new Square("B1", new Knight(pc, this), this));
        squares.Add(new Square("C1", new Bishop(pc, this), this));
        squares.Add(new Square("D1", new Queen(pc, this), this));
        squares.Add(new Square("E1", new King(pc, this), this));
        squares.Add(new Square("F1", new Bishop(pc, this), this));
        squares.Add(new Square("G1", new Knight(pc, this), this));
        squares.Add(new Square("H1", new Rook(pc, this), this));
        squares.Add(new Square("A2", new Pawn(pc, this), this));
        squares.Add(new Square("B2", new Pawn(pc, this), this));
        squares.Add(new Square("C2", new Pawn(pc, this), this));
        squares.Add(new Square("D2", new Pawn(pc, this), this));
        squares.Add(new Square("E2", new Pawn(pc, this), this));
        squares.Add(new Square("F2", new Pawn(pc, this), this));
        squares.Add(new Square("G2", new Pawn(pc, this), this));
        squares.Add(new Square("H2", new Pawn(pc, this), this));
        squares.Add(new Square("A3", this));
        squares.Add(new Square("B3", this));
        squares.Add(new Square("C3", this));
        squares.Add(new Square("D3", this));
        squares.Add(new Square("E3", this));
        squares.Add(new Square("F3", this));
        squares.Add(new Square("G3", this));
        squares.Add(new Square("H3", this));
        squares.Add(new Square("A4", this));
        squares.Add(new Square("B4", this));
        squares.Add(new Square("C4", this));
        squares.Add(new Square("D4", this));
        squares.Add(new Square("E4", this));
        squares.Add(new Square("F4", this));
        squares.Add(new Square("G4", this));
        squares.Add(new Square("H4", this));
        squares.Add(new Square("A5", this));
        squares.Add(new Square("B5", this));
        squares.Add(new Square("C5", this));
        squares.Add(new Square("D5", this));
        squares.Add(new Square("E5", this));
        squares.Add(new Square("F5", this));
        squares.Add(new Square("G5", this));
        squares.Add(new Square("H5", this));
        squares.Add(new Square("A6", this));
        squares.Add(new Square("B6", this));
        squares.Add(new Square("C6", this));
        squares.Add(new Square("D6", this));
        squares.Add(new Square("E6", this));
        squares.Add(new Square("F6", this));
        squares.Add(new Square("G6", this));
        squares.Add(new Square("H6", this));
        squares.Add(new Square("A7", new Pawn(cc, this), this));
        squares.Add(new Square("B7", new Pawn(cc, this), this));
        squares.Add(new Square("C7", new Pawn(cc, this), this));
        squares.Add(new Square("D7", new Pawn(cc, this), this));
        squares.Add(new Square("E7", new Pawn(cc, this), this));
        squares.Add(new Square("F7", new Pawn(cc, this), this));
        squares.Add(new Square("G7", new Pawn(cc, this), this));
        squares.Add(new Square("H7", new Pawn(cc, this), this));
        squares.Add(new Square("A8", new Rook(cc, this), this));
        squares.Add(new Square("B8", new Knight(cc, this), this));
        squares.Add(new Square("C8", new Bishop(cc, this), this));
        squares.Add(new Square("D8", new Queen(cc, this), this));
        squares.Add(new Square("E8", new King(cc, this), this));
        squares.Add(new Square("F8", new Bishop(cc, this), this));
        squares.Add(new Square("G8", new Knight(cc, this), this));
        squares.Add(new Square("H8", new Rook(cc, this), this));
        king = getSquare("E1").Piece();
    }

    public void movePiece(Piece piece, Square to)
    {
        //Debug.Log(findSquareWithPiece(piece)+": "+piece);
        if (piece.GetType().Equals(typeof(King)) && (piece as King).canCastle(to)) //Castling
        {
            if (to.Equals(getSquare("C1")))
            {
                Debug.Log("Castling Queen Side");
                getSquare("D1").addPiece(getSquare("A1").Piece());
                getSquare("A1").removePiece();
            }
            else
            {
                Debug.Log("Castling King Side");
                getSquare("F1").addPiece(getSquare("H1").Piece());
                getSquare("H1").removePiece();
            }
        }
        else if(piece.GetType().Equals(typeof(Rook)))
        {
            (piece as Rook).move();
        }
        Debug.Log(piece+",,"+findSquareWithPiece(piece));
        findSquareWithPiece(piece).removePiece();
        to.addPiece(piece);
        //updateBoard();
    }

    public void updateBoard()
    {
        Square[] s = squares.ToArray();
        foreach (Square square in s)
        {
            if (square.isEmpty() == false)
            {
                square.Piece().updatePossibleMoves();
            }
            square.updateAttackingPieces();
        }
        foreach (Square square in s)
        {
            if (square.isEmpty() == false)
            {
                square.Piece().updatePossibleMoves();
            }
            square.updateAttackingPieces();
        }
    }

    public bool isCheck()
    {
        foreach (Piece piece in findSquareWithPiece(king).getAttackingPieces(king.getColour()))
        {
            if (piece.getColour() != king.getColour())
            {
                Debug.Log("Check");
                return true;
            }
        }
        return false;
    }

    public List<Square> getSquaresOnBoard()
    {
        return squares;
    }

    public Square findSquareWithPiece(Piece piece)
    {
        Square[] s = squares.ToArray();
        foreach (Square square in s)
        {
            if(square.isEmpty() == false)
            {
                if (square.Piece().Equals(piece))
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
            if (square.Name() == name)
            {
                return square;
            }
        }
        return null;
    }

    public bool isRowBlocked(Square from, Square to)
    {
        int fromColumn = from.Column();
        int toColumn = to.Column();
        if (from.Equals(to))
        {
            return true;
        }
        foreach (Square sq in squares)
        {
            int colNum = sq.Column();
            if (sq.Row() == from.Row() && (colNum > fromColumn && colNum < toColumn || colNum < fromColumn && colNum > toColumn))
            {
                if (sq.isEmpty() == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool isColumnBlocked(Square from, Square to)
    {
        int fromRow = from.Row();
        int toRow = to.Row();
        if(from.Equals(to))
        {
            return true;
        }
        foreach (Square sq in squares)
        {
            int rowNum = sq.Row();
            if (sq.Column() == from.Column() && (rowNum > fromRow && rowNum < toRow || rowNum < fromRow && rowNum > toRow))
            {
                if (sq.isEmpty() == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool isDiagonalBlocked(Square from, Square to)
    {
        if (from.Equals(to))
        {
            return true;
        }
        foreach (Square sq in squares)
        {
            int colNum = sq.Column();
            int rowNum = sq.Row();

            int fromColumn = from.Column();
            int toColumn = to.Column();

            int fromRow = from.Row();
            int toRow = to.Row();

            int colDiff = colNum - fromColumn;
            int rowDiff = rowNum - fromRow;
            if (System.Math.Abs(colDiff) == System.Math.Abs(rowDiff) && (colNum > fromColumn && colNum < toColumn || colNum < fromColumn && colNum > toColumn) && (rowNum > fromRow && rowNum < toRow || rowNum < fromRow && rowNum > toRow))
            {
                if (sq.isEmpty() == false)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public override string ToString()
    {
        string fen = "";
        int count = 0;
        foreach(Square sq in squares)
        {
            if(sq.isEmpty())
            {
                count++;
            }
            else
            {
                if(count != 0)
                {
                    fen = fen + count;
                    count = 0;
                }
                string letter = sq.Piece().GetType().ToString().Substring(0, 1);
                if (sq.Piece().GetType().Equals(typeof(Knight)))
                {
                    letter = "N";
                }
                if(sq.Piece().getColour() == Colour.Black)
                {
                    letter = letter.ToLower();
                }
                fen = fen + letter;
            }
            if(sq.Column() == 8)
            {
                if (count != 0)
                {
                    fen = fen + count;
                }
                fen = fen + "/";
                count = 0;
            }
        }
        return fen;
    }

    public object Clone()
    {
        return new Board(this);
    }
}
