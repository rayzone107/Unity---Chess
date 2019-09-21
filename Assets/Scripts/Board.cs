using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

    public bool isCurrentPlayerWhite = true;
    public bool isPieceSelected;
    public Piece currentSelectedPiece;

    public int destroyedWhiteCount;
    public int destroyedBlackCount;

}
