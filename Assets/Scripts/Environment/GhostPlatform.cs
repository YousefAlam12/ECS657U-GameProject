using System.Collections;
using UnityEngine;


public class GhostPlatform : MonoBehaviour
{
    [SerializeField] string playerTag = "Player"; // Used to define the player entity
    [SerializeField] float disappearTime = 3; // Time before the platform disappears
    Animator myAnim; // Animator controlling the platform animation

    [SerializeField] bool canReset; // Can the platform reset after disappearing?
    [SerializeField] float resetTime; // Time before the platform resets

    private void Start()
    {
        myAnim = GetComponent<Animator>();
        myAnim.SetFloat("DisappearTime", 1 / disappearTime);
    }

    // on collision sets the platform to fade
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == playerTag)
        {
            myAnim.SetBool("Trigger", true);
        }
    }

    // restores the platform after dissapearing if set
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