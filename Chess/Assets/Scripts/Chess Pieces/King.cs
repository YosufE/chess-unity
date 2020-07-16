using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : ChessPiece
{
    public Rook castlingConnectedLeftRook;
    public Vector3Int leftRookCellDestination;

    public Rook castlingConnectedRighttRook;
    public Vector3Int rightRookCellDestination;


    private new void Awake()
    {
        base.Awake();

        castlingConnectedLeftRook = get_left_rook();
        leftRookCellDestination = get_cell_left_times(get_current_cell(), 1);

        castlingConnectedRighttRook = get_right_rook();
        rightRookCellDestination = get_cell_right_times(get_current_cell(), 1);
    }

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

        if (timesMoved == 0)
        {
            if (castlingConnectedLeftRook && castlingConnectedLeftRook.timesMoved == 0)
            {
                Vector3Int newCell = get_cell_left_times(get_current_cell(), 2);
                cells.Add(newCell);

                for (int i = 1; i < 4; i++)
                {
                    Vector3Int iLeftKingCell = get_cell_left_times(get_current_cell(), i);
                    if (is_free(iLeftKingCell) == false)
                    {
                        cells.Remove(newCell);
                        break;
                    }
                }
            }

            if (castlingConnectedRighttRook && castlingConnectedRighttRook.timesMoved == 0)
            {
                Vector3Int newCell = get_cell_right_times(get_current_cell(), 2);
                cells.Add(newCell);

                for (int i = 1; i < 3; i++)
                {
                    Vector3Int iRightKingCell = get_cell_right_times(get_current_cell(), i);
                    if (is_free(iRightKingCell) == false)
                    {
                        cells.Remove(newCell);
                        break;
                    }
                }
            }
        }


        return cells;
    }


    public Rook get_left_rook()
    {
        Vector3Int leftCell = get_cell_left_times(get_current_cell(), 4);
        GameObject leftPiece = get_piece_at_coord(get_center_of_cell(leftCell));
        if (leftPiece.GetComponentInChildren(typeof(Rook)))
        {
            Rook leftRook = (Rook) leftPiece.GetComponentInChildren(typeof(Rook));
            return leftRook;
        }

        return null;
    }

    public Rook get_right_rook()
    {
        Vector3Int rightCell = get_cell_right_times(get_current_cell(), 3);
        GameObject rightPiece = get_piece_at_coord(get_center_of_cell(rightCell));
        if (rightPiece.GetComponentInChildren(typeof(Rook)))
        {
            Rook rightRook = (Rook) rightPiece.GetComponentInChildren(typeof(Rook));
            return rightRook;
        }

        return null;
    }

    public void handle_castling_rule(Vector3Int oldCell, Vector3Int newCell)
    {
        GameObject[] markPoints = get_mark_points();

        if (gameObject.GetComponentInChildren(typeof(King)) && timesMoved == 0)
        {
            // Left Rook
            if (newCell.x - oldCell.x == -2)
            {
                foreach (var markPoint in markPoints)
                {
                    MarkPointController markPointController =
                        (MarkPointController) markPoint.GetComponentInChildren(typeof(MarkPointController));

                    if (leftRookCellDestination ==
                        get_cell_from_coord(markPointController.transform.position))
                    {
                        castlingConnectedLeftRook.move(leftRookCellDestination);
                    }
                }
            }

            // Right Rook
            if (newCell.x - oldCell.x == 2)
            {
                foreach (var markPoint in markPoints)
                {
                    MarkPointController markPointController =
                        (MarkPointController) markPoint.GetComponentInChildren(typeof(MarkPointController));

                    if (rightRookCellDestination ==
                        get_cell_from_coord(markPointController.transform.position))
                    {
                        castlingConnectedRighttRook.move(rightRookCellDestination);
                    }
                }
            }
        }
    }
}