using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Player")
        {
            if (PlayerPrefs.GetFloat("Endless", 0) == 0f || PlayerPrefs.GetFloat("Endless") < Timer.time)
            {
                PlayerPrefs.SetFloat("Endless", Timer.time);
            }
        }
    }
}
