using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Cutscene : MonoBehaviour {
    
    public bool playable = true;
    [SerializeField]
    bool playOnAwake = false;
    [SerializeField]
    string playString;

    Animator anim;

    // Use this for initialization
    protected void Start () {
        anim = GetComponent<Animator>();
        if (playString == "")
        {
            playString = "Play";
        }
        if (playOnAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        playable = false;
        GameManager.Instance.paused = true;
        anim.SetTrigger(playString);
        StartCoroutine(Cleanup());
    }

    IEnumerator Cleanup()
    {
        yield return new WaitUntil(() => !anim.GetCurrentAnimatorStateInfo(0).loop);
        Invoke("Finish", anim.GetCurrentAnimatorStateInfo(0).length);
    }
    
    protected void Finish()
    {
        GameManager.Instance.paused = false;
    }
}
