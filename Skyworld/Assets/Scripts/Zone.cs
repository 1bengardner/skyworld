using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    public const string PrefsKey = "Zone";
    [SerializeField]
    private string _name;
    public new string name
    {
        get
        {
            return _name;
        }
    }
    [SerializeField]
    private int _rank;
    public int rank
    {
        get
        {
            return _rank;
        }
    }
    public Color color;
    public AudioClip music;
    public float startMusicAtSeconds = 0f;
    public Transform spawnPoint;

    void Awake()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }
}
