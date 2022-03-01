using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    [SerializeField] RectTransform[] levelSelects;
    [SerializeField] RectTransform canvas;
    [SerializeField] float percentThreshold = 0.2f;
    [SerializeField] float easing = 0.175f;
    int totalPages = 5;

    public UnityAction destroyStars;
    public float[] starTimes;
    public GameObject[] starObject;
    [SerializeField] PBs PBs;

    void Start()
    {
        for(int i = 0; i < levelSelects.Length; i++)
        {
            levelSelects[i].localPosition = new Vector3(canvas.rect.width * i, 0, 0);
        }
        transform.position = transform.position + new Vector3(-Screen.width * (Controller.worldNR - 1), 0, 0);
        panelLocation = transform.position;
    }
    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);

        if (destroyStars != null)
        {
            destroyStars.Invoke();
        }
    }
    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            if (percentage > 0)
            {
                if (Controller.worldNR < totalPages)
                {
                    Controller.worldNR++;
                    newLocation += new Vector3(-Screen.width, 0, 0);
                }
                else
                {
                    Controller.worldNR = 1;
                    newLocation += new Vector3(Screen.width * (totalPages - 1), 0, 0);
                }
            }
            else if (percentage < 0)
            {
                if (Controller.worldNR > 1)
                {
                    Controller.worldNR--;
                    newLocation += new Vector3(Screen.width, 0, 0);
                }
                else
                {
                    Controller.worldNR = totalPages;
                    newLocation += new Vector3(-Screen.width * (totalPages - 1), 0, 0);
                }
            }
            StartCoroutine(SmoothMove(transform.position, newLocation, easing));
            panelLocation = newLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, easing));
        }
    }
    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        PlaceStars();

        if (Controller.inPBScreen)
        {
            PBs.PB();
        }
    }

    void PlaceStars()
    {
        for (int i = (Controller.worldNR - 1) * 6; i < Controller.worldNR * 6; i++)
        {
            for (int j = 2; j >= 0; j--)
            {
                float relevantTime = PlayerPrefs.GetFloat("PBTime" + (i + 1), 0);
                if (relevantTime != 0 && relevantTime <= starTimes[i * 3 + j])
                {
                    starObject[(i - (Controller.worldNR - 1) * 6) * 3 + j].SetActive(true);
                    j = -1;
                }
            }
        }
    }
}
