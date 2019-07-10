using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollectible : MonoBehaviour, ISavable {

    public int worth;
    Animator anim;
    [SerializeField]
    AudioClip collectClip;

	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(Collect(other));
        }
    }

    IEnumerator Collect(Collider2D collector)
    {
        GetComponent<CircleCollider2D>().enabled = false;
        GetComponent<Animator>().SetTrigger("Collect");
        collector.GetComponent<AudioSource>().PlayOneShot(collectClip);
        collector.GetComponent<PlayerCollecting>().CollectMoney(1);
        for (int i = 1; i < worth; i++)
        {
            yield return new WaitForSeconds(0.2f / worth);
            collector.GetComponent<AudioSource>().PlayOneShot(collectClip);
            collector.GetComponent<PlayerCollecting>().CollectMoney(1);
        }
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }

    StateRecord ISavable.GetRecord()
    {
        CoinRecord record = (CoinRecord)RecordFactory.Get(this);
        record.active = gameObject.GetComponent<CircleCollider2D>().enabled;
        return record;
    }

    void ISavable.SetData(StateRecord loaded)
    {
        CoinRecord record = (CoinRecord)loaded;
        gameObject.SetActive(record.active);
    }
}
