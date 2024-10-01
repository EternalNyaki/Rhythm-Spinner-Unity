using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Class loosely taken from this video by Code Monkey: https://www.youtube.com/watch?v=waEsGu--9P8
public class GridController : MonoBehaviour
{
    private const int width = 8;
    public float height;

    public const float k_heightBuffer = 50;

    public int beats { get; private set; }
    public float beatSpacing = 200;
    public float subdivision = 1 / 2;

    public GameObject noteObjectPrefab;
    public GameObject labelObjectPrefab;
    public GameObject columnObjectPrefab;

    public List<NoteChartObject> notes;
    public List<GameObject> gridObjects { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        beats = (int)(height / beatSpacing);

        notes = new List<NoteChartObject>();
        gridObjects = new List<GameObject>();

        gridObjects.Add(Instantiate(labelObjectPrefab, transform));
        for (int i = 1; i < width + 1; i++)
        {
            gridObjects.Add(Instantiate(columnObjectPrefab, transform));

            ChartColumn c = gridObjects[i].GetComponent<ChartColumn>();
            c.id = i;
        }
        gridObjects.ForEach((GameObject gridObject) => { gridObject.GetComponent<GridColumn>().SetHeight(height + k_heightBuffer * 2); });


        StartCoroutine(LateStart());
    }

    private IEnumerator LateStart()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForEndOfFrame();

        transform.parent.parent.GetComponent<ScrollRect>().verticalScrollbar.value = 0;
    }

    public float SnapYPositionToBeatGrid(float yPos)
    {
        float output = yPos + subdivision / 2 * 100;
        output -= output % (subdivision * beatSpacing);
        output = Mathf.Clamp(output, 0, beats * beatSpacing);
        return output;
    }

    public void AddNote(GameObject noteObject)
    {
        NoteChartObject obj = noteObject.GetComponent<NoteChartObject>(); ;
        if (obj != null)
        {
            notes.Add(obj);
        }
    }

    public void AddNote(NoteChartObject noteObject)
    {
        notes.Add(noteObject);
    }

    public void RemoveNote(GameObject noteObject)
    {
        NoteChartObject obj = noteObject.GetComponent<NoteChartObject>(); ;
        if (obj != null)
        {
            notes.Remove(obj);
        }
    }

    public void RemoveNote(NoteChartObject noteObject)
    {
        notes.Remove(noteObject);
    }

    public void SetHeightWithBeats(int beats)
    {
        this.beats = beats;
        height = beats * beatSpacing;
        gridObjects.ForEach((GameObject gridObject) => { gridObject.GetComponent<GridColumn>().SetHeight(height + k_heightBuffer * 2); });
    }
}
