using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {

    private int column;
    public bool startingPos;

    public Pawn(Player player, Board board, int c, int r) : base(player, board, c, r)
    {
        startingPos = true;
        value = 1;
    }

    public void promote(Square to, Piece piece)
    {
        to.addPiece(piece);
    }

    public void move()
    {
        startingPos = false;
    }
    public override List<Square> getPossibleMoves()
    {
        possibleMoves = new List<Square>();
        column = board.getSquare(this).getColumn();
        int row = board.getSquare(this).getRow();

        int dir = 1;
        if(getPlayer() == board.player2)
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

        return possibleMoves;
    }

    private bool checkPawnSquare(int c, int r)
    {
        if (board.getSquare(c, r) == null)
        {
            return false;
        }
        if (board.getSquare(c, r).isEmpty() && c == column)
        {
            possibleMoves.Add(board.getSquare(c, r));
            return true;
        }
        if(c != column && board.getSquare(c, r).isEmpty() == false && board.getSquare(c, r).getPiece().getPlayer() != getPlayer() || (board.getSquare(c, r).enPassant == true && board.getSquare(c, r).enPassantPiece.getPlayer() != getPlayer()))
        {
            possibleMoves.Add(board.getSquare(c, r));
        }
        return false;
    }
}
