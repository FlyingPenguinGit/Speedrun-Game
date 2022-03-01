using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiplayerLevelManager : MonoBehaviour
{
    [SerializeField] GameObject multiPlayerPrefab;
    [SerializeField] Color[] camColors;
    [SerializeField] Color camColor;

    void Awake()
    {
        var obj = PhotonNetwork.Instantiate(multiPlayerPrefab.name, Vector3.zero, Quaternion.identity, 0);
        obj.GetComponentInChildren<Camera>().backgroundColor = camColor;
    }
}
