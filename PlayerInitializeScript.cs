using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInitializeScript : MonoBehaviour
{
    [SerializeField] GameObject poolPrefab;
    static bool prefabInitialized = false;
    [SerializeField] GameObject actualSRObj;
    static AfterImagePool afterImagePool;

    private void Start()
    {
        Time.timeScale = 1;
        if (!prefabInitialized)
        {
            var prefab = Instantiate(poolPrefab);
            afterImagePool = prefab.GetComponent<AfterImagePool>();
            Player.afterImagePool = afterImagePool;
            PlayerOnline.afterImagePool = afterImagePool;
            afterImagePool.SRPlayer = actualSRObj.GetComponentInChildren<SpriteRenderer>(false);
            afterImagePool.InitialzePool();
            prefabInitialized = true;
        }
        else
        {
            afterImagePool.SRPlayer = actualSRObj.GetComponentInChildren<SpriteRenderer>(false);
            afterImagePool.UpdateSpriterenderers();
        }
    }
}
