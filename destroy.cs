using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class destroy : MonoBehaviour
{
    [SerializeField] PageSwiper pageSwiper;

    private void Awake()
    {
        pageSwiper.destroyStars += Deactivate;
    }
    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
