using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlinePlayerHandler : Photon.MonoBehaviour
{
    [SerializeField] GameObject[] uniqueObjects;
    [SerializeField] PlayerOnline playerOnline;
    [SerializeField] CircleCollider2D circleCollider2D;
    [SerializeField] Rigidbody2D rb;

    [SerializeField] Text nametagText;
    [SerializeField] GameObject nametagCanvas;


    SpriteRenderer sr;
    Color playerColor;

    private void Awake()
    {
        if (!photonView.isMine)
        {
            foreach (var obj in uniqueObjects)
            {
                obj.SetActive(false);
            }
            playerOnline.enabled = false;
            circleCollider2D.enabled = false;
            rb.isKinematic = true;
            nametagCanvas.SetActive(true);
            nametagText.text = photonView.owner.NickName;
        }
        else
        {
            photonView.RPC("SetColor", PhotonTargets.AllBuffered, PlayerPrefs.GetString("PlayerColor"));
        }
    }

    [PunRPC]
    void SetColor(string colorString)
    {

        ColorUtility.TryParseHtmlString("#" + colorString, out playerColor);
        sr = gameObject.GetComponentInChildren<SpriteRenderer>(false);
        sr.color = playerColor;
    }
}
