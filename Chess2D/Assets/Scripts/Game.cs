using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Game : MonoBehaviour {

    public static Board board;
    public static Player player1;
    public static Player player2;

	// Use this for initialization
	void Awake() {
        board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
