using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Missile : MonoBehaviour
{
    public Transform player;
    private Rigidbody2D rb;

    public float speed, rotateSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = (transform.right + transform.up).normalized * speed;
    }

    void FixedUpdate()
    {
        Vector2 direction = (Vector2)player.position - rb.position;
        Vector2 save = direction;
        direction.Normalize();

        Vector2 forward = (transform.right + transform.up).normalized;
        float rotateAmount = Vector3.Cross(direction, forward).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = forward * (save.magnitude + Time.timeSinceLevelLoad / 45f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            StartCoroutine(MissileHit());
        }
    }
    IEnumerator MissileHit()
    {
        if (PlayerPrefs.GetFloat("Endless", 0) == 0f || PlayerPrefs.GetFloat("Endless") < Timer.time)
        {
            PlayerPrefs.SetFloat("Endless", Timer.time);
        }
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
