using UnityEngine;
using System.Collections;

public class Piece : MonoBehaviour {
    [SerializeField] ColorType color;
    [SerializeField] PieceType type;
    [SerializeField] Square onBlock;
    [SerializeField] Board board;

    void Start() {
        gameObject.GetComponent<Renderer>().material.color = color.Equals(ColorType.Black) ? Color.black : Color.white;
    }

    void Update() {
    }

    void OnMouseDown() {
        Debug.Log("Object + " + gameObject.name + ", color = " + color + ", onBlock = " + onBlock.name);
        if (board.currentPlayerColor.Equals(color)) {

            if (board.currentSelectedPiece != null) {
                HighlightObject(board.currentSelectedPiece, false);
            }
            HighlightObject(this, true);
            gameObject.GetComponent<Renderer>().material.color = Color.red;
            board.isPieceSelected = true;
            board.currentSelectedPiece = this;
        }
    }

    void HighlightObject(Piece piece, bool isHighlighted) {
        Debug.Log("In Change Color");
        piece.gameObject.GetComponent<Renderer>().material.color = isHighlighted ? Color.red : (piece.color.Equals(ColorType.White) ? Color.white : Color.black);
    }
}
