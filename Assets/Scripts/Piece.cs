using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;
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
        List<Square> highlightSquares = new List<Square>();
        highlightSquares.AddRange(GetPawnMoveableSquares());
        highlightSquares.AddRange(GetPawnAttackableSquares());
        foreach (Square square in highlightSquares) {
            square.HighlightSquare(true);
        }
    }

    private List<Square> GetPawnMoveableSquares() {
        List<Square> moveableSquares = new List<Square>();
        Square oneAhead = board.squaresArray[onBlock.x + (isWhite ? 1 : -1), onBlock.y];
        if (oneAhead.pieceOnTop == null) {
            moveableSquares.Add(oneAhead);
        }

        if (numberOfTurnsPlayed == 0 && oneAhead.pieceOnTop == null) {
            Square twoAhead = board.squaresArray[onBlock.x + (isWhite ? 2 : -2), onBlock.y];
            if (twoAhead.pieceOnTop == null) {
                moveableSquares.Add(twoAhead);
            }
        }
        return moveableSquares;
    }

    public List<Square> GetPawnAttackableSquares() {
        List<Square> attackableSquares = new List<Square>();
        if (onBlock.y < 7) {
            Square leftSide = board.squaresArray[onBlock.x + (isWhite ? 1 : -1), onBlock.y + 1];
            if (leftSide.pieceOnTop != null && leftSide.pieceOnTop.isWhite != isWhite) {
                attackableSquares.Add(leftSide);
            }
        }
        if (onBlock.y > 0) {
            Square rightSide = board.squaresArray[onBlock.x + (isWhite ? 1 : -1), onBlock.y - 1];
            if (rightSide.pieceOnTop != null && rightSide.pieceOnTop.isWhite != isWhite) {
                attackableSquares.Add(rightSide);
            }
        }
        return attackableSquares;
    }

    private void HighlightForKnight() {
        List<Square> highlightSquares = GetKnightAttackableSquares();
        foreach (Square square in highlightSquares) {
            square.HighlightSquare(true);
        }
    }

    public List<Square> GetKnightAttackableSquares() {
        List<Square> attackableSquares = new List<Square>();
        if (onBlock.x < 6 && onBlock.y > 0) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x + 2, onBlock.y - 1]); // Top Right
        }
        if (onBlock.x < 6 && onBlock.y < 7) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x + 2, onBlock.y + 1]); // Top Left
        }
        if (onBlock.x < 7 && onBlock.y < 6) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x + 1, onBlock.y + 2]); // Left Top
        }
        if (onBlock.x > 0 && onBlock.y < 6) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x - 1, onBlock.y + 2]); // Left Bottom
        }
        if (onBlock.x > 1 && onBlock.y < 7) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x - 2, onBlock.y + 1]); // Bottom Left
        }
        if (onBlock.x > 1 && onBlock.y > 0) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x - 2, onBlock.y - 1]); // Bottom Right
        }
        if (onBlock.x > 0 && onBlock.y > 1) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x - 1, onBlock.y - 2]); // Right Bottom
        }
        if (onBlock.x < 7 && onBlock.y > 1) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x + 1, onBlock.y - 2]); // Right Top
        }
        return attackableSquares;
    }
   
    private void HighlightForKing() {
        List<Square> highlightSquares = GetKingAttackableSquares();
        foreach (Square square in highlightSquares) {
            square.HighlightSquare(true);
        }
    }

    public List<Square> GetKingAttackableSquares() {
        List<Square> attackableSquares = new List<Square>();
        if (onBlock.x < 7) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x + 1, onBlock.y]); // Top
        }
        if (onBlock.x < 7 && onBlock.y < 7) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x + 1, onBlock.y + 1]); // Top Left
        }
        if (onBlock.y < 7) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x, onBlock.y + 1]); // Left
        }
        if (onBlock.x > 0 && onBlock.y < 7) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x - 1, onBlock.y + 1]); // Left Bottom
        }
        if (onBlock.x > 0) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x - 1, onBlock.y]); // Bottom
        }
        if (onBlock.x > 0 && onBlock.y > 0) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x - 1, onBlock.y - 1]); // Bottom Right
        }
        if (onBlock.y > 1) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x, onBlock.y - 1]); // Right
        }
        if (onBlock.x < 7 && onBlock.y > 1) {
            CheckAndAddToList(attackableSquares, board.squaresArray[onBlock.x + 1, onBlock.y - 1]); // Right Top
        }

        List<Square> removedSquares = new List<Square>();
        foreach (Square square in attackableSquares) {
            if ((isWhite && square.blackPiecesThatCanAttack.Count != 0) ||
                (!isWhite && square.whitePiecesThatCanAttack.Count != 0)) {
                removedSquares.Add(square);
            }
        }

        return attackableSquares.Except(removedSquares).ToList();
    }

    private void CheckAndAddToList(List<Square> squaresList, Square square) {
        if (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite) {
            squaresList.Add(square);
        }
    }

    private void HighlightForBishop() {
        List<Square> highlightSquares = GetBishopAttackableSquares();
        foreach (Square square in highlightSquares) {
            square.HighlightSquare(true);
        }
    }

    public List<Square> GetBishopAttackableSquares() {
        List<Square> attackableSquares = new List<Square>();
        int x = onBlock.x;
        int y = onBlock.y;
        bool blockedByPiece = false;

        while (x < 7 && y > 0) {
            Square square = board.squaresArray[x + 1, y - 1];
            if (!blockedByPiece && (square.pieceOnTop == null || square.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, square);
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
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, square);
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
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, square);
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
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, square);
            } else {
                break;
            }
            x--;
            y--;
        }

        return attackableSquares;
    }

    private void HighlightForRook() {
        List<Square> highlightSquares = GetRookAttackableSquares();
        foreach (Square square in highlightSquares) {
            square.HighlightSquare(true);
        }
    }

    public List<Square> GetRookAttackableSquares() {
        List<Square> attackableSquares = new List<Square>();
        int x = onBlock.x;
        int y = onBlock.y;
        bool blockedByPiece = false;
        while (x < 7) {
            Square above = board.squaresArray[x + 1, y];
            if (!blockedByPiece && (above.pieceOnTop == null || above.pieceOnTop.isWhite != isWhite)) {
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, above);
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
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, below);
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
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, right);
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
                blockedByPiece = CheckIfBlockedAndAddToList(attackableSquares, left);
            } else {
                break;
            }
            y--;
        }
        return attackableSquares;
    }

    private bool CheckIfBlockedAndAddToList(List<Square> squaresList, Square square) {
        squaresList.Add(square);
        return (square.pieceOnTop != null && square.pieceOnTop.isWhite != isWhite);
    }

    private void HighlightForQueen() {
        List<Square> highlightSquares = GetQueenAttackableSquares();
        foreach (Square square in highlightSquares) {
            square.HighlightSquare(true);
        }
    }

    public List<Square> GetQueenAttackableSquares() {
        List<Square> attackableSquares = new List<Square>();
        attackableSquares.AddRange(GetBishopAttackableSquares());
        attackableSquares.AddRange(GetRookAttackableSquares());
        return attackableSquares;
    }

    public List<Square> GetAttackableSquares() {
        List<Square> attackableSquares = new List<Square>();
        switch (type) {
            case PieceType.King:
                attackableSquares.AddRange(GetKingAttackableSquares());
                break;
            case PieceType.Queen:
                attackableSquares.AddRange(GetQueenAttackableSquares());
                break;
            case PieceType.Knight:
                attackableSquares.AddRange(GetKnightAttackableSquares());
                break;
            case PieceType.Bishop:
                attackableSquares.AddRange(GetBishopAttackableSquares());
                break;
            case PieceType.Rook:
                attackableSquares.AddRange(GetRookAttackableSquares());
                break;
            case PieceType.Pawn:
                attackableSquares.AddRange(GetPawnAttackableSquares());
                break;
        }
        return attackableSquares;
    }
}
