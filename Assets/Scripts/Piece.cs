using UnityEngine;
using System.Collections;
using System;

public class Piece : MonoBehaviour {
    [SerializeField] public bool isWhite;
    [SerializeField] public PieceType type;
    [SerializeField] public Square onBlock;
    [SerializeField] Board board;
    public int numberOfTurnsPlayed;
    public bool isAlive = true;
    Square[] squares;

    private void Start() {
        squares = FindObjectsOfType<Square>();
    }

    void OnMouseDown() {
        if (board.isCurrentPlayerWhite == isWhite) {
            if (board.currentSelectedPiece != null) {
                board.currentSelectedPiece.HighlightPiece(false);
            }
            HighlightPiece(true);
            board.isPieceSelected = true;
            board.currentSelectedPiece = this;

            HighlightOptions();
        } else {
            onBlock.moveToBlock();
        }
    }

    public void HighlightPiece(bool isHighlighted) {
        gameObject.GetComponent<Renderer>().material.color =
            isHighlighted ? Constants.highlightedPiece : isWhite ? Constants.whitePiece : Constants.blackPiece;
    }

    private void HighlightOptions() {
        Square.RemoveHighlightFromAllSquares();

        switch (type) {
            case PieceType.Pawn:
                HighlightForPawn();
                break;
            case PieceType.Bishop:
                HighlightForBishop();
                break;
            case PieceType.Rook:
                HighlightForRook();
                break;
            case PieceType.Knight:
                HighlightForKnight();
                break;
            case PieceType.King:
                HighlightForKing();
                break;
            case PieceType.Queen:
                HighlightForQueen();
                break;
        }
    }

    private void HighlightForPawn() {
        foreach (Square square in squares) {
            if (square.pieceOnTop != null && (square.pieceOnTop.isWhite == isWhite)) {
                continue;
            }
            bool toBeHighlighted = false;
            if (square.yLocation == onBlock.yLocation) {
                if (numberOfTurnsPlayed == 0 && (square.xLocation == onBlock.xLocation + (isWhite ? -12 : 12))) {
                    if (square.pieceOnTop == null) {
                        toBeHighlighted = true;

                    }
                }
                if (square.xLocation == onBlock.xLocation + (isWhite ? -6 : 6)) {
                    if (square.pieceOnTop == null) {
                        toBeHighlighted = true;
                    }
                }
            }

            if (square.pieceOnTop != null &&
                !square.pieceOnTop.isWhite &&
                square.xLocation == onBlock.xLocation - 6 &&
                (square.yLocation == onBlock.yLocation - 6 || square.yLocation == onBlock.yLocation + 6) &&
                onBlock.pieceOnTop.isWhite) {
                toBeHighlighted = true;
            } else if (square.pieceOnTop != null &&
                square.pieceOnTop.isWhite &&
                square.xLocation == onBlock.xLocation + 6 &&
                (square.yLocation == onBlock.yLocation - 6 || square.yLocation == onBlock.yLocation + 6) &&
                !onBlock.pieceOnTop.isWhite) {
                toBeHighlighted = true;
            }

            if (toBeHighlighted) {
                square.HighlightSquare(true);
            }
        }
    }

    private void HighlightForBishop() {
        int onBlockX = (onBlock.xLocation + 21) / 6;
        int onBlockY = (onBlock.yLocation + 21) / 6;

        foreach (Square square in squares) {
            if (square.pieceOnTop != null && (square.pieceOnTop.isWhite == isWhite)) {
                continue;
            }

            bool toBeHighlighted = false;
            for (int i = 1; i < 8; i++) {
                int squareX = (square.xLocation + 21) / 6;
                int squareY = (square.yLocation + 21) / 6;
                if (squareX == onBlockX + i && squareY == onBlockY + i) {
                    toBeHighlighted = true;
                    break;
                } else if (squareX == onBlockX - i && squareY == onBlockY - i) {
                    toBeHighlighted = true;
                    break;
                } else if (squareX == onBlockX + i && squareY == onBlockY - i) {
                    toBeHighlighted = true;
                    break;
                } else if (squareX == onBlockX - i && squareY == onBlockY + i) {
                    toBeHighlighted = true;
                    break;
                }
            }
            if (toBeHighlighted) {
                square.HighlightSquare(true);
            }
        }
    }

    private void HighlightForRook() {
        foreach (Square square in squares) {
            if (square.pieceOnTop != null && (square.pieceOnTop.isWhite == isWhite)) {
                continue;
            }
            if (square.xLocation == onBlock.xLocation || square.yLocation == onBlock.yLocation) {
                square.HighlightSquare(true);
            }
        }
    }

    private void HighlightForKnight() {
        int onBlockX = (onBlock.xLocation + 21) / 6;
        int onBlockY = (onBlock.yLocation + 21) / 6;
        foreach (Square square in squares) {
            if (square.pieceOnTop != null && (square.pieceOnTop.isWhite == isWhite)) {
                continue;
            }

            int squareX = (square.xLocation + 21) / 6;
            int squareY = (square.yLocation + 21) / 6;

            if (squareX == onBlockX + 2 && (squareY == onBlockY + 1 || squareY == onBlockY - 1) ||
                squareX == onBlockX - 2 && (squareY == onBlockY + 1 || squareY == onBlockY - 1) ||
                squareY == onBlockY + 2 && (squareX == onBlockX + 1 || squareX == onBlockX - 1) ||
                squareY == onBlockY - 2 && (squareX == onBlockX + 1 || squareX == onBlockX - 1)) {
                square.HighlightSquare(true);
            }
        }
    }

    private void HighlightForKing() {
        int onBlockX = (onBlock.xLocation + 21) / 6;
        int onBlockY = (onBlock.yLocation + 21) / 6;
        foreach (Square square in squares) {
            if (square.pieceOnTop != null && (square.pieceOnTop.isWhite == isWhite)) {
                continue;
            }

            int squareX = (square.xLocation + 21) / 6;
            int squareY = (square.yLocation + 21) / 6;

            if ((squareX == onBlockX || squareX == onBlockX + 1 || squareX == onBlockX - 1) &&
                (squareY == onBlockY || squareY == onBlockY + 1 || squareY == onBlockY - 1)) {
                square.HighlightSquare(true);
            }
        }
    }

    private void HighlightForQueen() {
        HighlightForBishop();
        HighlightForRook();
    }
}
