using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {
    private string name;
    private int column;
    private int row;
    private Piece piece;
    public bool enPassant = false;
    public Piece enPassantPiece = null;
    private Board board;

    public Square(int column, int row, Piece piece, Board board)
    {
        this.piece = piece;
        string letters = "ABCDEFGH";
        name = letters[column-1]+row.ToString();
        this.column = column;
        this.row = row;
        this.board = board;
    }

    public void addPiece(Piece piece)
    {
        this.piece = piece;
    }

    public void removePiece()
    {
        piece = null;
    }

    public void promotePawn(Piece piece, Piece promotedPiece)
    {
        Square from = board.getSquare(piece);
        from.piece = null;
        this.piece = promotedPiece;
    }

    public int getColumn()
    {
        return column;
    }

    public int getRow()
    {
        return row;
    }

    public Piece getPiece()
    {
        return piece;
    }

    public bool isEmpty()
    {
        if(piece == null)
        {
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return name;
    }
}
