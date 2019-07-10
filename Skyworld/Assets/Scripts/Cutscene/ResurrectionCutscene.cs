using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurrectionCutscene : OpeningCutscene
{
    new void Start()
    {
        base.Start();
    }

    IEnumerator Sleep()
    {
        playerAnim.SetBool("Sleeping", true);
        yield return new WaitUntil(() => playerAnim.GetBool("Sleeping"));
        player.position = startingPosition.position + Vector3.right * 0.75f;
    }

    IEnumerator Rise()
    {
        playerMovement.GetComponent<Collider2D>().enabled = false;
        playerAnim.SetBool("Sleeping", false);
        yield return new WaitUntil(() => !(playerAnim.GetBool("Sleeping")));
        player.rotation = Quaternion.identity;
        player.position += Vector3.up * 0.5f + Vector3.left * 0.75f;
        player.localScale = new Vector3(-1f, player.localScale.y);
        playerMovement.GetComponent<Collider2D>().enabled = true;
    }
}