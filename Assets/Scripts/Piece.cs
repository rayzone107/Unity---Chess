using UnityEngine;
using System.Collections;
using System;
using UnityStandardAssets.CrossPlatformInput;

public class Piece : MonoBehaviour {
    [SerializeField] public bool isWhite;
    [SerializeField] public PieceType type;
    [SerializeField] public Square onBlock;
    [SerializeField] Board board;
    public int numberOfTurnsPlayed;
    public bool isAlive = true;
    public Square[] squares;

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
        Square oneAhead = board.squaresArray[onBlock.x + (isWhite ? 1 : -1), onBlock.y];
        if (oneAhead.pieceOnTop == null) {
            oneAhead.HighlightSquare(true);
        }

        if (numberOfTurnsPlayed == 0 && oneAhead.pieceOnTop == null) {
            Square twoAhead = board.squaresArray[onBlock.x + (isWhite ? 2 : -2), onBlock.y];
            if (twoAhead.pieceOnTop == null) {
                twoAhead.HighlightSquare(true);
            }
        }

        if (onBlock.y < 7) {
            Square leftSide = board.squaresArray[onBlock.x + (isWhite ? 1 : -1), onBlock.y + 1];
            if (leftSide.pieceOnTop != null && leftSide.pieceOnTop.isWhite != isWhite) {
                leftSide.HighlightSquare(true);
            }
        }
        if (onBlock.y > 0) {
            Square rightSide = board.squaresArray[onBlock.x + (isWhite ? 1 : -1), onBlock.y - 1];
            if (rightSide.pieceOnTop != null && rightSide.pieceOnTop.isWhite != isWhite) {
                rightSide.HighlightSquare(true);
            }
        }
    }

    private void HighlightForKnight() {
        if (onBlock.x < 6 && onBlock.y > 0) {
            CheckAndHighlight(board.squaresArray[onBlock.x + 2, onBlock.y - 1]); // Top Right
        }
        if (onBlock.x < 6 && onBlock.y < 7) {
            CheckAndHighlight(board.squaresArray[onBlock.x + 2, onBlock.y + 1]); // Top Left
        }
        if (onBlock.x < 7 && onBlock.y < 6) {
            CheckAndHighlight(board.squaresArray[onBlock.x + 1, onBlock.y + 2]); // Left Top
        }
        if (onBlock.x > 0 && onBlock.y < 6) {
            CheckAndHighlight(board.squaresArray[onBlock.x - 1, onBlock.y + 2]); // Left Bottom
        }
        if (onBlock.x > 1 && onBlock.y < 7) {
            CheckAndHighlight(board.squaresArray[onBlock.x - 2, onBlock.y + 1]); // Bottom Left
        }
        if (onBlock.x > 1 && onBlock.y > 0) {
            CheckAndHighlight(board.squaresArray[onBlock.x - 2, onBlock.y - 1]); // Bottom Right
        }
        if (onBlock.x > 0 && onBlock.y > 1) {
            CheckAndHighlight(board.squaresArray[onBlock.x - 1, onBlock.y - 2]); // Right Bottom
        }
        if (onBlock.x < 7 && onBlock.y > 1) {
            CheckAndHighlight(board.squaresArray[onBlock.x + 1, onBlock.y - 2]); // Right Top
        }
    }

    private void HighlightForKing() {
        if (onBlock.x < 7) {
            CheckAndHighlight(board.squaresArray[onBlock.x + 1, onBlock.y]); // Top
        }
        if (onBlock.x < 7 && onBlock.y < 7) {
            CheckAndHighlight(board.squaresArray[onBlock.x + 1, onBlock.y + 1]); // Top Left
        }
        if (onBlock.y < 7) {
            CheckAndHighlight(board.squaresArray[onBlock.x, onBlock.y + 1]); // Left
        }
        if (onBlock.x > 0 && onBlock.y < 7) {
            CheckAndHighlight(board.squaresArray[onBlock.x - 1, onBlock.y + 1]); // Left Bottom
        }
        if (onBlock.x > 0) {
            CheckAndHighlight(board.squaresArray[onBlock.x - 1, onBlock.y]); // Bottom
        }
        if (onBlock.x > 0 && onBlock.y > 0) {
            CheckAndHighlight(board.squaresArray[onBlock.x - 1, onBlock.y - 1]); // Bottom Right
        }
        if (onBlock.y > 1) {
            CheckAndHighlight(board.squaresArray[onBlock.x, onBlock.y - 1]); // Right
        }
        if (onBlock.x < 7 && onBlock.y > 1) {
            CheckAndHighlight(board.squaresArray[onBlock.x + 1, onBlock.y - 1]); // Right Top
        }
    }

    private void CheckAndHighlight(Square square) {
        if (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite) {
            square.HighlightSquare(true);
        }
    }

    private void HighlightForBishop() {
        int x = onBlock.x;
        int y = onBlock.y;
        bool blockedByPiece = false;

        while (x < 7 && y > 0) {
            Square square = board.squaresArray[x + 1, y - 1];
            if (!blockedByPiece && (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(square);
            } else {
                break;
            }
            x++;
            y--;
        }

        x = onBlock.x;
        y = onBlock.y;
        blockedByPiece = false;
        while (x > 0 && y < 7) {
            Square square = board.squaresArray[x - 1, y + 1];
            if (!blockedByPiece && (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(square);
            } else {
                break;
            }
            x--;
            y++;
        }

        x = onBlock.x;
        y = onBlock.y;
        blockedByPiece = false;
        while (x < 7 && y < 7) {
            Square square = board.squaresArray[x + 1, y + 1];
            if (!blockedByPiece && (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(square);
            } else {
                break;
            }
            x++;
            y++;
        }

        x = onBlock.x;
        y = onBlock.y;
        blockedByPiece = false;
        while (x > 0 && y > 0) {
            Square square = board.squaresArray[x - 1, y - 1];
            if (!blockedByPiece && (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(square);
            } else {
                break;
            }
            x--;
            y--;
        }
    }

    private void HighlightForRook() {
        int x = onBlock.x;
        int y = onBlock.y;
        bool blockedByPiece = false;
        while (x < 7) {
            Square above = board.squaresArray[x + 1, y];
            if (!blockedByPiece && (above.pieceOnTop == null || above.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(above);
            } else {
                break;
            }
            x++;
        }

        x = onBlock.x;
        y = onBlock.y;
        blockedByPiece = false;
        while (x > 0) {
            Square below = board.squaresArray[x - 1, y];
            if (!blockedByPiece && (below.pieceOnTop == null || below.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(below);
            } else {
                break;
            }
            x--;
        }

        x = onBlock.x;
        y = onBlock.y;
        blockedByPiece = false;
        while (y < 7) {
            Square right = board.squaresArray[x, y + 1];
            if (!blockedByPiece && (right.pieceOnTop == null || right.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(right);
            } else {
                break;
            }
            y++;
        }

        x = onBlock.x;
        y = onBlock.y;
        blockedByPiece = false;
        while (y > 0) {
            Square left = board.squaresArray[x, y - 1];
            if (!blockedByPiece && (left.pieceOnTop == null || left.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndHighlight(left);
            } else {
                break;
            }
            y--;
        }
    }

    bool CheckIfBlockedAndHighlight(Square square) {
        square.HighlightSquare(true);
        return (square.pieceOnTop != null && square.pieceOnTop.isWhite != isWhite);
    }

    private void HighlightForQueen() {
        HighlightForBishop();
        HighlightForRook();
    }
}
