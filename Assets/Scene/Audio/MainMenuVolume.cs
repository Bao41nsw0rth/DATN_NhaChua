using UnityEngine;
using UnityEngine.UI;

public class MainMenuVolume : MonoBehaviour
{
    public AudioSource audioSource;  // Nh?c n?n
    public Slider volumeSlider;      // Thanh ch?nh âm lý?ng

    void Start()
    {
        // Gán giá tr? m?c ð?nh cho Slider
        if (volumeSlider != null && audioSource != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void SetVolume(float volume)
    {
        audioSource.volume = volume; // ch?nh âm lý?ng nh?c theo slider
    }
}
