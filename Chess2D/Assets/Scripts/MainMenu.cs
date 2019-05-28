using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour {

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void loadSinglePlayer()
    {
        SceneManager.LoadScene("NewSP");
    }

    public void loadMultiplayer()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }
}
