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

    // Use this for initialization
    void Awake() {
        board = new Board("rnb1kbnr/pp1ppppp/2p5/q7/8/3P1N2/PPP1PPPP/RNBQKB1R w KQkq - 2 3");
        board = new Board("1ppp2k1/6p1/8/8/8/2P5/PPK4p/8 w - - 0 1");
        board = new Board("8/B4pK1/r1pk2PP/Ppbn3b/PppR2pP/1NRPr1PB/1p2QnpP/2N3q1 w - - 0 1");
        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }

    public void Start()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}
}
