using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    public AudioClip footstepSound; // 在Inspector中拖入脚步声文件
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
        // 检测WASD键输入
        bool movingInput = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) ||
                          Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (movingInput && !isMoving)
        {
            // 开始移动时播放
            isMoving = true;
            audioSource.clip = footstepSound;
            audioSource.loop = true;
            audioSource.Play();
        }
        else if (!movingInput && isMoving)
        {
            // 停止移动时停止播放
            isMoving = false;
            audioSource.Stop();
        }
    }
}