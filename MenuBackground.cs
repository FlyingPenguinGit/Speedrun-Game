using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    [SerializeField] LineRenderer lr;
    [SerializeField] SpriteRenderer spriteRenderer;
    Color playerColor;

    private void Start()
    {
        lr.SetPosition(0, Vector3.zero);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("PlayerColor"), out playerColor);
        spriteRenderer.color = playerColor;
    }
    void Update()
    {
        lr.SetPosition(1, transform.position);
    }
}
