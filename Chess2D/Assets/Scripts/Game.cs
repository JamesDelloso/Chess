using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {

    private Player player;
    private Computer computer;
    private static Board board;
    private Board realBoard;

    private bool whitesTurn = true;
    public static bool wkCastle = true;
    public static bool wqCastle = true;
    public static bool bkCastle = true;
    public static bool bqCastle = true;
    private Square enPassant = null;
    private Piece enPassantPiece = null;
    private int halfMove = 0;
    private float fullMove = 1;

    private string pgn = "";

    public Game()
    {
        new Game("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    public Game(string fen)
    {
        //board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        board = new Board(fen);
        player = new Player(Colour.White);
        computer = new Computer(Colour.Black);
        //Board board2 = new Board(board.ToString());
        //board2.removePiece(board2.getSquare("A1"));
        //board2.removePiece(board2.getSquare("B1"));
        //board2.removePiece(board2.getSquare("C1"));
        //board2.getSquare("A1").removePiece();
        //board2.getSquare("B1").removePiece();
        //board2.getSquare("C1").removePiece();
        //Debug.Log(board2);
    }

    public Player getPlayer()
    {
        return player;
    }

    public Computer getComputer()
    {
        return computer;
    }

    public void movePiece(Piece piece, Square from, Square to)
    {
        whitesTurn = !whitesTurn;
        if(enPassant != null)
        {
            if (piece.GetType().Equals((typeof(Pawn))) && to.ToString().Equals(enPassant.ToString()))
            {
                board.removePiece(board.findSquareWithPiece(enPassantPiece));
            }
            enPassant.enPassant = false;
            enPassant.enPassantPiece = null;
            enPassant = null;
            enPassantPiece = null;
        }
        fullMove += 0.5f;

        if(piece.GetType().Equals(typeof(Pawn)))
        {
            (piece as Pawn).startingPos = false;
        }

        if(piece.GetType().Equals((typeof(Pawn))) && Mathf.Abs(from.getRow() - to.getRow()) == 2) {
            enPassant = board.getSquare(from.getColumn(), (from.getRow() + to.getRow()) / 2);
            enPassantPiece = piece;
            board.getSquare(from.getColumn(), (from.getRow() + to.getRow()) / 2).enPassant = true;
            board.getSquare(from.getColumn(), (from.getRow() + to.getRow()) / 2).enPassantPiece = piece;
        }
        else if(piece.GetType().Equals((typeof(King))) && piece.getColour() == Colour.White)
        {
            if(from.ToString().Equals("E1") && to.ToString().Equals("G1"))
            {
                board.setPiece(board.getSquare("H1").getPiece(), board.getSquare("F1"));
                board.removePiece(board.getSquare("H1"));
            }
            else if (from.ToString().Equals("E1") && to.ToString().Equals("C1"))
            {
                board.setPiece(board.getSquare("A1").getPiece(), board.getSquare("D1"));
                board.removePiece(board.getSquare("A1"));
            }
            wkCastle = false;
            wqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(King))) && piece.getColour() == Colour.Black)
        {
            if (from.ToString().Equals("E8") && to.ToString().Equals("G8"))
            {
                board.setPiece(board.getSquare("H8").getPiece(), board.getSquare("F8"));
                board.removePiece(board.getSquare("H8"));
            }
            else if (from.ToString().Equals("E8") && to.ToString().Equals("C8"))
            {
                board.setPiece(board.getSquare("A8").getPiece(), board.getSquare("D8"));
                board.removePiece(board.getSquare("A8"));
            }
            bkCastle = false;
            bqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getColour() == Colour.White && from.ToString().Equals("A1"))
        {
            wqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getColour() == Colour.White && from.ToString().Equals("H1"))
        {
            wkCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getColour() == Colour.Black && from.ToString().Equals("A8"))
        {
            bqCastle = false;
        }
        else if (piece.GetType().Equals((typeof(Rook))) && piece.getColour() == Colour.Black && from.ToString().Equals("H8"))
        {
            bkCastle = false;
        }
        addToPGN(piece, to);
        //board.removePiece(from);
        //board.setPiece(piece, to);
        from.removePiece();
        to.addPiece(piece);
    }

    public string getFen()
    {
        string fen = board.ToString();
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
        if(piece.getColour() == Colour.White)
        {
            pgn += (int)fullMove+".  ";
        }
        if(piece.GetType().Equals(typeof(Rook))) {
            pgn += "R";
        }
        else if (piece.GetType().Equals(typeof(Bishop))) {
            pgn += "B";
        }
        else if (piece.GetType().Equals(typeof(Knight))) {
            pgn += "N";
        }
        else if (piece.GetType().Equals(typeof(Queen))) {
            pgn += "Q";
        }
        else if (piece.GetType().Equals(typeof(King))) {
            Square from = board.findSquareWithPiece(piece);
            if(from.getColumn() == 5 && to.getColumn() == 3)
            {
                pgn += "O-O-O ";
                castled = true;
            }
            else if(from.getColumn() == 5 && to.getColumn() == 7)
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
        if(castled == false)
        {
            pgn += to.ToString().ToLower() + " ";
        }
        return pgn;
    }

    public string getPGN()
    {
        return pgn;
    }

    public static Board getBoard()
    {
        return board;
    }
}
