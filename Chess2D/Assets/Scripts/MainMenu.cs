using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : NetworkBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void singlePlayer()
    {
        GameObject.Find("Panel").GetComponent<AudioSource>().Play();
        Game.mode = Game.Mode.SinglePlayer;
        SceneManager.LoadSceneAsync("SinglePlayer");
    }

    public void multiplayer()
    {
        GameObject.Find("Panel").GetComponent<AudioSource>().Play();
        Game.mode = Game.Mode.Multiplayer;
        SceneManager.LoadSceneAsync("Multiplayer");
        //SceneManager.LoadSceneAsync("MultiplayerLobby");
        //SceneManager.LoadSceneAsync("Multiplayer",LoadSceneMode.Additive);
    }
}
