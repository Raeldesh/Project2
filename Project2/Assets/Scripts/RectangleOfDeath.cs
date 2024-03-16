using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RectangleOfDeath : MonoBehaviour
{
    private TMP_Text endText;

    // Start is called before the first frame update
    void Start()
    {
        endText = FindObjectOfType<TMP_Text>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlatformerPlayer>() != null)
        {
            other.gameObject.GetComponent<PlatformerPlayer>().Kill();
            endText.text = "Game over!";
        }
        else if (other.gameObject.GetComponent<EnemyScript>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
