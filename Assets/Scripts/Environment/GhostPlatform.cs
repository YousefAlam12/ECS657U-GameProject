using System.Collections;
using UnityEngine;

public class GhostPlatform : MonoBehaviour
{
    [SerializeField] string playerTag = "Player"; // Used to define the player entity
    [SerializeField] string ghostPlatformTag = "GhostPlatform"; // Tag for platforms
    [SerializeField] float disappearTime = 3; // Time before the platform disappears
    Animator myAnim; // Animator controlling the platform animation

    [SerializeField] bool canReset; // Can the platform reset after disappearing?
    [SerializeField] float resetTime; // Time before the platform resets

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myAnim.SetFloat("DisappearTime", 1 / disappearTime);
    }

    // on collision, check if the player or the platform tag is the trigger
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(playerTag) || collision.collider.CompareTag(ghostPlatformTag))
        {
            myAnim.SetBool("Trigger", true);
        }
    }

    // restores the platform after disappearing if set
    public void TriggerReset()
    {
        if (canReset)
        {
            StartCoroutine(Reset());
        }
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(resetTime);
        myAnim.SetBool("Trigger", false);
    }
}
