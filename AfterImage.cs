using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour
{
    [SerializeField] SpriteRenderer SR;
    public SpriteRenderer playerSR;
    float alpha = 0.8f, alphaMultiplier = 0.9f;
    Color color;
    float timeActivated, activeTime = 0.5f;
    public AfterImagePool afterImagePool;

    private void OnEnable()
    {
        alpha = 0.8f;
        timeActivated = Time.time;
        var playerTrans = playerSR.transform;
        transform.position = playerTrans.position;
        transform.localScale = playerTrans.localScale;
    }
    public bool Initialize()
    {
        if (GameObject.Find("Rotate") == null)
        {
            Destroy(gameObject);
            return false;
        }
        color = playerSR.color;
        SR.color = color;
        SR.sprite = playerSR.sprite;
        return true;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;
        color.a = alpha;
        SR.color = color;
        transform.localScale *= 0.98f;

        if(Time.time > (timeActivated + activeTime))
        {
            afterImagePool.AddToPool(gameObject);
        }
    }
}
