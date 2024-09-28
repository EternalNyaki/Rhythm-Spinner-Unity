using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    public static Conductor Instance { get; private set; }

    public float songPosition { get; private set; }
    private bool playing = false;

    public AudioClip countInAudio;

    public SongInfo song { get; private set; }

    private AudioSource audioPlayer;

    private float dspStartTime;

    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();

        if(Instance != null)
        {
            Debug.LogError("Cannot have multiple Conductor objects in one scene.");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(playing)
        {
            songPosition = (float)(AudioSettings.dspTime - dspStartTime - song.offset) / song.crotchet;
        }
    }

    public bool isPlaying()
    {
        return playing;
    }

    public void SetSong(SongInfo song)
    {
        if(!audioPlayer.isPlaying)
        {
            this.song = song;
            audioPlayer.clip = song.audio;
        }
    }

    public IEnumerator Play()
    {
        Debug.Log("Playing " + song.title + " - " + song.artist);
        if(song.crotchet * 4 > song.offset)
        {
            StartCoroutine(CountIn());
            yield return new WaitForSeconds(song.crotchet * 4 - song.offset);
            audioPlayer.Play();
        }
        else
        {
            audioPlayer.Play();
            yield return new WaitForSeconds(song.offset - song.crotchet * 4);
            StartCoroutine(CountIn());
        }
        dspStartTime = (float)AudioSettings.dspTime;
        playing = true;
    }

    private IEnumerator CountIn()
    {
        for (int i = 0; i < 3; i++)
        {
            audioPlayer.PlayOneShot(countInAudio);
            yield return new WaitForSeconds(song.crotchet);
        }
        audioPlayer.PlayOneShot(countInAudio);
    }
}
