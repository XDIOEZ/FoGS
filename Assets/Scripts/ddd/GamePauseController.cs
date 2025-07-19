using UnityEngine;
using UnityEngine.UI;

public class GamePauseController : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button pauseButton;
    [SerializeField] private Text buttonText;

    [Header("Audio Settings")]
    [SerializeField] private AudioSource backgroundMusic;
    [SerializeField] private AudioSource[] sfxSources;

    private bool isPaused = false;
    private const string PAUSE_TEXT = "II"; // 暂停符号
    private const string RESUME_TEXT = "▶"; // 播放符号

    private void Start()
    {
        // 初始化按钮
        if (pauseButton != null)
        {
            pauseButton.onClick.AddListener(TogglePause);
        }

        // 设置音乐不受暂停影响
        if (backgroundMusic != null)
        {
            backgroundMusic.ignoreListenerPause = true;
        }

        // 设置所有音效受暂停影响
        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
            {
                sfx.ignoreListenerPause = false;
            }
        }

        UpdateButtonAppearance();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        // 控制游戏时间
        Time.timeScale = isPaused ? 0f : 1f;

        // 控制音频监听器(不影响背景音乐)
        AudioListener.pause = isPaused;

        // 单独控制音效
        foreach (var sfx in sfxSources)
        {
            if (sfx != null)
            {
                if (isPaused) sfx.Pause();
                else sfx.UnPause();
            }
        }

        UpdateButtonAppearance();
    }

    private void UpdateButtonAppearance()
    {
        if (buttonText != null)
        {
            buttonText.text = isPaused ? RESUME_TEXT : PAUSE_TEXT;
        }
    }

    private void OnDestroy()
    {
        // 确保游戏结束时恢复时间
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
