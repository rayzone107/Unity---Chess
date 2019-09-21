using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

    [SerializeField] public int xLocation;
    [SerializeField] public int yLocation;
    [SerializeField] public ColorType color;
    [SerializeField] public Piece pieceOnTop;
    [SerializeField] Board board;
    public bool isAllowed;

    private void OnMouseDown() {
        if (board.isPieceSelected) {
            moveToBlock();
        }
    }

    public static void RemoveHighlightFromAllSquares() {
        Square[] squares = FindObjectsOfType<Square>();
        foreach (Square square in squares) {
            square.HighlightSquare(false);
        }
    }

    public void HighlightSquare(bool isHighlighted) {
        isAllowed = isHighlighted;
        gameObject.GetComponent<Renderer>().material.color =
            isHighlighted ? Constants.highlightedSquare : (color.Equals(ColorType.White) ? Constants.whiteSquare : Constants.blackSquare);
    }

    public void moveToBlock() {
        if (isAllowed) {
            if (pieceOnTop != null) {
                pieceOnTop.isAlive = false;
                moveToDestroyedArea(pieceOnTop);
            }

            Vector3 moveTo = new Vector3(xLocation, board.currentSelectedPiece.transform.position.y, yLocation);
            board.currentSelectedPiece.transform.position = moveTo;
            board.isCurrentPlayerWhite = !board.isCurrentPlayerWhite;

            pieceOnTop = board.currentSelectedPiece; // Set piece on square to current piece
            board.currentSelectedPiece = null; // Cleared current Selected piece
            pieceOnTop.onBlock.pieceOnTop = null; // Set piece on last square to null
            pieceOnTop.onBlock = this; // Set square for current piece to this square
            pieceOnTop.numberOfTurnsPlayed++;
            pieceOnTop.HighlightPiece(false);
            RemoveHighlightFromAllSquares();
        }
    }

    private void moveToDestroyedArea(Piece piece) {
        if (piece.isWhite) {
            Vector3 moveTo;
            if (board.destroyedWhiteCount < 8) {
                moveTo = new Vector3(21 - (board.destroyedWhiteCount * 6), piece.transform.position.y, 32);
            } else {
                moveTo = new Vector3(21 - ((board.destroyedWhiteCount - 8) * 6), piece.transform.position.y, 40);
            }
            piece.transform.position = moveTo;
            board.destroyedWhiteCount++;
        } else {
            Vector3 moveTo;
            if (board.destroyedBlackCount < 8) {
                moveTo = new Vector3((-1) * (21 - (board.destroyedBlackCount * 6)), piece.transform.position.y, -32);
            } else {
                moveTo = new Vector3((-1) * (21 - ((board.destroyedBlackCount - 8) * 6)), piece.transform.position.y, -40);
            }
            piece.transform.position = moveTo;
            board.destroyedBlackCount++;
        }
    }
}
