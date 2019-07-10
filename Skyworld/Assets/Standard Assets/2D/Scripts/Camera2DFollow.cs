using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2DFollow : MonoBehaviour
{
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;
    public float offsetY;
    public float distanceDamping = 5f;

    private float m_OffsetZ;
    private Vector3 m_LastTargetPosition;
    private Vector3 m_CurrentVelocity;
    private Vector3 m_LookAheadPos;
    private Stack<Transform> m_StoredTargets;

    // Use this for initialization
    private void Start()
    {
        m_StoredTargets = new Stack<Transform>();
        m_LastTargetPosition = target.position;
        m_OffsetZ = (transform.position - target.position).z;
        transform.parent = null;
    }

    // Update is called once per frame
    private void LateUpdate()
    {
        // only update lookahead pos if accelerating or changed direction
        float xMoveDelta = (target.position - m_LastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor*Vector3.right*Mathf.Sign(xMoveDelta);
        }
        else
        {
            m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
        }

        Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;
        float distance = ((Vector2)(target.position - transform.position)).magnitude;
        Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping / (distance / distanceDamping + 1f));

        transform.position = newPos;

        m_LastTargetPosition = target.position;
    }

    public void SnapTo(Vector3 newPos)
    {
        transform.position = newPos + Vector3.forward * m_OffsetZ;
    }

    // Allow the coroutine to run even when caller is deactivated
    public void SetTempTargetWrapper(Transform tempTarget, float duration)
    {
        StartCoroutine(SetTempTarget(tempTarget, duration));
    }

    IEnumerator SetTempTarget(Transform tempTarget, float duration)
    {
        PushTarget(tempTarget);
        yield return new WaitForSecondsRealtime(duration);
        PopTarget();
    }

    public void PushTarget(Transform newTarget)
    {
        m_StoredTargets.Push(target);
        target = newTarget;
    }

    public Transform PopTarget()
    {
        Transform st = m_StoredTargets.Pop();
        if (st.tag == "Player")
            target = st;
        return st;
    }
}
