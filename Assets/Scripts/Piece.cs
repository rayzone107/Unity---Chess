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

    private void HighlightForBishop() {
        int x = onBlock.x;
        int y = onBlock.y;
        bool blockedByPiece = false;

        while (x < 7 && y > 0) {
            Square square = board.squaresArray[x + 1, y - 1];
            if (!blockedByPiece && (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite)) {
                if (square.pieceOnTop != null && square.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                square.HighlightSquare(true);
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
                if (square.pieceOnTop != null && square.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                square.HighlightSquare(true);
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
                if (square.pieceOnTop != null && square.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                square.HighlightSquare(true);
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
                if (square.pieceOnTop != null && square.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                square.HighlightSquare(true);
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
                if (above.pieceOnTop != null && above.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                above.HighlightSquare(true);
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
                if (below.pieceOnTop != null && below.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                below.HighlightSquare(true);
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
                if (right.pieceOnTop != null && right.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                right.HighlightSquare(true);
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
                if (left.pieceOnTop != null && left.pieceOnTop.isWhite != isWhite) {
                    blockedByPiece = true;
                }
                left.HighlightSquare(true);
            } else {
                break;
            }
            y--;
        }
    }

    private void HighlightForKnight() {
        if (onBlock.x < 6 && onBlock.y > 0) {
            Square topRight = board.squaresArray[onBlock.x + 2, onBlock.y - 1];
            if (topRight.pieceOnTop == null || topRight.pieceOnTop.isWhite != isWhite) {
                topRight.HighlightSquare(true);
            }
        }
        if (onBlock.x < 6 && onBlock.y < 7) {
            Square topLeft = board.squaresArray[onBlock.x + 2, onBlock.y + 1];
            if (topLeft.pieceOnTop == null || topLeft.pieceOnTop.isWhite != isWhite) {
                topLeft.HighlightSquare(true);
            }
        }
        if (onBlock.x < 7 && onBlock.y < 6) {
            Square leftTop = board.squaresArray[onBlock.x + 1, onBlock.y + 2];
            if (leftTop.pieceOnTop == null || leftTop.pieceOnTop.isWhite != isWhite) {
                leftTop.HighlightSquare(true);
            }
        }
        if (onBlock.x > 0 && onBlock.y < 6) {
            Square leftBottom = board.squaresArray[onBlock.x - 1, onBlock.y + 2];
            if (leftBottom.pieceOnTop == null || leftBottom.pieceOnTop.isWhite != isWhite) {
                leftBottom.HighlightSquare(true);
            }
        }
        if (onBlock.x > 1 && onBlock.y < 7) {
            Square bottomLeft = board.squaresArray[onBlock.x - 2, onBlock.y + 1];
            if (bottomLeft.pieceOnTop == null || bottomLeft.pieceOnTop.isWhite != isWhite) {
                bottomLeft.HighlightSquare(true);
            }
        }
        if (onBlock.x > 1 && onBlock.y > 0) {
            Square bottomRight = board.squaresArray[onBlock.x - 2, onBlock.y - 1];
            if (bottomRight.pieceOnTop == null || bottomRight.pieceOnTop.isWhite != isWhite) {
                bottomRight.HighlightSquare(true);
            }
        }
        if (onBlock.x > 0 && onBlock.y > 1) {
            Square rightBottom = board.squaresArray[onBlock.x - 1, onBlock.y - 2];
            if (rightBottom.pieceOnTop == null || rightBottom.pieceOnTop.isWhite != isWhite) {
                rightBottom.HighlightSquare(true);
            }
        }
        if (onBlock.x < 7 && onBlock.y > 1) {
            Square rightTop = board.squaresArray[onBlock.x + 1, onBlock.y - 2];
            if (rightTop.pieceOnTop == null || rightTop.pieceOnTop.isWhite != isWhite) {
                rightTop.HighlightSquare(true);
            }
        }
    }

    //private void CheckAndHighlight(Square topRight) {
    //    if (topRight.pieceOnTop == null || topRight.pieceOnTop.isWhite != isWhite) {
    //        topRight.HighlightSquare(true);
    //    }
    //}

    private void HighlightForKing() {
        if (onBlock.x < 7) {
            Square top = board.squaresArray[onBlock.x + 1, onBlock.y];
            if (top.pieceOnTop == null || top.pieceOnTop.isWhite != isWhite) {
                top.HighlightSquare(true);
            }
        }
        if (onBlock.x < 7 && onBlock.y < 7) {
            Square topLeft = board.squaresArray[onBlock.x + 1, onBlock.y + 1];
            if (topLeft.pieceOnTop == null || topLeft.pieceOnTop.isWhite != isWhite) {
                topLeft.HighlightSquare(true);
            }
        }
        if (onBlock.y < 7) {
            Square left = board.squaresArray[onBlock.x, onBlock.y + 1];
            if (left.pieceOnTop == null || left.pieceOnTop.isWhite != isWhite) {
                left.HighlightSquare(true);
            }
        }
        if (onBlock.x > 0 && onBlock.y < 7) {
            Square leftBottom = board.squaresArray[onBlock.x - 1, onBlock.y + 1];
            if (leftBottom.pieceOnTop == null || leftBottom.pieceOnTop.isWhite != isWhite) {
                leftBottom.HighlightSquare(true);
            }
        }
        if (onBlock.x > 0) {
            Square bottom = board.squaresArray[onBlock.x - 1, onBlock.y];
            if (bottom.pieceOnTop == null || bottom.pieceOnTop.isWhite != isWhite) {
                bottom.HighlightSquare(true);
            }
        }
        if (onBlock.x > 0 && onBlock.y > 0) {
            Square bottomRight = board.squaresArray[onBlock.x - 1, onBlock.y - 1];
            if (bottomRight.pieceOnTop == null || bottomRight.pieceOnTop.isWhite != isWhite) {
                bottomRight.HighlightSquare(true);
            }
        }
        if (onBlock.y > 1) {
            Square right = board.squaresArray[onBlock.x, onBlock.y - 1];
            if (right.pieceOnTop == null || right.pieceOnTop.isWhite != isWhite) {
                right.HighlightSquare(true);
            }
        }
        if (onBlock.x < 7 && onBlock.y > 1) {
            Square rightTop = board.squaresArray[onBlock.x + 1, onBlock.y - 1];
            if (rightTop.pieceOnTop == null || rightTop.pieceOnTop.isWhite != isWhite) {
                rightTop.HighlightSquare(true);
            }
        }
    }

    private void HighlightForQueen() {
        HighlightForBishop();
        HighlightForRook();
    }
}
