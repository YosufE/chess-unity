using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChessPiece : MonoBehaviour
{
    public GameObject markPointPrefab;
    public int timesMoved = 0;

    private GridLayout gridLayout;
    private Grid grid;
    private Tilemap tilemap;

    private void Awake()
    {
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
        GameObject pieceAtCoord = get_piece_at_coord(coordinate);
        Destroy(pieceAtCoord);

        destroy_all_mark_points();
        transform.position = coordinate;

        timesMoved += 1;
    }

    public bool up_is_free(Vector3Int coordinate)
    {
        Vector3 upperCellCenter = get_center_of_cell(get_cell_up_times(coordinate, 1));
        Debug.Log(upperCellCenter);
        if (get_piece_at_coord(upperCellCenter))
        {
            Debug.Log(get_piece_at_coord(upperCellCenter));
            return false;
        }

        return true;
    }

    private GameObject get_piece_at_coord(Vector3 coord)
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
                    Debug.Log(pieceAtCoord);
                }
            }
        }

        return pieceAtCoord;
    }

    private GameObject[] get_mark_points()
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

    public List<Vector3> get_available_moves(List<Vector3> wholeMoveMapCenterCells)
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

    private String get_chess_piece()
    {
        String gameObjectName = gameObject.name;
        return gameObjectName;
    }

    public Vector3Int get_cell_pos()
    {
        Vector3Int cellPosition = gridLayout.WorldToCell(transform.position);
        return cellPosition;
    }

    public Vector3 get_center_of_cell(Vector3Int cellPos)
    {
        Vector3 worldPosition = grid.GetCellCenterWorld(cellPos);
        return worldPosition;
    }

    private string[] get_chess_lang()
    {
        Vector3Int cellPos = get_cell_pos();
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