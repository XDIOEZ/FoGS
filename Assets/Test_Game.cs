﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Test_Game : MonoBehaviour
{
    public Button FristButton;
    public Button SecondButton;
    public Button ThirdButton;
    public Button NextButton;

    public GameObject ReferenceParent;

    // 用一个空的容器 GameObject 来挂载实例化的显示对象
    // 在编辑器中把一个空 GameObject 拖进来，或者用 ShowMeshFilter.gameObject 也可以
    public Transform ShowContainer;

    public Text ScoreText; // 显示得分的文本

    public float ShowAnswerDelay = 1.5f;

    public Color DefaultColor = Color.white;
    public Color CorrectColor = Color.green;
    public Color WrongColor = Color.red;

    GameObject[] GameObject_List;
    MeshFilter[] MeshFilters;

    int answerIndex;
    bool canClick = true;

    int Score = 0;

    // 新增：当前显示的实例对象
    private GameObject currentDisplayObject;

    void Start()
    {
        FristButton.onClick.AddListener(() => OnButtonClick(0));
        SecondButton.onClick.AddListener(() => OnButtonClick(1));
        ThirdButton.onClick.AddListener(() => OnButtonClick(2));
        NextButton.onClick.AddListener(() => StartGame());

        NextButton.gameObject.SetActive(false);

        UpdateScoreText();
        StartGame();
    }

    void StartGame()
    {
        // 每次开始新一题前，清理上一次显示的实例
        ClearCurrentDisplay();

        SetRandomMesh();      // 随机选并实例化
        SetButtonText();
        ResetButtonColors();

        canClick = true;
        NextButton.gameObject.SetActive(false);
    }

    public void SetRandomMesh()
    {
        bool validSelection = false;

        while (!validSelection)
        {
            var allChildren = ReferenceParent.GetComponentsInChildren<Transform>()
                                             .Where(t => t != ReferenceParent.transform)
                                             .Select(t => t.gameObject)
                                             .ToList();

            GameObject_List = allChildren.OrderBy(x => Random.value).Take(3).ToArray();
            MeshFilters = GameObject_List.Select(x => x.GetComponent<MeshFilter>()).ToArray();

            // 检查是否全部有 MeshFilter
            bool allHaveMeshFilter = MeshFilters.All(x => x != null);

            // 检查是否有重名
            bool hasDuplicateName = GameObject_List.GroupBy(x => x.name).Any(g => g.Count() > 1);

            // 检查是否有包含 "?" 的对象
            bool hasInvalidName = GameObject_List.Any(x => x.name.Contains("?"));

            if (!allHaveMeshFilter)
            {
                Debug.LogWarning("检测到选中的对象中有未挂载 MeshFilter，重新随机选择...");
                continue; // 重新随机
            }

            if (hasDuplicateName)
            {
                Debug.LogWarning("检测到选中的对象中有重名，重新随机选择...");
                continue; // 重新随机
            }

            if (hasInvalidName)
            {
                Debug.LogWarning("检测到选中的对象名称中包含非法字符 '?', 重新随机选择...");
                continue; // 重新随机
            }

            validSelection = true; // 条件全部满足才结束
        }

        answerIndex = Random.Range(0, MeshFilters.Length);
        Debug.Log($"正确答案是：{GameObject_List[answerIndex].name}");

        InstantiateSelectedObject();
    }


    void InstantiateSelectedObject()
    {
        if (ShowContainer == null)
        {
            Debug.LogError("ShowContainer 未设置，请在 Inspector 里拖入一个空 GameObject 作为容器。");
            return;
        }

        // 销毁上一次实例
        ClearCurrentDisplay();

        GameObject prefabOrSceneObj = GameObject_List[answerIndex];

        // 实例化：将选中的对象复制一份，作为 ShowContainer 的子对象
        currentDisplayObject = Instantiate(prefabOrSceneObj, ShowContainer);

        // 重置 Transform
        currentDisplayObject.transform.localPosition = Vector3.zero;
        currentDisplayObject.transform.localRotation = Quaternion.identity;
        currentDisplayObject.transform.localScale = Vector3.one;

        //遍历所有子对象，将其 MeshCollider 组件禁用
        foreach (Transform child in currentDisplayObject.transform)
        {
            child.GetComponent<MeshCollider>().enabled = false;
        }
        currentDisplayObject.GetComponent<MeshCollider>().enabled = false;


        // 自动居中调整
        CenterMesh(currentDisplayObject);
    }

    /// <summary>
    /// 静态展示用：运行时动态调整 Mesh 轴心居中
    /// </summary>
    void CenterMesh(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

        if (renderers.Length == 0)
        {
            Debug.LogWarning("目标对象没有渲染组件，无法计算中心点。");
            return;
        }

        Bounds bounds = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++)
        {
            bounds.Encapsulate(renderers[i].bounds);
        }

        Vector3 meshCenter = bounds.center;
        Vector3 offset = obj.transform.position - meshCenter;

        obj.transform.position += offset;
    }


    void ClearCurrentDisplay()
    {
        if (currentDisplayObject != null)
        {
            // 如果希望销毁实例：
            Destroy(currentDisplayObject);
            currentDisplayObject = null;
        }
        // 如果 ShowContainer 下可能还有残留的其它对象，也可以：
        // foreach (Transform child in ShowContainer) Destroy(child.gameObject);
    }

    public void SetButtonText()
    {
        FristButton.GetComponentInChildren<Text>().text = GameObject_List[0].name;
        SecondButton.GetComponentInChildren<Text>().text = GameObject_List[1].name;
        ThirdButton.GetComponentInChildren<Text>().text = GameObject_List[2].name;
    }

    void ResetButtonColors()
    {
        FristButton.GetComponent<Image>().color = DefaultColor;
        SecondButton.GetComponent<Image>().color = DefaultColor;
        ThirdButton.GetComponent<Image>().color = DefaultColor;
    }

    void OnButtonClick(int index)
    {
        if (!canClick) return;

        canClick = false;

        if (index == answerIndex)
        {
            Debug.Log("答对了！😘");
            Score++;
            UpdateScoreText();
            // 答对直接下一题：清理并开始新一题
            StartGame();
        }
        else
        {
            Debug.Log("答错了！😢 正确答案已显示，请点击下一题继续。");

            // 更改按钮颜色提示
            GetButtonByIndex(index).GetComponent<Image>().color = WrongColor;
            GetButtonByIndex(answerIndex).GetComponent<Image>().color = CorrectColor;

            // 也可以在场景中的实例对象上做高亮、闪烁等效果：
            // 例如：将 currentDisplayObject 或它的某个子 MeshRenderer 改成 CorrectColor：
            HighlightDisplayObject(answerIndex, CorrectColor);
            HighlightDisplayObject(index, WrongColor);

            StartCoroutine(ShowNextButtonDelay());
        }
    }

    Button GetButtonByIndex(int index)
    {
        if (index == 0) return FristButton;
        if (index == 1) return SecondButton;
        return ThirdButton;
    }

    IEnumerator ShowNextButtonDelay()
    {
        yield return new WaitForSeconds(ShowAnswerDelay);
        NextButton.gameObject.SetActive(true);
    }

    void UpdateScoreText()
    {
        if (ScoreText != null)
        {
            ScoreText.text = "得分：" + Score;
        }
    }

    // 可选：根据索引高亮场景中显示的对象。因为 currentDisplayObject 始终只对应正确答案实例，
    // 若要显示错选的实例，需要提前也实例化所有三个？或者只高亮正确实例即可。
    // 这里只演示高亮当前实例（正确答案）。
    void HighlightDisplayObject(int idx, Color color)
    {/*
        // 如果要高亮错误选项的场景对象，一般需要提前把三选项都实例化到场景里，但通常只显示正确答案即可。
        if (idx == answerIndex && currentDisplayObject != null)
        {
            var renderers = currentDisplayObject.GetComponentsInChildren<MeshRenderer>();
            foreach (var rend in renderers)
            {
                // 使用 rend.material 会实例化材质副本，不影响原资源
                rend.material.color = color;
            }
        }*/
    }
}
