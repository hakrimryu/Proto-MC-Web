using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private ParticleSystem enemyParticular;


    [Header("Health Controller")]

    [SerializeField] int maxHealth;
    private int health;

    [SerializeField] int damage;

    [SerializeField] float fireCd;
    private float fireTimer;

    [Header("Settings")]
    [SerializeField] private float moveSpeed;
    private Vector3 _targetPosition;
    private Rigidbody _rb;

    [SerializeField] bool isBig;
    private Vector3 startScale;
    void Start()
    {
        health = maxHealth;
        GameObject castle = GameObject.FindGameObjectWithTag("PlayerCastle");
        if (castle != null)
        {
            _targetPosition = castle.transform.position;
            // Maintain current Y to prevent sinking
            _targetPosition.y = transform.position.y;
        }

        startScale = transform.localScale;
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
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
        // Update Y every frame in case the floor height changes or for safety
        _targetPosition.y = transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, Time.deltaTime * moveSpeed);
    }
    public void getHit(int damage)
    {
        health -= damage;
        EnemyHitEffect();

        if (isBig)
        {
            transform.localScale = startScale * health / maxHealth;
            EnemyHitEffect();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && fireTimer <= 0)
        {
            collision.gameObject.GetComponent<PlayerController>().getHit(damage);

            fireTimer = fireCd;
        }
        if (collision.gameObject.CompareTag("PlayerCastle") && fireTimer <= 0)
        {

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("SpeedReducePoint"))
        {

            moveSpeed = 2;
        }
        if (other.gameObject.CompareTag("EndGame"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        }
    }
    private void EnemyHitEffect()
    {
        enemyParticular.Play();
    }

}
