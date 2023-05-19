using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    
 
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Ground_Sensor m_groundSensor;
    private bool m_grounded = false;
    private float m_delayToIdle = 0.0f;
    private float inputX;
    


    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Ground_Sensor>();
    }

  
    public void MoveUp()
    {
        if (m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
    }
   
    // Update is called once per frame
    void Update()
    {
          inputX = Input.GetAxis("Horizontal");
         
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        else if (m_grounded && !m_groundSensor.State())
        {
            StartCoroutine(ResetGroundedState(0.3f)); // wait for 0.3 sec before resetting grounded state
        }

        // -- Handle input and movement --
        //inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        //Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --

        //Death
      
        //Jump
        if (Input.GetKeyDown("space") && m_grounded )
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }

        //Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }

        //Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }
    }

    IEnumerator ResetGroundedState(float delay)
    {
        yield return new WaitForSeconds(delay);
        m_grounded = false;
        m_animator.SetBool("Grounded", m_grounded);
    }

    void FixedUpdate()
    {
        //Move
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
    }
}