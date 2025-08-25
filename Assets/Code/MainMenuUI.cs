using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [Header("Main Buttons")]
    public Button playButton;
    public Button optionButton;
    public Button exitButton;

    [Header("Option Panel")]
    public GameObject optionPanel;
    public Slider volumeSlider;
    public Button backButton;

    private void Start()
    {
        // G?n s? ki?n khi ng�?i ch�i nh?n c�c n�t
        playButton.onClick.AddListener(OnPlayClicked);
        optionButton.onClick.AddListener(OnOptionClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        backButton.onClick.AddListener(OnBackClicked);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // ?n OptionPanel khi b?t �?u
        optionPanel.SetActive(false);

        // G�n volume ban �?u n?u b?n c� PlayerPrefs ? b?n n�ng cao
        AudioListener.volume = volumeSlider.value;
    }

    void OnPlayClicked()
    {
        // ?n OptionPanel (n?u �ang m?)
        optionPanel.SetActive(false);

        // �?i th�nh t�n scene ch�i game c?a b?n
        SceneManager.LoadScene("LoadingScene");
    }

    void OnOptionClicked()
    {
        // Hi?n OptionPanel
        optionPanel.SetActive(true);
    }

    void OnBackClicked()
    {
        // ?n OptionPanel khi ng�?i d�ng nh?n n�t "Back"
        optionPanel.SetActive(false);
    }

    void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("Tho�t game");
    }

    void OnVolumeChanged(float value)
    {
        // Thay �?i �m l�?ng to�n game
        AudioListener.volume = value;
    }
}
