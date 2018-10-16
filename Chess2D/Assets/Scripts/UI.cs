using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour {

    public static Piece selectedPiece;
    public static GameObject selectedGO;
    public static bool promotingPawn = false;
    public static Vector2Int promotingPos;

    private GameObject[] prevSquares = new GameObject[0];
    private Material[] prevMaterials = new Material[0];
    private Material prevMaterial;

    public static GameObject prevSquare1;
    public static GameObject prevSquare2;
    //public static List<GameObject> 

    private int undoIndex = -1;

    public Font arial;

    //private AI ai = new AI(Colour.Black);

    // Use this for initialization
    void Start()
    {
        //GameObject.Find("Room No.").GetComponent<Text>().text = "Room No: " + Game.roomNum;
        if (FEN.generate(Game.board) != "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1")
        {
            //GameObject.Find("Continue Game").GetComponent<Canvas>().enabled = true;
        }
        if(Game.board.whitesTurn == false)
        {
            Invoke("playAI", 1);
            undoIndex++;
            using (StreamWriter sw = new StreamWriter("Assets/GameStatus.txt", false))
            {
                sw.WriteLine(FEN.generate(Game.board));
            }
        }
    }

	// Update is called once per frame
	void Update () {
        
    }

    public void squareSelected(Image image)
    {
        if(promotingPawn == false && ((Game.mode == Game.Mode.SinglePlayer && GameObject.Find("Continue Game").GetComponent<Canvas>().enabled == false )|| Game.player2 != null))
        {
            int file = int.Parse(image.name.Substring(0, 1));
            int rank = int.Parse(image.name.Substring(2, 1));
            bool samePiece = false;
            if (Game.board.getPosition(selectedPiece) == new Vector2Int(file, rank))
            {
                //samePiece = true;
            }
            for (int i = 0; i < prevSquares.Length; i++)
            {
                Destroy(prevSquares[i]);
            }
            if(prevMaterial != null && selectedGO != null)
            {
                selectedGO.transform.parent.GetComponent<Image>().material = prevMaterial;
            }
            if (selectedPiece != null && selectedPiece.generatePossibleMoves(Game.board).Contains(new Vector2Int(file, rank)))
            {
                int x = Game.board.getPosition(selectedPiece).x;
                int y = Game.board.getPosition(selectedPiece).y;
                if (selectedPiece.GetType().Equals(typeof(Pawn)) && rank == 7 || selectedPiece.GetType().Equals(typeof(Pawn)) && rank == 0)
                {
                    if(rank == 7)
                    {
                        GameObject.Find("White Promotion").GetComponent<Canvas>().enabled = true;
                    }
                    else
                    {
                        GameObject.Find("Black Promotion").GetComponent<Canvas>().enabled = true;
                    }
                    promotingPawn = true;
                    promotingPos = new Vector2Int(file, rank);
                    try
                    {
                        Destroy(GameObject.Find(file.ToString() + "," + rank.ToString()).transform.GetChild(0).gameObject);
                    }
                    catch { }
                    selectedGO.transform.SetParent(GameObject.Find(file.ToString() + "," + rank.ToString()).transform);
                    selectedGO.transform.position = GameObject.Find(file.ToString() + "," + rank.ToString()).transform.position;
                }
                else
                {
                    int audioSource = 0;
                    if (Game.board.squares[file, rank] != null)
                    {
                        audioSource = 1;
                    }
                    Game.currentPlayer.move(x, y, file, rank);
                    if(Game.mode == Game.Mode.Multiplayer)
                    {
                        Game.currentPlayer.CmdMove(x, y, file, rank);
                    }
                    else
                    {
                        //Lags game badly.
                        //
                        //if (undoIndex + 1 != Game.boardHistory.Count - 1)
                        //{
                        //    List<Board> newHistory = Game.boardHistory;
                        //    //Game.boardHistory = new List<Board>();
                        //    //Game.board.moves = Game.boardHistory[undoIndex+1].moves;
                        //    for (int i = 0; i < undoIndex + 2; i++)
                        //    {
                        //        newHistory.Add(new Board(FEN.generate(Game.boardHistory[i])));
                        //        newHistory[i].moves = Game.boardHistory[i].moves;
                        //    }
                        //    Game.boardHistory = newHistory;
                        //    //Game.board.moves = Game.boardHistory[Game.boardHistory.Count - 1].moves;
                        //    //Game.boardHistory.Add(Game.board);
                        //    //print(undoIndex + 2);
                        //    for (int i = undoIndex; i < GameObject.Find("Moves Grid").transform.childCount; i++)
                        //    {
                        //        Destroy(GameObject.Find("Moves Grid").transform.GetChild(i).gameObject);
                        //    }
                        //    undoIndex = Game.boardHistory.Count - 2;
                        //}
                        Game.board.movePiece(x, y, file, rank);
                        Game.currentPlayer.seeIfCheckOrStaleMate();
                        //print(Game.board.moves.Count);
                        undoIndex++;
                        Invoke("playAI", 1);
                        //int a;
                        //int b;
                        //int c;
                        //int d;
                        //Game.ai.getMove(Game.board, out a, out b, out c, out d);
                        ////print(a + "," + b + " : " + c + "," + d);
                        //Game.currentPlayer.move(a, b, c, d);
                        //Game.board.movePiece(a, b, c, d);
                        //Game.currentPlayer.seeIfCheckOrStaleMate();
                        //Game.board.whitesTurn = !Game.board.whitesTurn;
                        undoIndex++;
                        using (StreamWriter sw = new StreamWriter("Assets/GameStatus.txt", false))
                        {
                            sw.WriteLine(FEN.generate(Game.board));
                        }
                        updateMoves();
                    }
                    selectedPiece = null;
                    selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    selectedGO = null;
                    if (Game.board.wKing.isCheck(Game.board) || Game.board.bKing.isCheck(Game.board))
                    {
                        audioSource = 2;
                    }
                    GetComponents<AudioSource>()[audioSource].Play();
                }
            }
            else if (selectedPiece != null && !selectedPiece.generatePossibleMoves(Game.board).Contains(new Vector2Int(file, rank)))
            {
                selectedGO.transform.localPosition = Vector3.zero;
                selectedPiece = null;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                selectedGO = null;
            }
            if (samePiece == false && Game.board.getPiece(file, rank) != null && ((Game.board.getPiece(file, rank).colour == Colour.White && Game.board.whitesTurn && name == "White Board") || (Game.board.getPiece(file, rank).colour == Colour.Black && !Game.board.whitesTurn && name == "Black Board")))
            {
                List<Vector2Int> possibleMoves = Game.board.getPiece(file, rank).generatePossibleMoves(Game.board);
                prevMaterials = new Material[possibleMoves.Count + 1];
                prevSquares = new GameObject[possibleMoves.Count + 1];
                for (int i = 0; i < possibleMoves.Count; i++)
                {
                    Vector2Int sq = possibleMoves[i];
                    GameObject dot = new GameObject();
                    dot.name = "dot";
                    dot.transform.parent = GameObject.Find("Canvas").transform;
                    dot.transform.position = GameObject.Find(sq.x.ToString() + "," + sq.y.ToString()).transform.position;
                    dot.AddComponent<Image>();
                    Sprite sprite = Resources.Load<Sprite>("Materials/Dot");
                    dot.GetComponent<Image>().sprite = sprite;
                    dot.GetComponent<Image>().color = new Color32(0, 0, 0, 75);
                    dot.GetComponent<Image>().raycastTarget = false;
                    dot.GetComponent<RectTransform>().sizeDelta = new Vector2(70,70);
                    dot.GetComponent<RectTransform>().localScale = Vector3.one;
                    dot.gameObject.tag = "dot";
                    prevSquares[i] = dot;
                }
                prevMaterial = image.material;
                image.material = (Material)Resources.Load("Materials/SelectedSquare");
                selectedPiece = Game.board.getPiece(file, rank);
                selectedGO = GameObject.Find(file + "," + rank).transform.GetChild(0).gameObject;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 2;
                //Debug.Log(selectedPiece.getMobilityValue(file, rank));
            }
        }
    }

    public void playAI()
    {
        int a;
        int b;
        int c;
        int d;
        Game.ai.getMove(Game.board, out a, out b, out c, out d);
        //print(a + "," + b + " : " + c + "," + d);
        Game.currentPlayer.move(a, b, c, d);
        Game.board.movePiece(a, b, c, d);
        Game.currentPlayer.seeIfCheckOrStaleMate();
        //Game.board.whitesTurn = true;
        print(Game.board.whitesTurn);
    }

    public void onDragPiece()
    {
        if(selectedPiece != null && promotingPawn == false)
        {
            Vector3 v = Input.mousePosition;
            v.z = 100;
            v = Camera.main.ScreenToWorldPoint(v);
            if (Input.GetMouseButtonUp(0))
            {
                squareSelected(selectedGO.transform.parent.GetComponent<Image>());
            }
            else
            {
                v.x = Mathf.Clamp(v.x, -50, 50);
                v.y = Mathf.Clamp(v.y, -50, 50);
                selectedGO.transform.position = v;
            }
        }
    }

    public void selectPromotion(Image image)
    {

        GameObject.Find("White Promotion").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Black Promotion").GetComponent<Canvas>().enabled = false;
        Vector2Int square = Game.board.getPosition(selectedPiece);
        string colour = "White";
        if(selectedPiece.colour == Colour.Black)
        {
            colour = "Black";
        }
        Game.currentPlayer.CmdpromotePawn(image.name, colour, square.x, square.y, promotingPos.x, promotingPos.y);
        selectedPiece = null;
        selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
        selectedGO = null;
        promotingPawn = false;
    }

    public void writeInChat(InputField chatText)
    {
        if(Game.player1.GetComponent<NetworkIdentity>().hasAuthority == true)
        {
            Game.player1.CmdWriteInChat(chatText.text);
        }
        else
        {
            Game.player2.CmdWriteInChat(chatText.text);
        }
        chatText.text = "";
    }

    public void fenEntered(InputField fenInput)
    {
        print(fenInput.text);
        try
        {
            Game.board = new Board(fenInput.text);
            Game.currentPlayer.loadPieces();
        }
        catch
        {
        }
        fenInput.text = "";
    }

    public void copyFEN()
    {
        TextEditor te = new TextEditor();
        te.text = FEN.generate(Game.board);
        te.SelectAll();
        te.Copy();
    }

    public void newGame()
    {
        GameObject.Find("Game End").GetComponent<Canvas>().enabled = false;
        Game.board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        Game.currentPlayer.loadPieces();
        selectedPiece = null;
        if(selectedGO != null)
        {
            selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
        }
        selectedGO = null;
        using (StreamWriter sw = new StreamWriter("Assets/GameStatus.txt", false))
        {
            sw.WriteLine(FEN.generate(Game.board));
        }
    }

    public void continueGame(string answer)
    {
        if(answer == "no")
        {
            newGame();
        }
        GameObject.Find("Continue Game").GetComponent<Canvas>().enabled = false;
        GameObject.Find("Canvas").GetComponent<AudioSource>().Play();

    }


    public void menu()
    {
        GameObject.Find("Canvas").GetComponent<AudioSource>().Play();
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void undo()
    {
        print(undoIndex);
        if(undoIndex == Game.boardHistory.Count - 1)
        {
            undoIndex--;
        }
        Game.boardHistory[undoIndex].moves = Game.board.moves;
        Game.board = Game.boardHistory[undoIndex];
        //Game.board.moves.RemoveAt(Game.board.moves.Count - 1);
        Game.player1.loadPieces();
        for (int i = 0; i < GameObject.Find("Moves Grid").transform.childCount; i++)
        {
            GameObject.Find("Moves Grid").transform.GetChild(i).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        }
        if (undoIndex > 0)
        {
            GameObject.Find("Moves Grid").transform.GetChild(undoIndex - 1).GetComponent<Text>().color = new Color32(240, 175, 40, 255);
            undoIndex--;
        }
        //updateMoves();
    }

    public void redo()
    {
        if(undoIndex != Game.boardHistory.Count - 1)
        {
            Game.boardHistory[undoIndex].moves = Game.board.moves;
            Game.board = Game.boardHistory[undoIndex + 1];
            //Game.board.moves.RemoveAt(Game.board.moves.Count - 1);
            Game.player1.loadPieces();
            for (int i = 0; i < GameObject.Find("Moves Grid").transform.childCount; i++)
            {
                GameObject.Find("Moves Grid").transform.GetChild(i).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
            }
            GameObject.Find("Moves Grid").transform.GetChild(undoIndex).GetComponent<Text>().color = new Color32(240, 175, 40, 255);
            undoIndex++;
        }
        //updateMoves();
    }

    public void updateMoves()
    {
        GameObject moveText = (GameObject)Instantiate(Resources.Load("MoveText"));
        moveText.name = Game.board.moves[Game.board.moves.Count - 1];
        string num = "";
        if(Game.board.moves.Count % 2 != 0)
        {
            if(Game.board.moves.Count / 2 < 10)
            {
                num = " ";
            }
            if(Game.board.moves.Count > 10)
            {
                GameObject.Find("Moves Grid").GetComponent<RectTransform>().sizeDelta = new Vector2(200, GameObject.Find("Moves Grid").GetComponent<RectTransform>().sizeDelta.y + 40);
                GameObject.Find("Moves Grid").GetComponent<RectTransform>().localPosition = new Vector3(0, GameObject.Find("Moves Grid").GetComponent<RectTransform>().sizeDelta.y, 0);
            }
            num += Game.board.moves.Count / 2 + 1 + ". ";
        }
        moveText.GetComponent<Text>().text = num + moveText.name;
        moveText.transform.SetParent(GameObject.Find("Moves Grid").transform, false);
        for (int i = 0; i < GameObject.Find("Moves Grid").transform.childCount-1; i++)
        {
            GameObject.Find("Moves Grid").transform.GetChild(i).GetComponent<Text>().color = new Color32(255, 255, 255, 255);
        }
    }
}
