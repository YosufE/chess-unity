using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Chess_Pieces
{
    public class ChessPiece : MonoBehaviour
    {
        public GameObject markPointPrefab;
        public int timesMoved = 0;
        public TurnController turnController;

        private GridLayout gridLayout;
        private Grid grid;
        private Tilemap tilemap;

        public void Awake()
        {
            GameObject turnControllerGameObject = GameObject.FindGameObjectWithTag("Turn Controller");
            turnController = (TurnController) turnControllerGameObject.GetComponentInChildren(typeof(TurnController));
            gridLayout = transform.parent.GetComponentInParent<GridLayout>();
            grid = transform.parent.GetComponentInParent<Grid>();
        }

//    private void OnMouseDown()
//    {
//        String chessPiece = get_chess_piece();
//        Vector3Int cellPos = get_cell_pos();
//        String[] chessCoord = get_chess_lang();
//        Vector3 centerOfCell = get_center_of_cell(cellPos);
//        Debug.Log("piece: " + chessPiece + " | chess: (" + chessCoord[0] + ", " + chessCoord[1] + ") | cell: " +
//                  cellPos + " | center: " + centerOfCell);
//
//        move();
//    }

        public void move(Vector3 coordinate)
        {
            Vector3Int oldCell = get_current_cell();
            Vector3Int newCell = gridLayout.WorldToCell(coordinate);
            Vector3 newCellCenter = get_center_of_cell(newCell);

            GameObject pieceAtCoord = get_piece_at_coord(newCellCenter);
            Destroy(pieceAtCoord);

            destroy_all_mark_points();
            transform.position = newCellCenter;

            King otherKingComponent = null;
            try
            {
                otherKingComponent = (King) pieceAtCoord.GetComponentInChildren(typeof(King));
            }
            catch
            {
            }

            if (otherKingComponent)
            {
                disable_all_piece_colliders();
                turnController.show_end_screen();
            }
            else
            {
                // If killed Piece is NOT a King
                // Rule handling
                // ---- Pawn ----
                Pawn pawnComponent = (Pawn) GetComponentInChildren(typeof(Pawn));
                if (pawnComponent)
                {
                    pawnComponent.handle_en_passant(oldCell, newCell);
                    pawnComponent.handle_upgrade(oldCell, newCell);
                }

                // ---- King ----
                King kingComponent = (King) GetComponentInChildren(typeof(King));
                if (kingComponent)
                {
                    kingComponent.handle_castling_rule(oldCell, newCell);
                }

                turnController.handle_turn();
            }


            timesMoved += 1;
        }

        public void disable_all_piece_colliders()
        {
            List<GameObject> allPieces = get_all_pieces();
            foreach (var piece in allPieces)
            {
                piece.GetComponent<BoxCollider2D>().enabled = false;
            }
        }

        public bool is_free(Vector3Int cellPos)
        {
            if (get_piece_at_coord(get_center_of_cell(cellPos)))
            {
                return false;
            }

            return true;
        }


        public GameObject get_piece_at_coord(Vector3 coord)
        {
            GameObject pieceAtCoord = null;
            List<String> pieceTypes = new List<string>
            {
                "White Piece",
                "Black Piece"
            };

            foreach (var pieceType in pieceTypes)
            {
                GameObject[] chessPieces = GameObject.FindGameObjectsWithTag(pieceType);
                foreach (var chessPiece in chessPieces)
                {
                    if (coord == chessPiece.transform.position)
                    {
                        pieceAtCoord = chessPiece;
                    }
                }
            }

            return pieceAtCoord;
        }

        public GameObject[] get_mark_points()
        {
            GameObject[] markPoints = GameObject.FindGameObjectsWithTag("Mark Point");

            return markPoints;
        }

        private void destroy_all_mark_points()
        {
            GameObject[] markPoints = get_mark_points();

            foreach (var markPoint in markPoints)
            {
                Destroy(markPoint);
            }
        }

        public void instantiate_mark_points(List<Vector3> coords)
        {
            foreach (var coord in coords)
            {
                Vector3 newCoord = coord;
                newCoord.z -= 1;

                GameObject markPoint = Instantiate(markPointPrefab, newCoord, Quaternion.identity);
                MarkPointController markPointController = markPoint.GetComponent<MarkPointController>();
                markPointController.connectedPieceGameObject = this;
            }
        }

        public void handle_mark_points(List<Vector3> coords)
        {
            GameObject[] markPoints = get_mark_points();

            if (markPoints.Length <= 0)
            {
                instantiate_mark_points(coords);
                markPoints = get_mark_points();

                // Linking Mark Points 
                Pawn pawnComponent = (Pawn) GetComponentInChildren(typeof(Pawn));
                if (pawnComponent)
                {
                    pawnComponent.link_en_passant_rule_objs_to_mark_points(markPoints);
                }
            }
            else
            {
                destroy_all_mark_points();
            }
        }

        public List<Vector3Int> get_whole_move_map_cells()
        {
            throw new NotImplementedException();
        }

        public List<Vector3> filter_out_own_pieces_and_outer_ones(List<Vector3> wholeMoveMapCenterCells)
        {
            List<Vector3> availableMoves = new List<Vector3>();
            GameObject[] currentPieceColorObjects = GameObject.FindGameObjectsWithTag(gameObject.tag);
            GameObject[] chessTileObjects = GameObject.FindGameObjectsWithTag("Tile");

            foreach (var chessTileObject in chessTileObjects)
            {
                if (wholeMoveMapCenterCells.Contains(chessTileObject.transform.position))
                {
                    availableMoves.Add(chessTileObject.transform.position);
                }
            }

            foreach (var currentPieceColorObject in currentPieceColorObjects)
            {
                if (wholeMoveMapCenterCells.Contains(currentPieceColorObject.transform.position))
                {
                    availableMoves.Remove(currentPieceColorObject.transform.position);
                }
            }

            return availableMoves;
        }

        public List<Vector3> convert_into_whole_move_map_center_cells(List<Vector3Int> wholeMoveMapCells)
        {
            List<Vector3> wholeMoveMapCenterCells = new List<Vector3>();

            foreach (var cell in wholeMoveMapCells)
            {
                Vector3 centerCell = get_center_of_cell(cell);
                wholeMoveMapCenterCells.Add(centerCell);
            }

            return wholeMoveMapCenterCells;
        }

        public List<GameObject> get_all_pieces()
        {
            List<GameObject> allPieces = new List<GameObject>();
            List<String> pieceTypes = new List<string>
            {
                "White Piece",
                "Black Piece"
            };

            foreach (var pieceType in pieceTypes)
            {
                GameObject[] chessPieces = GameObject.FindGameObjectsWithTag(pieceType);
                foreach (var chessPiece in chessPieces)
                {
                    allPieces.Add(chessPiece);
                }
            }

            return allPieces;
        }

        public Vector3Int get_cell_up_times(Vector3Int cellPos, int amount)
        {
            cellPos.y += amount;
            return cellPos;
        }

        public Vector3Int get_cell_right_times(Vector3Int cellPos, int amount)
        {
            cellPos.x += amount;
            return cellPos;
        }

        public Vector3Int get_cell_left_times(Vector3Int cellPos, int amount)
        {
            cellPos.x -= amount;
            return cellPos;
        }

        public Vector3Int get_cell_down_times(Vector3Int cellPos, int amount)
        {
            cellPos.y -= amount;
            return cellPos;
        }

        public Vector3Int get_current_cell()
        {
            Vector3Int cellPosition = gridLayout.WorldToCell(transform.position);
            return cellPosition;
        }

        public Vector3Int get_cell_from_coord(Vector3 coord)
        {
            return gridLayout.WorldToCell(coord);
        }

        public Vector3 get_center_of_cell(Vector3Int cellPos)
        {
            Vector3 worldPosition = grid.GetCellCenterWorld(cellPos);
            return worldPosition;
        }

        public List<Vector3Int> get_all_up()
        {
            List<Vector3Int> allUp = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneUpLast = get_cell_up_times(lastCell, 1);
                allUp.Add(oneUpLast);
                lastCell = oneUpLast;
            }

            return allUp;
        }

        public List<Vector3Int> get_all_down()
        {
            List<Vector3Int> allDown = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneDownLast = get_cell_down_times(lastCell, 1);
                allDown.Add(oneDownLast);
                lastCell = oneDownLast;
            }

            return allDown;
        }

        public List<Vector3Int> get_all_left()
        {
            List<Vector3Int> allLeft = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneLeftLast = get_cell_left_times(lastCell, 1);
                allLeft.Add(oneLeftLast);
                lastCell = oneLeftLast;
            }

            return allLeft;
        }

        public List<Vector3Int> get_all_right()
        {
            List<Vector3Int> allRight = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneRightLast = get_cell_right_times(lastCell, 1);
                allRight.Add(oneRightLast);
                lastCell = oneRightLast;
            }

            return allRight;
        }

        public List<Vector3Int> get_all_diagonal_up_right()
        {
            List<Vector3Int> allDiagonalUpRight = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneDiagonalUpRightLast = get_cell_up_times(lastCell, 1);
                oneDiagonalUpRightLast = get_cell_right_times(oneDiagonalUpRightLast, 1);
                allDiagonalUpRight.Add(oneDiagonalUpRightLast);
                lastCell = oneDiagonalUpRightLast;
            }

            return allDiagonalUpRight;
        }

        public List<Vector3Int> get_all_diagonal_up_left()
        {
            List<Vector3Int> allDiagonalUpLeft = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneDiagonalUpLeftLast = get_cell_up_times(lastCell, 1);
                oneDiagonalUpLeftLast = get_cell_left_times(oneDiagonalUpLeftLast, 1);
                allDiagonalUpLeft.Add(oneDiagonalUpLeftLast);
                lastCell = oneDiagonalUpLeftLast;
            }

            return allDiagonalUpLeft;
        }

        public List<Vector3Int> get_all_diagonal_down_left()
        {
            List<Vector3Int> allDiagonalDownLeft = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneDiagonalDownLeftLast = get_cell_down_times(lastCell, 1);
                oneDiagonalDownLeftLast = get_cell_left_times(oneDiagonalDownLeftLast, 1);
                allDiagonalDownLeft.Add(oneDiagonalDownLeftLast);
                lastCell = oneDiagonalDownLeftLast;
            }

            return allDiagonalDownLeft;
        }

        public List<Vector3Int> get_all_diagonal_down_right()
        {
            List<Vector3Int> allDiagonalDownRight = new List<Vector3Int>();
            Vector3Int lastCell = get_current_cell();
            for (int i = 0; i < 8; i++)
            {
                Vector3Int oneDiagonalDownRightLast = get_cell_down_times(lastCell, 1);
                oneDiagonalDownRightLast = get_cell_right_times(oneDiagonalDownRightLast, 1);
                allDiagonalDownRight.Add(oneDiagonalDownRightLast);
                lastCell = oneDiagonalDownRightLast;
            }

            return allDiagonalDownRight;
        }


        public Vector3Int get_up_1()
        {
            Vector3Int up1 = get_current_cell();
            up1 = get_cell_up_times(up1, 1);

            return up1;
        }

        public Vector3Int get_up_2()
        {
            Vector3Int up2 = get_current_cell();
            up2 = get_cell_up_times(up2, 2);

            return up2;
        }

        public Vector3Int get_down_1()
        {
            Vector3Int down1 = get_current_cell();
            down1 = get_cell_down_times(down1, 1);

            return down1;
        }

        public Vector3Int get_down_2()
        {
            Vector3Int down2 = get_current_cell();
            down2 = get_cell_down_times(down2, 2);

            return down2;
        }

        public Vector3Int get_down()
        {
            Vector3Int down = get_current_cell();
            down = get_cell_down_times(down, 1);

            return down;
        }

        public Vector3Int get_left()
        {
            Vector3Int left = get_current_cell();
            left = get_cell_left_times(left, 1);

            return left;
        }

        public Vector3Int get_right()
        {
            Vector3Int right = get_current_cell();
            right = get_cell_right_times(right, 1);

            return right;
        }

        public Vector3Int get_diagonal_up_right()
        {
            Vector3Int upRight = get_current_cell();
            upRight = get_cell_up_times(upRight, 1);
            upRight = get_cell_right_times(upRight, 1);

            return upRight;
        }

        public Vector3Int get_diagonal_up_left()
        {
            Vector3Int upLeft = get_current_cell();
            upLeft = get_cell_up_times(upLeft, 1);
            upLeft = get_cell_left_times(upLeft, 1);

            return upLeft;
        }

        public Vector3Int get_diagonal_down_left()
        {
            Vector3Int downLeft = get_current_cell();
            downLeft = get_cell_down_times(downLeft, 1);
            downLeft = get_cell_left_times(downLeft, 1);

            return downLeft;
        }

        public Vector3Int get_diagonal_down_right()
        {
            Vector3Int downRight = get_current_cell();
            downRight = get_cell_down_times(downRight, 1);
            downRight = get_cell_right_times(downRight, 1);

            return downRight;
        }

        private String get_chess_piece()
        {
            String gameObjectName = gameObject.name;
            return gameObjectName;
        }

        private string[] get_chess_lang()
        {
            Vector3Int cellPos = get_current_cell();
            String[] converted = new String[2];

            String xPos = cellPos[0].ToString();
            String yPos = cellPos[1].ToString();

            String columns = "abcdefgh";
            xPos = columns[Int32.Parse(xPos) - 1].ToString();

            converted[0] = xPos;
            converted[1] = yPos;

            return converted;
        }
    }
}