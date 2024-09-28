using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeNote : Note
{
    public int direction;

    protected override bool HitCondition()
    {
        return Spinner.Instance.LaneShifted() && Spinner.Instance.selectedLane == lane + Mathf.Sign(direction);
    }
}
