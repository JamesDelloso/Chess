using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {

    private Square left;
    private Square right;
    private int column;
    public bool startingPos;

    public Pawn(Colour colour) : base(colour)
    {
        startingPos = true;
        value = 1;
    }

    public void promote(Square to, Piece piece)
    {
        to.addPiece(piece);
    }

    public Square getLeftSquare()
    {
        return left;
    }

    public Square getRightSquare()
    {
        return right;
    }

    public void move()
    {
        startingPos = false;
    }
    public override void updatePossibleMoves()
    {
        possibleMoves = new List<Square>();
        column = Game.getBoard().findSquareWithPiece(this).getColumn();
        int row = Game.getBoard().findSquareWithPiece(this).getRow();

        int dir = 1;
        if(getColour() == Colour.Black)
        {
            dir = -1;
        }

        checkPawnSquare(column - 1, row + dir);
        checkPawnSquare(column + 1, row + dir);
        bool check = checkPawnSquare(column, row + dir);

        if (startingPos == true && check == true)
        {
            checkPawnSquare(column, row + dir + dir);
        }
    }

    private bool checkPawnSquare(int c, int r)
    {
        if (Game.getBoard().getSquare(c, r) == null)
        {
            return false;
        }
        if (Game.getBoard().getSquare(c, r).isEmpty() && c == column)
        {
            possibleMoves.Add(Game.getBoard().getSquare(c, r));
            return true;
        }
        if(c != column && Game.getBoard().getSquare(c, r).isEmpty() == false && Game.getBoard().getSquare(c, r).getPiece().getColour() != getColour() || (Game.getBoard().getSquare(c, r).enPassant == true && Game.getBoard().getSquare(c, r).enPassantPiece.getColour() != getColour()))
        {
            possibleMoves.Add(Game.getBoard().getSquare(c, r));
        }
        return false;
    }
}
