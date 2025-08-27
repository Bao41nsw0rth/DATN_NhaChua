using UnityEngine;
using System.Collections;

public class SanityManager : MonoBehaviour
{
    [SerializeField] private GameObject fullScreenFX;
    [SerializeField] private Material screenDistortionMaterial;
    [SerializeField] private float decreaseInterval = 1.25f;
    [SerializeField] private float recoverInterval = 5f;
    [SerializeField] private bool canLoseSanity = false;

    private int Sanity = 100;
    private Coroutine sanityCoroutine;

    private enum PlayerState
    {
        normal,
        distortion,
        insane
    }

    private PlayerState state = PlayerState.normal;

    private void Start()
    {
        sanityCoroutine = StartCoroutine(SanityRoutine());
    }

    private IEnumerator SanityRoutine()
    {
        float decreaseTimer = 0f;
        float recoverTimer = 0f;

        while (true)
        {
            yield return null;

            decreaseTimer += Time.deltaTime;
            recoverTimer += Time.deltaTime;

            if (canLoseSanity && decreaseTimer >= decreaseInterval)
            {
                Sanity = Mathf.Max(Sanity - 1, 0);
                decreaseTimer = 0f;
            }

            if (!canLoseSanity && recoverTimer >= recoverInterval)
            {
                Sanity = Mathf.Min(Sanity + 10, 100);
                recoverTimer = 0f;
            }
        }
    }

    public void SetLoseSanity(bool value)
    {
        canLoseSanity = value;
    }

    private void Update()
    {
        Debug.Log(message: "sanity " + Sanity);
        float newBlend = Mathf.PingPong(Time.time * 0.1f, 0.2f);
        UpdateState();

        switch (state)
        {
            case PlayerState.normal:
                if (fullScreenFX != null) fullScreenFX.SetActive(false);
                break;

            case PlayerState.distortion:
                if (newBlend > 0.1f) fullScreenFX.SetActive(true);
                else fullScreenFX.SetActive(false);
                break;

            case PlayerState.insane:
                if (fullScreenFX != null) fullScreenFX.SetActive(true);
                if (screenDistortionMaterial != null)
                {
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