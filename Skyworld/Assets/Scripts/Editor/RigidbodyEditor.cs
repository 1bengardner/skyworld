using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(Rigidbody2D))]
public class RigidbodyEditor : Editor
{
    void OnSceneGUI()
    {
        Rigidbody2D rb = target as Rigidbody2D;
        Handles.color = Color.red;
        Handles.CircleCap(1, rb.transform.TransformPoint(rb.centerOfMass), Quaternion.Euler(0f, 0f, rb.rotation), 0.1f);
    }
}