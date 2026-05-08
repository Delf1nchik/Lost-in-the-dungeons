using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        // ”станавливаем значени€ слайдеров при старте (можно потом добавить загрузку из PlayerPrefs)
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        // ѕодписываемс€ на изменени€ слайдеров
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        // ”станавливаем значение в микшере. 
        // ≈сли значение минимальное (-40), можно ставить -80 дл€ полной тишины.
        if (value <= -39f) value = -80f;
        mainMixer.SetFloat("MusicVol", value);
    }

    public void SetSFXVolume(float value)
    {
        if (value <= -39f) value = -80f;
        mainMixer.SetFloat("SFXVol", value);
    }
}