using UnityEngine;
using UnityEngine.UI;

public class MainMenuVolume : MonoBehaviour
{
    public AudioSource audioSource;  // Nh?c n?n
    public Slider volumeSlider;      // Thanh ch?nh �m l�?ng

    void Start()
    {
        // G�n gi� tr? m?c �?nh cho Slider
        if (volumeSlider != null && audioSource != null)
        {
            volumeSlider.value = audioSource.volume;
            volumeSlider.onValueChanged.AddListener(SetVolume);
        }
    }

    void SetVolume(float volume)
    {
        audioSource.volume = volume; // ch?nh �m l�?ng nh?c theo slider
    }
}
