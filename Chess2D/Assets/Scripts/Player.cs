using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {

    private Colour colour;
    private List<Piece> pieces = new List<Piece>();

    public Player(Colour c)
    {
        colour = c;
        foreach (Square square in Game.getBoard().getSquaresOnBoard())
        {
            if (square.getPiece() != null && square.getPiece().getColour() == colour)
            {
                pieces.Add(square.getPiece());
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
}
