using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

    public bool isCurrentPlayerWhite = true;
    public bool isPieceSelected;
    public Piece currentSelectedPiece;

    public int destroyedWhiteCount;
    public int destroyedBlackCount;

    public Square[,] squaresArray = new Square[8, 8];

    private void Start() {
        SquareRow[] rows = FindObjectsOfType<SquareRow>();
        foreach(SquareRow row in rows) {
            int childIndex = 0;
            foreach(Transform child in row.transform) {
                squaresArray[row.rowLocation, childIndex] = child.gameObject.GetComponent<Square>();
                childIndex++;
            }
        }
    }
}
