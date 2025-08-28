using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMainSceneManager : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private Slider slider;

    private bool isPaused = false;

    private void Start()
    {
        continueButton.onClick.AddListener(OnContinueClick);
        optionButton.onClick.AddListener(OnOptionClick);
        exitButton.onClick.AddListener(OnExitClick);

        pausePanel.SetActive(false);
        optionPanel.SetActive(false);

        slider.value = AudioListener.volume;
        slider.onValueChanged.AddListener(SetVolume);

        // Ẩn chuột khi bắt đầu game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        pausePanel.SetActive(isPaused);

        if (isPaused)
        {
            // Hiện chuột và dừng game
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else
        {
            // Ẩn chuột và tiếp tục game
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
    }

    void OnContinueClick()
    {
        isPaused = false;
        pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;
    }

    void OnOptionClick()
    {
        optionPanel.SetActive(!optionPanel.activeInHierarchy);
    }

    void OnExitClick()
    {
        Time.timeScale = 1f; // reset tốc độ game khi thoát
        SceneManager.LoadScene("MenuScene");
    }
    
    void SetVolume(float volume)
    {
        AudioListener.volume = volume;
    }
}
