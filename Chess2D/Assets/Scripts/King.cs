using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {

    public King(Colour colour) : base(colour)
    {
        value = 100;
    }

    public override void updatePossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = Game.getBoard().findSquareWithPiece(this).getColumn();
        int row = Game.getBoard().findSquareWithPiece(this).getRow();

        checkSquare(column - 1, row + 1);
        checkSquare(column, row + 1);
        checkSquare(column + 1, row + 1);
        checkSquare(column - 1, row);
        checkSquare(column, row);
        checkSquare(column + 1, row);
        checkSquare(column - 1, row - 1);
        checkSquare(column, row - 1);
        checkSquare(column + 1, row - 1);

        if(getColour() == Colour.White && Game.wqCastle == true && Game.getBoard().getSquare("B1").isEmpty() && Game.getBoard().getSquare("C1").isEmpty() && Game.getBoard().getSquare("D1").isEmpty())
        {
            possibleMoves.Add(Game.getBoard().getSquare("C1"));
        }
        if (getColour() == Colour.White && Game.wkCastle == true && Game.getBoard().getSquare("F1").isEmpty() && Game.getBoard().getSquare("G1").isEmpty())
        {
            possibleMoves.Add(Game.getBoard().getSquare("G1"));
        }
        else if (getColour() == Colour.Black && Game.bqCastle == true && Game.getBoard().getSquare("B8").isEmpty() && Game.getBoard().getSquare("C8").isEmpty() && Game.getBoard().getSquare("D8").isEmpty())
        {
            possibleMoves.Add(Game.getBoard().getSquare("C8"));
        }
        if (getColour() == Colour.Black && Game.bkCastle == true && Game.getBoard().getSquare("F8").isEmpty() && Game.getBoard().getSquare("G8").isEmpty())
        {
            possibleMoves.Add(Game.getBoard().getSquare("G8"));
        }
    }


    //public bool isCheck(Square square)
    //{
    //    foreach (Piece piece in square.getAttackingPieces(getColour()))
    //    {
    //        if (piece.getColour() != getColour())
    //        {
    //            //Debug.Log("Check");
    //            //if(square.Name().Equals("D1"))
    //            //Debug.Log(square+":" + piece);
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
