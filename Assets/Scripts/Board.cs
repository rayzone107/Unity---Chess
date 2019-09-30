using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Board : MonoBehaviour {

    public bool isCurrentPlayerWhite = true;
    public bool isPieceSelected;
    public Piece currentSelectedPiece;

    public int destroyedWhiteCount;
    public int destroyedBlackCount;

    public Square[,] squaresArray = new Square[8, 8];

    private void Start() {
        SquareRow[] rows = FindObjectsOfType<SquareRow>();
        foreach (SquareRow row in rows) {
            int childIndex = 0;
            foreach (Transform child in row.transform) {
                squaresArray[row.rowLocation, childIndex] = child.gameObject.GetComponent<Square>();
                childIndex++;
            }
        }

        MarkAttackableSquares();
    }

    public void MarkAttackableSquares() {
        Piece[] pieces = FindObjectsOfType<Piece>();

        foreach (Square square in squaresArray) {
            square.blackPiecesThatCanAttack.Clear();
            square.whitePiecesThatCanAttack.Clear();
        }

        foreach (Piece piece in pieces) {
            if (piece.isWhite) {
                foreach (Square attackableSquare in piece.GetAttackableSquares()) {
                    attackableSquare.whitePiecesThatCanAttack.Add(piece);
                }
            } else {
                foreach (Square attackableSquare in piece.GetAttackableSquares()) {
                    attackableSquare.blackPiecesThatCanAttack.Add(piece);
                }
            }
        }
    }

    public bool IsWhiteCheck() {
        Piece[] pieces = FindObjectsOfType<Piece>();

        foreach (Piece piece in pieces) {
            if (piece.isWhite && piece.type == PieceType.King) {
                return piece.onBlock.blackPiecesThatCanAttack.Count > 0;
            }
        }
        return false;
    }

    public bool IsBlackCheck() {
        Piece[] pieces = FindObjectsOfType<Piece>();

        foreach (Piece piece in pieces) {
            if (!piece.isWhite && piece.type == PieceType.King) {
                return piece.onBlock.whitePiecesThatCanAttack.Count > 0;
            }
        }
        return false;
    }

    public bool isWhiteCheckmate() {
        Piece[] pieces = FindObjectsOfType<Piece>();

        foreach (Piece piece in pieces) {
            return !piece.isWhite && piece.type == PieceType.King && piece.onBlock.whitePiecesThatCanAttack.Count > 0 && piece.GetKingAttackableSquares().Count == 0;
        }
        return false;
    }

    public bool isBlackCheckmate() {
        Piece[] pieces = FindObjectsOfType<Piece>();

        foreach (Piece piece in pieces) {
            return piece.isWhite && piece.type == PieceType.King && piece.onBlock.blackPiecesThatCanAttack.Count > 0 && piece.GetKingAttackableSquares().Count == 0;
        }
        return false;
    }
}
