using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMoveController : MonoBehaviour
{

    [Header("Movement")]
    public float moveAccel;
    public float maxSpeed;

    [Header("Jump")]
    public float jumpAccel;
    private bool isJump;
    private bool onGround;
    private Animator anim;

    [Header("GroundRaycast")]
    public float groundRaycastDistance;
    public LayerMask groundLayerMask;


    private CharacterSound sound;

    private Rigidbody2D rb;

    [Header("Scoring")]
    public ScoreController score;
    public float scoringRatio;
    private float lastPositionX;


    [Header("GameOver")]
    public GameObject gameOverScreen;
    public float fallPositionY;

    [Header("Camera")]
    public CameraFollowController gameCamera;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sound = GetComponent<CharacterSound>();
    }

    private void Update()
    {
        
        //Fungsi Kondidi Lompat
        if(Input.GetMouseButtonDown(0))
        {
            if (onGround)
            {
                isJump = true;
                sound.PlayJump();
            }
        }

        
    }
    // Update is called once per frame
    private void FixedUpdate()
    {
        Vector2 velocityVector = rb.velocity;

        if (isJump)
        {
            velocityVector.y += jumpAccel;
            isJump = false;
        }
        velocityVector.x = Mathf.Clamp(velocityVector.x + moveAccel * Time.deltaTime, 0.0f, maxSpeed);
        rb.velocity = velocityVector;

        //Raycast Ground

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);

        if (hit)
        {
            if (!onGround && rb.velocity.y <= 0)
            {
                onGround = true;

            }
        }
        else
        {
            onGround = false;
        }

        anim.SetBool("onGround", onGround);

        // calculate score
        int distancePassed = Mathf.FloorToInt(transform.position.x - lastPositionX);
        int scoreIncrement = Mathf.FloorToInt(distancePassed / scoringRatio);

        if (scoreIncrement > 0)
        {
            score.IncreaseCurrentScore(scoreIncrement);
            lastPositionX += distancePassed;
        }
        // game over
        if (transform.position.y < fallPositionY)
        {
            GameOver();
        }

    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.position + (Vector3.down * groundRaycastDistance), Color.white);
    }

    private void GameOver()
    {
        // set high score
        score.FinishScoring();

        // stop camera movement
        gameCamera.enabled = false;

        // show gameover
        gameOverScreen.SetActive(true);

        // disable this too
        this.enabled = false;
    }
}
