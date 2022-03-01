using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeIndicator : MonoBehaviour
{
    void Update()
    {
        if(gameObject.activeInHierarchy && Input.touchCount > 0)
        {
            gameObject.SetActive(false);
        }
    }
}
