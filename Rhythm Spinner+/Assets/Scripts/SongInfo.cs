using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SongInfo
{
    public int bpm;
    public float crotchet
    {
        get { return (float)60 / bpm; }
    }
    public float offset;
    public float countIn;
    public int beatsPerBar;
    public float duration;
    public AudioClip audio;
    public NoteInfo[] chartInfo;
    public string title, artist;
    public Sprite coverImage;

    public SongInfo(int bpm, float offset, float countIn, int beatsPerBar, float duration, AudioClip audio = null, NoteInfo[] chartInfo = null, string title = "", string artist = "", Sprite coverImage = null)
    {
        this.bpm = bpm;
        this.offset = offset;
        this.countIn = countIn;
        this.beatsPerBar = beatsPerBar;
        this.duration = duration;
        this.audio = audio;
        this.chartInfo = chartInfo;
        this.title = title;
        this.artist = artist;
        this.coverImage = coverImage;
    }
}
