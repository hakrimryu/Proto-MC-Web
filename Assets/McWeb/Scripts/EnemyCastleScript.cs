using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyCastleScript : MonoBehaviour
{
    [SerializeField] ParticleSystem castleParticular;
    [SerializeField] int health = 100;

    [SerializeField] Transform spawnPoint;
    [SerializeField] GameObject enemy;

    [Header("Spawn: Time (x) - Enemy Amount (y)")]
    [SerializeField] Vector3[] spawnEvents;
    private float spawnTimer;

    void Start()
    {
        spawnTimer = 0;
    }

    void Update()
    {
        if (health <= 0) { Destroy(gameObject); }
        spawnTimer += Time.deltaTime;

        for (int i = 0; i < spawnEvents.Length; i++)
        {
            if (spawnEvents[i].x == (int)spawnTimer)
            {
                spawn((int)spawnEvents[i].y);
                spawnEvents[i].x = 0;
            }
        }
    }

    private void spawn(int EnemyAmount)
    {
        for (int i = 0; i < EnemyAmount; i++)
        {
            Vector3 newSpawnPoint = spawnPoint.position;
            int spawnX = Random.Range(-2, 2);
            int spawnZ = Random.Range(-3, 3);

            newSpawnPoint.z += spawnZ;
            newSpawnPoint.x += spawnX;
            GameObject enemySpawned = Instantiate(enemy, newSpawnPoint, Quaternion.identity);
            enemySpawned.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }
    public void getHit(int damage)
    {
        health -= damage;
        CastleHitEffect();
    }

    private void CastleHitEffect()
    {
        castleParticular.Play();
    }
}
