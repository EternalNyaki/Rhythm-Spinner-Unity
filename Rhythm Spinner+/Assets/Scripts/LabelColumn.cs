using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LabelColumn : GridColumn
{
    public GameObject labelPrefab;

    public override void LoadVisual()
    {
        base.LoadVisual();

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int beat = 0; beat < gridController.beats; beat++)
        {
            GameObject label = Instantiate(labelPrefab, new Vector3(rect.position.x, rect.position.y - gridController.height / 2 + beat * gridController.beatSpacing), Quaternion.identity, transform);
            TMP_Text text = label.GetComponent<TMP_Text>();
            text.text = $"{beat / ChartUIManager.Instance.songInfo.beatsPerBar + 1}.{(beat % ChartUIManager.Instance.songInfo.beatsPerBar) + 1}";
        }
    }
}
