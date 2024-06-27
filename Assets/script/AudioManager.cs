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
            // ��ȡ AudioSource ���
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                // ���û�� AudioSource ��������һ��
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
