using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    public int lane;
    public float beat;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, lane * -45);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector2(Mathf.Sin(lane * Mathf.PI / 4) * ((beat - Conductor.Instance.songPosition) * 1 + Spinner.hitLineOffset),
                                         Mathf.Cos(lane * Mathf.PI / 4) * ((beat - Conductor.Instance.songPosition) * 1 + Spinner.hitLineOffset));

        
        if(Mathf.Abs(Conductor.Instance.songPosition - beat) * Conductor.Instance.song.crotchet < 0.15f && HitCondition())
        {
            Debug.Log("Hit!");
            Destroy(gameObject);
        }

        if((Conductor.Instance.songPosition - beat) * Conductor.Instance.song.crotchet > 0.15f)
        {
            Debug.Log("Miss :(");
            Destroy(gameObject);
        }
    }

    protected virtual bool HitCondition()
    {
        return Input.GetButtonDown("Hit") && Spinner.Instance.selectedLane == lane;
    }
}
