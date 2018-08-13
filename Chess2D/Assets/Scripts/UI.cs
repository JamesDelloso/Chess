using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.EventSystems;

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

    // Use this for initialization
    void Start()
    {
        using (StreamReader sr = new StreamReader("Assets/GameStatus.txt"))
        {
            //Game.board = new Board(sr.ReadLine());
            //Game.board = new Board("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            //Game.board = new Board("rnbqkbnr/pppp1ppp/8/4p3/3PP3/8/PPP2PPP/RNBQKBNR b KQkq e3 0 2");
            //Game.board = new Board("rnbqkbnr/pppp1ppp/8/4pQ2/2BPP3/8/PPP2PPP/RNB1K1NR w KQkq - 7 5");
        }
        //Game.board = Game.Game.board;
        //updatePieces();
    }

	// Update is called once per frame
	void Update () {
        
    }

    public void squareSelected(Image image)
    {
        if(promotingPawn == false && (Game.mode == Game.Mode.SinglePlayer || Game.player2 != null))
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
                    Game.currentPlayer.move(x, y, file, rank);
                    if(Game.mode == Game.Mode.Multiplayer)
                    {
                        Game.currentPlayer.CmdMove(x, y, file, rank);
                    }
                    else
                    {
                        Game.board.movePiece(x, y, file, rank);
                        Game.currentPlayer.seeIfCheckOrStaleMate();
                    }
                    selectedPiece = null;
                    selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 1;
                    selectedGO = null;
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
                    prevSquares[i] = dot;
                }
                prevMaterial = image.material;
                image.material = (Material)Resources.Load("Materials/SelectedSquare");
                selectedPiece = Game.board.getPiece(file, rank);
                selectedGO = GameObject.Find(file + "," + rank).transform.GetChild(0).gameObject;
                selectedGO.GetComponent<SpriteRenderer>().sortingOrder = 2;
            }
        }
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

    public void updateMoves()
    {
        string whiteMoves = "";
        string blackMoves = "";
        for (int i = 0; i < Game.board.moves.Count; i++)
        {
            if (i % 2 == 0)
            {
                whiteMoves += (i/2+1) + ". " + Game.board.moves[i] + "\n";
            }
            else
            {
                blackMoves += Game.board.moves[i] + "\n";
            }
        }
        GameObject.Find("White Moves").GetComponent<Text>().text = whiteMoves;
        GameObject.Find("Black Moves").GetComponent<Text>().text = blackMoves;
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
    }
}
