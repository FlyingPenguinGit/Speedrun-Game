using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public GameObject door;
    public void claim()
    {
        Destroy(door);
        Destroy(gameObject);
    }
}
