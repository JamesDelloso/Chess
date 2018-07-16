using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece {

    public Knight(Colour colour, Board board, int c, int r) : base(colour, board, c, r)
    {
        value = 3;
    }

    public override List<Square> getPossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = board.getSquare(this).getColumn();
        int row = board.getSquare(this).getRow();

        checkSquare(column - 2, row - 1);
        checkSquare(column - 1, row - 2);
        checkSquare(column + 2, row - 1);
        checkSquare(column + 1, row - 2);
        checkSquare(column - 2, row + 1);
        checkSquare(column - 1, row + 2);
        checkSquare(column + 2, row + 1);
        checkSquare(column + 1, row + 2);

        return possibleMoves;
    }
}
