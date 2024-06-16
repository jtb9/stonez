using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioSource source;
    public AudioClip backgroundMusic1;

    void Start()
    {
        source = GetComponent<AudioSource>();

        source.loop = true;

        source.clip = backgroundMusic1;

        source.volume = 0.3f;

        source.Play();
    }
}
