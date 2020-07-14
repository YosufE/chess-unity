using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkPointController : MonoBehaviour
{
    public ChessPiece connectedPieceGameObject;

    private void OnMouseDown()
    {
        Vector3 pieceCoord = transform.position;
        pieceCoord.z += 1;
        connectedPieceGameObject.move(pieceCoord);
    }
}
