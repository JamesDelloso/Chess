using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Runtime.Serialization;

public class Board {

    private List<Square> squares;
    private string fen;

    public Player player1;
    public Player player2;

    public King wKing;
    public King bKing;

    private bool whitesTurn = true;
    public bool wkCastle = true;
    public bool wqCastle = true;
    public bool bkCastle = true;
    public bool bqCastle = true;
    private Square enPassant = null;
    private Piece enPassantPiece = null;
    private int halfMove = 0;
    private float fullMove = 1;
    private string pgn = "";

    public Board(string fen)
    {
        player1 = new Player();
        player2 = new Player();
        squares = new List<Square>();
        //fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
        //fen = "8/1k3n2/2BQ4/8/4b3/8/2K3p1/3R4 w - - 0 1";
        //fen = "8/1k3n2/2B5/8/8/8/2K3p1/3R4 w - - 0 1";
        string board = fen.Split(' ')[0];

        int rank = 8;
        int file = 0;
        foreach (char c in board)
        {
            int number;
            bool isNumber = int.TryParse(c.ToString(), out number);
            file++;
            if (c.Equals('/'))
            {
                rank--;
                file = 0;
            }
            else if (c.Equals('r'))
            {
                squares.Add(new Square(file, rank, new Rook(player2, this, file, rank), this));
                //Debug.Log(file + "," + rank);
            }
            else if (c.Equals('n'))
            {
                squares.Add(new Square(file, rank, new Knight(player2, this, file, rank), this));
            }
            else if (c.Equals('b'))
            {
                squares.Add(new Square(file, rank, new Bishop(player2, this, file, rank), this));
            }
            else if (c.Equals('q'))
            {
                squares.Add(new Square(file, rank, new Queen(player2, this, file, rank), this));
            }
            else if (c.Equals('k'))
            {
                bKing = new King(player2, this, file, rank);
                squares.Add(new Square(file, rank, bKing, this));
            }
            else if (c.Equals('p'))
            {
                squares.Add(new Square(file, rank, new Pawn(player2, this, file, rank), this));
            }
            else if (c.Equals('R'))
            {
                squares.Add(new Square(file, rank, new Rook(player1, this, file, rank), this));
            }
            else if (c.Equals('N'))
            {
                squares.Add(new Square(file, rank, new Knight(player1, this, file, rank), this));
            }
            else if (c.Equals('B'))
            {
                squares.Add(new Square(file, rank, new Bishop(player1, this, file, rank), this));
            }
            else if (c.Equals('Q'))
            {
                squares.Add(new Square(file, rank, new Queen(player1, this, file, rank), this));
            }
            else if (c.Equals('K'))
            {
                wKing = new King(player1, this, file, rank);
                squares.Add(new Square(file, rank, wKing, this));
            }
            else if (c.Equals('P'))
            {
                squares.Add(new Square(file, rank, new Pawn(player1, this, file, rank), this));
            }
            else if (isNumber)
            {
                for (int i=file;i<file+number;i++)
                {
                    squares.Add(new Square(i, rank, null, this));
                }
                file = file + number - 1;
            }
        }
    }

