using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController current;
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

    [Header("Bullets")]
    public ParticleSystem basicBullet;

    public float projectileSpeed = 12f;
    public float projectileDamage = 1f;
    public float bulletCooldown;

    [Header("iFrames")]
    public float iframeTime = 1f;
    private float iframeTimer = 0f;
    public float knockForce;
    public float knockbackTime = 0.5f;

    [Header("Interaction")]
    public GameObject feet;
    public KeyCode interactionKey = KeyCode.E;
    public float interactionRadius = 1.0f;

    // component references
    private SpriteRenderer playerSprite;
    private Rigidbody2D rb;
    CameraMovement camMovement;

    // status monitors
    [Header("Testing")]
    public bool mounted = false;
    public float mountSpeed = 10;
    public bool playerCanMove = true;
    bool canFireBullet = true;
    public bool teleporting = false;

    // Animation variables.
    Animator anim;
    float xSpee;
    float ySpee;

    // Start is called before the first frame update
    void Start()
    {
        current = this;
        rb = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        camMovement = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraMovement>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                Debug.Log(hit.transform.gameObject.name);
            }
        }
        Inputs();
        if (iframeTimer >= 0)
        {
            Iframes();
        }

        if (GetComponent<Animator>() != null)
        {
            Animate();
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    public void Animate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            anim.StopPlayback();
            anim.speed = Mathf.Abs(ySpee/5);
            anim.SetBool("WalkUp", true);
            anim.SetBool("WalkDown", false);
            anim.SetBool("WalkLeft", false);
            anim.SetBool("WalkRight", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            anim.StopPlayback();
            anim.speed = Mathf.Abs(ySpee/5);
            anim.SetBool("WalkUp", false);
            anim.SetBool("WalkDown", true);
            anim.SetBool("WalkLeft", false);
            anim.SetBool("WalkRight", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            GetComponent<SpriteRenderer>().flipX = true;
            anim.speed = Mathf.Abs(xSpee/5);
            anim.SetBool("WalkUp", false);
            anim.SetBool("WalkDown", false);
            anim.SetBool("WalkLeft", true);
            anim.SetBool("WalkRight", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            GetComponent<SpriteRenderer>().flipX = false;
            anim.speed = Mathf.Abs(xSpee/5);
            anim.SetBool("WalkUp", false);
            anim.SetBool("WalkDown", false);
            anim.SetBool("WalkLeft", false);
            anim.SetBool("WalkRight", true);
        }
        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("WalkUp", false);
            anim.SetBool("WalkDown", false);
            anim.SetBool("WalkLeft", false);
            anim.SetBool("WalkRight", false);
        }
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

            xSpee = rb.velocity.x;
            ySpee = rb.velocity.y;

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

        var dir3 = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir3.y, dir3.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

    }

    public void Inputs()
    {
        if ((Input.GetMouseButton(0) && canFireBullet && !UIHoverListener.current.isUIOverride)) // ranged attack
        {
            if (basicBullet == null)
            {
                return;
            }

            basicBullet.transform.position = transform.position - transform.up * .5f;
            basicBullet.transform.position = new Vector3(basicBullet.transform.position.x, basicBullet.transform.position.y, transform.position.z + .1f);
            Vector2 lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
            basicBullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, lookAngle - 90));
            basicBullet.GetComponent<Projectile>().SetDeathTimer(0, projectileSpeed, projectileDamage, "player");
            basicBullet.Play();
            canFireBullet = false;
            StartCoroutine(Timer(bulletRefresh => canFireBullet = true, bulletCooldown));
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

        }
    }

    public IEnumerator Timer(Action<bool> assigner, float timer)
    {
        yield return new WaitForSeconds(timer);
        assigner(true);
    }

    public void Iframes()
    {
        iframeTimer -= Time.deltaTime;
        if ((iframeTimer - (iframeTimer % .1)) % .2 != 0)
        {
            playerSprite.color = new Color(255, 0, 0, .7f);
        }
        else if ((iframeTimer - (iframeTimer % .1)) % .2 == 0)
        {
            playerSprite.color = Color.white;
        }
    }

    public void OnTriggerStay2D(Collider2D c)
    {
        // Player presses interaction key.
        if (Input.GetKey(interactionKey)) {
            if (c.GetComponent<InteractTrigger>() == true)
            {
                c.GetComponent<InteractTrigger>().Interact();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, interactionRadius);
    }
}
