using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PhaseShift : MonoBehaviour {

    [SerializeField]
    AudioClip shift;
    [SerializeField]
    float maxShiftDistance = 2.5f;
    public float secondsBetweenShifts;
    [SerializeField]
    GameObject smokePuffPrefab;
    [SerializeField]
    LayerMask whatIsImpassable;
    const float circleCheckRadius = 0.25f;
    float secondsSinceLastShift;
    public delegate void ShiftAction();
    public event ShiftAction OnPhaseShift;
    PlayerMovement parentMove;

    void Start () {
        secondsSinceLastShift = secondsBetweenShifts;
        parentMove = GetComponentInParent<PlayerMovement>();
    }
	
	// Update is called once per frame
	void Update () {
        secondsSinceLastShift += Time.deltaTime;
        if (CrossPlatformInputManager.GetButtonDown("Shift") && !parentMove.Climbing && secondsSinceLastShift >= secondsBetweenShifts && (GameManager.Instance == null || !GameManager.Instance.paused))
        {
            StartCoroutine(Shift());
            secondsSinceLastShift = 0f;
        }
    }

    bool ColliderIsNullOrPassable(Collider2D c)
    {
        return c == null || c.isTrigger || c.GetComponent<PlatformEffector2D>() != null;
    }

    IEnumerator Shift()
    {
        const float legRoom = 0.1f;
        GetComponent<ParticleSystem>().Play();
        yield return new WaitForEndOfFrame();
        float shiftDistance = 0f;
        Vector2 forwardVector = transform.parent.localScale.x * Vector2.right;

        // Travel maximum distance if it's clear, otherwise go forward until something impassable is hit
        Collider2D[] collidersAtMaxDistance = Physics2D.OverlapCircleAll((Vector2)transform.position + forwardVector * (maxShiftDistance - circleCheckRadius + legRoom), circleCheckRadius, whatIsImpassable);
        if (collidersAtMaxDistance.All(c => ColliderIsNullOrPassable(c)))
        {
            shiftDistance = maxShiftDistance;
        }
        else
        {
            IEnumerable<RaycastHit2D> hits = Physics2D.RaycastAll(transform.position, forwardVector, maxShiftDistance, whatIsImpassable).OrderByDescending(raycast => raycast.distance);
            foreach (RaycastHit2D hit in hits)
            {
                Collider2D[] collidersAtDestination = Physics2D.OverlapCircleAll(hit.point - forwardVector * (circleCheckRadius + legRoom), circleCheckRadius, whatIsImpassable);
                if (collidersAtDestination.All(c => ColliderIsNullOrPassable(c)))
                {
                    shiftDistance = hit.distance - circleCheckRadius;
                    break;
                }
            }
        }
        Instantiate(smokePuffPrefab, transform.position, Quaternion.identity, transform);
        transform.parent.Translate(forwardVector * shiftDistance);
        transform.parent.GetComponent<AudioSource>().PlayOneShot(shift);
        if (OnPhaseShift != null)
        {
            OnPhaseShift();
        }
    }
}