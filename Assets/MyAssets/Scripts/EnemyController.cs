using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;
    public ParticleSystem smokeEffect;
    public AudioClip fixedRobotSound;

    private AudioSource audioSource;
    private bool isBroken = true;
    private Animator animator;
    new private Rigidbody2D rigidbody;
    private float timer;
    private int direction = 1;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isBroken)
        {
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction *= -1;
            timer = changeTime;
        }
    }

    void FixedUpdate()
    {
        if (!isBroken)
        {
            return;
        }
        Vector2 position = rigidbody.position;
        
        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        
        rigidbody.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        isBroken = false;
        rigidbody.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.Stop();
        audioSource.PlayOneShot(fixedRobotSound);
    }
}
