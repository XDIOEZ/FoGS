using UnityEngine;

public class SimpleDecompositionPause : MonoBehaviour
{
    // 时间控制变量
    private float startTime;
    private float pausedTime;
    private bool isRunning = false;
    private bool isPaused = false;

    // 进度(0-1)
    private float currentProgress = 0f;

    void Update()
    {
        if (!isRunning || isPaused) return;

        // 计算当前进度（基于时间流逝）
        float elapsed = Time.time - startTime;
        currentProgress = Mathf.Clamp01(elapsed / 5f); // 假设总时长5秒

        Debug.Log("当前进度: " + currentProgress.ToString("F2"));

        // 当进度完成时停止
        if (currentProgress >= 1f)
        {
            StopProcess();
        }
    }

    // 开始进程
    public void StartProcess()
    {
        if (isRunning) return;

        startTime = Time.time - (pausedTime * 5f); // 补偿暂停的时间
        isRunning = true;
        isPaused = false;
    }

    // 暂停进程
    public void PauseProcess()
    {
        if (!isRunning || isPaused) return;

        pausedTime = currentProgress; // 记录暂停时的进度
        isPaused = true;
    }

    // 停止/重置进程
    public void StopProcess()
    {
        isRunning = false;
        isPaused = false;
        pausedTime = 0f;
        currentProgress = 0f;
    }

    // 获取当前进度（供其他脚本使用）
    public float GetCurrentProgress()
    {
        return currentProgress;
    }
}
