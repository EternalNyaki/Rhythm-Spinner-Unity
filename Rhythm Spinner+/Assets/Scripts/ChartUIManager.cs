using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChartUIManager : MonoBehaviour
{
    private static ChartUIManager _instance;
    public static ChartUIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<ChartUIManager>();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    public SongInfo songInfo;

    private int noteSubdivision = 4;
    private int gridSubdivision = 16;
    public float subdivision
    {
        get { return noteSubdivision / gridSubdivision; }
    }

    public NoteInfo info;

    public TMP_InputField timeSignatureBeatField;
    public TMP_InputField timeSignatureSubdivisionField;
    public TMP_InputField gridSubdivisionField;
    public TMP_InputField bpmField;
    public TMP_InputField offsetField;
    public TMP_InputField songLengthMinutesField;
    public TMP_InputField songLengthSecondsField;

    public TMP_Dropdown noteTypeField;
    public TMP_InputField laneField;
    public TMP_InputField beatField;
    public TMP_InputField miscIntField;
    public TMP_InputField miscStringField;

    public GridController gridController;

    // Start is called before the first frame update
    void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            Initialize();
        }
        else if (_instance != this)
        {
            Debug.LogError($"Cannot have multiple {this.GetType().Name} objects in one scene.");
            Destroy(this);
        }
    }

    private void Initialize()
    {
        //noteTypeField.ClearOptions();
        //noteTypeField.AddOptions(new List<string>(Enum.GetNames(typeof(NoteType))));

        songInfo = new SongInfo(120, 0f, 0f, 4, 150f);

        SetSongFieldValues(songInfo);
    }

    // Update is called once per frame
    void Update()
    {
        if (NoteChartObject.selectedNote != null)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !EventSystem.current.IsPointerOverGameObject())
            {
                NoteChartObject.selectedNote.image.color = Color.white;
                NoteChartObject.selectedNote = null;

                SetNoteFieldValues(new NoteInfo());
            }

            if (Input.GetKeyDown(KeyCode.Delete))
            {
                gridController.RemoveNote(NoteChartObject.selectedNote);
                Destroy(NoteChartObject.selectedNote.gameObject);

                SetNoteFieldValues(new NoteInfo());
            }
        }
    }

    public void SetSongFieldValues(SongInfo songInfo)
    {
        timeSignatureBeatField.text = songInfo.beatsPerBar.ToString();
        timeSignatureSubdivisionField.text = noteSubdivision.ToString();
        gridSubdivisionField.text = gridSubdivision.ToString();
        bpmField.text = songInfo.bpm.ToString();
        offsetField.text = songInfo.offset.ToString();
        songLengthMinutesField.text = ((int)(songInfo.length / 60)).ToString();
        songLengthSecondsField.text = ((int)(songInfo.length % 60)).ToString();
    }

    public void SetNoteFieldValues(NoteInfo noteInfo)
    {
        //noteTypeField.value = (int)noteInfo.type;
        laneField.text = noteInfo.lane.ToString();
        beatField.text = noteInfo.beat.ToString();
        miscIntField.text = noteInfo.intParam.ToString();
        miscStringField.text = noteInfo.stringParam;

        info = new NoteInfo(noteInfo);
    }

    public void OnNoteTypeChanged(int value)
    {
        if (NoteChartObject.selectedNote != null)
        {
            NoteChartObject.selectedNote.info.type = (NoteType)value;
        }
        else
        {
            info.type = (NoteType)value;
        }
    }

    public void OnLaneChanged(string value)
    {
        if (NoteChartObject.selectedNote != null && int.TryParse(value, out NoteChartObject.selectedNote.info.lane))
        {
            NoteChartObject.selectedNote.transform.SetParent(gridController.gridObjects[NoteChartObject.selectedNote.info.lane].transform, false);
        }
    }

    public void OnBeatChanged(string value)
    {
        if (NoteChartObject.selectedNote != null && float.TryParse(value, out NoteChartObject.selectedNote.info.beat))
        {
            RectTransform rect = gridController.gridObjects[NoteChartObject.selectedNote.info.lane].GetComponent<ChartColumn>().rect;
            NoteChartObject.selectedNote.transform.position = new Vector3(NoteChartObject.selectedNote.transform.position.x, NoteChartObject.selectedNote.info.beat * gridController.beatSpacing + (rect.position.y - gridController.height / 2));
        }
    }

    public void OnMiscIntChanged(string value)
    {
        if (NoteChartObject.selectedNote != null)
        {
            int.TryParse(value, out NoteChartObject.selectedNote.info.intParam);
        }
        else
        {
            info.intParam = int.TryParse(value, out info.intParam) ? info.intParam : 0;
        }
    }

    public void OnMiscStringChanged(string value)
    {
        if (NoteChartObject.selectedNote != null)
        {
            NoteChartObject.selectedNote.info.stringParam = value;
        }
        else
        {
            info.stringParam = miscStringField.text;
        }
    }

    public void OnTimeSignatureBeatsChanged(string value)
    {
        int.TryParse(value, out songInfo.beatsPerBar);
    }

    public void OnTimeSignatureSubdivisionChanged(string value)
    {
        int.TryParse(value, out noteSubdivision);
    }

    public void OnGridSubdivisionChanged(string value)
    {
        int.TryParse(value, out gridSubdivision);
    }

    public void OnBPMChanged(string value)
    {
        int.TryParse(value, out songInfo.bpm);
    }

    public void OnOffsetChanged(string value)
    {
        float.TryParse(value, out songInfo.offset);
    }

    public void OnSongLengthMinutesChanged(string value)
    {
        int mins;
        int.TryParse(value, out mins);
        songInfo.length = mins * 60 + songInfo.length % 60;
    }

    public void OnSongLengthSecondsChanged(string value)
    {
        int secs;
        int.TryParse(value, out secs);
        songInfo.length = songInfo.length - songInfo.length % 60 + secs;
    }
}
