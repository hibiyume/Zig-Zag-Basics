using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] private Transform rayStart;
    [SerializeField] private GameObject crystalEffect;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float timeBeforeGameRestart;
    
    private Rigidbody rb;
    private bool walkingRight = true;
    private Animator anim;
    private GameManager gameManager;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if (!gameManager.gameStarted)
            return;
        else
            anim.SetTrigger("gameStarted");
        
        rb.transform.position = transform.position + transform.forward * (movementSpeed / 100f);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameManager.isFalling && gameManager.gameStarted)
                SwitchDirection();
        }

        RaycastHit hit;
        if (!Physics.Raycast(rayStart.position, -transform.up, out hit, Mathf.Infinity))
        {
            gameManager.isFalling = true;
            anim.SetTrigger("isFalling");
        }

        if (gameManager.isFalling)
        {
            gameManager.Invoke("EndGame", timeBeforeGameRestart);
        }
    }
    private void SwitchDirection()
    {
        walkingRight = !walkingRight;
        
        if (walkingRight)
            transform.rotation = Quaternion.Euler(0f, 45f, 0f);
        else
            transform.rotation = Quaternion.Euler(0f, -45f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Crystal")
        {
            gameManager.IncreaseScore();

            GameObject g = Instantiate(crystalEffect, rayStart.transform.position, crystalEffect.transform.rotation);
            ParticleSystem ps = g.GetComponent<ParticleSystem>();
            ps.Play();
            Destroy(g, ps.main.duration);
            Destroy(other.gameObject);
        }
    }
    
    public float GetMovementSpeed()
    {
        return movementSpeed;
    }
}
