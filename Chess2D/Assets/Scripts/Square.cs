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

    public Square(string position, Piece piece)
    {
        this.piece = piece;
        string letters = "ABCDEFGH";
        column = letters.IndexOf(position.Substring(0, 1)) + 1;
        row = int.Parse(position.Substring(1, 1));
        name = position;
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
        Square from = Game.getBoard().findSquareWithPiece(piece);
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
