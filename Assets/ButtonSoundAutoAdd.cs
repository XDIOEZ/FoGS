using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]
public class ButtonSoundAutoAdd : MonoBehaviour
{
    [Header("������������")]
    [Tooltip("���ڲ��ŵ��/���������� AudioSource������ָ�����ڱ� GameObject �ϳ��Ի�ȡ���Զ����һ�� AudioSource��")]
    public AudioSource audioSource;

    [Tooltip("�Ƿ�����Ǽ���״̬�µ� Button/Slider ��������Ϊ true��������Ѽ��س����е����и��������Ӷ��󣬲��� inactive ״̬�������")]
    public bool includeInactive = false;

    [Header("Slider �¼�����")]
    [Tooltip("�Ƿ���ÿ�� Slider ֵ�仯ʱ��������������ֻ������ק�����������ض�����ʱ���ţ����޸� OnSliderValueChanged ���߼���")]
    public bool playOnEveryValueChange = true;

    void Start()
    {
        // ȷ���� AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                // ���ﲻ���� clip����ʹ������ Inspector �������ʱ��ֵ
            }
        }
        else
        {
            audioSource.playOnAwake = false;
        }

        // �󶨳��������� Button/Slider
        BindButtons();
        BindSliders();
    }

    /// <summary>
    /// �������Ѽ��س����б����� GameObject������ Button ������� onClick ��������
    /// </summary>
    private void BindButtons()
    {
        List<Button> allButtons = FindAllComponentsInLoadedScenes<Button>(includeInactive);
        foreach (var btn in allButtons)
        {
            if (btn == null) continue;
            // �Ƴ����лص��Ա����ظ�
            btn.onClick.RemoveListener(PlaySound);
            btn.onClick.AddListener(PlaySound);
        }
    }

    /// <summary>
    /// �������Ѽ��س����б����� GameObject������ Slider ������� onValueChanged ��������
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
    /// ͨ�÷������ڵ�ǰ�����Ѽ��س����в������� T �����ͨ�� SceneManager ����ÿ�������� Root GameObject����ʹ�� GetComponentsInChildren��
    /// </summary>
    /// <typeparam name="T">Component ���ͣ��� Button��Slider</typeparam>
    /// <param name="includeInactive">�Ƿ���� inactive ����</param>
    /// <returns>�ҵ�������б�</returns>
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
                // GetComponentsInChildren �� GameObject ��֧�� includeInactive �������ɵݹ��ȡ
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
    /// Button ���ʱ���ã���������
    /// </summary>
    private void PlaySound()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("[ButtonSoundAutoAdd] AudioSource Ϊ�գ��޷�����������");
            return;
        }
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("[ButtonSoundAutoAdd] AudioSource.clip δ���ã��޷�����������");
        }
    }

    /// <summary>
    /// Slider ֵ�仯ʱ���ã��������þ����Ƿ񲥷�����
    /// </summary>
    /// <param name="value"></param>
    private void OnSliderValueChanged(float value)
    {
        if (!playOnEveryValueChange)
        {
            // ��ϣ��������ק����ʱ���ţ����ڴ˴���չ�߼��������� IPointerUpHandler��
            return;
        }
        PlaySound();
    }

    /// <summary>
    /// ��������ʱ��̬�����µ� UI��Button/Slider��ʱ�����ֶ����ô˷����������°󶨡�
    /// </summary>
    public void RefreshBindings()
    {
        BindButtons();
        BindSliders();
    }
}
