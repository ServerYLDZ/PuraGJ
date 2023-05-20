using UnityEngine;

public class PieceGun : MonoBehaviour
{
    [Header("Piece")]
    [SerializeField] GameObject piecePrefab;
    [SerializeField] float pieceSpeed = 10f;
    [SerializeField] float fireRate = 0.2f;
    [SerializeField] float nextFireTime = 0f;

    [Header("Transform Refrences:")]
    [SerializeField] Transform gunHolder;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform firePoint;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 80)][SerializeField] private float rotationSpeed = 4;
    void Update()
    {
        RotateGun(Camera.main.ScreenToWorldPoint(Input.mousePosition), false);

        if (Input.GetKey(KeyCode.Mouse0))
        {
            FireBullet();
        }

        if (!transform.root.GetComponent<PlayerMovement>().grapped)
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void FireBullet()
    {
        if (Time.time >= nextFireTime && !transform.parent.transform.parent.GetComponent<PlayerMovement>().grapped)
        {
            nextFireTime = Time.time + fireRate;

            GameObject bullet = Instantiate(piecePrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
            bulletRigidbody.velocity = firePoint.right * pieceSpeed;
            Destroy(bullet, 1f);
        }
    }
    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            Quaternion startRotation = gunPivot.rotation;
            gunPivot.rotation = Quaternion.Lerp(startRotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }
    void OnDestroy()
    {
        
    }
}
