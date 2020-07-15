using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
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
        Vector3Int up = get_up_1();
        Vector3Int down = get_down();
        Vector3Int left = get_left();
        Vector3Int right = get_right();
        Vector3Int diagonalUpLeft = get_diagonal_up_left();
        Vector3Int diagonalUpRight = get_diagonal_up_right();
        Vector3Int diagonalDownLeft = get_diagonal_down_left();
        Vector3Int diagonalDownRight = get_diagonal_down_right();
         
        List<Vector3Int> cells = new List<Vector3Int>
        {
            up,
            down,
            left,
            right,
            diagonalUpLeft,
            diagonalUpRight,
            diagonalDownLeft,
            diagonalDownRight
        };

        return cells;
    }
}
