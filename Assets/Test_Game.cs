using System.Collections;
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
    public MeshFilter ShowMeshFilter;

    public Text ScoreText; // 新增：显示得分的文本

    public float ShowAnswerDelay = 1.5f;

    public Color DefaultColor = Color.white;
    public Color CorrectColor = Color.green;
    public Color WrongColor = Color.red;

    GameObject[] GameObject_List;
    MeshFilter[] MeshFilters;

    int answerIndex;
    bool canClick = true;

    int Score = 0;

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
        SetRandomMesh();
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
            validSelection = MeshFilters.All(x => x != null);

            if (!validSelection)
            {
                Debug.LogWarning("检测到选中的对象中有未挂载 MeshFilter，重新随机选择...");
            }
        }

        answerIndex = Random.Range(0, MeshFilters.Length);
        ShowMeshFilter.mesh = MeshFilters[answerIndex].sharedMesh;

        Debug.Log($"正确答案是：{GameObject_List[answerIndex].name}");
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
            StartGame();
        }
        else
        {
            Debug.Log("答错了！😢 正确答案已显示，请点击下一题继续。");

            GetButtonByIndex(index).GetComponent<Image>().color = WrongColor;
            GetButtonByIndex(answerIndex).GetComponent<Image>().color = CorrectColor;

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
}
