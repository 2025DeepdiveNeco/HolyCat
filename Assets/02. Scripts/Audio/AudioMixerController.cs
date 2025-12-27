using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AudioMixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer m_AudioMixer;
    [SerializeField] private GameObject m_SettingsPanel; // 띄워줄 설정창 판넬

    [Header("Sliders")]
    [SerializeField] private Slider m_MusicMasterSlider;
    [SerializeField] private Slider m_MusicBGMSlider;
    [SerializeField] private Slider m_MusicSFXSlider;

    private bool isOpened = false;

    private void Awake()
    {
        // 슬라이더 이벤트 연결
        m_MusicMasterSlider.onValueChanged.AddListener(SetMasterVolume);
        m_MusicBGMSlider.onValueChanged.AddListener(SetMusicVolume);
        m_MusicSFXSlider.onValueChanged.AddListener(SetSFXVolume);

        // 시작할 때 판넬은 꺼둠
        m_SettingsPanel.SetActive(false);
    }
    private void Start()
    {       
        m_MusicMasterSlider.value = 1f;
        m_MusicBGMSlider.value = 1f;
        m_MusicSFXSlider.value = 1f;
        
        SetMasterVolume(1f);
        SetMusicVolume(1f);
        SetSFXVolume(1f);
    }

    private void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleSettings();
        }
    }

    public void ToggleSettings()
    {
        isOpened = !isOpened;
        m_SettingsPanel.SetActive(isOpened);

        if (isOpened)
        {
            Time.timeScale = 0f;
            // 마우스 커서 보이기
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Time.timeScale = 1f;
            Cursor.visible = false;
        }
    }

    public void SetMasterVolume(float volume)
    {
        m_AudioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        m_AudioMixer.SetFloat("BGM", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        m_AudioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}