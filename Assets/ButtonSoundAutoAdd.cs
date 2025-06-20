using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class ButtonSoundAutoAdd : MonoBehaviour
{
    [Header("声音播放设置")]
    [Tooltip("用于播放点击/滑动声音的 AudioSource，若不指定会在本 GameObject 上尝试获取或自动添加一个 AudioSource。")]
    public AudioSource audioSource;

    [Tooltip("是否包含非激活状态下的 Button/Slider 组件。如果为 true，会遍历已加载场景中的所有根对象及其子对象，查找 inactive 状态的组件。")]
    public bool includeInactive = false;

    [Header("Slider 事件设置")]
    [Tooltip("是否在每次 Slider 值变化时都播放声音；若只想在拖拽结束或满足特定条件时播放，可修改 OnSliderValueChanged 中逻辑。")]
    public bool playOnEveryValueChange = true;

    void Start()
    {
        // 确保有 AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                // 这里不设置 clip，由使用者在 Inspector 里或运行时赋值
            }
        }
        else
        {
            audioSource.playOnAwake = false;
        }

        // 绑定场景中所有 Button/Slider
        BindButtons();
        BindSliders();
    }

    /// <summary>
    /// 在所有已加载场景中遍历根 GameObject，查找 Button 组件并绑定 onClick 播放声音
    /// </summary>
    private void BindButtons()
    {
        List<Button> allButtons = FindAllComponentsInLoadedScenes<Button>(includeInactive);
        foreach (var btn in allButtons)
        {
            if (btn == null) continue;
            // 移除已有回调以避免重复
            btn.onClick.RemoveListener(PlaySound);
            btn.onClick.AddListener(PlaySound);
        }
    }

    /// <summary>
    /// 在所有已加载场景中遍历根 GameObject，查找 Slider 组件并绑定 onValueChanged 播放声音
    /// </summary>
    private void BindSliders()
    {
        List<Slider> allSliders = FindAllComponentsInLoadedScenes<Slider>(includeInactive);
        foreach (var sl in allSliders)
        {
            if (sl == null) continue;
            sl.onValueChanged.RemoveListener(OnSliderValueChanged);
            sl.onValueChanged.AddListener(OnSliderValueChanged);
        }
    }

    /// <summary>
    /// 通用方法：在当前所有已加载场景中查找类型 T 组件。通过 SceneManager 遍历每个场景的 Root GameObject，再使用 GetComponentsInChildren。
    /// </summary>
    /// <typeparam name="T">Component 类型，如 Button、Slider</typeparam>
    /// <param name="includeInactive">是否包含 inactive 对象</param>
    /// <returns>找到的组件列表</returns>
    private List<T> FindAllComponentsInLoadedScenes<T>(bool includeInactive) where T : Component
    {
        List<T> results = new List<T>();

        int sceneCount = SceneManager.sceneCount;
        for (int i = 0; i < sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            GameObject[] roots = scene.GetRootGameObjects();
            foreach (GameObject root in roots)
            {
                // GetComponentsInChildren 在 GameObject 上支持 includeInactive 参数，可递归获取
                T[] comps = root.GetComponentsInChildren<T>(includeInactive);
                if (comps != null && comps.Length > 0)
                {
                    results.AddRange(comps);
                }
            }
        }
        return results;
    }

    /// <summary>
    /// Button 点击时调用：播放声音
    /// </summary>
    private void PlaySound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("[ButtonSoundAutoAdd] AudioSource 为空，无法播放声音。");
            return;
        }
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("[ButtonSoundAutoAdd] AudioSource.clip 未设置，无法播放声音。");
        }
    }

    /// <summary>
    /// Slider 值变化时调用：根据设置决定是否播放声音
    /// </summary>
    /// <param name="value"></param>
    private void OnSliderValueChanged(float value)
    {
        if (!playOnEveryValueChange)
        {
            // 若希望仅在拖拽结束时播放，可在此处扩展逻辑，例如结合 IPointerUpHandler。
            return;
        }
        PlaySound();
    }

    /// <summary>
    /// 当在运行时动态生成新的 UI（Button/Slider）时，可手动调用此方法进行重新绑定。
    /// </summary>
    public void RefreshBindings()
    {
        BindButtons();
        BindSliders();
    }
}
