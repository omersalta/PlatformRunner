using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterControl : MonoBehaviour
{
    [SerializeField] private float m_vAcceleration;
    [SerializeField] private float m_speedMult;
    [SerializeField] private float m_jumpForce;
    [SerializeField] private float m_turnSpeed;
    
    [SerializeField] private Animator m_animator = null;
    [SerializeField] private Rigidbody m_rigidBody = null;
    
    private bool startLock = true;
    
    //private float m_currentV = 0;
    private float m_currentHVelocity = 0;
    private float m_currentVVelocity = 0;
    private float hoverTime = 0;
    
    private readonly float m_interpolation = 10;

    private bool m_wasGrounded;
    private Vector3 m_currentDirection = Vector3.zero;
    
    private float m_jumpTimeStamp = 0;
    private float m_minJumpInterval = 0.25f;
    private bool m_jumpInput = false;
    private bool Stunned;
    private bool m_isGrounded;
    private bool playerCannotGoLeftAnyMore;
    private bool playerCannotGoRigthAnyMore;
    
    
    private List<Collider> m_collisions = new List<Collider>();
    private InputState IS;
    private Player PLAYER;

    public void Unlock() {
        startLock = false;
        GetComponent<Rigidbody>().useGravity = true;
        foreach (var component in GetComponents<Collider>()) {
            component.enabled = true;
        }
    }
    
    public void Lock() {
        startLock = true;
        GetComponent<Rigidbody>().useGravity = false;
        foreach (var component in GetComponents<Collider>()) {
            component.enabled = false;
        }
    }
    
    
    private void Awake()
    {
        if (!m_animator) { gameObject.GetComponent<Animator>(); }
        if (!m_rigidBody) { gameObject.GetComponent<Animator>(); }

        Lock();
        Stunned = false;
        IS = FindObjectOfType<InputState>();
        PLAYER = FindObjectOfType<Player>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }
    
    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }
    
    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }
    
    private void Update()
    {
        if (!m_jumpInput && Input.GetKey(KeyCode.Space))
        {
            m_jumpInput = true;
        }
    }
    
    private void FixedUpdate()
    {
        if (!startLock) {
            if (!Stunned) {
                m_animator.SetBool("Grounded", m_isGrounded);

                DirectUpdate();

                m_wasGrounded = m_isGrounded;
                m_jumpInput = false;
            }
            else {
                m_animator.SetFloat("MoveSpeed", Vector3.zero.magnitude);
            }
        }
        
    }
    
    private void DirectUpdate()
    //overrided from store asset script
    {
        //Camera.main.transform;
        Transform camera = Camera.main.transform;

        var currentGap = PLAYER.LimitedGapCalculator();
        
        m_currentHVelocity = currentGap/(400/m_turnSpeed);
        m_currentVVelocity = Mathf.Lerp(m_vAcceleration, m_currentVVelocity, 0.7f);

        if (!m_isGrounded) {
            m_currentVVelocity = 0;
        }
        Bot.UpdateTargetV((m_currentVVelocity * m_speedMult) * Vector3.forward);
        Vector3 hDirection = camera.right * m_currentHVelocity + m_currentVVelocity * Vector3.forward;
        
        if (hDirection != Vector3.zero)
        {
            m_currentDirection = Vector3.Slerp(m_currentDirection, hDirection, Time.deltaTime * m_interpolation + 0.8f);
            
            transform.rotation = Quaternion.LookRotation(m_currentDirection);
            transform.position += m_currentDirection * m_speedMult * Time.deltaTime;

            m_animator.SetFloat("MoveSpeed", hDirection.magnitude);
        }
        
        JumpingAndLanding();
    }
    
    public void StunPlayer(float stunTime) {
        Debug.Log("player stund for "+stunTime);
        Stunned = true;
        StartCoroutine(stunCourutine());
        
        IEnumerator stunCourutine() {
            yield return new WaitForSeconds(stunTime);
            Stunned = false;
            StopCoroutine(stunCourutine());
        }
    }
    
    
    
    private void JumpingAndLanding()
    {
        bool jumpCooldownOver = (Time.time - m_jumpTimeStamp) >= m_minJumpInterval;
        
        if (jumpCooldownOver && m_isGrounded && m_jumpInput) {
            
            m_jumpTimeStamp = Time.time;
            m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
        }

        if (!m_wasGrounded && m_isGrounded) {   
            
            m_currentVVelocity = 0;
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded) {
            
            m_animator.SetTrigger("Jump");
        }
    }
}