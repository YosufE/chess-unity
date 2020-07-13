using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    private void OnMouseDown()
    {
        get_whole_move_map_cells();
    }
}