using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelEditorSelect : MonoBehaviour
{
    public Tile tile;
    public Tilemap highlightMap;
    Vector3 pos1, pos2;
    Vector3Int posCell1, posCell2;
    bool pos;
    int count = 1;
    public static bool selection;
    void Update()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            count++;
            if (!pos)
            {
                highlightMap.ClearAllTiles();
                if (count > 1)
                {
                    selection = false;
                    pos1 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                    pos = true;
                }
            }
            else if (pos)
            {
                count = 0;
                pos = false;
                pos2 = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                selection = true;
                FillSelection(highlightMap, false);
            }
        }
    }
    public void FillSelection(Tilemap map, bool eraser)
    {
        posCell1 = map.WorldToCell(pos1);
        posCell2 = map.WorldToCell(pos2);
        for (int y = posCell1.y; y != posCell2.y; y++)
        {

            for (int x = posCell1.x; x != posCell2.x; x++)
            {
                if (eraser)
                {
                    map.SetTile(new Vector3Int(x, y, posCell1.z), null);
                }
                else
                {
                    map.SetTile(new Vector3Int(x, y, posCell1.z), tile);
                }

                if (posCell1.x > posCell2.x)
                {
                    x -= 2;
                }
            }
            if (posCell1.y > posCell2.y)
            {
                y -= 2;
            }
        }
    }
    public void RectSelection(Tilemap map)
    {
        posCell1 = map.WorldToCell(pos1);
        posCell2 = map.WorldToCell(pos2);

        int y = posCell1.y;
        for (int x = posCell1.x; x != posCell2.x; x++)
        {
            map.SetTile(new Vector3Int(x, y, posCell1.z), tile);

            if (posCell1.x > posCell2.x)
            {
                x -= 2;
            }
        }
        y = posCell2.y;
        for (int x = posCell1.x; x != posCell2.x; x++)
        {
            map.SetTile(new Vector3Int(x, y, posCell1.z), tile);

            if (posCell1.x > posCell2.x)
            {
                x -= 2;
            }
        }
        int cellx = posCell1.x;
        for (y = posCell1.y; y != posCell2.y; y++)
        {
            map.SetTile(new Vector3Int(cellx, y, posCell1.z), tile);
            if (posCell1.y > posCell2.y)
            {
                y -= 2;
            }
        }
        cellx = posCell2.x;
        for (y = posCell1.y; y != posCell2.y; y++)
        {
            map.SetTile(new Vector3Int(cellx, y, posCell1.z), tile);
            if (posCell1.y > posCell2.y)
            {
                y -= 2;
            }
        }
    }
}
