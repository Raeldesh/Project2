using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlatformerPlayer : MonoBehaviour
{
    [SerializeField] private float speed = 4.5f;
    private Rigidbody2D body;
    private Animator anim;
    [SerializeField] private float jumpForce = 12.0f;
    private BoxCollider2D box;
    public int coins;
    [SerializeField] TMP_Text coinsText;
    [SerializeField] GameObject goldPlatform;

    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box = GetComponent<BoxCollider2D>();

        coins = 0;
        coinsText.text = "Coins: 0/10";
    }

    // Update is called once per frame
    void Update()
    {
        float deltaX = Input.GetAxis("Horizontal") * speed;
        Vector2 movement = new Vector2(deltaX, body.velocity.y);
        body.velocity = movement;
        Vector3 max = box.bounds.max;
        Vector3 min = box.bounds.min;

        Vector2 corner1 = new Vector2(max.x, min.y - .1f);
        Vector2 corner2 = new Vector2(max.x, min.y - .2f);

        Collider2D hit = Physics2D.OverlapArea(corner1, corner2);

        bool grounded = false;

        if (hit != null)
        {
            grounded = true;
        }

        body.gravityScale = (grounded && Mathf.Approximately(deltaX, 0)) ? 0 : 1;

        if (grounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        MovingPlatform platform = null;

        if (hit != null)
        {
            platform = hit.GetComponent<MovingPlatform>();
        }

        if (platform != null)
        {
            transform.parent = platform.transform;
        }
        else
        {
            transform.parent = null;
        }


        anim.SetFloat("speed", Mathf.Abs(deltaX));
        Vector3 pScale = Vector3.one;

        if (platform != null)
        {
            pScale = platform.transform.localScale;
        }

        if (!Mathf.Approximately(deltaX, 0))
        {
            transform.localScale = new Vector3(Mathf.Sign(deltaX) / pScale.x, 1 / pScale.y, 1);
        }


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "coin")
        {
            Destroy(other.gameObject);
            coins ++;
            coinsText.text = "Coins: " + coins + "/10";
            if (coins == 10)
            {
                StartCoroutine(SpawnGoldenPlatform());
            }
        }
    }

    IEnumerator SpawnGoldenPlatform()
    {
        cam.orthographicSize = 5.7f;
        Vector3 spawnPos = new Vector3(5, 25f, 0);
        Instantiate(goldPlatform, spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(8);
        cam.orthographicSize = 2.0f;
    }

    public void Kill()
    {
        Destroy(this.gameObject);
    }

    public void Jump()
    {
        body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
}
