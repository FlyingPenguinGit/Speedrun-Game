using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelEditorMoveItems : MonoBehaviour
{
    bool gothit = false;
    RaycastHit2D hit;
    Transform hitObject;
    public GameObject button;
    private void Update()
    {
        if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
        {
            Touch t = Input.GetTouch(0);
            if(hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(t.position), transform.forward))
            {
                gothit = true;
                if (hit.transform.parent != null)
                {
                    hitObject = hit.transform.parent.gameObject.transform;
                }
                else
                {
                    hitObject = hit.transform;
                }
            }
        }
        else if(Input.touchCount == 1)
        {
            if (gothit)
            {
                hitObject.transform.position = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                button.SetActive(true);
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                {
                    if (hitObject != null)
                    {
                        Destroy(hitObject.gameObject);
                        gothit = false;
                    }
                }
            }
            
        }
        else
        {
            button.SetActive(false);
            gothit = false;
        }
    }
}
