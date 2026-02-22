using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireCd = 0.2f;

    private float _fireTimer;

    private void Start()
    {
        _fireTimer = 0;
    }

    private void Update()
    {
        _fireTimer -= Time.deltaTime;

        if (Pointer.current != null && Pointer.current.press.isPressed)
        {
            if (_fireTimer <= 0)
            {
                Shoot();
                _fireTimer = fireCd;
            }
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
