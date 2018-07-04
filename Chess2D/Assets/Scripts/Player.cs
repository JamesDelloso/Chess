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
            if (square.Piece() != null && square.Piece().getColour() == colour)
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
}
