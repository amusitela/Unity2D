using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip shootClip;
    public AudioClip swordClip;
    public AudioClip explodeClip;
    public AudioClip walkClip;
    public AudioClip switchClip;
    public AudioClip hitClip;
    public AudioClip dieClip;
    public AudioClip levelUpClip; 

    private AudioSource audioSource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            // 获取 AudioSource 组件
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // 如果没有 AudioSource 组件，添加一个
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }


    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }



}
