using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class Player : MonoBehaviour
{
    [Header("General Things:")]
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

    [Header("Customize:")]
    //customize
    public SpriteRenderer _spriteRenderer;
    Color playerColor, trailColor;

    //WR rewards
    public TrailRenderer tr;
    public Gradient[] trailGradient;
    public GameObject[] skins, hats;

    [Header("Gameplay:")]
    //grapple
    [SerializeField] AudioSource grappleSound;
    public LineRenderer lr;
    public DistanceJoint2D distanceJoint;
    public Transform firePos;
    bool blocked = false;
    public LayerMask whatIsGrappleable;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;
    [SerializeField] GrappleAnimation grappleAnimationScript;

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

    //special
    public GameObject fail;


    private void Start()
    {
        Camera.main.orthographicSize = PlayerPrefs.GetFloat("camzoom", 5);
        skins[PlayerPrefs.GetInt("skin", 0)].SetActive(true);
        if(PlayerPrefs.GetInt("hat", 0) != 0)
        {
            hats[PlayerPrefs.GetInt("hat") - 1].SetActive(true);
        }
        if (PlayerPrefs.GetInt("customColor", 0) == 1)
        {
            int length = 2;
            var colorKeys = new GradientColorKey[length];
            var alphaKeys = new GradientAlphaKey[length];

            ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("TrailColor"), out trailColor);

            colorKeys[0].color = trailColor;
            colorKeys[0].time = 53;
            alphaKeys[0].alpha = trailColor.a;
            alphaKeys[0].time = 53;

            colorKeys[1].color = trailColor;
            colorKeys[1].time = 100;
            alphaKeys[1].alpha = trailColor.a;
            alphaKeys[1].time = 100;


            Gradient gradient = new Gradient();
            gradient.SetKeys(colorKeys, alphaKeys);
            tr.colorGradient = gradient;
            tr.enabled = true;
        }
        else if (PlayerPrefs.GetInt("SelectedReward", 0) != 0)
        {
            tr.colorGradient = trailGradient[PlayerPrefs.GetInt("SelectedReward") - 1];
            tr.enabled = true;
        }
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("PlayerColor"), out playerColor);
        _spriteRenderer.color = playerColor;
        if (PlayerPrefs.GetInt("postP", 0) == 1)
        {
            PPL.enabled = true;
        }
        else
        {
            PPL.enabled = false;
        }
        if(PlayerPrefs.GetInt("speed", 0) == 1)
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

        if(Controller.challengePlaying && PlayerPrefs.GetInt("adFree", 0) == 0)
        {
            AdsManager.adFree = false;
        }
    }
    private void Update()
    {
        if (invincibilitySlider.gameObject.active)
        {
            invincibilitySlider.value -= Time.deltaTime;
            if(invincibilitySlider.value <= 0)
            {
                invincibilitySlider.gameObject.SetActive(false);
                invincible = false;
            }
        }
        if (Input.touches.Length > 0 && waitedTime >= cooldown)
        {
            Touch t = Input.GetTouch(Input.touches.Length - 1);
            if(t.position.x > Screen.width / 2)
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
                    lr.enabled = false;
                    StartCoroutine(Dash(1, rightIndicator));
                }
                //swipe down
                if (currentSwipe.y < -(OptionsMenu.sensi))
                {
                    distanceJoint.enabled = false;
                    //lr.enabled = false;
                    grappleAnimationScript.enabled = false;
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
                    grapplePoint = hit.point;
                    //lr.SetPosition(0, grapplePoint);
                    //lr.SetPosition(1, transform.position);
                    grappleDistanceVector = grapplePoint - (Vector2)transform.position;
                    distanceJoint.connectedAnchor = grapplePoint;
                    grappleAnimationScript.enabled = true;

                    distanceJoint.enabled = true;
                    grappleSound.pitch = Random.Range(8.0f, 12.0f) / 10;
                    grappleSound.PlayDelayed(0.05f);
                    //lr.enabled = true;
                }
            }
        }
        else if(Input.touchCount == 0 && blocked)
        {
            blocked = false;
            distanceJoint.enabled = false;
            lr.enabled = false;
            grappleAnimationScript.enabled = false;
        }

        if (distanceJoint.enabled)
        {
            //distanceJoint.distance *= 0.9985f;
            distanceJoint.distance *= (1 - Time.deltaTime / 9);
            lr.SetPosition(1, transform.position);
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
    void DrawRope()
    {

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        int active = SceneManager.GetActiveScene().buildIndex;
            
        if(col.collider.tag == "death" && invincible == false)
        {
            if (Controller.challengePlaying)
            {
                Controller.challengePlaying = false;
                fail.SetActive(true);
                Time.timeScale = 0;
                StartCoroutine(LoadMenu());
            }
            else
            {
                if (active % 6 != 1)
                {
                    PlayerPrefs.SetFloat("tempWorldTime" + Controller.worldNR, PlayerPrefs.GetFloat("tempWorldTime" + Controller.worldNR, 0) + Timer.time);
                }
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        if(col.collider.tag == "finish")
        {
            if (Controller.challengePlaying)
            {
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins",0) + 250);
                Controller.challengePlaying = false;
            }
            PlayerPrefs.SetInt("playedlvl", active);
            if(PlayerPrefs.GetFloat("PBTime" + active) == 0f || PlayerPrefs.GetFloat("PBTime" + active) > Timer.time)
            {
                PlayerPrefs.SetFloat("PBTime" + active, Timer.time);
                pbHit = true;
            }
            if (Controller.inWorldSpeedrun && active % 6 != 0)
            {                
                PlayerPrefs.SetFloat("tempWorldTime" + Controller.worldNR, PlayerPrefs.GetFloat("tempWorldTime" + Controller.worldNR, 0) + Timer.time);
                SceneManager.LoadScene(active + 1);
            }
            else if (Controller.inWorldSpeedrun && SceneManager.GetActiveScene().buildIndex % 6 == 0)
            {
                if (PlayerPrefs.GetFloat("WorldTime" + Controller.worldNR, 0) == 0f || PlayerPrefs.GetFloat("WorldTime" + Controller.worldNR) > PlayerPrefs.GetFloat("tempWorldTime" + Controller.worldNR, 0) + Timer.time)
                {
                    PlayerPrefs.SetFloat("WorldTime" + Controller.worldNR, PlayerPrefs.GetFloat("tempWorldTime" + Controller.worldNR, 0) + Timer.time);
                }
                Controller.inWorldSpeedrun = false;                
            }
            if(!Controller.inWorldSpeedrun && PlayerPrefs.GetInt("showpb", 0) == 1)
            {
                SceneManager.LoadScene("PB Screen");
            }
            else if(!Controller.inWorldSpeedrun)
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "key")
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
            grappleAnimationScript.enabled = false;
            rb.velocity = new Vector2(rb.velocity.x / 8, speedarrowSpeed);
        }
        else if (collision.gameObject.layer == 10)
        {
            distanceJoint.enabled = false;
            //lr.enabled = false;
            grappleAnimationScript.enabled = false;
            rb.velocity = new Vector2(rb.velocity.x / 8, 12);
        }
        else if(collision.gameObject.layer == 11)
        {
            distanceJoint.enabled = false;
            //lr.enabled = false;
            grappleAnimationScript.enabled = false;
            rb.velocity = new Vector2(rb.velocity.x / 8, -9);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        rb.velocity *= 1.03f;
    }
    IEnumerator LoadMenu()
    {
        yield return new WaitForSecondsRealtime(2);
        SceneManager.LoadScene("Main Menu");
    }
}
