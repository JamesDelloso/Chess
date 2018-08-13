using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void singlePlayer()
    {
        using (StreamWriter sw = new StreamWriter("Assets/GameStatus.txt", false))
        {
            sw.WriteLine("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        }
        SceneManager.LoadSceneAsync("SinglePlayer");
    }

    public void multiplayer()
    {
        SceneManager.LoadSceneAsync("Multiplayer");
    }
}
