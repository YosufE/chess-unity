﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rook : ChessPiece
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
        List<Vector3Int> allUp = get_all_up();
        List<Vector3Int> allDown = get_all_down();
        List<Vector3Int> allLeft = get_all_left();
        List<Vector3Int> allRight = get_all_right();
        List<List<Vector3Int>> tempList = new List<List<Vector3Int>>
        {
            allUp,
            allDown,
            allLeft,
            allRight
        };

        List<Vector3Int> cells = new List<Vector3Int>();
        foreach (var direction in tempList)
        {
            foreach (var cell in direction)
            {
                if (is_free(cell))
                {
                    cells.Add(cell);
                }
                else
                {
                    GameObject chessPiece = get_piece_at_coord(get_center_of_cell(cell));
                    if (chessPiece)
                    {
                        cells.Add(cell);
                    }

                    break;
                }
            }
        }

        return cells;
    }

    
}