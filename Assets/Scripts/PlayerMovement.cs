using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    public float pressTime=0;
    private bool canFly=false;
    public float healt=100;
    public float m_healt
    {
        get
        {
            return healt;
        }
        set
        {
            m_slider.DOValue(value/100, .2f);
            healt = value;
        }
    }
 
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Ground_Sensor m_groundSensor;
    [SerializeField] Slider m_slider;

    private bool m_grounded = false;
    private float m_delayToIdle = 0.0f;
    private float inputX;
    public float flySpeed=10;
    
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Ground_Sensor>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ChangeHealt(-20);
        } 
        if (Input.GetKeyDown(KeyCode.T))
        {
            ChangeHealt(20);
        }
        // Grounded and jump thing
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
        inputX = Input.GetAxis("Horizontal");

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
        if (Input.GetKeyDown("e"))
        {
            m_animator.SetTrigger("Death");
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && m_grounded)
        {
            
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
        //Fly
       if(Input.GetKeyDown(KeyCode.Z) && m_grounded){
                canFly=true;
                pressTime=Time.time;
        }
        //Run
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
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
       
        m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
  
        
    if(Input.GetKey(KeyCode.Z) && canFly && Time.time-pressTime<=1){
        Fly();
        
        
        }
        else{
            canFly=false;
        }
       
    }

    public void  Fly () {
        
        m_body2d.AddForce(Vector2.up*flySpeed);
    }

    public void ChangeHealt(int x)
    {
        if(m_healt+x >= 100)
        {
            m_healt = 100;
        }
        else if( m_healt+x <= 0) 
        {
            m_healt = 0;
            // ölme fonskiyonu
        }
        else
        {
            m_healt += x;
        }

    }
}