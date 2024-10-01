using UnityEngine;
using UnityEngine.UI;

public class ChartColumn : GridColumn
{
    public int id;

    public GameObject gridLinePrefab;

    public override void SetHeight(float height)
    {
        base.SetHeight(height);
        for (float pos = 0; pos <= gridController.height; pos += gridController.beatSpacing * ChartUIManager.Instance.subdivision)
        {
            GameObject gridLine = Instantiate(gridLinePrefab, new Vector3(rect.position.x, rect.position.y - gridController.height / 2 + pos), Quaternion.identity, transform);
            ((RectTransform)gridLine.transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, gridController.beatSpacing * ChartUIManager.Instance.subdivision);
            if (pos % gridController.beatSpacing != 0)
            {
                gridLine.GetComponent<Image>().color = Color.white * 0.4f;
            }
        }
    }

    public void OnClicked()
    {
        float chartYMin = rect.position.y - gridController.height / 2;
        float deltaY = gridController.SnapYPositionToBeatGrid(Input.mousePosition.y - chartYMin);

        if (gridController.notes.TrueForAll((NoteChartObject note) => { return note.info.lane != id || note.info.beat != deltaY / gridController.beatSpacing; }))
        {
            GameObject note = Instantiate(gridController.noteObjectPrefab, new Vector3(rect.position.x, deltaY + chartYMin), Quaternion.identity, transform);
            NoteChartObject script = note.GetComponent<NoteChartObject>();
            script.info = new NoteInfo(ChartUIManager.Instance.info);
            script.info.lane = id;
            script.info.beat = deltaY / gridController.beatSpacing;
            gridController.AddNote(note);
        }
        else
        {
            Debug.Log("Ha get fucked");
        }
    }
}
