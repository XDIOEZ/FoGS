using UnityEngine;
using DG.Tweening;

public class BuildingItem : MonoBehaviour
{
    [Header("��Ʒ��Ϣ")]
    public ItemData itemData;
    [Header("΢������")]
    [Tooltip("Ӱ�쵱ǰ�������Ķ���ϵ��")]
    public float disassembleMultiplier = 1f;

    [Tooltip("����ķ���ƫ�ƣ����ڶ��Ʊ����ⷽ��")]
    public Vector3 extraDisassembleDirection = Vector3.zero;

    [Header("ȫ������")]
    [Tooltip("�Ƿ�Բ�ⷽ����й�һ�������� Manager ���ƣ�")]
    public bool normalizeDirection = true;

    [Header("��������")]
    [Tooltip("���/��ԭ����ʱ��")]
    public float animationDuration = 0.5f;


    [Header("�����߲���")]
    [Tooltip("�����߸߶�")]
    public float reassembleArcHeight = 2f;

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;
    private Vector3 defaultScale;
    private bool isDisassembled = false;

    private void Awake()
    {
        // ��¼��ʼ״̬
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
        defaultScale = transform.localScale;
        Debug.Log("000");
    }

    [ContextMenu("ѯ��AI")]
    public void AskAI()
    {
       
        // ͨ�� Tag ��ȡ���Ϊ "AISample" �� GameObject
        GameObject aiSample = GameObject.FindGameObjectWithTag("AIChat");
      
        if (aiSample != null)
        {
            Debug.Log("�ҵ� AISample�� " + aiSample.name);
            // ��������������� GameObject ����һ�������������������ĳ������
            aiSample.GetComponent<PreInput>().Show();
            aiSample.GetComponent<PreInput>().inputString = itemData.name+ " ��ʲô?";
            aiSample.GetComponent<PreInput>().SyncInput();
            aiSample.GetComponent<PreInput>().SendMessage();
        }
        else
        {
            Debug.LogWarning("δ�ҵ� Tag Ϊ AISample �Ķ���");
        }
    }


public void Start()
    {
        if (itemData.name == "")
            itemData.name = gameObject.name;
    }

    /// <summary>
    /// ִ�в�⶯��
    /// </summary>
    public void Disassemble(Vector3 globalDirection, float globalAmount)
    {
        if (isDisassembled) return;

        // �ϲ�ƫ�Ʒ���
        Vector3 combinedDir = globalDirection + extraDisassembleDirection;
        if (normalizeDirection)
            combinedDir = combinedDir.normalized;

        // Ŀ��λ�� = ��ʼλ�� + �������� * ȫ��ϵ�� * ����ϵ��
        Vector3 targetPos = defaultPosition + combinedDir * (globalAmount * disassembleMultiplier);

        // DOTween ƽ���ƶ�/��ת/����
        transform.DOMove(targetPos, animationDuration).SetEase(Ease.InOutSine);
        transform.DORotateQuaternion(defaultRotation, animationDuration).SetEase(Ease.InOutSine);
        transform.DOScale(defaultScale, animationDuration).SetEase(Ease.InOutSine);

        isDisassembled = true;
    }

    /// <summary>
    /// �ö����ѿ��ͻس�ʼ״̬
    /// </summary>
    public void Reassemble(float animationDuration)
    {
        if(animationDuration == 0)
        {
            animationDuration = this.animationDuration;
        }

        if (!isDisassembled) return;

        transform.DOMove(defaultPosition, animationDuration).SetEase(Ease.InOutSine);
        transform.DORotateQuaternion(defaultRotation, animationDuration).SetEase(Ease.InOutSine);
        transform.DOScale(defaultScale, animationDuration)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(() => isDisassembled = false);
    }

    /// <summary>
    /// �����߻�ԭ�������ȸ�������
    /// </summary>
    public void ReassembleWithArc(float animationDuration)
    {
        if (animationDuration == 0)
        {
            animationDuration = this.animationDuration;
        }

        if (!isDisassembled) return;

        Vector3 startPos = transform.position;
        Vector3 endPos = defaultPosition;

        // �����е�߶ȣ����ӻ���
        Vector3 peakPos = (startPos + endPos) / 2f + Vector3.up * reassembleArcHeight;

        // ������·��
        Vector3[] path = new Vector3[] { startPos, peakPos, endPos };

        // DOTween ·������
        transform.DOPath(path, animationDuration, PathType.CatmullRom)
                 .SetEase(Ease.InOutSine)
                 .OnComplete(() => isDisassembled = false);

        // ͬʱ��ԭ��ת������
        transform.DORotateQuaternion(defaultRotation, animationDuration).SetEase(Ease.InOutSine);
        transform.DOScale(defaultScale, animationDuration).SetEase(Ease.InOutSine);
    }

}
[System.Serializable]
public class ItemData
{
    // ��Ʒ����
    public string name;
    // ��Ʒ����
    [TextArea]
    public string description;
}
