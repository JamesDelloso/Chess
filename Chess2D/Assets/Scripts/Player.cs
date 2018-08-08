using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : NetworkBehaviour {

    public void Start()
    {
        if (Game.player1 == null)
        {
            Game.player1 = this;
            name = "Player 1";
            if(isLocalPlayer)
            {
                GameObject.Find("Board").name = "White Board";
            }
            UI.player = this;
        }
        else
        {
            Game.player2 = this;
            name = "Player 2";
            if(isLocalPlayer)
            {
                GameObject.Find("Board").transform.eulerAngles = new Vector3(0, 0, 180);
                GameObject.Find("Board").name = "Black Board";
            }
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (GameObject.Find(i.ToString() + "," + j.ToString()).transform.childCount != 0)
                    {
                        GameObject go = GameObject.Find(i.ToString() + "," + j.ToString()).transform.GetChild(0).gameObject;
                        Destroy(go);
                    }
                    if (Game.board.squares[i, j] != null)
                    {
                        GameObject go = (GameObject)Instantiate(Resources.Load(Game.board.squares[i, j].ToString().Replace(" ", string.Empty)));
                        go.name = Game.board.squares[i, j].ToString();
                        go.transform.SetParent(GameObject.Find(i.ToString() + "," + j.ToString()).transform);
                        go.transform.position = GameObject.Find(i.ToString() + "," + j.ToString()).transform.position;
                    }
                }
            }
        }
    }

    [Command]
    public void CmdMove(int a, int b, int c, int d)
    {
        RpcMove(a, b, c, d);
    }

    [ClientRpc]
    public void RpcMove(int a, int b, int c, int d)
    {
        //print(this);
        UI.board.movePiece(a, b, c, d);
        try
        {
            GameObject to = GameObject.Find(c.ToString() + "," + d.ToString()).transform.GetChild(0).gameObject;
            Destroy(to);
        }
        catch { }
        GameObject go = GameObject.Find(a.ToString() + "," + b.ToString()).transform.GetChild(0).gameObject;
        GameObject parent = GameObject.Find(c.ToString() + "," + d.ToString()).gameObject;
        go.transform.SetParent(parent.transform);
        go.transform.position = parent.transform.position;
        if (UI.player == Game.player1)
        {
            UI.player = Game.player2;
        }
        else
        {
            UI.player = Game.player1;
        }
        seeIfCheckOrStaleMate();
    }

    [Command]
    public void CmdpromotePawn(string piece, string colour, int a, int b, int c, int d)
    {
        RpcpromotePawn(piece, colour, a, b, c, d);
    }

    [ClientRpc]
    public void RpcpromotePawn(string piece, string colour, int a, int b, int c, int d)
    {
        if(colour == "White")
        {
            if(piece == "Queen")
            {
                UI.board.squares[a, b] = new Queen(Colour.White);
            }
            else if(piece == "Rook")
            {
                UI.board.squares[a, b] = new Rook(Colour.White);
            }
            else if (piece == "Bishop")
            {
                UI.board.squares[a, b] = new Bishop(Colour.White);
            }
            else if (piece == "Knight")
            {
                UI.board.squares[a, b] = new Knight(Colour.White);
            }
        }
        else if (colour == "Black")
        {
            if (piece == "Queen")
            {
                UI.board.squares[a, b] = new Queen(Colour.Black);
            }
            else if (piece == "Rook")
            {
                UI.board.squares[a, b] = new Rook(Colour.Black);
            }
            else if (piece == "Bishop")
            {
                UI.board.squares[a, b] = new Bishop(Colour.Black);
            }
            else if (piece == "Knight")
            {
                UI.board.squares[a, b] = new Knight(Colour.Black);
            }
        }
        UI.board.movePiece(a, b, c, d);
        try
        {
            GameObject to = GameObject.Find(c.ToString() + "," + d.ToString()).transform.GetChild(0).gameObject;
            Destroy(to);
        }
        catch { }
        Destroy(GameObject.Find(a.ToString() + "," + b.ToString()).transform.GetChild(0).gameObject);
        GameObject go = (GameObject)Instantiate(Resources.Load(UI.board.squares[c, d].ToString().Replace(" ", string.Empty)));
        GameObject parent = GameObject.Find(c.ToString() + "," + d.ToString()).gameObject;
        go.transform.SetParent(parent.transform);
        go.transform.position = parent.transform.position;
        if (UI.player == Game.player1)
        {
            UI.player = Game.player2;
        }
        else
        {
            UI.player = Game.player1;
        }
        seeIfCheckOrStaleMate();
    }

    public void seeIfCheckOrStaleMate()
    {
        if (Game.board.isCheckMate(Game.board.wKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nBlack Wins";
        }
        else if (Game.board.isCheckMate(Game.board.bKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Checkmate!\n\nWhite Wins";
        }
        else if (Game.board.isStaleMate(Game.board.wKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Stalemate!\n\nDraw";
        }
        else if (Game.board.isStaleMate(Game.board.bKing) == true)
        {
            GameObject.Find("Game End").GetComponent<Canvas>().enabled = true;
            GameObject.Find("Game End").transform.GetChild(0).GetComponent<Text>().text = "Stalemate!\n\nDraw";
        }
    }
}
