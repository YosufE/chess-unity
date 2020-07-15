using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : ChessPiece
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
        // Top Right Corner
        Vector3Int topRight = get_top_right();
        Vector3Int rightTop = get_right_top();

        // Top Left Corner
        Vector3Int topLeft = get_top_left();
        Vector3Int leftTop = get_left_top();

        // Bottom Left Corner
        Vector3Int bottomLeft = get_bottom_left();
        Vector3Int leftBottom = get_left_bottom();

        // Bottom Right Corner
        Vector3Int bottomRight = get_bottom_right();
        Vector3Int rightBottom = get_right_bottom();

        List<Vector3Int> cells = new List<Vector3Int>
        {
            topRight,
            rightTop,
            topLeft,
            leftTop,
            bottomLeft,
            leftBottom,
            bottomRight,
            rightBottom
        };

        return cells;
    }

    private Vector3Int get_top_right()
    {
        Vector3Int topRight = get_cell_pos();
        topRight = get_cell_up_times(topRight, 2);
        topRight = get_cell_right_times(topRight, 1);

        return topRight;
    }

    private Vector3Int get_right_top()
    {
        Vector3Int rightTop = get_cell_pos();
        rightTop = get_cell_right_times(rightTop, 2);
        rightTop = get_cell_up_times(rightTop, 1);

        return rightTop;
    }

    private Vector3Int get_top_left()
    {
        Vector3Int topLeft = get_cell_pos();
        topLeft = get_cell_up_times(topLeft, 2);
        topLeft = get_cell_left_times(topLeft, 1);

        return topLeft;
    }

    private Vector3Int get_left_top()
    {
        Vector3Int leftTop = get_cell_pos();
        leftTop = get_cell_left_times(leftTop, 2);
        leftTop = get_cell_up_times(leftTop, 1);

        return leftTop;
    }

    private Vector3Int get_bottom_left()
    {
        Vector3Int bottomLeft = get_cell_pos();
        bottomLeft = get_cell_down_times(bottomLeft, 2);
        bottomLeft = get_cell_left_times(bottomLeft, 1);

        return bottomLeft;
    }

    private Vector3Int get_left_bottom()
    {
        Vector3Int leftBottom = get_cell_pos();
        leftBottom = get_cell_left_times(leftBottom, 2);
        leftBottom = get_cell_down_times(leftBottom, 1);

        return leftBottom;
    }

    private Vector3Int get_bottom_right()
    {
        Vector3Int bottomRight = get_cell_pos();
        bottomRight = get_cell_down_times(bottomRight, 2);
        bottomRight = get_cell_right_times(bottomRight, 1);

        return bottomRight;
    }

    private Vector3Int get_right_bottom()
    {
        Vector3Int rightBottom = get_cell_pos();
        rightBottom = get_cell_right_times(rightBottom, 2);
        rightBottom = get_cell_down_times(rightBottom, 1);

        return rightBottom;
    }
}