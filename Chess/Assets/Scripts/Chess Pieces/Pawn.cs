using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool inverted;

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
        Vector3Int diagonalUpRight = get_diagonal_up_right();
        Vector3Int diagonalUpLeft = get_diagonal_up_left();

        Vector3Int down1 = get_down_1();
        Vector3Int down2 = get_down_2();
        Vector3Int diagonalDownRight = get_diagonal_down_right();
        Vector3Int diagonalDownLeft = get_diagonal_down_left();


        List<Vector3Int> cells = new List<Vector3Int>();

        if (inverted)
        {
            if (is_free(diagonalDownRight) == false)
            {
                cells.Add(diagonalDownRight);
            }

            if (is_free(diagonalDownLeft) == false)
            {
                cells.Add(diagonalDownLeft);
            }

            if (is_free(down1))
            {
                cells.Add(down1);
            }

            if (timesMoved == 0 && is_free(down1) && is_free(down2))
            {
                cells.Add(down2);
            }
        }
        else
        {
            if (is_free(diagonalUpRight) == false)
            {
                cells.Add(diagonalUpRight);
            }

            if (is_free(diagonalUpLeft) == false)
            {
                cells.Add(diagonalUpLeft);
            }

            if (is_free(up1))
            {
                cells.Add(up1);
            }

            if (timesMoved == 0 && is_free(up1) && is_free(up2))
            {
                cells.Add(up2);
            }
        }


        return cells;
    }
}