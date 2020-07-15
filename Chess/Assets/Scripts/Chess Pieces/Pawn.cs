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
        List<Vector3> filteredMoves = filter_out_own_pieces_and_outer_ones(wholeMoveMapCenterCells);
        handle_mark_points(filteredMoves);
    }

    private new List<Vector3Int> get_whole_move_map_cells()
    {
        Vector3Int up1 = get_up_1();
        Vector3Int up2 = get_up_2();
        Vector3Int upRight = get_diagonal_up_right();
        Vector3Int upLeft = get_diagonal_up_left();

        List<Vector3Int> cells = new List<Vector3Int>();
        
        if (is_free(upRight) == false)
        {
            cells.Add(upRight);
        }

        if (is_free(upLeft) == false)
        {
            cells.Add(upLeft);
        }

        if (is_free(up1))
        {
            cells.Add(up1);
        }

        if (timesMoved == 0 && is_free(up2))
        {
            cells.Add(up2);
        }

        return cells;
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