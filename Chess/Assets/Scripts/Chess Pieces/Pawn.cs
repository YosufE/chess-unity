using System.Collections.Generic;
using UnityEngine;

namespace Chess_Pieces
{
    public class Pawn : ChessPiece
    {
        public bool inverted;
        public GameObject upgradeMenuPrefab;
        UpgradeMenu upgradeMenu;
        public List<GameObject> enPassantRuleLinked;

        [Header("White Pieces Prefabs")] public GameObject whiteQueenPrefab;
        public GameObject whiteRookPrefab;
        public GameObject whiteBishopPrefab;
        public GameObject whiteKnightPrefab;

        [Header("Black Pieces Prefabs")] public GameObject blackQueenPrefab;
        public GameObject blackRookPrefab;
        public GameObject blackBishopPrefab;
        public GameObject blackKnightPrefab;


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

                    foreach (var killGameObject in pawnComponent.enPassantRuleLinked)
                    {
                        Pawn pawnComp = (Pawn) killGameObject.GetComponentInChildren(typeof(Pawn));
                        if (inverted)
                        {
                            Vector3 killMarkPoint =
                                get_center_of_cell(get_cell_down_times(pawnComp.get_current_cell(), 1));
                            if (markPointController.transform.position.x == killMarkPoint.x &&
                                markPointController.transform.position.y == killMarkPoint.y)
                            {
                                markPointController.connectedKillGameObject = killGameObject;
                            }
                        }
                        else
                        {
                            Vector3 killMarkPoint =
                                get_center_of_cell(get_cell_up_times(pawnComp.get_current_cell(), 1));
                            if (markPointController.transform.position.x == killMarkPoint.x &&
                                markPointController.transform.position.y == killMarkPoint.y)
                            {
                                markPointController.connectedKillGameObject = killGameObject;
                            }
                        }
                    }
                }
            }
        }

        public void link_kill_en_passant(Vector3Int oldCell, Vector3Int newCell)
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

        public void handle_upgrade(Vector3Int oldCell, Vector3Int newCell)
        {
            if ((inverted && newCell.y == 1) || (inverted == false && newCell.y == 8))
            {
                disable_all_piece_colliders();

                GameObject upgradeMenuGameObject = Instantiate(upgradeMenuPrefab);
                upgradeMenu = (UpgradeMenu) upgradeMenuGameObject.GetComponentInChildren(typeof(UpgradeMenu));
                upgradeMenu.connectedPawn = this;
                upgradeMenu.connectButtons();
            }
        }

        public void convert_to_queen()
        {
            GameObject newQueenObject;
            if (gameObject.tag == "White Piece")
            {
                newQueenObject = Instantiate(whiteQueenPrefab, transform.parent, true);
            }
            else
            {
                newQueenObject = Instantiate(blackQueenPrefab, transform.parent, true);
            }

            Queen queenComponent = (Queen) newQueenObject.GetComponentInChildren(typeof(Queen));
            queenComponent.move(get_current_cell());
            destroy_all_upgrade_menus();
            turnController.handle_turn();
        }

        public void convert_to_rook()
        {
            GameObject newRookObject;
            if (gameObject.tag == "White Piece")
            {
                newRookObject = Instantiate(whiteRookPrefab, transform.parent, true);
            }
            else
            {
                newRookObject = Instantiate(blackRookPrefab, transform.parent, true);
            }

            Rook rookComponent = (Rook) newRookObject.GetComponentInChildren(typeof(Rook));
            rookComponent.move(get_current_cell());
            destroy_all_upgrade_menus();
            turnController.handle_turn();
        }

        public void convert_to_bishop()
        {
            GameObject newBishopObject;
            if (gameObject.tag == "White Piece")
            {
                newBishopObject = Instantiate(whiteBishopPrefab, transform.parent, true);
            }
            else
            {
                newBishopObject = Instantiate(blackBishopPrefab, transform.parent, true);
            }

            Bishop bishopComponent = (Bishop) newBishopObject.GetComponentInChildren(typeof(Bishop));
            bishopComponent.move(get_current_cell());
            destroy_all_upgrade_menus();
            turnController.handle_turn();
        }

        public void convert_to_knight()
        {
            GameObject newKnightObject;
            if (gameObject.tag == "White Piece")
            {
                newKnightObject = Instantiate(whiteKnightPrefab, transform.parent, true);
            }
            else
            {
                newKnightObject = Instantiate(blackKnightPrefab, transform.parent, true);
            }

            Knight knightComponent = (Knight) newKnightObject.GetComponentInChildren(typeof(Knight));
            knightComponent.move(get_current_cell());
            destroy_all_upgrade_menus();
            turnController.handle_turn();
        }

        private GameObject[] get_all_upgrade_menu_game_objects()
        {
            GameObject[] upgradeMenuGameObjects = GameObject.FindGameObjectsWithTag("Upgrade Menu");
            return upgradeMenuGameObjects;
        }

        private void destroy_all_upgrade_menus()
        {
            GameObject[] upgradeMenus = get_all_upgrade_menu_game_objects();
            foreach (var upgradeMenu in upgradeMenus)
            {
                Destroy(upgradeMenu);
            }
        }
    }
}