﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Game : MonoBehaviour {

    public static Board board;
    public static Player player1;
    public static Player player2;
    public static Player currentPlayer;

    public static ArrayList whitePiecesTaken = new ArrayList();
    public static ArrayList blackPiecesTaken = new ArrayList();

    public enum Mode { SinglePlayer, Multiplayer};
    public static Mode mode;

    // Use this for initialization
    void Awake() {
        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    public void Start()
    {
        if(mode == Mode.SinglePlayer)
        {
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            player.GetComponent<NetworkIdentity>().localPlayerAuthority = true;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}