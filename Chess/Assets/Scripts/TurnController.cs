using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnController : MonoBehaviour
{
    public String currentTurn = "White Piece";
    public String otherTurn = "Black Piece";

    private void Awake()
    {
        disable_other_pieces();
    }

    public void handle_turn()
    {
        switch_turn();
        enable_current_pieces();
        disable_other_pieces();
    }

    private void switch_turn()
    {
        String temp = currentTurn;
        currentTurn = otherTurn;
        otherTurn = temp;
    }

    private void disable_other_pieces()
    {
        GameObject[] otherPieces = get_other_pieces();
        foreach (var piece in otherPieces)
        {
            piece.GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    private void enable_current_pieces()
    {
        GameObject[] currentPieces = get_current_pieces();
        foreach (var piece in currentPieces)
        {
            piece.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    private GameObject[] get_other_pieces()
    {
        GameObject[] otherPieces = GameObject.FindGameObjectsWithTag(otherTurn);

        return otherPieces;
    }

    private GameObject[] get_current_pieces()
    {
        GameObject[] currentPieces = GameObject.FindGameObjectsWithTag(currentTurn);

        return currentPieces;
    }
}