using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square {
    private string name;
    private int column;
    private int row;
    private List<Piece> attackingPieces = new List<Piece>();
    private List<Piece> defendingPieces = new List<Piece>();
    private Piece piece;
    private Board board;
    private int value = 0;

    public Square(string position, Board board)
    {
        piece = null;
        List<string> letters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };
        column = letters.IndexOf(position.Substring(0, 1)) + 1;
        row = int.Parse(position.Substring(1, 1));
        name = position;
        this.board = board;
        //updateAttackingPieces();
    }

    public Square(string position, Piece piece, Board board)
    {
        this.piece = piece;
        List<string> letters = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H" };
        column = letters.IndexOf(position.Substring(0, 1)) + 1;
        row = int.Parse(position.Substring(1, 1));
        name = position;
        this.board = board;
        //updateAttackingPieces();
    }

    public Square deepCopy(Board b)
    {
        Square sq = null;
        if(isEmpty())
        {
            sq = new Square(name, b);
        }
        else
        {
            sq = new Square(name, piece.deepCopy(b), b);
        }
        return sq;
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
        Square from = board.findSquareWithPiece(piece);
        from.piece = null;
        this.piece = promotedPiece;
    }

    public List<Piece> getAttackingPieces(Colour colour)
    {
        List<Piece> list = new List<Piece>();
        foreach(Piece piece in attackingPieces)
        {
            if(piece.getColour() != colour)
            {
                list.Add(piece);
            }
        }
        return list;
    }

    public List<Piece> getDefendingPieces(Colour colour)
    {
        List<Piece> list = new List<Piece>();
        foreach (Piece piece in attackingPieces)
        {
            if (piece.getColour() == colour)
            {
                list.Add(piece);
            }
        }
        return list;
    }

    public void updateAttackingPieces()
    {
        attackingPieces = new List<Piece>();
        foreach (Square square in board.getSquaresOnBoard())
        {
            if (square.isEmpty() == false && (square.Piece().getPossibleMoves().Contains(this) || square.Piece().getProtectingSquares().Contains(this)))
            {
                attackingPieces.Add(square.Piece());
            }
            if (square.isEmpty() == false && square.Piece().GetType().Equals(typeof(Pawn)))
            {
                if (square.Column().Equals(Column()))
                {
                    attackingPieces.Remove(square.Piece());
                }
                else if (((square.Piece() as Pawn).getLeftSquare() != null && (square.Piece() as Pawn).getLeftSquare().Equals(this)) || ((square.Piece() as Pawn).getRightSquare() != null && (square.Piece() as Pawn).getRightSquare().Equals(this)))
                {
                    attackingPieces.Add((square.Piece()));
                    //attackingPieces.Add((square.Piece() as Pawn).getRightSquare().Piece());
                }
            }
        }
    }

    public int Column()
    {
        return column;
    }

    public int Row()
    {
        return row;
    }

    public string Name()
    {
        return name;
    }

    public Piece Piece()
    {
        return piece;
    }

    public int getValue(Colour colour)
    {
        foreach(Piece piece in attackingPieces)
        {
            if(piece.getColour() == colour)
            {
                value += piece.getValue();
            }
            else
            {
                value -= piece.getValue();
            }
        }
        return value;
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
