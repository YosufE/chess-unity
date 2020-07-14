using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    private void OnMouseDown()
    {
        List<Vector3Int> wholeMoveMapCells = get_whole_move_map_cells();
        List<Vector3> wholeMoveMapCenterCells = convert_into_whole_move_map_center_cells(wholeMoveMapCells);
        List<Vector3> availableMoves = get_available_moves(wholeMoveMapCenterCells);
        handle_mark_points(availableMoves);
    }

    private new List<Vector3Int> get_whole_move_map_cells()
    {
        Vector3Int up1 = get_up_1();
        Vector3Int up2 = get_up_2();
        Vector3Int upRight = get_diagonal_up_right();
        Vector3Int upLeft = get_diagonal_up_left();
        

        List<Vector3Int> coords = new List<Vector3Int>
        {
            up1,
            up2,
            upRight,
            upLeft
        };

        return coords;
    }

    private Vector3Int get_up_1()
    {
        Vector3Int up1 = get_cell_pos();
        up1 = get_cell_up_times(up1, 1);

        return up1;
    }

    private Vector3Int get_up_2()
    {
        Vector3Int up2 = get_cell_pos();
        up2 = get_cell_up_times(up2, 2);

        return up2;
    }

    private Vector3Int get_diagonal_up_right()
    {
        Vector3Int up2 = get_cell_pos();
        up2 = get_cell_up_times(up2, 1);
        up2 = get_cell_right_times(up2, 1);

        return up2;
    }

    private Vector3Int get_diagonal_up_left()
    {
        Vector3Int up2 = get_cell_pos();
        up2 = get_cell_up_times(up2, 1);
        up2 = get_cell_left_times(up2, 1);

        return up2;
    }
}