    public void movePiece(Piece piece, Square from, Square to, bool simulatedMove)
    {
        //Board temp = new Board(getFen());
        //temp.getSquare(from.ToString()).removePiece();
        //temp.getSquare(to.ToString()).addPiece(temp.getSquare(piece.getPosition().x, piece.getPosition().y).getPiece());
        //Debug.Log(temp.getFen());
        whitesTurn = !whitesTurn;
        if (enPassant != null)
        {
            if (piece.GetType().Equals((typeof(Pawn))) && to.ToString().Equals(enPassant.ToString()))
            {
                getSquare(enPassantPiece).removePiece();
            }
            enPassant.enPassant = false;
            enPassant.enPassantPiece = null;
            enPassant = null;
            enPassantPiece = null;
        }
        fullMove += 0.5f;

        if (piece.GetType().Equals(typeof(Pawn)))
        {
            (piece as Pawn).startingPos = false;
        }

        if (piece.GetType().Equals((typeof(Pawn))) && Mathf.Abs(from.getRow() - to.getRow()) == 2)
        {
            enPassant = getSquare(from.getColumn(), (from.getRow() + to.getRow()) / 2);
            enPassantPiece = piece;
            getSquare(from.getColumn(), (from.getRow() + to.getRow()) / 2).enPassant = true;
            getSquare(from.getColumn(), (from.getRow() + to.getRow()) / 2).enPassantPiece = piece;
        }
        else if (piece.GetType().Equals((typeof(King))) && piece.getPlayer() == player1)
        {
            if (from.ToString().Equals("E1") && to.ToString().Equals("G1"))
            {
                getSquare("F1").addPiece(getSquare("H1").getPiece());
                getSquare("H1").removePiece();
            }
            else if (from.ToString().Equals("E1") && to.ToString().Equals("C1"))
            {
                getSquare("D1").addPiece(getSquare("A1").getPiece());
                getSquare("A1").removePiece();
            }
            wkCastle = false;
            wqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(King))) && piece.getPlayer() == player2)
        {
            if (from.ToString().Equals("E8") && to.ToString().Equals("G8"))
            {
                getSquare("F8").addPiece(getSquare("H8").getPiece());
                getSquare("H8").removePiece();
            }
            else if (from.ToString().Equals("E8") && to.ToString().Equals("C8"))
            {
                getSquare("D8").addPiece(getSquare("A8").getPiece());
                getSquare("A8").removePiece();
            }
            bkCastle = false;
            bqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getPlayer() == player1 && from.ToString().Equals("A1"))
        {
            wqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getPlayer() == player1 && from.ToString().Equals("H1"))
        {
            wkCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getPlayer() == player2 && from.ToString().Equals("A8"))
        {
            bqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getPlayer() == player2 && from.ToString().Equals("H8"))
        {
            bkCastle = false;
        }
        addToPGN(piece, to);
        from.removePiece();
        to.addPiece(piece);
    }

    public bool isCheck()
    {
        if(wKing.getAttackingSquares().Count > 0)
        {
            Debug.Log("Check on White");
            //wKing.getCheckPath();
            return true;
        }
        if(bKing.getAttackingSquares().Count > 0)
        {
            Debug.Log("Check on Black");
            return true;
        }
        return false;
    }

    public void setPiece(Piece piece, Square to)
    {
        to.addPiece(piece);
    }

    public void removePiece(Square square)
    {
        square.removePiece();
    }

    public List<Square> getSquaresOnBoard()
    {
        return squares;
    }

    public Square getSquare(Piece piece)
    {
        foreach (Square square in squares)
        {
            if(square.isEmpty() == false)
            {
                if (square.getPiece().Equals(piece))
                {
                    return square;
                }
            }
        }
        return null;
    }

    public Square getSquare(string name)
    {
        foreach (Square square in squares)
        {
            if (square.ToString() == name)
            {
                return square;
            }
        }
        return null;
    }

    public Square getSquare(int column, int row)
    {
        foreach (Square square in squares)
        {
            if (square.getColumn() == column && square.getRow() == row)
            {
                return square;
            }
        }
        return null;
    }

    public string getFen()
    {
        string fen = ToString();
        if (whitesTurn) fen += " w ";
        else fen += " b ";
        if (wkCastle) fen += "K";
        if (wqCastle) fen += "Q";
        if (bkCastle) fen += "k";
        if (bqCastle) fen += "q";
        if (enPassant == null) fen += " - ";
        else fen += " " + enPassant.ToString().ToLower() + " ";
        fen += halfMove + " " + (int)fullMove;
        return fen;
    }

    public string addToPGN(Piece piece, Square to)
    {
        bool castled = false;
        if (piece.getPlayer() == player1)
        {
            pgn += (int)fullMove + ".  ";
        }
        if (piece.GetType().Equals(typeof(Rook)))
        {
            pgn += "R";
        }
        else if (piece.GetType().Equals(typeof(Bishop)))
        {
            pgn += "B";
        }
        else if (piece.GetType().Equals(typeof(Knight)))
        {
            pgn += "N";
        }
        else if (piece.GetType().Equals(typeof(Queen)))
        {
            pgn += "Q";
        }
        else if (piece.GetType().Equals(typeof(King)))
        {
            Square from = getSquare(piece);
            if (from.getColumn() == 5 && to.getColumn() == 3)
            {
                pgn += "O-O-O ";
                castled = true;
            }
            else if (from.getColumn() == 5 && to.getColumn() == 7)
            {
                pgn += "O-O ";
                castled = true;
            }
            else
            {
                pgn += "K";
            }
        }
        if (to.isEmpty() == false)
        {
            pgn += "x";
        }
        if (castled == false)
        {
            pgn += to.ToString().ToLower() + " ";
        }
        return pgn;
    }

    public string getPGN()
    {
        return pgn;
    }

    public override string ToString()
    {
        //  rnbqkbnr/pp1ppppp/8/2p5/4P3/5N2/PPPP1PPP/RNBQKB1R b KQkq -1 2
        string fen = "";
        int file = 0;
        int num = 0;
        squares.Reverse<Square>();
        for(int i=0;i<squares.Count;i++)
        {
            file++;
            if(squares[i].isEmpty())
            {
                num++;
            }
            else
            {
                string letter = squares[i].getPiece().GetType().ToString().Substring(0, 1);
                if (squares[i].getPiece().GetType().Equals(typeof(Knight)))
                {
                    letter = "N";
                }
                if (squares[i].getPiece().getPlayer() == player2)
                {
                    letter = letter.ToLower();
                }
                if(num > 0)
                {
                    fen += num;
                }
                fen += letter;
                num = 0;
            }
            if(file >= 8)
            {
                if(num > 0)
                {
                    fen += num;
                }
                fen += "/";
                file = 0;
                num = 0;
            }
        }
        fen = fen.Remove(fen.Length - 1);
        return fen;
    }
}
