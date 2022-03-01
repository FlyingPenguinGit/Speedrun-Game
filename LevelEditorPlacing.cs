using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class LevelEditorPlacing : MonoBehaviour
{
    public Tile tile;
    public Tilemap[] highlightMap;

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (LevelEditor.tilemapIndex == 2)
            {
                for(int i = 0; i < highlightMap.Length; i++)
                {
                    // get current grid location
                    Vector3Int currentCell = highlightMap[i].WorldToCell(Camera.main.ScreenToWorldPoint(t.position));
                    // set the new tile
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.x -= 1;
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.y -= 1;
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.x += 1;
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.x += 1;
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.y += 1;
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.y += 1;
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.x -= 1;
                    highlightMap[i].SetTile(currentCell, null);
                    currentCell.x -= 1;
                    highlightMap[i].SetTile(currentCell, null);
                }
            }
            else
            {
                // get current grid location
                Vector3Int currentCell = highlightMap[LevelEditor.tilemapIndex].WorldToCell(Camera.main.ScreenToWorldPoint(t.position));
                // set the new tile
                highlightMap[LevelEditor.tilemapIndex].SetTile(currentCell, tile);
            }
        }
        else if(Input.touchCount == 2)
        {
            Touch t = Input.GetTouch(1);

            // get current grid location
            Vector3Int currentCell = highlightMap[LevelEditor.tilemapIndex].WorldToCell(Camera.main.ScreenToWorldPoint(t.position));
            // set the new tile
            highlightMap[LevelEditor.tilemapIndex].SetTile(currentCell, null);
        }
    }
}
