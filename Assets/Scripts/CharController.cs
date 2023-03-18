using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharController : MonoBehaviour
{
    [SerializeField] private Transform rayStart1;
    [SerializeField] private Transform rayStart2;
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

    private void Start()
    {
        anim.SetTrigger("gameStarted");
        InvokeRepeating("IncreaseMovementSpeed", 5f, 1f);
    }

    private void FixedUpdate()
    {
        rb.transform.position = transform.position + transform.forward * (movementSpeed / 100f);
    }

    private void Update()
    {
        CheckForGround();
        if (gameManager.isFalling)
        {
            gameManager.Invoke("EndGame", timeBeforeGameRestart);
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!gameManager.isFalling)
                SwitchDirection();
        }
    }

    private void CheckForGround()
    {
        RaycastHit hit;
        if (!Physics.Raycast(rayStart1.position, -transform.up, out hit, Mathf.Infinity) &&
            !Physics.Raycast(rayStart2.position, -transform.up, out hit, Mathf.Infinity))
        {
            gameManager.isFalling = true;
            anim.SetTrigger("isFalling");
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
            
            GameObject g = Instantiate(crystalEffect, transform.position + GetComponent<CapsuleCollider>().center, crystalEffect.transform.rotation);
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

    private void IncreaseMovementSpeed()
    {
        movementSpeed += 0.0075f;
    } // Invoked every second
}