using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece {

    protected List<Square> possibleMoves;
    private Colour colour;
    protected int value; 

	public Piece(Colour colour) {
        this.colour = colour;
    }

    public Colour getColour()
    {
        return colour;
    }

    public int getValue()
    {
        return value;
    }

    public virtual void updatePossibleMoves()
    {

    }

    protected bool checkSquare(int c, int r)
    {
        if (Game.getBoard().getSquare(c, r) == null)
        {
            return false;
        }
        if (Game.getBoard().getSquare(c, r).isEmpty())
        {
            possibleMoves.Add(Game.getBoard().getSquare(c, r));
            return true;
        }
        else if (Game.getBoard().getSquare(c, r).getPiece().getColour() != getColour())
        {
            possibleMoves.Add(Game.getBoard().getSquare(c, r));
        }
        return false;
    }

    public List<Square> getPossibleMoves()
    {
        return possibleMoves;
    }

    public override string ToString()
    {
        return getColour() + " " + GetType();
    }
}
