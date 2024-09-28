using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NoteType
{
    None,
    Tap,
    SwipeLeft,
    SwipeRight,
    HoldStart,
    HoldEnd,
    HoldVert,
    Spin,
    Event
}

[Serializable]
public class NoteInfo
{
    public NoteType type;
    public int lane;
    public float beat;
    public int intParam;
    public string stringParam;

    public NoteInfo()
    {
        this.type = 0;
        this.lane = 0;
        this.beat = 0;
        this.intParam = 0;
        this.stringParam = "";
    }

    public NoteInfo(NoteInfo noteInfo)
    {
        this.type = noteInfo.type;
        this.lane = noteInfo.lane;
        this.beat = noteInfo.beat;
        this.intParam = noteInfo.intParam;
        this.stringParam = noteInfo.stringParam;
    }

    public NoteInfo(NoteType type, int lane, float beat, int intParam = 0, string stringParam = "")
    {
        this.type = type;
        this.lane = lane;
        this.beat = beat;
        this.intParam = intParam;
        this.stringParam = stringParam;
    }

    public override string ToString()
    {
        return $"Type: {type}, Lane: {lane}, Beat: {beat}, Int Parameter: {intParam}, String Parameter: {stringParam}";
    }
}
