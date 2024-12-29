using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using  UnityEngine.Audio;

// Sounds applied for given player actions
public class PlayerSoundManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip damageSound;
    public AudioClip loseSound;
    public AudioClip powerUpSound;

    public void PlayJumpSound()
    {
        audioSource.PlayOneShot(jumpSound);
    }

    public void PlayDamageSound()
    {
        audioSource.PlayOneShot(damageSound);
    }

    public void PlayLoseSound()
    {
        audioSource.PlayOneShot(loseSound);
    }

    public void PlayPowerUpSound()
    {
        audioSource.PlayOneShot(powerUpSound);
    }
}