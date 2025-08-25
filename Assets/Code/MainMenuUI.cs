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
        // G?n s? ki?n khi ngý?i chõi nh?n các nút
        playButton.onClick.AddListener(OnPlayClicked);
        optionButton.onClick.AddListener(OnOptionClicked);
        exitButton.onClick.AddListener(OnExitClicked);
        backButton.onClick.AddListener(OnBackClicked);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);

        // ?n OptionPanel khi b?t ð?u
        optionPanel.SetActive(false);

        // Gán volume ban ð?u n?u b?n có PlayerPrefs ? b?n nâng cao
        AudioListener.volume = volumeSlider.value;
    }

    void OnPlayClicked()
    {
        // ?n OptionPanel (n?u ðang m?)
        optionPanel.SetActive(false);

        // Ð?i thành tên scene chõi game c?a b?n
        SceneManager.LoadScene("LoadingScene");
    }

    void OnOptionClicked()
    {
        // Hi?n OptionPanel
        optionPanel.SetActive(true);
    }

    void OnBackClicked()
    {
        // ?n OptionPanel khi ngý?i dùng nh?n nút "Back"
        optionPanel.SetActive(false);
    }

    void OnExitClicked()
    {
        Application.Quit();
        Debug.Log("Thoát game");
    }

    void OnVolumeChanged(float value)
    {
        // Thay ð?i âm lý?ng toàn game
        AudioListener.volume = value;
    }
}
