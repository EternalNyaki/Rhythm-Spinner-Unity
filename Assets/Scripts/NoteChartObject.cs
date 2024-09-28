using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class NoteChartObject : MonoBehaviour
{
    public static NoteChartObject selectedNote;

    public NoteInfo info;

    public Image image;

    private void Start()
    {
        if (selectedNote != null) selectedNote.image.color = Color.white;
        selectedNote = this;
        selectedNote.image.color = Color.white * 0.75f;
        ChartUIManager.Instance.SetNoteFieldValues(info);
    }

    public void OnClicked()
    {
        if (selectedNote != this)
        {
            if (selectedNote != null) selectedNote.image.color = Color.white;
            selectedNote = this;
            selectedNote.image.color = Color.white * 0.75f;
            ChartUIManager.Instance.SetNoteFieldValues(info);
        }
    }
}