using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject boss;
    [SerializeField] GameObject player;

    public int enemiesRemaining;
    private bool bossIsSpawned;
 
    void Awake()
    {
        enemiesRemaining = 0;
        bossIsSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossIsSpawned && enemiesRemaining == 0)
        {
            Instantiate(boss, new Vector3(player.transform.position.x + 3f, this.transform.position.y, 0), Quaternion.identity);
            bossIsSpawned = true;
        }
    }
}
