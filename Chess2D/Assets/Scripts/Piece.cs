using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece {

    protected List<Square> possibleMoves;
    private List<Square> attackingSquares;
    private Player player;
    protected int value;
    protected Board board;
    protected Vector2Int position = new Vector2Int();

    public Piece(Player player, Board board, Vector2Int position)
    {
        this.player = player;
        this.board = board;
        this.position = position;
        attackingSquares = new List<Square>();
    }

    public Piece(Player player, Board board, int c, int r) {
        this.player = player;
        this.board = board;
        //position.x = c;
        //position.y = r;
        position = new Vector2Int(c, r);
        attackingSquares = new List<Square>();
    }

    public Player getPlayer()
    {
        return player;
    }

    public int getValue()
    {
        return value;
    }

    protected bool checkSquare(int c, int r)
    {
        Board temp = new Board(board.getFen());
        if (board.getSquare(c, r) == null)
        {
            return false;
        }
        if (board.getSquare(c, r).isEmpty())
        {
            possibleMoves.Add(board.getSquare(c, r));
            return true;
        }
        else if (board.getSquare(c, r).getPiece().getPlayer() != getPlayer())
        {
            possibleMoves.Add(board.getSquare(c, r));
        }
        return false;
    }

    public virtual List<Square> getPossibleMoves()
    {
        return possibleMoves;
    }

    public List<Square> getAttackingSquares()
    {
        attackingSquares = new List<Square>();
        foreach(Square square in board.getSquaresOnBoard())
        {
            if(square.isEmpty() == false && square.getPiece().getPossibleMoves().Contains(board.getSquare(position.x, position.y)))
            {
                attackingSquares.Add(square);
                Debug.Log("Attacked from: " + square+"by"+square.getPiece());
            }
        }
        return attackingSquares;
    }

    public Vector2Int getPosition()
    {
        return position;
    }

    public override string ToString()
    {
        return getPlayer() + " " + GetType();
    }
}
