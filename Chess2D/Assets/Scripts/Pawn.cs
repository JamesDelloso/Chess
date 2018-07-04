using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : Piece {

    private Square left;
    private Square right;

    public Pawn(Colour colour, Board board) : base(colour, board)
    {
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
    public override Piece deepCopy(Board b)
    {
        return new Pawn(getColour(), b);
    }
    public override void updatePossibleMoves()
    {
        possibleMoves = new List<Square>();
        protectingSquares = new List<Square>();
        //Board b = getBoard().deepCopy();
        //Board b = new Board(getBoard());
        //Debug.Log(b);
        int column1 = getBoard().findSquareWithPiece(this).Column();
        int row1 = getBoard().findSquareWithPiece(this).Row();
        Square[] s = getBoard().getSquaresOnBoard().ToArray();
        foreach (Square square in s)
        {
            int column2 = square.Column();
            int row2 = square.Row();
            int colDiff = column2 - column1;
            int rowDiff = row2 - row1;
            if (getColour() == Colour.White)
            {
                if (colDiff == 0 && row2 > row1 && square.isEmpty())
                {
                    int maxMoves = 1;
                    if (row1 == 2)
                    {
                        maxMoves = 2;
                    }
                    if (row2 - row1 <= maxMoves && getBoard().isColumnBlocked(getBoard().findSquareWithPiece(this), square) == false)
                    {
                        possibleMoves.Add(square);
                        //Square s3 = getBoard().findSquareWithPiece(this);
                        //Square s2 = getBoard().getSquare(square.Name());
                        //Debug.Log(b.getSquare(getBoard().findSquareWithPiece(this).Name())+","+square);
                        //Debug.Log(s3);
                        //Board b = new Board(getBoard());
                        //Debug.Log(b.getSquare(s3.Name()).Piece()+","+b.getSquare(s2.Name()));
                        //Debug.Log(b.getSquare("D1").Piece());
                        //b.movePiece(b.getSquare(s3.Name()).Piece(), b.getSquare(s2.Name()));
                        //Square sq = getBoard().findSquareWithPiece(this);
                        //getBoard().movePiece(this, square);
                        //if(getBoard().isCheck())
                        //{
                        //    Debug.Log("Check in Pawn Class");
                        //    possibleMoves.Remove(square);
                        //}
                        //getBoard().movePiece(this, sq);
                        //Debug.Log(b);
                    }
                }
                else if (System.Math.Abs(colDiff) == 1 && System.Math.Abs(rowDiff) == 1 && row2 > row1)
                {
                    if (square.isEmpty() == false)
                    {
                        if (!square.Piece().getColour().Equals(getColour()))
                        {
                            possibleMoves.Add(square);
                        }
                    }
                    if (column2 < column1)
                    {
                        left = square;
                    }
                    else
                    {
                        right = square;
                    }
                }
            }
            else
            {
                if (colDiff == 0 && row2 < row1 && square.isEmpty())
                {
                    int maxMoves = 1;
                    if (row1 == 7)
                    {
                        maxMoves = 2;
                    }
                    if (row1 - row2 <= maxMoves && getBoard().isColumnBlocked(getBoard().findSquareWithPiece(this), square) == false)
                    {
                        possibleMoves.Add(square);
                    }
                }
                else if (System.Math.Abs(colDiff) == 1 && System.Math.Abs(rowDiff) == 1 && row2 < row1)
                {
                    if (square.isEmpty() == false)
                    {
                        if (!square.Piece().getColour().Equals(getColour()))
                        {
                            possibleMoves.Add(square);
                        }
                        else
                        {
                            protectingSquares.Add(square);
                        }
                    }
                    if (column2 < column1)
                    {
                        left = square;
                    }
                    else
                    {
                        right = square;
                    }
                }
            }
        }
    }
}
