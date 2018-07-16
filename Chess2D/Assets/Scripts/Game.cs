using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {

    private Player player;
    private Computer computer;
    private Board board;
    private Board realBoard;

    public Game()
    {
        //new Game("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        player = new Player(Colour.White);
        computer = new Computer(Colour.Black);
    }

    public Game(string fen)
    {
        //board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        board = new Board(fen);
        player = new Player(Colour.White);
        computer = new Computer(Colour.Black);
    }

    public Player getPlayer()
    {
        return player;
    }

    public Computer getComputer()
    {
        return computer;
    }

    public Board getBoard()
    {
        return board;
    }
}
