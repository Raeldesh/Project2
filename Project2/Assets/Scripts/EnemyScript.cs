using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyScript : MonoBehaviour
{
    private Vector3 finishPos;
    [SerializeField] private float speed = 0.5f;

    private Vector3 startPos;
    private float trackPercent = 0;
    private int direction = 1;
    [SerializeField] private int distance = 2;
    private bool alive;
    private Animator anim;
    private BossSpawnerScript spawner;
    private TMP_Text endText;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        finishPos = new Vector3 (startPos.x - distance, 0, 0);
        alive = true;
        anim = GetComponent<Animator>();
        spawner = FindObjectOfType<BossSpawnerScript>();
        spawner.enemiesRemaining += 1;
        endText = FindObjectOfType<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (alive)
        {
            trackPercent += direction * speed * Time.deltaTime;
            float x = (finishPos.x - startPos.x) * trackPercent + startPos.x;
            float y = (finishPos.y - startPos.y) * trackPercent + startPos.y;
    
            transform.position = new Vector3(x, startPos.y, startPos.z);
            
            
            if ((direction == 1 && trackPercent > .9f) ||
                (direction == -1 && trackPercent < .1f))
            {
                direction*= -1;
                GetComponent<SpriteRenderer>().flipX = (direction == -1);
            }
        }
        else
        {
            transform.position += new Vector3(0, -8f * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.GetComponent<PlatformerPlayer>() != null)
        {
            other.gameObject.GetComponent<PlatformerPlayer>().Kill();
            endText.text = "Game over!";
        }
        else if (other.gameObject.GetComponent<RectangleOfDeath>() != null)
        {
            spawner.enemiesRemaining -= 1;
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlatformerPlayer>() != null)
        {
            other.gameObject.GetComponent<PlatformerPlayer>().Jump();
            GetComponent<SpriteRenderer>().flipY = true;
            alive = false;
            anim.enabled = false;
        }
    }
}
