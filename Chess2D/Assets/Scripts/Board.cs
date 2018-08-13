using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board {

    //private string fen;
    public Piece[,] squares = new Piece[8, 8];
    public Vector2Int? enPassant;
    BitArray ba = new BitArray(64);
    public bool whitesTurn = true;
    public bool wkCastle = false;
    public bool wqCastle = false;
    public bool bkCastle = false;
    public bool bqCastle = false;
    public int halfMove = 0;
    public float fullMove = 1;
    public King wKing;
    public King bKing;
    public List<string> moves = new List<string>();

    public Board(string fen)
    {
        squares = FEN.readBoard(fen);
        if (fen.Split(' ')[1] == "w") whitesTurn = true;
        else whitesTurn = false;
        foreach(char c in fen.Split(' ')[2])
        {
            if (c == 'K') wkCastle = true;
            if (c == 'Q') wqCastle = true;
            if (c == 'k') bkCastle = true;
            if (c == 'q') bqCastle = true;
        }
        if(fen.Split(' ')[3] != "-")
        {
            string letters = "abcdefgh";
            enPassant = new Vector2Int(letters.IndexOf(fen.Split(' ')[3].Substring(0, 1)), int.Parse(fen.Split(' ')[3].Substring(1, 1)) - 1);
        }
        halfMove = int.Parse(fen.Split(' ')[4]);
        fullMove = int.Parse(fen.Split(' ')[5]);

        foreach (Piece p in squares)
        {
            if (p != null)
            {
                if(p.ToString() == "White King")
                {
                    wKing = (King)p;
                }
                else if(p.ToString() == "Black King")
                {
                    bKing = (King)p;
                }
            }
        }
        foreach (Piece p in squares)
        {
            if (p != null)
            {
                p.generatePossibleMoves(this);
            }
        }
    }

    public void movePiece(int fromX, int fromY, int toX, int toY)
    {
        halfMove++;
        Piece piece = getPiece(fromX, fromY);
        if (piece.GetType().Equals(typeof(Pawn)) && enPassant.Equals(new Vector2Int(toX, toY)))
        {
            if(piece.colour == Colour.White)
            {
                squares[toX, 4] = null;
            }
            else
            {
                squares[toX, 3] = null;
            }
        }
        enPassant = null;
        if (piece.GetType().Equals(typeof(Pawn)))
        {
            if(Mathf.Abs(fromY - toY) == 2)
            {
                enPassant = new Vector2Int(fromX, (fromY + toY) / 2);
            }
            halfMove = 0;
        }
        else if(piece.ToString() == "White King")
        {
            if(fromX == 4 && fromY == 0 && toX == 6 && toY == 0)
            {
                squares[5, 0] = getPiece(7, 0);
                squares[7, 0] = null;
            }
            else if(fromX == 4 && fromY == 0 && toX == 2 && toY == 0)
            {
                squares[3, 0] = getPiece(0, 0);
                squares[0, 0] = null;
            }
            wkCastle = false;
            wqCastle = false;
        }
        else if (piece.ToString() == "Black King")
        {
            if (fromX == 4 && fromY == 7 && toX == 6 && toY == 7)
            {
                squares[5, 7] = getPiece(7, 7);
                squares[7, 7] = null;
            }
            else if (fromX == 4 && fromY == 7 && toX == 2 && toY == 7)
            {
                squares[3, 7] = getPiece(0, 7);
                squares[0, 7] = null;
            }
            bkCastle = false;
            bqCastle = false;
        }
        else if (piece.ToString() == "White Rook" && fromX == 0 && fromY == 0)
        {
            wqCastle = false;
        }
        else if (piece.ToString() == "White Rook" && fromX == 7 && fromY == 0)
        {
            wkCastle = false;
        }
        else if (piece.ToString() == "Black Rook" && fromX == 0 && fromY == 7)
        {
            bqCastle = false;
        }
        else if (piece.ToString() == "Black Rook" && fromX == 7 && fromY == 7)
        {
            bkCastle = false;
        }
        if(squares[toX, toY] != null)
        {
            halfMove = 0;
        }
        moves.Add(PGN.convertToString(this, piece, fromX, toX, toY));
        squares[toX, toY] = piece;
        squares[fromX, fromY] = null;
        whitesTurn = !whitesTurn;
        fullMove += 0.5f;
        Debug.Log(FEN.generate(this));
        foreach(Piece p in squares)
        {
            if(p != null) {
                p.generatePossibleMoves(this);
            }
        }
    }

    public Piece isCastling(int fromX, int fromY, int toX, int toY)
    {
        Piece piece = getPiece(fromX, fromY);
        if (piece.ToString() == "White King")
        {
            if (fromX == 4 && fromY == 0 && toX == 6 && toY == 0)
            {
                return getPiece(7, 0);
            }
            else if (fromX == 4 && fromY == 0 && toX == 2 && toY == 0)
            {
                return getPiece(0, 0);
            }
        }
        else if (piece.ToString() == "Black King")
        {
            if (fromX == 4 && fromY == 7 && toX == 6 && toY == 7)
            {
                return getPiece(7, 7);
            }
            else if (fromX == 4 && fromY == 7 && toX == 2 && toY == 7)
            {
                return getPiece(0, 7);
            }
        }
        return null;
    }

    public bool isCheckMate(King king)
    {
        if(king.isCheck(this) == false)
        {
            return false;
        }
        foreach (Piece p in squares)
        {
            if (p != null && p.colour == king.colour && p.possibleMoves.Count > 0)
            {
                return false;
            }
        }
        return true;
    }

    public bool isStaleMate(King king)
    {
        foreach (Piece p in squares)
        {
            if (p != null && p.colour == king.colour && p.possibleMoves.Count > 0)
            {
                return false;
            }
        }
        if (king.isCheck(this) == true)
        {
            return false;
        }
        return true;
    }

    public Piece getPiece(int file, int rank)
    {
        return squares[file, rank];
    }

    public Piece getPiece(Vector2Int pos)
    {
        return squares[pos.x, pos.y];
    }

    public Vector2Int getPosition(Piece piece)
    {
        for(int i=0;i<8;i++)
        {
            for(int j=0;j<8;j++)
            {
                if(squares[i,j] != null && squares[i,j].Equals(piece))
                {
                    return new Vector2Int(i, j);
                }
            }
        }
        return new Vector2Int(-1, -1);
    }
}
