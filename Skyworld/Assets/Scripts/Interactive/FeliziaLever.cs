using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeliziaLever : Lever
{
    [SerializeField]
    AudioClip wrongClip;
    [System.Serializable]
    class NumberSlot
    {
        [SerializeField]
        Collider2D number;
        [SerializeField]
        Collider2D slot;
        Vector2 startPo;
        Quaternion startRo;

        public bool hasNumberInSlot
        {
            get
            {
                return number.IsTouching(slot);
            }
        }

        public void Initialize()
        {
            startPo = number.transform.position;
            startRo = number.transform.rotation;
        }

        public void Reset(GameObject resetEffect)
        {
            number.transform.position = startPo;
            number.transform.rotation = startRo;
            Instantiate(resetEffect, startPo, Quaternion.identity, number.transform.parent);
        }
    }
    [SerializeField]
    List<NumberSlot> slots;

    void Start()
    {
        slots.ForEach(slot => slot.Initialize());
    }

    protected override void Pull(AudioSource clipSource)
    {
        pulled = true;
        if (slots.TrueForAll(slot => slot.hasNumberInSlot))
        {
            StartCoroutine(Activate(clipSource));
        }
        else
        {
            clipSource.PlayOneShot(soundClip);
            SetSwitch(true);
            StartCoroutine(Reset(clipSource));
        }
    }

    protected virtual IEnumerator Reset(AudioSource clipSource)
    {
        yield return new WaitForSeconds(0.4f);
        clipSource.PlayOneShot(wrongClip);
        SetSwitch(false);
        slots.ForEach(slot => slot.Reset(smokePuffPrefab));
        pulled = false;
    }
}
