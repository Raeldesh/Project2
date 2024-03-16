using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenPlatform : MonoBehaviour
{
    private Vector3 startPos;
    private Vector3 finishPos;
    private bool holdingPlayer = false;

    private PlatformerPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        player = FindObjectOfType<PlatformerPlayer>();
        finishPos = new Vector3(5, player.gameObject.transform.position.y - 2f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingPlayer == false && transform.position.y > finishPos.y)
        {
            transform.position = new Vector3(startPos.x, transform.position.y - 0.03f, startPos.z);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (transform.position.y < startPos.y)
        {
            transform.position = new Vector3(startPos.x, transform.position.y + 0.03f, startPos.z);
        }
        holdingPlayer = true;
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        holdingPlayer = false;
    }
}