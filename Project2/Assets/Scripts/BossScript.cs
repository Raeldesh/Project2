using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossScript : MonoBehaviour
{
    private Vector3 finishPos;
    [SerializeField] private float speed = 0.5f;
    public int lives;

    private Vector3 startPos;
    private float trackPercent = 0;
    private int direction = 1;
    [SerializeField] private int distance = 2;
    private bool alive;
    private Animator anim;

    private bool jumping;
    private int jumpDirection = 1;
    [SerializeField] float jumpHeight = 2f;
    [SerializeField] float jumpSpeed = 0.1f;

    private ParticleSystem particles;
    private TMP_Text endText;
    private PlatformerPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        finishPos = new Vector3 (startPos.x - distance, startPos.y + jumpHeight, 0);
        alive = true;
        anim = GetComponent<Animator>();

        jumping = false;
        lives = 3;

        particles = GetComponent<ParticleSystem>();

        endText = FindObjectOfType<TMP_Text>();
        player = FindObjectOfType<PlatformerPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            if (Random.Range(1, 1000) == 1)
            {
                jumping = true;
            }

            trackPercent += direction * speed * Time.deltaTime;
            float x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
            float y = startPos.y;
            if (jumping)
            {
                y = transform.position.y + (jumpSpeed * jumpDirection);
            }
    
            transform.position = new Vector3(x, y, startPos.z);

            if ((direction == 1 && trackPercent > .9f) ||
                (direction == -1 && trackPercent < .1f) ||
                (Random.Range(1, 500) == 1)
                )
            {
                direction*= -1;
                GetComponent<SpriteRenderer>().flipX = (direction == -1);
            }

            if ((jumpDirection == 1 && transform.position.y > (startPos.y + jumpHeight)) ||
                (jumpDirection == -1 && transform.position.y <= startPos.y))
            {
                jumpDirection *= -1;
            }
            if (jumping && transform.position.y <= startPos.y)
            {
                jumping = false;
            }

            
        }
        else
        {
            transform.position += new Vector3(0, -8f * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.GetComponent<PlatformerPlayer>() != null && jumping == false)
        {
            other.gameObject.GetComponent<PlatformerPlayer>().Kill();
            endText.text = "Game over!";
        }
        else if (other.gameObject.GetComponent<RectangleOfDeath>() != null)
        {
            endText.text = "Congrats! You won!";
            player.enabled = false;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlatformerPlayer>() != null)
        {
            other.gameObject.GetComponent<PlatformerPlayer>().Jump();
            particles.Play();
            lives -= 1;
            if (lives == 0)
            {
                GetComponent<SpriteRenderer>().flipY = true;
                alive = false;
                anim.enabled = false;
            }
        }
        else
        {
            jumping = false;
        }
    }
}
