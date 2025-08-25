using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class LoadingManager : MonoBehaviour
{
    public Image fillImage; // Image có Image Type = Filled
    public float fillSpeed = 3f;

    private float targetProgress = 0f;

    void Start()
    {
        StartCoroutine(LoadAsyncScene());
    }

    void Update()
    {
        // Tăng dần fillAmount về phía targetProgress
        if (fillImage != null)
        {
            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, targetProgress, Time.deltaTime * fillSpeed);
        }
    }

    IEnumerator LoadAsyncScene()
    {


        AsyncOperation operation = SceneManager.LoadSceneAsync("MainScene");
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Chuyển progress 0 → 1 mượt mà
            targetProgress = Mathf.Clamp01(operation.progress / 0.9f);

            // Khi scene load gần xong
            if (operation.progress >= 0.9f && fillImage.fillAmount >= 0.995f)
            {
                yield return new WaitForSeconds(0.5f);;
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }


}
