using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MultiplayerLobby : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject roomList;
    [SerializeField]
    private GameObject roomButtonPrefab;
    [SerializeField]
    private GameObject createRoomButton;

    // Use this for initialization
    void Start () {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();

        createRoomButton.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        createRoomButton.SetActive(true);
    }

    public void createRoom()
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("Game");
    }

    public void joinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> roomListInfo)
    {
        foreach (RoomInfo ri in roomListInfo)
        {
            if (roomList.transform.Find(ri.Name) != null)
            {
                DestroyImmediate(roomList.transform.Find(ri.Name).gameObject);
            }
            if (ri.PlayerCount != 0)
            {
                GameObject rb = Instantiate(roomButtonPrefab);
                rb.transform.SetParent(roomList.transform);
                rb.transform.localPosition = Vector3.back;
                rb.transform.localScale = Vector3.one;
                rb.name = ri.Name;

                rb.GetComponent<Button>().onClick.AddListener(delegate { joinRoom(ri.Name); });

                rb.transform.GetChild(0).GetComponent<TMP_Text>().text = ri.Name;
                if (ri.PlayerCount == ri.MaxPlayers)
                {
                    rb.GetComponent<Button>().interactable = false;
                }
            }
        }
    }
}
