using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditorMovement : MonoBehaviour
{
    Vector3 touchStart = Vector3.zero;
    public float zoomOutMin = 1;
    public float zoomOutMax = 8;
    bool zooming;
    float cooldown;
    public bool overview;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            touchStart = Vector3.zero;
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            zooming = true;
            zoom(difference * 0.002f);
        }
        else if (Input.touchCount == 1 && (cooldown <= 0 || overview))
        {
            print("good");
            if (!zooming)
            {
                print("better");
                if (Input.GetTouch(0).phase == TouchPhase.Began || touchStart == Vector3.zero)
                {
                    print("best");
                    touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }
                Vector3 direction = touchStart - Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                direction.z = 0;
                Camera.main.transform.position += direction;
            }
            else
            {
                zooming = false;
                cooldown = 0.125f;
            }
        }
        cooldown -= Time.deltaTime;
    }

    void zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, zoomOutMin, zoomOutMax);
    }
}
