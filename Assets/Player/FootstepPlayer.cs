using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioClip footstepSound; // ��Inspector������Ų����ļ�
    private AudioSource audioSource;
    private bool isMoving = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        // ���WASD������
        bool movingInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                          Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (movingInput && !isMoving)
        {
            // ��ʼ�ƶ�ʱ����
            isMoving = true;
            audioSource.clip = footstepSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!movingInput && isMoving)
        {
            // ֹͣ�ƶ�ʱֹͣ����
            isMoving = false;
            audioSource.Stop();
        }
    }
}