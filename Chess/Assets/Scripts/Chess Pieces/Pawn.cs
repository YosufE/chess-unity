using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : ChessPiece
{
    public bool inverted;
    public List<GameObject> enPassantRuleLinked;

    private void OnMouseDown()
    {
        List<Vector3Int> wholeMoveMapCells = get_whole_move_map_cells();
        List<Vector3> wholeMoveMapCenterCells = convert_into_whole_move_map_center_cells(wholeMoveMapCells);
        List<Vector3> filteredMoves = filter_out_own_pieces_and_outer_ones(wholeMoveMapCenterCells);
        handle_mark_points(filteredMoves, gameObject);
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

            foreach (var obj in enPassantRuleLinked)
            {
                if (obj != null)
                {
                    if (is_free(get_cell_from_coord(obj.transform.position)) == false)
                    {
                        Vector3Int cellBelowLinkedPiece = new Vector3Int();
                        cellBelowLinkedPiece = get_cell_down_times(get_cell_from_coord(obj.transform.position), 1);
                        cells.Add(cellBelowLinkedPiece);
                    }
                }
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

            foreach (var obj in enPassantRuleLinked)
            {
                if (obj != null)
                {
                    if (is_free(get_cell_from_coord(obj.transform.position)) == false)
                    {
                        Vector3Int cellAboveLinkedPiece = new Vector3Int();
                        cellAboveLinkedPiece = get_cell_up_times(get_cell_from_coord(obj.transform.position), 1);
                        cells.Add(cellAboveLinkedPiece);
                    }
                }
            }
        }


        return cells;
    }

    public void link_en_passant_rule_objs_to_mark_points(GameObject[] markPoints)
    {
        if (gameObject.GetComponentInChildren(typeof(Pawn)))
        {
            Pawn pawnComponent = (Pawn) GetComponentInChildren(typeof(Pawn));
            foreach (var markPoint in markPoints)
            {
                MarkPointController markPointController =
                    (MarkPointController) markPoint.GetComponentInChildren(typeof(MarkPointController));

                foreach (var ob in pawnComponent.enPassantRuleLinked)
                {
                    markPointController.connectedKillGameObject = ob;
                }
            }
        }
    }

    public void handle_en_passant(Vector3Int oldCell, Vector3Int newCell)
    {
        if (gameObject.GetComponentInChildren(typeof(Pawn)))
        {
            if (newCell.y - oldCell.y == 2 || newCell.y - oldCell.y == -2)
            {
                GameObject leftPiece = null;
                GameObject rightPiece = null;
                leftPiece = get_piece_at_coord(get_center_of_cell(get_cell_left_times(newCell, 1)));
                rightPiece = get_piece_at_coord(get_center_of_cell(get_cell_right_times(newCell, 1)));
                GameObject[] pieces = {leftPiece, rightPiece};

                foreach (var piece in pieces)
                {
                    if (piece && piece.gameObject.tag != gameObject.tag)
                    {
                        Pawn pawnComponent = (Pawn) piece.GetComponentInChildren(typeof(Pawn));
                        if (pawnComponent)
                        {
                            pawnComponent.enPassantRuleLinked.Add(gameObject);
                        }
                    }
                }
            }
        }
    }
}