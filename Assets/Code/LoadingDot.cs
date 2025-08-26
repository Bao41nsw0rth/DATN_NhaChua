using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LoadingDot : MonoBehaviour
{
    public TextMeshProUGUI loadingText;
    public float dotInterval = 0.5f; // thời gian giữa mỗi dấu chấm

    private void OnEnable()
    {
        StartCoroutine(AnimateLoading());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator AnimateLoading()
    {
        string baseText = "Loading ";
        int dotCount = 1;

        while (true)
        {
            loadingText.text = baseText +new string('.', dotCount)+" ";
            dotCount = (dotCount + 1) % 4; // 0,1,2,3 rồi quay lại 0
            yield return new WaitForSeconds(dotInterval);
        }
    }
}
