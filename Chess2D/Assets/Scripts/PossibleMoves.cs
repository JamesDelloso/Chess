using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PossibleMoves
{
    private static List<string> moves = new List<string>();
    private static string letters = "ABCDEFGH";
    private static int file = 0;
    private static int rank = 0;
    private static string fileStr;
    private static string pieces;
    private static char piece;
    private static Board thisBoard;

    public static List<string> search(Board board, string position)
    {
        thisBoard = board;
        file = letters.IndexOf(position[0].ToString().ToUpper());
        rank = int.Parse(position[1].ToString());
        fileStr = position[0].ToString().ToUpper();
        pieces = board.fen.Split(' ')[0];

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

        piece = pieces[(8 - rank) * 9 + file];
        char king = 'K';
        if (char.IsLower(piece))
        {
            king = 'k';
        }
        if (piece == 'R' || piece == 'r')
        {
            moves = rookSearch();
        }
        else if (piece == 'N' || piece == 'n')
        {
            moves = knightSearch();
        }
        else if (piece == 'B' || piece == 'b')
        {
            moves = bishopSearch();
        }
        else if (piece == 'Q' || piece == 'q')
        {
            moves = rookSearch();
            moves.AddRange(bishopSearch());
        }
        else if (piece == 'K' || piece == 'k')
        {
            moves = kingSearch(false);
        }
        else if (piece == 'P' || piece == 'p')
        {
            moves = pawnSearch(board.enPassant());
        }
        for (int i = 0; i < moves.Count; i++)
        {
            string fen = board.fen;
            board.movePiece(position, moves[i]);
            if (isAttacked(board, letters[board.getPieces().IndexOf(king) % 9] + "" + (8 - board.getPieces().IndexOf(king) / 9)))
            {
                moves.Remove(moves[i]);
                i--;
            }
            board.fen = fen;
        }
        return moves;
    }

    public static bool isAttacked(Board board, string position)
    {
        thisBoard = board;
        pieces = board.fen.Split(' ')[0];

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

        char p = pieces[(8 - int.Parse(position[1].ToString())) * 9 + letters.IndexOf(position[0].ToString().ToUpper())];
        for (int i = 0; i < pieces.Length; i++)
        {
            piece = pieces[i];
            if (pieces[i] != '/' && pieces[i] != '1' && !isOwnPiece(p))
            {
                file = i % 9;
                rank = 8 - i / 9;
                fileStr = letters[file].ToString();
                if ((pieces[i] == 'R' || pieces[i] == 'r') && rookSearch().Contains(position))
                {
                    return true;
                }
                else if ((pieces[i] == 'N' || pieces[i] == 'n') && knightSearch().Contains(position))
                {
                    return true;
                }
                else if ((pieces[i] == 'B' || pieces[i] == 'b') && bishopSearch().Contains(position))
                {
                    return true;
                }
                else if ((pieces[i] == 'Q' || pieces[i] == 'q') && (rookSearch().Contains(position) || bishopSearch().Contains(position)))
                {
                    return true;
                }
                else if ((pieces[i] == 'K' || pieces[i] == 'k') && kingSearch(true).Contains(position))
                {
                    return true;
                }
                else if ((pieces[i] == 'P' || pieces[i] == 'p') && pawnSearch(thisBoard.enPassant()).Contains(position))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private static List<string> rookSearch()
    {
        List<string> rookMoves = new List<string>();
        for (int i = file + 1; i <= 7; i++)
        {
            int pos = (8 - rank) * 9 + letters.IndexOf(letters[i]);
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    rookMoves.Add(letters[i] + "" + rank.ToString());
                }
                break;
            }
            rookMoves.Add(letters[i] + "" + rank.ToString());
        }
        for (int i = file - 1; i >= 0; i--)
        {
            int pos = (8 - rank) * 9 + letters.IndexOf(letters[i]);
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    rookMoves.Add(letters[i] + "" + rank.ToString());
                }
                break;
            }
            rookMoves.Add(letters[i] + "" + rank.ToString());
        }
        for (int i = rank + 1; i <= 8; i++)
        {
            int pos = (8 - i) * 9 + file;
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    rookMoves.Add(fileStr + "" + i);
                }
                break;
            }
            rookMoves.Add(fileStr + "" + i);
        }
        for (int i = rank - 1; i >= 1; i--)
        {
            int pos = (8 - i) * 9 + file;
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    rookMoves.Add(fileStr + "" + i);
                }
                break;
            }
            rookMoves.Add(fileStr + "" + i);
        }
        return rookMoves;
    }

    private static List<string> knightSearch()
    {
        List<string> knightMoves = new List<string>();
        if (file - 2 >= 0 && rank - 1 >= 1 && (pieces[(8 - (rank - 1)) * 9 + file - 2] == '1' || !isOwnPiece(pieces[(8 - (rank - 1)) * 9 + file - 2])))
        {
            knightMoves.Add(letters[file - 2] + "" + (rank - 1));
        }
        if (file - 1 >= 0 && rank - 2 >= 1 && (pieces[(8 - (rank - 2)) * 9 + file - 1] == '1' || !isOwnPiece(pieces[(8 - (rank - 2)) * 9 + file - 1])))
        {
            knightMoves.Add(letters[file - 1] + "" + (rank - 2));
        }
        if (file + 2 <= 7 && rank - 1 >= 1 && (pieces[(8 - (rank - 1)) * 9 + file + 2] == '1' || !isOwnPiece(pieces[(8 - (rank - 1)) * 9 + file + 2])))
        {
            knightMoves.Add(letters[file + 2] + "" + (rank - 1));
        }
        if (file + 1 <= 7 && rank - 2 >= 1 && (pieces[(8 - (rank - 2)) * 9 + file + 1] == '1' || !isOwnPiece(pieces[(8 - (rank - 2)) * 9 + file + 1])))
        {
            knightMoves.Add(letters[file + 1] + "" + (rank - 2));
        }
        if (file - 2 >= 0 && rank + 1 <= 8 && (pieces[(8 - (rank + 1)) * 9 + file - 2] == '1' || !isOwnPiece(pieces[(8 - (rank + 1)) * 9 + file - 2])))
        {
            knightMoves.Add(letters[file - 2] + "" + (rank + 1));
        }
        if (file - 1 >= 0 && rank + 2 <= 8 && (pieces[(8 - (rank + 2)) * 9 + file - 1] == '1' || !isOwnPiece(pieces[(8 - (rank + 2)) * 9 + file - 1])))
        {
            knightMoves.Add(letters[file - 1] + "" + (rank + 2));
        }
        if (file + 2 <= 7 && rank + 1 <= 8 && (pieces[(8 - (rank + 1)) * 9 + file + 2] == '1' || !isOwnPiece(pieces[(8 - (rank + 1)) * 9 + file + 2])))
        {
            knightMoves.Add(letters[file + 2] + "" + (rank + 1));
        }
        if (file + 1 <= 7 && rank + 2 <= 8 && (pieces[(8 - (rank + 2)) * 9 + file + 1] == '1' || !isOwnPiece(pieces[(8 - (rank + 2)) * 9 + file + 1])))
        {
            knightMoves.Add(letters[file + 1] + "" + (rank + 2));
        }
        return knightMoves;
    }

    private static List<string> bishopSearch()
    {
        List<string> bishopMoves = new List<string>();
        int r = rank - 1;
        for (int f = file - 1; f >= 0 && r >= 1; f--, r--)
        {
            int pos = (8 - r) * 9 + f;
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    bishopMoves.Add(letters[f] + "" + r);
                }
                break;
            }
            bishopMoves.Add(letters[f] + "" + r);
        }
        r = rank + 1;
        for (int f = file + 1; f <= 7 && r <= 8; f++, r++)
        {
            int pos = (8 - r) * 9 + f;
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    bishopMoves.Add(letters[f] + "" + r);
                }
                break;
            }
            bishopMoves.Add(letters[f] + "" + r);
        }
        r = rank + 1;
        for (int f = file - 1; f >= 0 && r <= 8; f--, r++)
        {
            int pos = (8 - r) * 9 + f;
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    bishopMoves.Add(letters[f] + "" + r);
                }
                break;
            }
            bishopMoves.Add(letters[f] + "" + r);
        }
        r = rank - 1;
        for (int f = file + 1; f <= 7 && r >= 1; f++, r--)
        {
            int pos = (8 - r) * 9 + f;
            if (pieces[pos] != '1')
            {
                if (!isOwnPiece(pieces[pos]))
                {
                    bishopMoves.Add(letters[f] + "" + r);
                }
                break;
            }
            bishopMoves.Add(letters[f] + "" + r);
        }
        return bishopMoves;
    }

    private static List<string> kingSearch(bool fromIsAttacked)
    {
        List<string> kingMoves = new List<string>();
        for (int f = -1; f <= 1; f++)
        {
            for (int r = -1; r <= 1; r++)
            {
                if ((file + f >= 0 && file + f <= 7 && rank + r >= 1 && rank + r <= 8) && !isOwnPiece(pieces[(8 - (rank + r)) * 9 + file + f]))
                {
                    kingMoves.Add(letters[file + f] + "" + (rank + r));
                }
            }
        }

        if (fromIsAttacked == false)
        {
            string fen = thisBoard.fen;
            char p = piece;
            if (piece == 'K' && thisBoard.canCastle('K') && !isAttacked(thisBoard, "E1") && thisBoard.getPiece("F1") == '1' && thisBoard.getPiece("G1") == '1')
            {
                thisBoard.movePiece("E1", "F1");
                if (!isAttacked(thisBoard, "F1"))
                {
                    kingMoves.Add("G1");
                }
                thisBoard.fen = fen;
            }
            if (p == 'K' && thisBoard.canCastle('Q') && !isAttacked(thisBoard, "E1") && thisBoard.getPiece("B1") == '1' && thisBoard.getPiece("C1") == '1' && thisBoard.getPiece("D1") == '1')
            {
                thisBoard.movePiece("E1", "D1");
                Board tempBoard = new Board(fen);
                tempBoard.movePiece("E1", "C1");
                if (!isAttacked(thisBoard, "D1") && !isAttacked(tempBoard, "C1"))
                {
                    kingMoves.Add("C1");
                }
                thisBoard.fen = fen;
            }
            else if (piece == 'k' && thisBoard.canCastle('k') && !isAttacked(thisBoard, "E8") && thisBoard.getPiece("F8") == '1' && thisBoard.getPiece("G8") == '1')
            {
                thisBoard.movePiece("E8", "F8");
                if (!isAttacked(thisBoard, "F8"))
                {
                    kingMoves.Add("G8");
                }
                thisBoard.fen = fen;
            }
            if (p == 'k' && thisBoard.canCastle('q') && !isAttacked(thisBoard, "E8") && thisBoard.getPiece("B8") == '1' && thisBoard.getPiece("C8") == '1' && thisBoard.getPiece("D8") == '1')
            {
                thisBoard.movePiece("E8", "D8");
                Board tempBoard = new Board(fen);
                tempBoard.movePiece("E8", "C8");
                if (!isAttacked(thisBoard, "D8") && !isAttacked(tempBoard, "C8"))
                {
                    kingMoves.Add("C8");
                }
                thisBoard.fen = fen;
            }
        }
        return kingMoves;
    }

    private static List<string> pawnSearch(string enPassant)
    {
        List<string> pawnMoves = new List<string>();
        string[] tempMoves = new string[4];
        int dir = 1;
        if (char.IsLower(piece))
        {
            dir = -1;
        }
        if (pieces[(8 - (rank + dir)) * 9 + file] == '1')
        {
            pawnMoves.Add(letters[file] + "" + (rank + dir));
            if ((rank == 2 && dir == 1 || rank == 7 && dir == -1) && pieces[(8 - (rank + dir + dir)) * 9 + file] == '1')
            {
                pawnMoves.Add(letters[file] + "" + (rank + dir + dir));
            }
        }
        if (file - 1 >= 0 && (pieces[(8 - (rank + dir)) * 9 + file - 1] != '1' && !isOwnPiece(pieces[(8 - (rank + dir)) * 9 + file - 1]) || letters[file - 1] + "" + (rank + dir) == enPassant.ToUpper()))
        {
            pawnMoves.Add(letters[file - 1] + "" + (rank + dir));

        }
        if (file + 1 <= 7 && (pieces[(8 - (rank + dir)) * 9 + file + 1] != '1' && !isOwnPiece(pieces[(8 - (rank + dir)) * 9 + file + 1]) || letters[file + 1] + "" + (rank + dir) == enPassant.ToUpper()))
        {
            pawnMoves.Add(letters[file + 1] + "" + (rank + dir));
        }
        for (int i = 0; i < pawnMoves.Count; i++)
        {
            if (pawnMoves[i][1].ToString() == "8" && pawnMoves[i].Length == 2)
            {
                pawnMoves.Add(pawnMoves[i] + " " + "Q");
                pawnMoves.Add(pawnMoves[i] + " " + "R");
                pawnMoves.Add(pawnMoves[i] + " " + "N");
                pawnMoves.Add(pawnMoves[i] + " " + "B");
                pawnMoves.Remove(pawnMoves[i]);
                i--;
            }
            else if (pawnMoves[i][1].ToString() == "1" && pawnMoves[i].Length == 2)
            {
                pawnMoves.Add(pawnMoves[i] + " " + "q");
                pawnMoves.Add(pawnMoves[i] + " " + "r");
                pawnMoves.Add(pawnMoves[i] + " " + "n");
                pawnMoves.Add(pawnMoves[i] + " " + "b");
                pawnMoves.Remove(pawnMoves[i]);
                i--;
            }
        }
        return pawnMoves;
    }

    private static bool isOwnPiece(char p)
    {
        if ((char.IsLower(piece) && char.IsLower(p)) || char.IsUpper(piece) && char.IsUpper(p))
        {
            return true;
        }
        return false;
    }
}
