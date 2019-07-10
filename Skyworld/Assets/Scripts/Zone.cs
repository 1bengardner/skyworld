using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour {
    [HideInInspector]
    public string id
    {
        get
        {
            return name;
        }
    }
    public Color color;
    public AudioClip music;
    public Transform spawnPoint;

    void Awake()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }
}
