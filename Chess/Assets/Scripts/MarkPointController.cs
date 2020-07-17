using System;
using System.Collections;
using System.Collections.Generic;
using Chess_Pieces;
using UnityEngine;

public class MarkPointController : MonoBehaviour
{
    public ChessPiece connectedPieceGameObject;
    public GameObject connectedKillGameObject;

    private void OnMouseDown()
    {
        Vector3 pieceCoord = transform.position;
        pieceCoord.z += 1;
        
        Destroy(connectedKillGameObject);
        
        connectedPieceGameObject.move(pieceCoord);
    }
}
