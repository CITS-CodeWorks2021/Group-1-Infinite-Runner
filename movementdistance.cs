using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class movementdistance : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D coll;
    private SpriteRenderer sr;
    private Animator anim;
    [SerializeField] private Text coinText;


    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    [SerializeField] private float acc = 0f;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumForce = 20f;
    [SerializeField] private float metersTraveled = 0f;


    private enum MovementState { idle, walk, dash, jump, fall, run }



    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        rb.velocity = (new Vector2(dirX * moveSpeed + acc, rb.velocity.y));


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = (new Vector2(rb.velocity.x, jumForce));
        }

        UpdateAnimationState();
        AccelerationTimer();
        distance();

    }

    private void AccelerationTimer()
    {
        if (dirX > 0f)
        {
            if (acc < 130)
            {
                acc += .1f;
            }

            else
            {
                acc += 0f;
            }
        }
        else if (dirX < 0f)
        {
            if (acc > -130)
            {
                acc -= .1f;
            }

            else
            {
                acc -= 0f;
            }
        }
        else
        {
            acc = 0f;
            moveSpeed = 7f;
        }
    }

    private void UpdateAnimationState()
    {
        MovementState state;

        if ((dirX > 0f) & (acc < 15) & (acc > 0))
        {
            state = MovementState.walk;
            sr.flipX = false;
        }
        else if ((dirX > 0f) & (acc < 51) & (acc > 15))
        {
            state = MovementState.run;
            sr.flipX = false;
        }
        else if ((dirX > 0f) & (acc > 51))
        {
            state = MovementState.dash;
            sr.flipX = false;
        }

        else if ((dirX < 0f) & (acc > -15) & (acc < 0))
        {
            state = MovementState.walk;
            sr.flipX = true;
        }

        else if ((dirX < 0f) & (acc > -51) & (acc < -15))
        {
            state = MovementState.run;
            sr.flipX = true;
        }
        else if ((dirX < 0f) & (acc < -51))
        {
            state = MovementState.dash;
            sr.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        if (rb.velocity.y > .05f)
        {
            state = MovementState.jump;
        }

        else if (rb.velocity.y < -.05f)
        {
            state = MovementState.fall;
        }

        anim.SetInteger("state", (int)state);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("boost"))
        {
            acc = dirX * 200;
        }
    }


    private bool IsGrounded()
    {
        return Physics2D.BoxCast(coll.bounds.center, coll.bounds.size, 0f, Vector2.down, .1f, jumpableGround);
    }

    private void distance()
    {
        if (dirX > 0f)
        {
            float currentposition = Mathf.Floor(transform.position.x / 1);
            metersTraveled = currentposition > metersTraveled ? currentposition : metersTraveled;
           
            coinText.text = "metersTraveled:" + metersTraveled.ToString();
        }
    }


}
