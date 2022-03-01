using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using System.Text;

public class PlayerOnline : Photon.MonoBehaviour
{
    Rigidbody2D rb;
    public Vector2 startvelocity;
    public PostProcessLayer PPL;

    public static bool pbHit;
    public float speedarrowSpeed = 30f;
    public GameObject speedText;
    public Text speedTXT;

    //star
    bool invincible = false;
    public Slider invincibilitySlider;

    //customize

    //grapple
    [SerializeField] AudioSource grappleSound;
    public LineRenderer lr;
    public DistanceJoint2D distanceJoint;
    public Transform firePos;
    bool blocked = false;
    public LayerMask whatIsGrappleable;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [SerializeField] GrappleAnimationOnline grappleAnimationScript;

    //swipe
    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;
    float swipetime = 0.8f;
    float swipestart;

    //abilities
    float cooldown = 5, waitedTime;
    public Slider CooldownSlider;

    //dash
    [SerializeField] AudioSource dashSound;
    public float dashSpeed, dashtime;
    bool dashing;
    float rightIndicator;
    public static AfterImagePool afterImagePool;

    public int checkpoint = 0;
    [SerializeField] Transform[] checkpointPositions;

    [SerializeField] GameObject countdownDisplay;

    private void Start()
    {
        checkpointPositions = GameObject.Find("Checkpoints").GetComponentsInChildren<Transform>();
        PhotonNetwork.player.SetScore(checkpoint);

        Time.timeScale = 1;
        Camera.main.orthographicSize = PlayerPrefs.GetFloat("camzoom", 5);
        if (PlayerPrefs.GetInt("postP", 0) == 1)
        {
            PPL.enabled = true;
        }
        else
        {
            PPL.enabled = false;
        }
        if (PlayerPrefs.GetInt("speed", 0) == 1)
        {
            speedText.SetActive(true);
        }
        else
        {
            speedText.SetActive(false);
        }

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = startvelocity;
        distanceJoint.enabled = false;
        lr.enabled = false;
        CooldownSlider.maxValue = cooldown;
        waitedTime = cooldown;
    }
    private void Update()
    {
        if (invincibilitySlider.gameObject.active)
        {
            invincibilitySlider.value -= Time.deltaTime;
            if (invincibilitySlider.value <= 0)
            {
                invincibilitySlider.gameObject.SetActive(false);
                invincible = false;
            }
        }
        if (Input.touches.Length > 0 && waitedTime >= cooldown)
        {
            Touch t = Input.GetTouch(Input.touches.Length - 1);
            if (t.position.x > Screen.width / 2)
            {
                rightIndicator = 1;
            }
            else if (t.position.x < Screen.width / 2)
            {
                rightIndicator = -1;
            }
            if (t.phase == TouchPhase.Began)
            {
                swipestart = Time.time;
                //save began touch 2d point
                firstPressPos = new Vector2(t.position.x, t.position.y);
            }
            if (t.phase == TouchPhase.Ended && swipestart > Time.time - swipetime)
            {
                //save ended touch 2d point
                secondPressPos = new Vector2(t.position.x, t.position.y);

                //create vector from the two points
                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                //swipe upwards
                if (currentSwipe.y > OptionsMenu.sensi)
                {
                    distanceJoint.enabled = false;
                    //lr.enabled = false;
                    photonView.RPC("EnableLR", PhotonTargets.All, false, Vector2.zero);
                    StartCoroutine(Dash(1, rightIndicator));
                }
                //swipe down
                if (currentSwipe.y < -(OptionsMenu.sensi))
                {
                    distanceJoint.enabled = false;
                    //lr.enabled = false;
                    photonView.RPC("EnableLR", PhotonTargets.All, false, Vector2.zero);
                    StartCoroutine(Dash(-1, rightIndicator));
                }
            }
        }
        if (dashing)
        {
            afterImagePool.ActivateImage();
        }

        if (Input.touchCount > 0 && !blocked)
        {
            if (!Helpers.IsOverUI(Input.GetTouch(0).position))
            {
                blocked = true;
                RaycastHit2D hit;
                if (Input.GetTouch(0).position.x > Screen.width / 2)
                {
                    hit = Physics2D.Raycast(firePos.transform.position, transform.right + transform.up, 100f, whatIsGrappleable);
                }
                else
                {
                    hit = Physics2D.Raycast(firePos.transform.position, -transform.right + transform.up, 100f, whatIsGrappleable);
                }

                if (hit.collider != null)
                {
                    //lr.SetPosition(0, hitPos);
                    //lr.SetPosition(1, transform.position);
                    //photonView.RPC("SetLRPositions", PhotonTargets.All, grapplePoint, transform.position);
                    grapplePoint = hit.point;
                    distanceJoint.connectedAnchor = grapplePoint;

                    distanceJoint.enabled = true;
                    grappleSound.pitch = Random.Range(8.0f, 12.0f) / 10;
                    grappleSound.PlayDelayed(0.05f);
                    //lr.enabled = true;
                    photonView.RPC("EnableLR", PhotonTargets.All, true, hit.point);
                }
            }
        }
        else if (Input.touchCount == 0 && blocked)
        {
            blocked = false;
            distanceJoint.enabled = false;
            //lr.enabled = false;
            photonView.RPC("EnableLR", PhotonTargets.All, false, Vector2.zero);
        }

        if (distanceJoint.enabled)
        {
            //distanceJoint.distance *= 0.9985f;
            distanceJoint.distance *= (1 - Time.deltaTime / 9);
            //lr.SetPosition(1, transform.position);
            //photonView.RPC("SetLRPosition", PhotonTargets.All, transform.position);
        }
        CooldownSlider.value = waitedTime;
        waitedTime += Time.deltaTime;
        if (speedText.active)
        {
            speedTXT.text = (Mathf.Round(Mathf.Abs(rb.velocity.x) * 100) / 100).ToString();
        }
    }
    IEnumerator Dash(float up, float right)
    {
        dashing = true;
        Vector2 saveVelocity = new Vector2(right * 4f + right * Mathf.Abs(rb.velocity.x) / 3.5f, up * 2.5f);
        rb.velocity = new Vector2(dashSpeed * 0.7f * right, dashSpeed * up);
        dashSound.Play();
        yield return new WaitForSeconds(dashtime);
        dashing = false;
        rb.velocity = saveVelocity;
        waitedTime = 0;
    }
    /*
    IEnumerator PowerUp()
    {
        dashPowerUp = true;
        yield return new WaitForSeconds(7);
        dashPowerUp = false;
    }
    */

    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        transform.position = checkpointPositions[checkpoint].position;
        photonView.RPC("EnableLR", PhotonTargets.All, false, Vector2.zero);
        distanceJoint.enabled = false;
        waitedTime = cooldown;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        int active = SceneManager.GetActiveScene().buildIndex;

