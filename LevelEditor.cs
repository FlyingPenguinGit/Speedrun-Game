using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using System.IO;

public class LevelEditor : MonoBehaviour
{
    //moving && placing
    public LevelEditorMovement editorMovement;
    public LevelEditorPlacing editorPlacing;
    public LevelEditorMoveItems moveItems;
    public LevelEditorSelect levelEditorSelect;    
    public TilemapManager tilemapManager;
    public static int tilemapIndex = 0;
    public GameObject Options;
    bool move, place;
    public Dropdown dropdown;
    List<string> files = new List<string>();
    string[] paths;

    //colors
    public Color[] camColors, obstacleColors;
    public GameObject[] worldbuttons;
    public Tilemap[] tilemap;
    int worldnr;

    //items
    public GameObject[] itemsArray;
    public int[] itemsRotations;
    public GameObject itemContainer;
    public void EnableMove()
    {
        editorMovement.enabled = true;
        editorPlacing.enabled = false;
        moveItems.enabled = false;
        levelEditorSelect.enabled = false;
    }
    public void SelectTilemap(int index)
    {
        if (LevelEditorSelect.selection)
        {
            LevelEditorSelect.selection = false;
            tilemap[2].ClearAllTiles();
            if(index == 0)
            {
                levelEditorSelect.FillSelection(tilemap[index], false);
            }
            else
            {
                levelEditorSelect.FillSelection(tilemap[0], true);
                levelEditorSelect.FillSelection(tilemap[1], true);
                if(index == 1)
                {
                    levelEditorSelect.RectSelection(tilemap[1]);
                }
            }
        }
        else
        {
            tilemapIndex = index;
            editorMovement.enabled = false;
            editorPlacing.enabled = true;
            moveItems.enabled = false;
            levelEditorSelect.enabled = false;
        }
    }
    public void MoveItems()
    {
        moveItems.enabled = true;
        editorMovement.enabled = false;
        levelEditorSelect.enabled = false;
        editorPlacing.enabled = false;
    }
    public void Items()
    {
        levelEditorSelect.enabled = false;
        editorMovement.enabled = false;
        editorPlacing.enabled = false;
        itemContainer.SetActive(!itemContainer.active);
    }
    public void SelectTool()
    {
        levelEditorSelect.enabled = true;
        editorMovement.enabled = false;
        editorPlacing.enabled = false;
        moveItems.enabled = false;
    }
    public void Pause()
    {
        Options.SetActive(true);
        move = editorMovement.isActiveAndEnabled;
        place = editorPlacing.isActiveAndEnabled;
        editorMovement.enabled = false;
        editorPlacing.enabled = false;
        moveItems.enabled = false;
        dropdown.ClearOptions();

        paths = System.IO.Directory.GetFiles(Application.persistentDataPath);
        foreach (string file in paths)
        {
            files.Add(Path.GetFileName(file));
        }
        dropdown.AddOptions(files);
    }
    public void Load()
    {
        tilemapManager.LoadMap(paths[dropdown.value]);
    }
    public void Unpause()
    {
        Options.SetActive(false);
        editorMovement.enabled = move;
        editorPlacing.enabled = place;
    }
    public void Menu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void SwitchWorld()
    {
        worldnr++;
        if(worldnr == 6)
        {
            worldnr = 1;
        }
        for(int i = 0; i < worldbuttons.Length; i++)
        {
            worldbuttons[i].SetActive(false);
        }
        worldbuttons[worldnr % 5].SetActive(true);
        Camera.main.backgroundColor = camColors[worldnr - 1];
        tilemap[1].color = obstacleColors[worldnr - 1];
    }    
    public void SelectItem(int itemNR)
    {
        itemContainer.SetActive(false);
        var instantiatedItem = Instantiate(itemsArray[itemNR]);
        instantiatedItem.SetActive(true);
        tilemapManager.Items.Add(instantiatedItem);
    }    
    public void Instantiate(int ID, float X, float Y)
    {
        if (ID > 0)
        {
            GameObject instantiatedItem = Instantiate(itemsArray[ID - 1], new Vector3(X, Y, 0), Quaternion.Euler(0,0, itemsRotations[ID - 1]));
            instantiatedItem.SetActive(true);
            tilemapManager.Items.Add(instantiatedItem);
        }
    }    
}
