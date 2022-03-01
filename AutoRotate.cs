using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    public Rigidbody2D rb;
    float rotation = 0;
    private void Update()
    {
        if (rb.velocity.x > 0)
        {
            rotation -= 100 * Time.deltaTime * Mathf.Abs(rb.velocity.x);
        }
        else
        {
            rotation += 100 * Time.deltaTime * Mathf.Abs(rb.velocity.x);
        }
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
}
