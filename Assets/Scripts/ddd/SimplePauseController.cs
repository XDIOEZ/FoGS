using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SimplePauseController : MonoBehaviour
{
    private Button pauseButton;
    private bool isPaused = false;

    // 使用不同文本标识状态（可选）
    private Text buttonText;
    private const string PAUSE_TEXT = "暂停";
    private const string RESUME_TEXT = "继续";

    private void Awake()
    {
        // 获取按钮组件
        pauseButton = GetComponent<Button>();

        // 尝试获取按钮上的文本组件（可选）
        buttonText = pauseButton.GetComponentInChildren<Text>();

        // 添加点击事件监听
        pauseButton.onClick.AddListener(TogglePause);
    }

    private void TogglePause()
    {
        isPaused = !isPaused;

        // 设置时间刻度
        Time.timeScale = isPaused ? 0f : 1f;

        // 暂停/恢复音频
        AudioListener.pause = isPaused;

        // 更新按钮文本（可选）
        if (buttonText != null)
        {
            buttonText.text = isPaused ? RESUME_TEXT : PAUSE_TEXT;
        }
    }

    private void OnDestroy()
    {
        // 确保游戏对象销毁时恢复时间刻度
        Time.timeScale = 1f;
        AudioListener.pause = false;
    }
}
