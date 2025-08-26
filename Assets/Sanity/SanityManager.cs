using UnityEngine;
using System.Collections;

public class SanityManager : MonoBehaviour
{
    [SerializeField] private GameObject fullScreenFX;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Material screenDistortionMaterial;

    [SerializeField] private float decreaseInterval = 2f;
    [SerializeField] private bool canLoseSanity = false;

    private int Sanity = 100;
    private Coroutine sanityCoroutine;

    private enum PlayerState
    {
        normal,
        distortion,
        insane
    }

    private PlayerState state;

    private void Start()
    {
        if (canLoseSanity)
            sanityCoroutine = StartCoroutine(DecreaseSanityOverTime());
    }

    private IEnumerator DecreaseSanityOverTime()
    {
        while (Sanity > 0)
        {
            yield return new WaitForSeconds(decreaseInterval);

            if (canLoseSanity)
            {
                Sanity -= 1;
                Debug.Log("Sanity: " + Sanity);
            }
        }
    }

    public void SetLoseSanity(bool value)
    {
        canLoseSanity = value;

        if (canLoseSanity && sanityCoroutine == null)
        {
            sanityCoroutine = StartCoroutine(DecreaseSanityOverTime());
        }
        else if (!canLoseSanity && sanityCoroutine != null)
        {
            StopCoroutine(sanityCoroutine);
            sanityCoroutine = null;
        }
    }

    public void RecoverSanity(int amount)
    {
        Sanity += amount;

        if (Sanity > 100)
            Sanity = 100;

        Debug.Log("Sanity recovered! Current Sanity: " + Sanity);
    }


    private void FixedUpdate()
    {
        UpdateState();

        switch (state)
        {
            case PlayerState.normal:
                if (fullScreenFX != null) fullScreenFX.SetActive(false);

                if (screenDistortionMaterial != null)
                    screenDistortionMaterial.SetFloat("_Blend", 0f);
                break;

            case PlayerState.distortion:
                if (fullScreenFX != null) fullScreenFX.SetActive(true);
                break;

            case PlayerState.insane:
                if (fullScreenFX != null) fullScreenFX.SetActive(true);
                if (screenDistortionMaterial != null)
                {
                    float newBlend = Mathf.PingPong(Time.time * 0.1f, 0.2f);
                    screenDistortionMaterial.SetFloat("_Blend", newBlend);
                }
                break;
        }
    }

    private void UpdateState()
    {
        if (Sanity > 50)
            state = PlayerState.normal;
        else if (Sanity > 30)
            state = PlayerState.distortion;
        else if (Sanity > 0)
            state = PlayerState.insane;
        else
            Debug.Log("game over");
    }
}