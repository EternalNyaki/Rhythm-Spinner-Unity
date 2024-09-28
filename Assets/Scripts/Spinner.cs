using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    public static Spinner Instance;

    public static float hitLineOffset = 1.0f;

    public int selectedLane = -1;
    public int prevLane = -1;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (Instance != null)
        {
            Debug.LogError("Cannot have multiple Spinner objects in one scene.");
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            if(Input.GetAxisRaw("Vertical") > 0)
            {
                selectedLane = 1;
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                selectedLane = 2;
            }
            else
            {
                selectedLane = 3;
            }
        }
        else if(Input.GetAxisRaw("Horizontal") == 0)
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                selectedLane = 0;
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                selectedLane = -2;
            }
            else
            {
                selectedLane = 4;
            }
        }
        else
        {
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                selectedLane = 7;
            }
            else if (Input.GetAxisRaw("Vertical") == 0)
            {
                selectedLane = 6;
            }
            else
            {
                selectedLane = 5;
            }
        }

        animator.SetFloat("x", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("y", Input.GetAxisRaw("Vertical"));
    }

    private void LateUpdate()
    {
        prevLane = selectedLane;
    }

    public bool LaneShifted()
    {
        if((selectedLane == 7 && prevLane == 0) || (selectedLane == 0 && prevLane == 7))
        {
            return true;
        }
        else
        {
            return Mathf.Abs(selectedLane - prevLane) == 1;
        }
    }
}
