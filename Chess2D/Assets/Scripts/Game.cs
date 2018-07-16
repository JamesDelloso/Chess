using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game {

    private Player player;
    private Board board;
    private Board realBoard;

    public Game()
    {
        //new Game("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    public Game(string fen)
    {
        //board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        board = new Board(fen);
    }

    public Player getPlayer()
    {
        return player;
    }

    public Board getBoard()
    {
        return board;
    }
}
