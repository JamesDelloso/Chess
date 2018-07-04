using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer {

    private Colour colour;
    private List<Piece> pieces = new List<Piece>();

    public Computer(Colour c)
    {
        colour = c;
        foreach (Square square in Game.getBoard().getSquaresOnBoard())
        {
            if(square.Piece() != null && square.Piece().getColour() == colour)
            {
                pieces.Add(square.Piece());
            }
        }
    }

    public Colour getColour()
    {
        return colour;
    }

    public List<Piece> getPieces()
    {
        return pieces;
    }

    public void play()
    {
        Board b = new Board(Game.getBoard());
        List<Square> sto = new List<Square>();
        List<Piece> pto = new List<Piece>();
        int value = 0;
        Piece p = null;
        Square s = null;
        foreach(Piece pi in pieces)
        {
            sto.Add(Game.getBoard().findSquareWithPiece(pi));
            pto.Add(pi);
        }
        for(int i=0;i<1;i++)
        {
            foreach (Piece piece in pieces)
            {
                if (i == 0)
                {
                    //sto.Add(Game.getBoard().findSquareWithPiece(piece));
                    //pto.Add(piece);
                }
                foreach (Square square in piece.getPossibleMoves())
                {
                    b = new Board(Game.getBoard());
                    b.movePiece(b.getSquare(Game.getBoard().findSquareWithPiece(piece).Name()).Piece(), b.getSquare(Game.getBoard().getSquare(square.Name()).Name()));
                    b.updateBoard();
                    //Game.getBoard().movePiece(piece, square);
                    //Game.getBoard().updateBoard();
                    if ((getMoveCount(b) > value || value == 0) && square.getAttackingPieces(colour).Count == 0)
                    {
                        value = getMoveCount(b);
                        p = piece;
                        s = square;
                    }
                }
            }
        }
        //for (int i = 0; i < sto.Count; i++)
        //{
        //    Debug.Log(pto[i] + "," + sto[i]);
        //    Game.getBoard().movePiece(pto[i], sto[i]);
        //}
        //Game.getBoard().movePiece(p, s);
    }

    private void simulateMove()
    {
        foreach(Piece piece in pieces)
        {
            foreach(Square square in piece.getPossibleMoves())
            {

            }
        }
    }

    private int getMoveCount(Board b)
    {
        int value = 0;
        foreach (Square square in b.getSquaresOnBoard())
        {
            if (square.isEmpty() == false && square.Piece().getColour() == colour)
            {
                value += square.Piece().getPossibleMoves().Count;
            }
        }
        //Debug.Log(value);
        return value;
    }

    private Square isGettingAttacked()
    {
        foreach (Square square in Game.getBoard().getSquaresOnBoard())
        {
            if (square.isEmpty() == false && square.Piece().getColour() == colour)
            {
                if(square.getAttackingPieces(colour).Count != 0)
                {
                    return square;
                }
            }
        }
        return null;
    }
}
