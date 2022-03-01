using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AfterImagePool : MonoBehaviour
{

    [SerializeField] GameObject afterImage;
    private Queue<GameObject> objects = new Queue<GameObject>();
    public static AfterImagePool Instance { get; private set; }
    public SpriteRenderer SRPlayer;

    public  void InitialzePool()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Grow(28);
    }

    public void ActivateImage()
    {
        if(objects.Count == 0)
        {
            Grow(4);
        }
        var instance = objects.Dequeue();
        instance.SetActive(true);
    }
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        AfterImage AI = instance.GetComponent<AfterImage>();
        AI.afterImagePool = this;
        AI.playerSR = SRPlayer;
        if (AI.Initialize())
        {
            objects.Enqueue(instance);
        }        
    }
    void Grow(int size)
    {
        for (int i = 0; i < size; i++)
        {
            var instaceToAdd = Instantiate(afterImage);
            instaceToAdd.transform.SetParent(transform);
            AddToPool(instaceToAdd);
        }
    }

    public void UpdateSpriterenderers()
    {
        foreach(var obj in objects)
        {
            obj.GetComponent<AfterImage>().playerSR = SRPlayer;
        }
    }
}
