using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeMovement : EnemyMovement {
    
    public float speed;
    [SerializeField] CircleCollider2D patrolArea;
    Transform destination;
    bool inPursuit = false;

    float speedHigh;
    float speedLow;

    protected override void Start()
    {
        base.Start();
        audioSource.loop = true;
        audioSource.Play();
        destination = new GameObject("Destination").transform;
        destination.parent = transform.parent;
        destination.position = RandomDestination();
        speedLow = speed;
        speedHigh = speed * 2f;
        if (patrolArea == null)
        {
            Debug.LogError("Bee is missing a patrol area!");
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player" && patrolArea.OverlapPoint(other.transform.position))
        {
            SetTarget(Vector2.Lerp(transform.position, other.transform.position, 0.25f), true);
        }
        else if (other.tag == "Player" && !patrolArea.OverlapPoint(other.transform.position) && inPursuit)
        {
            SetTarget(RandomDestination(), false);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && inPursuit)
        {
            SetTarget(RandomDestination(), false);
        }
    }

    void SetTarget(Vector3 targetPoint, bool chasingPlayer)
    {
        inPursuit = chasingPlayer;
        speed = chasingPlayer ? speedHigh : speedLow;
        destination.position = targetPoint;
    }

    Vector3 RandomDestination()
    {
        return patrolArea.transform.position + (Vector3)(Random.insideUnitCircle * patrolArea.radius);
    }

    protected override void Move()
    {
        const float tol = 0.2f;
        // Change destination if destination was reached and was not player
        if (!inPursuit && (destination.position - transform.position).magnitude < tol)
        {
            SetTarget(RandomDestination(), false);
        }
        rb.velocity = (destination.position - transform.position).normalized * speed;
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.magnitude));
        if (rb.velocity.x > 0 != facingRight)
        {
            Flip();
        }
    }
    
    void OnDisable()
    {
        speed = speedLow;
        destination.position = RandomDestination();
    }
}
