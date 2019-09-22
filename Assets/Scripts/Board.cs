using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

    public bool isCurrentPlayerWhite = true;
    public bool isPieceSelected;
    public Piece currentSelectedPiece;

    public int destroyedWhiteCount;
    public int destroyedBlackCount;

    public Square[,] squares = new Square[8, 8];

    private void Start() {
        SquareRow[] rows = FindObjectsOfType<SquareRow>();
        foreach(SquareRow row in rows) {
            int childIndex = 0;
            foreach(Transform child in row.transform) {
                //Debug.Log("Row = " + row.rowLocation + ", Column = " + childIndex + ", Object = " + child.name);
                squares[row.rowLocation, childIndex] = child.gameObject.GetComponent<Square>();
                childIndex++;
            }
        }

        for (int i = 0; i < 8; i++) {
            for (int j = 0; j < 8; j++) {
                Debug.Log("Row = " + i + ", Column = " + j + ", Object = " + squares[i, j]);
            }
        }
    }
}
