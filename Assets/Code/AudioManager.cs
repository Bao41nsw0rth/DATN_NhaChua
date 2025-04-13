using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] FootSteps;
    [SerializeField] private AudioClip torchClick;

    private int randomIndex;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void TryPlayFootSteps()
    {
        if (FootSteps == null || FootSteps.Length == 0 || audioSource == null) return;

        randomIndex = Random.Range(0, FootSteps.Length);
        audioSource.PlayOneShot(FootSteps[randomIndex]);
    }

    public void TryPlayTorchClick()
    {
        audioSource.PlayOneShot(torchClick);
    }
}

