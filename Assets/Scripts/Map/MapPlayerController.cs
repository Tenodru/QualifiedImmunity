using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MapPlayerController : MonoBehaviour
{
    public static MapPlayerController current;
    [Header("References")]
    [SerializeField] Camera mapCamera;

    [Header("Movement")]
    public float maxSpeed = 7;
    public float maxSpeedScaling = .3f;
    private float currentMaxSpeed;
    [Range(.6f, .9f)] public float diagonalMultiplier = .8f;
    [Range(.05f, 2)] public float timeToMaxSpeed = .27f;

    [Header("Friction")]
    public float directionChangeMultiplier = 3f;
    public float stopMultiplier = 3f; // bigger is faster
    private float xSpeedSmoothing;
    private float ySpeedSmoothing;

    [Header("iFrames")]
    public float iframeTime = 1f;
    private float iframeTimer = 0f;

    // Component references
    private Rigidbody2D rb;
    CameraMovement camMovement;

    // Status monitors
    [Header("Testing")]
    public bool mounted = false;
    public float mountSpeed = 10;
    bool playerCanMove = true;
    //bool canFireBullet = true;
    private UIHoverListener hoverCheck;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        rb = GetComponent<Rigidbody2D>();
        //camMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        hoverCheck = UIHoverListener.FindObjectOfType<UIHoverListener>();

        this.GetComponent<Rigidbody2D>().gravityScale = 0f;
        ResetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        Inputs();
        if (iframeTimer >= 0)
        {
            Iframes();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void LateUpdate()
    {

    }

    void Movement()
    {
        if (!mounted)
        {
            Vector2 input = new Vector2(playerCanMove ? Input.GetAxisRaw("Horizontal") : 0, playerCanMove ? Input.GetAxisRaw("Vertical") : 0); // get player movement input
            float xSpeed = currentMaxSpeed / timeToMaxSpeed * input.x * Time.fixedDeltaTime; // amount to change current horizontal speed by
            float ySpeed = currentMaxSpeed / timeToMaxSpeed * input.y * Time.fixedDeltaTime; // amount to change current vertical speed by

            if (input.x == 0) // no horizontal input
            {
                xSpeed = (-Mathf.SmoothDamp(rb.velocity.x, 0, ref xSpeedSmoothing, .05f) * Time.fixedDeltaTime) * stopMultiplier; // change horizontal speed by negative amount
            }

            if (input.y == 0) // no vertical input
            {
                ySpeed = (-Mathf.SmoothDamp(rb.velocity.y, 0, ref ySpeedSmoothing, .05f) * Time.fixedDeltaTime) * stopMultiplier; // change vertical speed by negative amount
            }

            // change current speed
            rb.velocity += new Vector2(input.x == rb.velocity.x / Mathf.Abs(rb.velocity.x) ? xSpeed : xSpeed * directionChangeMultiplier, input.y == rb.velocity.y / Mathf.Abs(rb.velocity.y) ? ySpeed : ySpeed * directionChangeMultiplier);

            // limit to max speed
            rb.velocity = new Vector2(Mathf.Abs(rb.velocity.x) > currentMaxSpeed ? currentMaxSpeed * input.x : rb.velocity.x, Mathf.Abs(rb.velocity.y) > currentMaxSpeed ? currentMaxSpeed * input.y : rb.velocity.y);

            if (Mathf.Abs(rb.velocity.x) > 1f && Mathf.Abs(rb.velocity.y) > 1f) // if moving diagonally
            {
                currentMaxSpeed = maxSpeed * diagonalMultiplier; // lower max speed
            }
            else // if not moving diagonally
            {
                currentMaxSpeed = maxSpeed; // retain max speed
            }
        }

        if (mounted)
        {
            Vector2 dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
            Vector2 dir2 = dir.normalized;
            if (playerCanMove)
            {
                rb.velocity = new Vector2(dir2.x * mountSpeed, dir2.y * mountSpeed);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }

        var dir3 = Input.mousePosition - mapCamera.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir3.y, dir3.x) * Mathf.Rad2Deg;

    }

    /// <summary>
    /// Re-centers this player.
    /// </summary>
    public void ResetPlayer()
    {
        transform.position = new Vector3(-44.5f, -1.4f, -10f);
    }

    public void Inputs()
    {

    }

    public IEnumerator Timer(Action<bool> assigner, float timer)
    {
        yield return new WaitForSeconds(timer);
        assigner(true);
    }

    public void Iframes()
    {
        iframeTimer -= Time.deltaTime;
    }

}
