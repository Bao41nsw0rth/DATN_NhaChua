using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryTeller : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI storyText;        
    public TextMeshProUGUI promptText;       
    public CanvasGroup canvasGroup;          
    public CanvasGroup promptCanvasGroup;    

    [Header("Story")]
    [TextArea(3, 10)]
    public string[] storyParts;              
    public float typingSpeed = 0.03f;        
    public float punctuationPause = 0.20f;   

    [Header("Fade & Scene")]
    public float fadeDuration = 0.6f;
    public string nextSceneName = "MainScene";        

    [Header("Audio (optional)")]
    public AudioSource sfxSource;           
    public AudioClip typingSfx;
    public AudioSource ambientSource;       

    private int currentIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private string currentFullText = "";

    IEnumerator Start()
    {
        if (canvasGroup != null) canvasGroup.alpha = 0f;
        if (promptCanvasGroup != null) promptCanvasGroup.alpha = 0f;
        if (promptText != null) promptText.gameObject.SetActive(false);
        if (ambientSource != null) ambientSource.Play();

        if (canvasGroup != null) yield return StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));
        yield return new WaitForSeconds(0.15f);
        ShowNextPart();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                if (typingCoroutine != null) StopCoroutine(typingCoroutine);
                storyText.text = currentFullText;
                isTyping = false;
                if (promptCanvasGroup != null) StartCoroutine(FadePrompt(0f, 1f, 0.18f));
                else if (promptText != null) promptText.gameObject.SetActive(true);
            }
            else
            {
                ShowNextPart();
            }
        }
    }

    void ShowNextPart()
    {
        if (currentIndex < storyParts.Length)
        {
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeText(storyParts[currentIndex]));
            currentIndex++;
        }
        else
        {
            StartCoroutine(EndSequence());
        }
    }

    IEnumerator TypeText(string textToShow)
    {
        currentFullText = textToShow;
        isTyping = true;
        storyText.text = "";
        if (promptCanvasGroup != null) promptCanvasGroup.alpha = 0f;
        if (promptText != null) promptText.gameObject.SetActive(false);

        storyText.lineSpacing = 50f; 

        for (int i = 0; i < textToShow.Length; i++)
        {
            char c = textToShow[i];
            storyText.text += c;

            if (typingSfx != null && sfxSource != null)
            {
                sfxSource.pitch = Random.Range(0.95f, 1.08f);
                sfxSource.PlayOneShot(typingSfx, 0.6f);
            }

            float wait = typingSpeed;
            if (IsPunctuation(c)) wait += punctuationPause;
            yield return new WaitForSeconds(wait);
        }

        storyText.text += "\n";

        isTyping = false;

        if (promptCanvasGroup != null) StartCoroutine(FadePrompt(0f, 1f, 0.2f));
        else if (promptText != null) promptText.gameObject.SetActive(true);
    }


    IEnumerator FadePrompt(float from, float to, float duration)
    {
        if (promptCanvasGroup == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            promptCanvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        promptCanvasGroup.alpha = to;
    }

    IEnumerator FadeCanvas(float from, float to, float duration)
    {
        if (canvasGroup == null) yield break;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    IEnumerator EndSequence()
    {
        if (promptCanvasGroup != null) StartCoroutine(FadePrompt(promptCanvasGroup.alpha, 0f, 0.15f));
        yield return new WaitForSeconds(0.12f);
        if (canvasGroup != null) yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));

        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.Log("Story finished. nextSceneName chưa được đặt.");
    }

    bool IsPunctuation(char c)
    {
        return ".!?,;…".IndexOf(c) >= 0;
    }
}
