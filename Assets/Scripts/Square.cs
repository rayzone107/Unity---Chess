using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour {

    [SerializeField] int xLocation;
    [SerializeField] int yLocation;
    [SerializeField] Piece pieceOnTop;
    [SerializeField] Board board;

    void Start() {
    }

    void Update() {
    }

    private void OnMouseDown() {
        Debug.Log("Down = " + gameObject.name + ", xLocation: " + xLocation + ", yLocation: " + yLocation);

        if (board.isPieceSelected) {
            Vector3 moveTo = new Vector3(xLocation, board.currentSelectedPiece.transform.position.y, yLocation);
            board.currentSelectedPiece.transform.position = moveTo;
            board.currentPlayerColor = board.currentPlayerColor.Equals(ColorType.White) ? ColorType.Black : ColorType.White;
        }
    }
}
