using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpeningCutscene : Cutscene
{
    [SerializeField]
    protected Transform player;
    [SerializeField]
    protected Transform startingPosition;
    [SerializeField]
    protected Animator playerAnim;
    [SerializeField]
    protected PlayerMovement playerMovement;
    [SerializeField]
    ScreenOverlay alphaFade;
    [SerializeField]
    Transform followTarget;

    new void Start()
    {
        base.Start();
        Camera.main.GetComponent<Camera2DFollow>().target = followTarget;
        GameManager.Instance.toggleKeysEnabled = true;
    }

    IEnumerator Sleep()
    {
        DialogueManager.Instance.StartDialogue(GameManager.Instance.fileName, "Zzz...");
        playerAnim.SetBool("Sleeping", true);
        yield return new WaitUntil(() => playerAnim.GetBool("Sleeping"));
        player.position = startingPosition.position + Vector3.right * 0.75f;
    }

    IEnumerator Wake()
    {
        playerMovement.GetComponent<Collider2D>().enabled = false;
        DialogueManager.Instance.EndDialogue();
        playerAnim.SetBool("Sitting", true);
        playerAnim.SetBool("Sleeping", false);
        yield return new WaitUntil(() => playerAnim.GetBool("Sitting"));
        player.rotation = Quaternion.identity;
        player.position += Vector3.up * 0.5f + Vector3.left * 0.75f;
        playerMovement.GetComponent<Collider2D>().enabled = true;
    }

    IEnumerator Rise()
    {
        playerAnim.SetBool("Sitting", false);
        yield return new WaitUntil(() => playerAnim.GetBool("Ground"));
        playerMovement.Move(-1, 0, false, true, false);
    }
}
