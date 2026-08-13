using UnityEngine;
using UnityEngine.Playables;

public class TimelineAudioManager : MonoBehaviour
{
    public PlayableDirector director;          // El timeline de la intro
    public AudioSource backgroundAudio;        // Música de fondo

    private bool startedMusic = false;

    void Update()
    {
        // Cuando la timeline termina, empieza la música
        if (!startedMusic && director.state != PlayState.Playing)
        {
            backgroundAudio.Play();
            startedMusic = true;
        }
    }
}

