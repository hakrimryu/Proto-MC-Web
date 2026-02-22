using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{


    [Header("Health Controller")]
    [SerializeField] int maxHealth;
    private int health;
    private bool isReachedCheckPoint;
    [SerializeField] int damage;

    [SerializeField] float fireCd;
    private float fireTimer;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    private Vector3 target;
    private GameObject[] allTargets;
    public GameObject cloneSource = null;

    [SerializeField] bool isBig;
    private Vector3 startScale;
    private Rigidbody _rb;

    void Start()
    {
        health = maxHealth;
        startScale = transform.localScale;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        allTargets = GameObject.FindGameObjectsWithTag("EnemyCastle");
        var closest = closestTarget();
        if (closest != null)
        {
            target = closest.transform.position;
        }

        // Fix: Use current Y position instead of 0 to prevent sinking below the floor
        target.y = transform.position.y;

        fireTimer -= Time.deltaTime;

        if (_rb != null)
        {
            _rb.linearVelocity = Vector3.zero;
        }

        MoveForward();

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void MoveForward()
    {
        if (isReachedCheckPoint)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
        }
        else
        {
            // Fix: Ensure target.y is still current position.y even when overriding X
            target.x = transform.position.x;
            target.y = transform.position.y;
            transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * moveSpeed);
        }
    }
    public void getHit(int damage)
    {
        health -= damage;
        if (isBig)
        {

            transform.localScale = startScale * health / maxHealth;
        }
    }

    private GameObject closestTarget()
    {

        GameObject closestHere = gameObject;
        float leastDistance = Mathf.Infinity;

        foreach (var target in allTargets)
        {

            float distanceHere = Vector3.Distance(transform.position, target.transform.position);

            if (distanceHere < leastDistance)
            {
                leastDistance = distanceHere;
                closestHere = target;
            }

        }
        return closestHere;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && fireTimer <= 0)
        {
            ScoreScript.scoreValue += 2;
            collision.gameObject.GetComponent<EnemyController>().getHit(damage);
            fireTimer = fireCd;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedReducePoint"))
        {

            moveSpeed = 2;
        }
        if (other.gameObject.CompareTag("CheckPoint"))
        {
            isReachedCheckPoint = true;
        }
        if (other.gameObject.CompareTag("EnemyCastle") && fireTimer <= 0)
        {
            other.gameObject.GetComponent<EnemyCastleScript>().getHit(damage);
            fireTimer = fireCd;
        }

    }


}
