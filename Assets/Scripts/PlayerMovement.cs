﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    private float pressTime = 0;
    private bool canFly;
    private float healt = 100;
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
    private float flySpeed = 16;
    public bool grapped;
    public bool flyable;
    public GameObject lose;
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Ground_Sensor>();
    }
    void Update()
    {
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
        }
        else if (m_grounded && !m_groundSensor.State())
        {
            StartCoroutine(ResetGroundedState(0.3f));
        }

        // -- Handle input and movement --
        inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            if(transform.position.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
        }

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

       
        // -- Handle Animations --

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && m_grounded)
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
        //Fly Start
        if (Input.GetKeyDown(KeyCode.W) && m_grounded && flyable)
        {
            m_animator.SetTrigger("Fly");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
           
            canFly = true;
            pressTime = Time.time;
        }

        
        //Run
        if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        //Idle
        else
        {
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
        if (!grapped)
        {
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);
        }

        if (Input.GetKey(KeyCode.W) && canFly && Time.time - pressTime <= 2)
        {
                m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            Fly();
        }
        else
        {
            canFly = false;
        }
    }

    public void Fly()
    {

        m_body2d.AddForce(Vector2.up * flySpeed);
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
            lose.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            m_healt += x;
        }
    }

    public void TekrarDene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }
}