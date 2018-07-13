using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    public Knight(Colour colour) : base(colour)
    {
        value = 3;
    }

    public override void updatePossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = Game.getBoard().findSquareWithPiece(this).getColumn();
        int row = Game.getBoard().findSquareWithPiece(this).getRow();

        checkSquare(column - 2, row - 1);
        checkSquare(column - 1, row - 2);
        checkSquare(column + 2, row - 1);
        checkSquare(column + 1, row - 2);
        checkSquare(column - 2, row + 1);
        checkSquare(column - 1, row + 2);
        checkSquare(column + 2, row + 1);
        checkSquare(column + 1, row + 2);
    }
}
