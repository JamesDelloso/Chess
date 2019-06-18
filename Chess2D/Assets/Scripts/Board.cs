using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{

    public string fen;

    private string letters = "ABCDEFGH";

    private readonly double[] rookMobility = new double[] {0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.5,1.0,1.0,1.0,1.0,1.0,1.0,0.5,-0.5,0.0,0.0,0.0,0.0,0.0,0.0,-0.5,-0.5,0.0,0.0,0.0,0.0,0.0,0.0,-0.5,-0.5,0.0,0.0,0.0,0.0,0.0,0.0,-0.5,-0.5,0.0,0.0,0.0,0.0,0.0,0.0,-0.5,-0.5,0.0,0.0,0.0,0.0,0.0,0.0,-0.5,0.0,0.0,0.0,0.5,0.5,0.0,0.0,0.0};
    private readonly double[] knightMobility = new double[] { -5.0,-4.0,-3.0,-3.0,-3.0,-3.0,-4.0,-5.0,-4.0,-2.0,0.0,0.0,0.0,0.0,-2.0,-4.0,-3.0,0.0,1.0,1.5,1.5,1.0,0.0,-3.0,-3.0,0.5,1.5,2.0,2.0,1.5,0.5,-3.0,-3.0,0.0,1.5,2.0,2.0,1.5,0.0,-3.0,-3.0,0.5,1.0,1.5,1.5,1.0,0.5,-3.0,-4.0,-2.0,0.0,0.5,0.5,0.0,-2.0,-4.0,-5.0,-4.0,-3.0,-3.0,-3.0,-3.0,-4.0,-5.0};
    private readonly double[] bishopMobility = new double[] { -2.0,-1.0,-1.0,-1.0,-1.0,-1.0,-1.0,-2.0,-1.0,0.0,0.0,0.0,0.0,0.0,0.0,-1.0,-1.0,0.0,0.5,1.0,1.0,0.5,0.0,-1.0,-1.0,0.5,0.5,1.0,1.0,0.5,0.5,-1.0,-1.0,0.0,1.0,1.0,1.0,1.0,0.0,-1.0,-1.0,1.0,1.0,1.0,1.0,1.0,1.0,-1.0,-1.0,0.5,0.0,0.0,0.0,0.0,0.5,-1.0,-2.0,-1.0,-1.0,-1.0,-1.0,-1.0,-1.0,-2.0 };
    private readonly double[] queenMobility = new double[] { -2.0,-1.0,-1.0,-0.5,-0.5,-1.0,-1.0,-2.0,-1.0,0.0,0.0,0.0,0.0,0.0,0.0,-1.0,-1.0,0.0,0.5,0.5,0.5,0.5,0.0,-1.0,-0.5,0.0,0.5,0.5,0.5,0.5,0.0,-0.5,0.0,0.0,0.5,0.5,0.5,0.5,0.0,-0.5,-1.0,0.5,0.5,0.5,0.5,0.5,0.0,-1.0,-1.0,0.0,0.5,0.0,0.0,0.0,0.0,-1.0,-2.0,-1.0,-1.0,-0.5,-0.5,-1.0,-1.0,-2.0 };
    private readonly double[] kingMobility = new double[] { -3.0,-4.0,-4.0,-5.0,-5.0,-4.0,-4.0,-3.0,-3.0,-4.0,-4.0,-5.0,-5.0,-4.0,-4.0,-3.0,-3.0,-4.0,-4.0,-5.0,-5.0,-4.0,-4.0,-3.0,-3.0,-4.0,-4.0,-5.0,-5.0,-4.0,-4.0,-3.0,-2.0,-3.0,-3.0,-4.0,-4.0,-3.0,-3.0,-2.0,-1.0,-2.0,-2.0,-2.0,-2.0,-2.0,-2.0,-1.0,2.0,2.0,0.0,0.0,0.0,0.0,2.0,2.0,2.0,3.0,1.0,0.0,0.0,1.0,3.0,2.0 };
    private readonly double[] pawnMobility = new double[] { 0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0,5.0,5.0,5.0,5.0,5.0,5.0,5.0,5.0,1.0,1.0,2.0,3.0,3.0,2.0,1.0,1.0,0.5,0.5,1.0,2.5,2.5,1.0,0.5,0.5,0.0,0.0,0.0,2.0,2.0,0.0,0.0,0.0,0.5,-0.5,-1.0,0.0,0.0,-1.0,-0.5,0.5,0.5,1.0,1.0,-2.0,-2.0,1.0,1.0,0.5,0.0,0.0,0.0,0.0,0.0,0.0,0.0,0.0 };

    public Board(string fen)
    {
        this.fen = fen;
        Debug.Log(fen);
    }

    public void movePiece(string from, string to)
    {
        from = from.ToUpper();
        string pieces = getPieces();

        int fromPos = (8 - int.Parse(from[1].ToString())) * 9 + letters.IndexOf(from[0]);

        if (to.Length == 4)
        {
            pieces = pieces.Remove(fromPos, 1);
            pieces = pieces.Insert(fromPos, to[3].ToString());
        }

        string piece = pieces[fromPos].ToString();

        to = to.ToUpper();
        bool pieceTaken = false;
        int toPos = (8 - int.Parse(to[1].ToString())) * 9 + letters.IndexOf(to[0]);

        pieces = pieces.Remove(fromPos, 1);
        pieces = pieces.Insert(fromPos, "1");

        if (pieces[toPos] != '1')
        {
            pieceTaken = true;
        }
        pieces = pieces.Remove(toPos, 1);
        pieces = pieces.Insert(toPos, piece);
        if ((piece == "P" || piece == "p") && to == enPassant().ToUpper())
        {
            toPos = toPos - 9;
            if(piece == "P")
            {
                toPos = toPos + 18;
            }
            pieces = pieces.Remove(toPos, 1);
            pieces = pieces.Insert(toPos, "1");
        }

        if (isWhitesTurn())
        {
            fen = fen.Replace(" w ", " b ");
        }
        else
        {
            fen = fen.Replace(" b ", " w ");
            fen = fen.Replace(" "+fen.Split(' ')[5], " "+(int.Parse(fen.Split(' ')[5]) + 1).ToString());
        }
        fen = fen.Replace(fen.Split(' ')[3], "-");
        string castling = fen.Split(' ')[2];
        if (piece == "K")
        {
            if(from == "E1" && to == "G1")
            {
                pieces = pieces.Remove(70,1);
                pieces = pieces.Insert(70, "1");
                pieces = pieces.Remove(68,1);
                pieces = pieces.Insert(68, "R");
            }
            else if (from == "E1" && to == "C1")
            {
                pieces = pieces.Remove(63, 1);
                pieces = pieces.Insert(63, "1");
                pieces = pieces.Remove(66, 1);
                pieces = pieces.Insert(66, "R");
            }
            castling = castling.Replace("K", "");
            castling = castling.Replace("Q", "");
        }
        else if (piece == "R")
        {
            if (from == "A1")
            {
                castling = castling.Replace("Q", "");
            }
            else if (from == "H1")
            {
                castling = castling.Replace("K", "");
            }
        }
        else if(piece == "P" && from[1] == '2' && to[1] == '4')
        {
            fen = fen.Replace(fen.Split(' ')[3], from[0].ToString().ToLower() + "3");
        }
        else if (piece == "k")
        {
            if (from == "E8" && to == "G8")
            {
                pieces = pieces.Remove(7, 1);
                pieces = pieces.Insert(7, "1");
                pieces = pieces.Remove(5, 1);
                pieces = pieces.Insert(5, "r");
            }
            else if (from == "E8" && to == "C8")
            {
                pieces = pieces.Remove(0, 1);
                pieces = pieces.Insert(0, "1");
                pieces = pieces.Remove(3, 1);
                pieces = pieces.Insert(3, "r");
            }
            castling = castling.Replace("k", "");
            castling = castling.Replace("q", "");
        }
        else if (piece == "r")
        {
            if (from == "A8")
            {
                castling = castling.Replace("q", "");
            }
            else if (from == "H8")
            {
                castling = castling.Replace("k", "");
            }
        }
        else if (piece == "p" && from[1] == '7' && to[1] == '5')
        {
            fen = fen.Replace(fen.Split(' ')[3], from[0].ToString().ToLower() + "6");
        }
        if(toPos == 0)
        {
            castling = castling.Replace("q", "");
        }
        else if(toPos == 7)
        {
            castling = castling.Replace("k", "");
        }
        else if (toPos == 70)
        {
            castling = castling.Replace("K", "");
        }
        else if (toPos == 63)
        {
            castling = castling.Replace("Q", "");
        }
        if (castling == "")
        {
            castling = "-";
        }
        fen = fen.Replace(fen.Split(' ')[2], castling);
        if (pieceTaken == false && piece.ToLower() != "p")
        {
            fen = fen.Replace(" "+fen.Split(' ')[4], " "+(int.Parse(fen.Split(' ')[4]) + 1).ToString());
        }
        else
        {
            fen = fen.Replace(" "+fen.Split(' ')[4], " 0");
        }

        int number = 0;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i] == '1')
            {
                pieces = pieces.Remove(i, 1);
                i--;
                number++;
            }
            else if (number > 0)
            {
                pieces = pieces.Insert(i, number.ToString());
                i++;
                number = 0;
            }
        }
        if (number > 0)
        {
            pieces = pieces.Insert(pieces.Length, number.ToString());
        }

        fen = fen.Replace(fen.Split(' ')[0], pieces);
    }

    public bool isWhitesTurn()
    {
        if(fen.Split(' ')[1] == "w")
        {
            return true;
        }
        return false;
    }

    public string enPassant()
    {
        return fen.Split(' ')[3];
    }

    public bool canCastle(char side)
    {
        return fen.Split(' ')[2].Contains(side.ToString());
    }

    public char getPiece(string position)
    {
        return getPieces()[(8 - int.Parse(position[1].ToString())) * 9 + letters.IndexOf(position[0].ToString().ToUpper())];
    }

    public bool isCheck(char king)
    {
        string pieces = getPieces();
        string kingPos = letters[pieces.IndexOf(king) % 9] + "" + (8 - pieces.IndexOf(king) / 9);
        if (PossibleMoves.isAttacked(this, kingPos))
        {
            return true;
        }
        return false;
    }

    public bool isCheckMate(char king)
    {
        if(isCheck(king) == false)
        {
            return false;
        }
        bool isWhite = true;
        if (char.IsLower(king))
        {
            isWhite = false;
        }
        string pieces = getPieces();
        for(int i=0;i<pieces.Length;i++)
        {
            if ((char.IsUpper(pieces[i]) && isWhite && PossibleMoves.search(this, letters[i % 9] + "" + (8 - i / 9)).Count > 0) || (char.IsLower(pieces[i]) && !isWhite && PossibleMoves.search(this, letters[i % 9] + "" + (8 - i / 9)).Count > 0))
            {
                return false;
            }
        }
        return true;
    }

    public bool isStalemate(char king)
    {
        if (isCheck(king))
        {
            return false;
        }
        bool isWhite = true;
        if (char.IsLower(king))
        {
            isWhite = false;
        }
        string pieces = getPieces();
        for (int i = 0; i < pieces.Length; i++)
        {
            if ((char.IsUpper(pieces[i]) && isWhite && PossibleMoves.search(this, letters[i % 9] + "" + (8 - i / 9)).Count > 0) || (char.IsLower(pieces[i]) && !isWhite && PossibleMoves.search(this, letters[i % 9] + "" + (8 - i / 9)).Count > 0))
            {
                return false;
            }
        }
        return true;
    }

    public void promotePawn(char newPiece, string from, string to)
    {
        string pieces = getPieces();
        int fromPos = (8 - int.Parse(from[1].ToString())) * 9 + letters.IndexOf(from[0]);
        pieces = pieces.Remove(fromPos, 1);
        pieces = pieces.Insert(fromPos, newPiece.ToString());
        fen = fen.Replace(fen.Split(' ')[0], pieces);
        movePiece(from, to);
    }

    public string getPieces()
    {
        string pieces = fen.Split(' ')[0];
        for (int i = 0; i < pieces.Length; i++)
        {
            int num;
            if (int.TryParse(pieces[i].ToString(), out num))
            {
                pieces = pieces.Remove(i, 1);
                for (int j = 0; j < num; j++)
                {
                    pieces = pieces.Insert(i, "1");
                }
                i += num;
            }
        }
        return pieces;
    }

    public float getValue(bool forWhite)
    {
        float value = 0;

        string pieces = getPieces();
        for(int i=0;i<pieces.Length;i++)
        {
            if((forWhite && char.IsUpper(pieces[i])) || (!forWhite && char.IsLower(pieces[i])))
            {
                int mobIndex = i - (i % 9);
                if(forWhite == false)
                {
                    mobIndex = 63 - mobIndex;
                }
                char piece = char.ToUpper(pieces[i]);
                if (piece == 'R')
                {
                    value += 50 + (float)rookMobility[mobIndex];
                }
                else if (piece == 'N')
                {
                    value += 30 + (float)knightMobility[mobIndex];
                }
                else if (piece == 'B')
                {
                    value += 30 + (float)bishopMobility[mobIndex];
                }
                else if (piece == 'Q')
                {
                    value += 90 + (float)queenMobility[mobIndex];
                }
                else if (piece == 'P')
                {
                    value += 10 + (float)pawnMobility[mobIndex];
                }
                else if (piece == 'K')
                {
                    value += 1000 + (float)kingMobility[mobIndex];
                }
            }
            else if(pieces[i] != '/')
            {
                int mobIndex = i - (i % 9);
                if (forWhite == false)
                {
                    mobIndex = 63 - mobIndex;
                }
                char piece = char.ToUpper(pieces[i]);
                if (piece == 'R')
                {
                    value += -50 - (float)rookMobility[mobIndex];
                }
                else if (piece == 'N')
                {
                    value += -30 - (float)knightMobility[mobIndex];
                }
                else if (piece == 'B')
                {
                    value += -30 - (float)bishopMobility[mobIndex];
                }
                else if (piece == 'Q')
                {
                    value += -90 - (float)queenMobility[mobIndex];
                }
                else if (piece == 'P')
                {
                    value += -10 - (float)pawnMobility[mobIndex];
                }
                else if (piece == 'K')
                {
                    value += -1000 - (float)kingMobility[mobIndex];
                }
            }
        }
        return value;
    }
}
