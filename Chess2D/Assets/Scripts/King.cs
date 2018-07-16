using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece {

    private List<Square> checkPath;

    public King(Player player, Board board, int c, int r) : base(player, board, c, r)
    {
        value = 100;
    }

    public override List<Square> getPossibleMoves()
    {
        possibleMoves = new List<Square>();
        int column = board.getSquare(this).getColumn();
        int row = board.getSquare(this).getRow();

        checkSquare(column - 1, row + 1);
        checkSquare(column, row + 1);
        checkSquare(column + 1, row + 1);
        checkSquare(column - 1, row);
        checkSquare(column, row);
        checkSquare(column + 1, row);
        checkSquare(column - 1, row - 1);
        checkSquare(column, row - 1);
        checkSquare(column + 1, row - 1);

        if(getPlayer() == board.player1 && board.wqCastle == true && board.getSquare("B1").isEmpty() && board.getSquare("C1").isEmpty() && board.getSquare("D1").isEmpty())
        {
            possibleMoves.Add(board.getSquare("C1"));
        }
        if (getPlayer() == board.player1 && board.wkCastle == true && board.getSquare("F1").isEmpty() && board.getSquare("G1").isEmpty())
        {
            possibleMoves.Add(board.getSquare("G1"));
        }
        else if (getPlayer() == board.player2 && board.bqCastle == true && board.getSquare("B8").isEmpty() && board.getSquare("C8").isEmpty() && board.getSquare("D8").isEmpty())
        {
            possibleMoves.Add(board.getSquare("C8"));
        }
        if (getPlayer() == board.player2 && board.bkCastle == true && board.getSquare("F8").isEmpty() && board.getSquare("G8").isEmpty())
        {
            possibleMoves.Add(board.getSquare("G8"));
        }

        return possibleMoves;
    }
}
