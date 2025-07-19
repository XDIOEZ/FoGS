using UnityEngine;

public class ModelDecompositionController : MonoBehaviour
{
    // 分解/复原状态
    public enum DecompositionState
    {
        None,       // 无操作
        Decomposing, // 正在分解
        Restoring   // 正在复原
    }

    private DecompositionState currentState = DecompositionState.None;
    private float processStartTime;
    private float currentProgress = 0f; // 0=完全复原, 1=完全分解

    // 外部参数
    public float decompositionDuration = 3.0f; // 完全分解所需时间
    public float restorationDuration = 3.0f;   // 完全复原所需时间

    void Update()
    {
        if (currentState == DecompositionState.None) return;

        float elapsed = Time.time - processStartTime;
        float targetDuration = currentState == DecompositionState.Decomposing ?
                              decompositionDuration : restorationDuration;

        // 计算当前进度 (0-1)
        float newProgress = Mathf.Clamp01(elapsed / targetDuration);

        // 如果是复原过程，进度是反向的
        if (currentState == DecompositionState.Restoring)
        {
            newProgress = 1 - newProgress;
        }

        // 只有当进度有实际变化时才更新模型
        if (newProgress != currentProgress)
        {
            currentProgress = newProgress;
            UpdateModelDecomposition(currentProgress);
        }

        // 检查是否完成
        if (elapsed >= targetDuration)
        {
            currentState = DecompositionState.None;
        }
    }

    // 开始分解
    public void StartDecomposition()
    {
        // 如果已经在分解中，则不做任何操作
        if (currentState == DecompositionState.Decomposing) return;

        // 计算从当前进度到完全分解需要的时间
        float remainingTime = decompositionDuration * (1 - currentProgress);
        processStartTime = Time.time - (decompositionDuration - remainingTime);

        currentState = DecompositionState.Decomposing;
    }

    // 开始复原
    public void StartRestoration()
    {
        // 如果已经在复原中，则不做任何操作
        if (currentState == DecompositionState.Restoring) return;

        // 计算从当前进度到完全复原需要的时间
        float remainingTime = restorationDuration * currentProgress;
        processStartTime = Time.time - (restorationDuration - remainingTime);

        currentState = DecompositionState.Restoring;
    }

    // 停止当前过程
    public void StopProcess()
    {
        currentState = DecompositionState.None;
        // 进度会保持在当前位置
    }

    // 更新模型分解状态 (实现你的实际分解逻辑)
    private void UpdateModelDecomposition(float progress)
    {
        // 这里实现你的实际模型分解/复原逻辑
        // progress: 0 = 完全复原, 1 = 完全分解

        // 示例: 控制多个部件的位移/旋转/缩放
        // foreach (var part in decompositionParts)
        // {
        //     part.localPosition = Vector3.Lerp(originalPositions[part], decomposedPositions[part], progress);
        //     part.localRotation = Quaternion.Lerp(originalRotations[part], decomposedRotations[part], progress);
        // }

        Debug.Log("更新模型状态，当前进度: " + progress);
    }
}