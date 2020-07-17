using System;
using System.Collections;
using System.Collections.Generic;
using Chess_Pieces;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public Pawn connectedPawn;

    [Header("Button Gameobjects")] 
    public GameObject queenButtonGameObject;
    public GameObject rookButtonGameObject;
    public GameObject bishopButtonGameObject;
    public GameObject knightButtonGameObject;

    private Button queenButtonComponent;
    private Button rookButtonComponent;
    private Button bishopButtonComponent;
    private Button knightButtonComponent;


    private void Awake()
    {
        queenButtonComponent = queenButtonGameObject.GetComponent<Button>();
        rookButtonComponent = rookButtonGameObject.GetComponent<Button>();
        bishopButtonComponent = bishopButtonGameObject.GetComponent<Button>();
        knightButtonComponent = knightButtonGameObject.GetComponent<Button>();
    }

    public void connectButtons()
    {
        queenButtonComponent.onClick.AddListener(connectedPawn.convert_to_queen);
        rookButtonComponent.onClick.AddListener(connectedPawn.convert_to_rook);
        bishopButtonComponent.onClick.AddListener(connectedPawn.convert_to_bishop);
        knightButtonComponent.onClick.AddListener(connectedPawn.convert_to_knight);
    }
}