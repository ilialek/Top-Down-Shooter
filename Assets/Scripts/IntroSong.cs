using UnityEngine;

public class IntroSong : MonoBehaviour
{
    public AudioSource src;

    void Start()
    {
        src.PlayDelayed(.5f);
    }
}