        if (col.collider.tag == "death" && invincible == false)
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Respawn();
        }
        if (col.collider.tag == "finish")
        {
            photonView.RPC("PlayerFinished", PhotonTargets.All, PhotonNetwork.player.NickName);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "checkpoint")
        {
            checkpoint = collision.GetComponent<Checkpoint>().checkpointNR;
            PhotonNetwork.player.SetScore(checkpoint);
            ScoreboardHandler scoreboardHandler = GameObject.Find("ScoreboardCanvas").GetComponent<ScoreboardHandler>();
            scoreboardHandler.UpdateScoreboard();
        }
        if (collision.tag == "key")
        {
            collision.GetComponent<key>().claim();
        }
        if (collision.gameObject.layer == 8)
        {
            invincible = true;
            Destroy(collision.gameObject);
            invincibilitySlider.gameObject.SetActive(true);
        }

        if (collision.gameObject.layer == 9)
        {
            distanceJoint.enabled = false;
            //lr.enabled = false;
            photonView.RPC("EnableLR", PhotonTargets.All, false, Vector2.zero);
            rb.velocity = new Vector2(rb.velocity.x / 8, speedarrowSpeed);
        }
        else if (collision.gameObject.layer == 10)
        {
            distanceJoint.enabled = false;
            //lr.enabled = false;
            photonView.RPC("EnableLR", PhotonTargets.All, false, Vector2.zero);
            rb.velocity = new Vector2(rb.velocity.x / 8, 12);
        }
        else if (collision.gameObject.layer == 11)
        {
            distanceJoint.enabled = false;
            //lr.enabled = false;
            photonView.RPC("EnableLR", PhotonTargets.All, false, Vector2.zero);
            rb.velocity = new Vector2(rb.velocity.x / 8, -9);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag != "checkpoint")
        {
            rb.velocity *= 1.03f;
        }
    }

    [PunRPC]
    IEnumerator EnableLR(bool boolean, Vector2 grPos)
    {
        if (!photonView.isMine)
        {
            yield return new WaitForSeconds(0.07f);
        }

        grapplePoint = grPos;
        grappleDistanceVector = grapplePoint - (Vector2)transform.position;
        //lr.enabled = boolean;
        grappleAnimationScript.enabled = boolean;
    }
    /*
    [PunRPC]
    IEnumerator SetLRPositions(Vector2 hitPos, Vector3 playerPos)
    {
        if (!photonView.isMine)
        {
            yield return new WaitForSeconds(0.07f);
        }
        lr.SetPosition(0, hitPos);
        lr.SetPosition(1, playerPos);
    }

    [PunRPC]
    IEnumerator SetLRPosition(Vector3 playerPos)
    {
        if (!photonView.isMine)
        {
            yield return new WaitForSeconds(0.07f);
        }
        lr.SetPosition(1, playerPos);
    }
    */

    [PunRPC]
    IEnumerator PlayerFinished(string winningPlayer)
    {
        Time.timeScale = 0;
        countdownDisplay.SetActive(true);
        countdownDisplay.GetComponent<CountdownMatchStart>().winner = winningPlayer;
        yield return new WaitForSecondsRealtime(3);
        PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }
}

