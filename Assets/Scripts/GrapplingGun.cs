using System.Collections;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [SerializeField] GameObject arm;
    [Header("Scripts:")]
    public GrappleRope grappleRope;
    [Header("Layer Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Transform Refrences:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 80)] [SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = true;
    [SerializeField] private float maxDistance = 4;
    [SerializeField]float maxHookDistance = 10f;

    [Header("Launching")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType Launch_Type = LaunchType.Transform_Launch;
    [Range(0, 5)] [SerializeField] private float launchSpeed = 5;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoCongifureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequency = 3;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch,
    }

    [Header("Component Refrences:")]
    public SpringJoint2D m_springJoint2D;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 DistanceVector;
    Vector2 Mouse_FirePoint_DistanceVector;

    public Rigidbody2D playerRigidbody;
    GameObject hookTop;

    private void Start()
    {
        grappleRope.enabled = false;
        m_springJoint2D.enabled = false;
        playerRigidbody.gravityScale = 1;
    }

    private void Update()
    {
        if (transform.root.GetComponent<PlayerMovement>().grapped)
        {
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
        Mouse_FirePoint_DistanceVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        SetGrappleColor();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetGrapplePoint();
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                RotateGun(Camera.main.ScreenToWorldPoint(Input.mousePosition), false);
            }

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (Launch_Type == LaunchType.Transform_Launch)
                {
                    gunHolder.position = Vector3.Lerp(gunHolder.position, grapplePoint, Time.deltaTime * launchSpeed);
                }
            }

        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            grappleRope.enabled = false;
            m_springJoint2D.enabled = false;
            playerRigidbody.gravityScale = 1;
            transform.parent.transform.parent.GetComponent<PlayerMovement>().grapped = false;
            PoolManager.instance.Despawn(hookTop);
            arm.SetActive(false);
        }
        else
        {
            RotateGun(Camera.main.ScreenToWorldPoint(Input.mousePosition), true);
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

    void SetGrapplePoint()
    {
        if (Physics2D.Raycast(firePoint.position, Mouse_FirePoint_DistanceVector.normalized, maxHookDistance))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, Mouse_FirePoint_DistanceVector.normalized);
            if ((_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll) && ((Vector2.Distance(_hit.point, firePoint.position) <= maxDistance) || !hasMaxDistance))
            {
                grapplePoint = _hit.point;
                DistanceVector = grapplePoint - (Vector2)gunPivot.position;
                grappleRope.enabled = true;
                transform.parent.transform.parent.GetComponent<PlayerMovement>().grapped = true;

                arm.SetActive(true);
                hookTop = PoolManager.instance.Spawn("HookTop", grapplePoint, Quaternion.identity, true);
                if(hookTop.transform.position.y - transform.root.position.y < 0) { hookTop.GetComponent<SpriteRenderer>().flipY = true; }
                else { hookTop.GetComponent<SpriteRenderer>().flipY = false; }
            }
        }
    }

    void SetGrappleColor()
    {
        if (Physics2D.Raycast(firePoint.position, Mouse_FirePoint_DistanceVector.normalized, maxHookDistance))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, Mouse_FirePoint_DistanceVector.normalized);
            if ((_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll) && ((Vector2.Distance(_hit.point, firePoint.position) <= maxDistance) || !hasMaxDistance))
            {
                gameObject.GetComponent<SpriteRenderer>().color = Color.green;
            }
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public void Grapple()
    {
        if (!launchToPoint && !autoCongifureDistance)
        {
            m_springJoint2D.distance = targetDistance;
            m_springJoint2D.frequency = targetFrequency;
        }

        if (!launchToPoint)
        {
            if (autoCongifureDistance)
            {
                m_springJoint2D.autoConfigureDistance = true;
                m_springJoint2D.frequency = 0;
            }
            m_springJoint2D.connectedAnchor = grapplePoint;
            m_springJoint2D.enabled = true;
        }

        else
        {
            if (Launch_Type == LaunchType.Transform_Launch)
            {
                playerRigidbody.gravityScale = 0;
                playerRigidbody.velocity = Vector2.zero;
            }
            if (Launch_Type == LaunchType.Physics_Launch)
            {
                m_springJoint2D.connectedAnchor = grapplePoint;
                m_springJoint2D.distance = 0;
                m_springJoint2D.frequency = launchSpeed;
                m_springJoint2D.enabled = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

}
