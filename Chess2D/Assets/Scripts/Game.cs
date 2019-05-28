using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;

public class Game : MonoBehaviourPunCallbacks {

    public static Board board;
    public static PlayerOld player1;
    public static PlayerOld player2;
    public static AIOld ai;
    public static PlayerOld currentPlayer;

    public static ArrayList whitePiecesTaken = new ArrayList();
    public static ArrayList blackPiecesTaken = new ArrayList();

    public enum Mode { SinglePlayer, Multiplayer};
    public static Mode mode;

    public static int roomNum;

    public static List<Board> boardHistory = new List<Board>();

    // Use this for initialization
    void Awake() {
        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        //board = new Board("8/1k4P1/8/8/8/8/8/4K3 w KQkq - 0 1");
        //board = new Board("rnbqkbnr/ppppppp1/8/8/4P1p1/8/PPPP1P1P/RNBQKBNR w KQkq - 0 2");
        //board = new Board("rnbqkbnr/pppppppp/8/8/3P4/8/PPP1PPPP/RNBQKBNR b KQkq d3 0 1");
    }

    public void Start()
    {
        if (mode == Mode.SinglePlayer)
        {
            ai = new AIOld(Colour.Black);
            //using (StreamReader sr = new StreamReader("Assets/GameStatus.txt"))
            //{
            //    //board = new Board(sr.ReadLine());
            //}
            boardHistory.Add(new Board(FEN.generate(board)));
        }
        else
        {
            //NetworkManager.singleton.StartMatchMaker();
            //NetworkManager.singleton.matchMaker.ListMatches(0, 100, "", true, 0, 0, OnMatchList);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
