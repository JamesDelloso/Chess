using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class PlayerOld : NetworkBehaviour {

    public List<Piece> pieces = new List<Piece>();
    public Colour colour;

    public void Start()
    {
        //Game.board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        if (Game.player1 == null)
        {
            //Game.player1 = this;
            name = "Player 1";
            colour = Colour.White;
            if (isLocalPlayer || Game.mode == Game.Mode.SinglePlayer)
            {
                GameObject.Find("Board").name = "White Board";
                loadPieces();
            }
            //Game.currentPlayer = this;
        }
        else
        {
            //Game.player2 = this;
            name = "Player 2";
            colour = Colour.Black;
            if (isLocalPlayer)
            {
                GameObject.Find("Board").transform.eulerAngles = new Vector3(0, 0, 180);
                GameObject.Find("Board").name = "Black Board";
                loadPieces();
            }
        }
    }

    public void loadPieces()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject.Find(i.ToString() + "," + j.ToString()).GetComponent<Image>().material = (Material)Resources.Load("Materials/LightSquare");
                if ((i+j) % 2 == 0)
                {
                    GameObject.Find(i.ToString() + "," + j.ToString()).GetComponent<Image>().material = (Material)Resources.Load("Materials/DarkSquare");
                }
                if (GameObject.Find(i.ToString() + "," + j.ToString()).transform.childCount > 0)
                {
                    Destroy(GameObject.Find(i.ToString() + "," + j.ToString()).transform.GetChild(0).gameObject);
                }
                if (Game.board.squares[i, j] != null)
                {
                    GameObject go = (GameObject)Instantiate(Resources.Load(Game.board.squares[i, j].ToString().Replace(" ", string.Empty)));
                    go.name = Game.board.squares[i, j].ToString();
                    go.transform.SetParent(GameObject.Find(i.ToString() + "," + j.ToString()).transform);
                    go.transform.position = GameObject.Find(i.ToString() + "," + j.ToString()).transform.position;
                    if(Game.board.squares[i, j].colour == colour)
                    {
                        pieces.Add(Game.board.squares[i, j]);
                    }
                }
            }
        }
        foreach(GameObject dot in GameObject.FindGameObjectsWithTag("dot"))
        {
            Destroy(dot);
        }
    }

    public void move(int a, int b, int c, int d)
    {
        Piece rookToCastle = Game.board.isCastling(a, b, c, d);
        if (rookToCastle != null)
        {
            string pos = Game.board.getPosition(rookToCastle).x.ToString() + "," + Game.board.getPosition(rookToCastle).y.ToString();
            GameObject rook = GameObject.Find(pos).transform.GetChild(0).gameObject;
            pos = ((a + c) / 2).ToString() + "," + Game.board.getPosition(rookToCastle).y.ToString();
            GameObject square = GameObject.Find(pos).gameObject;
            rook.transform.SetParent(square.transform);
            rook.transform.position = square.transform.position;
        }
        else if(Game.board.enPassant == new Vector2Int(c, d))
        {
            if(Game.board.squares[a, b].colour == Colour.White)
            {
                pieceTaken(GameObject.Find(c.ToString() + "," + (d - 1).ToString()).transform.GetChild(0).gameObject);
            }
            else
            {
                pieceTaken(GameObject.Find(c.ToString() + "," + (d + 1).ToString()).transform.GetChild(0).gameObject);
            }
        }
        try
        {
            pieceTaken(GameObject.Find(c.ToString() + "," + d.ToString()).transform.GetChild(0).gameObject);
        }
        catch { }
        GameObject piece = GameObject.Find(a.ToString() + "," + b.ToString()).transform.GetChild(0).gameObject;
        GameObject parent = GameObject.Find(c.ToString() + "," + d.ToString()).gameObject;
        piece.transform.SetParent(parent.transform);
        piece.transform.position = parent.transform.position;
        if (UI.prevSquare1 != null)
        {
            int file = int.Parse(UI.prevSquare1.name.Substring(0, 1));
            int rank = int.Parse(UI.prevSquare1.name.Substring(2, 1));
            UI.prevSquare1.GetComponent<Image>().material = (Material)Resources.Load("Materials/LightSquare");
            if ((file + rank) % 2 == 0)
            {
                UI.prevSquare1.GetComponent<Image>().material = (Material)Resources.Load("Materials/DarkSquare");
            }
            file = int.Parse(UI.prevSquare2.name.Substring(0, 1));
            rank = int.Parse(UI.prevSquare2.name.Substring(2, 1));
            UI.prevSquare2.GetComponent<Image>().material = (Material)Resources.Load("Materials/LightSquare");
            if ((file + rank) % 2 == 0)
            {
                UI.prevSquare2.GetComponent<Image>().material = (Material)Resources.Load("Materials/DarkSquare");
            }
        }
        UI.prevSquare1 = GameObject.Find(a.ToString() + "," + b.ToString());
        UI.prevSquare2 = GameObject.Find(c.ToString() + "," + d.ToString());
        UI.prevSquare1.GetComponent<Image>().material = (Material)Resources.Load("Materials/PrevSquare");
        UI.prevSquare2.GetComponent<Image>().material = (Material)Resources.Load("Materials/PrevSquare");
    }

    [Command]
    public void CmdMove(int a, int b, int c, int d)
    {
        RpcMove(a, b, c, d);
    }

    [ClientRpc]
    public void RpcMove(int a, int b, int c, int d)
    {
        if(!isLocalPlayer)
        {
            move(a, b,c, d);
        }
        if(Game.mode == Game.Mode.Multiplayer)
        {
            if (Game.currentPlayer == Game.player1)
            {
                Game.currentPlayer = Game.player2;
            }
            else
            {
                Game.currentPlayer = Game.player1;
            }
        }
        Game.board.movePiece(a, b, c, d);
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
                Game.board.squares[a, b] = new Queen(Colour.White);
            }
            else if(piece == "Rook")
            {
                Game.board.squares[a, b] = new Rook(Colour.White);
            }
            else if (piece == "Bishop")
            {
                Game.board.squares[a, b] = new Bishop(Colour.White);
            }
            else if (piece == "Knight")
            {
                Game.board.squares[a, b] = new Knight(Colour.White);
            }
        }
        else if (colour == "Black")
        {
            if (piece == "Queen")
            {
                Game.board.squares[a, b] = new Queen(Colour.Black);
            }
            else if (piece == "Rook")
            {
                Game.board.squares[a, b] = new Rook(Colour.Black);
            }
            else if (piece == "Bishop")
            {
                Game.board.squares[a, b] = new Bishop(Colour.Black);
            }
            else if (piece == "Knight")
            {
                Game.board.squares[a, b] = new Knight(Colour.Black);
            }
        }
        Game.board.movePiece(a, b, c, d);
        try
        {
            GameObject to = GameObject.Find(c.ToString() + "," + d.ToString()).transform.GetChild(0).gameObject;
            Destroy(to);
        }
        catch { }
        if(!isLocalPlayer)
        {
            Destroy(GameObject.Find(a.ToString() + "," + b.ToString()).transform.GetChild(0).gameObject);
        }
        GameObject go = (GameObject)Instantiate(Resources.Load(Game.board.squares[c, d].ToString().Replace(" ", string.Empty)));
        GameObject parent = GameObject.Find(c.ToString() + "," + d.ToString()).gameObject;
        go.transform.SetParent(parent.transform);
        go.transform.position = parent.transform.position;

        if (Game.currentPlayer == Game.player1)
        {
            Game.currentPlayer = Game.player2;
        }
        else
        {
            Game.currentPlayer = Game.player1;
        }
        seeIfCheckOrStaleMate();
    }

    public void pieceTaken(GameObject piece)
    {
        if (piece.name.Contains("Black"))
        {
            if (Game.mode == Game.Mode.SinglePlayer)
            {
                Game.ai.pieces.Remove(Game.board.squares[int.Parse(piece.transform.parent.name.Split(',')[0]), int.Parse(piece.transform.parent.name.Split(',')[1])]);
            }
            else
            {
                Game.player2.pieces.Remove(Game.board.squares[int.Parse(piece.transform.parent.name.Split(',')[0]), int.Parse(piece.transform.parent.name.Split(',')[1])]);
            }
            if (!Game.whitePiecesTaken.Contains(piece.name))
            {
                Game.whitePiecesTaken.Add(piece.name);
                piece.transform.parent = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.Count - 1);
                piece.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.Count - 1).transform.position;
            }
            else
            {
                piece.transform.parent = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name));
                piece.transform.position = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)).transform.position;
                piece.transform.parent.GetComponent<TextMesh>().text = GameObject.Find("White Taken Pieces").transform.GetChild(Game.whitePiecesTaken.IndexOf(piece.name)).childCount.ToString();
            }
        }
        else
        {
            Game.player1.pieces.Remove(Game.board.squares[int.Parse(piece.transform.parent.name.Split(',')[0]), int.Parse(piece.transform.parent.name.Split(',')[1])]);
            if (!Game.blackPiecesTaken.Contains(piece.name))
            {
                Game.blackPiecesTaken.Add(piece.name);
                piece.transform.parent = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.Count - 1);
                piece.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.Count - 1).transform.position;
            }
            else
            {
                piece.transform.parent = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name));
                piece.transform.position = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)).transform.position;
                piece.transform.parent.GetComponent<TextMesh>().text = GameObject.Find("Black Taken Pieces").transform.GetChild(Game.blackPiecesTaken.IndexOf(piece.name)).childCount.ToString();
            }
        }
        piece.transform.localScale = new Vector3(100, 100, 100);
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

    [Command]
    public void CmdWriteInChat(string chatText)
    {
        RpcWriteInChat(chatText);
    }

    [ClientRpc]
    public void RpcWriteInChat(string chatText)
    {
        string text = "\nPlayer 1: ";
        if(this == Game.player2)
        {
            text = "\nPlayer 2: ";
        }
        text += chatText;
        GameObject.Find("Chat").transform.GetChild(0).GetComponent<Text>().text += text;
    }
}
