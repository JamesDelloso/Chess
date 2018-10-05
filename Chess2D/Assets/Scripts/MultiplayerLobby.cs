using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerLobby : MonoBehaviour {

    public bool joiningRoom = false;

	// Use this for initialization
	void Start () {
        NetworkManager.singleton.StartMatchMaker();
        refreshRoomList();
        InvokeRepeating("refreshRoomList", 0, 1);
    }
	
	// Update is called once per frame
	void Update () {

    }
    
    public void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            //NetworkServer.Listen(matchInfo, 7777);
            NetworkManager.singleton.StartHost(matchInfo);
            Game.mode = Game.Mode.Multiplayer;
            GameObject.Find("Lobby").GetComponent<Canvas>().enabled = false;
            CancelInvoke("refreshRoomList");
        }
    }

    public void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (success)
        {
            NetworkManager.singleton.StartClient(matchInfo);
            Game.mode = Game.Mode.Multiplayer;
            GameObject.Find("Lobby").GetComponent<Canvas>().enabled = false;
            CancelInvoke("refreshRoomList");
        }

    }

    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
    {
        if (success && GameObject.Find("RoomList").transform.childCount < matches.Count)
        {
            //print(matches.Count + " " + matchesCount);
            for (int i = 0;i<matches.Count;i++)
            {
                GameObject room = (GameObject)Instantiate(Resources.Load("Room"));
                room.transform.SetParent(GameObject.Find("RoomList").transform);
                room.transform.position = room.transform.parent.position;
                room.transform.localScale = Vector3.one;
                room.name = "Room " + (i + 1);
                room.transform.GetChild(0).GetComponent<Text>().text = room.name;
                room.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { joinRoom(matches[i - 1].networkId, room.transform.GetChild(1).GetComponent<Button>()); });
                room.transform.GetChild(1).GetComponent<Button>().interactable = true;
            } 
        }
    }

    private void refreshRoomList()
    {
        NetworkManager.singleton.matchMaker.ListMatches(0, 100, "", false, 0, 0, OnMatchList);
    }

    public void createRoom()
    {
        GameObject.Find("Canvas").GetComponent<AudioSource>().Play();
        GameObject.Find("Create Room").GetComponent<Button>().interactable = false;
        if(joiningRoom == false)
        {
            NetworkManager.singleton.matchMaker.CreateMatch("Room " + (GameObject.Find("RoomList").transform.childCount + 1), 2, true, "", "", "", 0, 0, OnMatchCreate);
            joiningRoom = true;
        }
    }

    public void joinRoom(NetworkID netId, Button button)
    {
        GameObject.Find("Canvas").GetComponent<AudioSource>().Play();
        button.interactable = false;
        if(joiningRoom == false)
        {
            NetworkManager.singleton.matchMaker.JoinMatch(netId, "", "", "", 0, 0, OnMatchJoined);
            joiningRoom = true;
        }
    }
}
