using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DeleteThis : MonoBehaviour
{
    private GridLayout gridLayout;
    public Tilemap tilemap;
    // Start is called before the first frame update
    void Start()
    {
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnMouseDown()
    {
        Vector3Int cellPos = get_cell_pos(gameObject);
        String[] chessCoord = convert_cell_to_chess_lang(cellPos);
        Vector3 worldPos = convert_cell_to_world(cellPos);
        Debug.Log("chess: (" + chessCoord[0] + ", " + chessCoord[1] + ") | cell: " + cellPos + " | world: " + worldPos);
//        Debug.Log(tilemap.GetTile(cellPos));
    }
    
    public Vector3Int get_cell_pos(GameObject gameObject)
    {
        Vector3Int cellPosition = gridLayout.WorldToCell(gameObject.transform.position);
//        transform.position = gridLayout.CellToWorld(cellPosition);
        return cellPosition;
    }
    
    public Vector3 convert_cell_to_world(Vector3Int cellPos)
    {
        Vector3 worldPosition = gridLayout.CellToWorld(cellPos);
        return worldPosition;
    }

    public string[] convert_cell_to_chess_lang(Vector3Int cellPos)
    {
        String[] converted = new String[2];
        
        String xPos = cellPos[0].ToString();
        String yPos = cellPos[1].ToString();

        String columns = "abcdefgh";
        xPos = columns[Int32.Parse(xPos)-1].ToString();
        
        converted[0] = xPos;
        converted[1] = yPos;
        
        return converted;
    }

}
