using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {

    private Player player;
    private Computer computer;
    private static Board board;
    private Board realBoard;

    public Game()
    {
        board = new Board(Colour.White, Colour.Black);
        player = new Player(Colour.White);
        computer = new Computer(Colour.Black);
        board.updateBoard();
        Board b = new Board(board);
        //Board b = board.deepCopy();
        //Debug.Log(b.findSquareWithPiece(b.getSquare("A1").Piece()));
        //b.movePiece(b.getSquare("A1").Piece(), b.getSquare("A4"));
        //Debug.Log("A4:"+b.getSquare("A4").Piece());
    }

    public Player getPlayer()
    {
        return player;
    }

    public Computer getComputer()
    {
        return computer;
    }

    public static Board getBoard()
    {
        return board;
    }
}
