using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeatTest : MonoBehaviour
{
    public SongInfo testSong;

    public Color color1;
    public Color color2;

    private int prevBeat;

    private SpriteRenderer spriteRenderer;
    private AudioSource click;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        click = GetComponent<AudioSource>();

        if (Conductor.Instance == null)
        {
            Debug.LogError("Scene has no Conductor.");
            Application.Quit();
        }

        Conductor.Instance.SetSong(testSong);
        StartCoroutine(delayedStart(2.5f));
    }

    // Update is called once per frame
    void Update()
    {
        if(Conductor.Instance.isPlaying() && Conductor.Instance.songPosition > prevBeat + 1)
        {
            //click.Play();
            StartCoroutine(Pulse());

            prevBeat += 1;
        }
    }

    private IEnumerator delayedStart(float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(Conductor.Instance.Play());
    }

    private IEnumerator Pulse()
    {
        float startTime = Time.time;
        while(Time.time < startTime + 0.1f)
        {
            float l = (Time.time - startTime) / 0.1f;
            transform.localScale = Vector3.Lerp(new Vector3(6, 6, 1), new Vector3(5, 5, 1), l);
            spriteRenderer.color = Color.Lerp(color2, color1, l);
            yield return null;
        }
    }
}
