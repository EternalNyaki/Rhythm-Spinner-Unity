using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    /// <summary>
    /// Enum for flag positions of specific inputs within a byte.
    /// This is an index, not a bitmask.
    /// </summary>
    public enum InputFlag
    {
        Cancel = 0,
        Confirm = 1,
        Right = 4,
        Left = 5,
        Down = 6,
        Up = 7
    }

    //Singleton instance of the class
    private static InputBuffer _instance;
    //Public interface to the private instance
    public static InputBuffer Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<InputBuffer>();
                _instance.Initialize();
            }
            return _instance;
        }
    }

    //Size of the buffer (the number of previous frames it tracks)
    private const int k_bufferSize = 8;

    //Array of input flags for the previous bufferSize frames
    //Each byte (8 bits) represents a frame
    //Flags are arranged UDLR00AB
    private BitArray _bufferFlags;

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
        //Initialize buffer
        _bufferFlags = new BitArray(k_bufferSize * 8);
    }

    // Update is called once per frame
    void Update()
    {
        //Shift buffer
        _bufferFlags.LeftShift(8);

        //Populate new array with the inputs for the current frame
        BitArray input = new BitArray(_bufferFlags.Count);

        input[(int)InputFlag.Cancel] = Input.GetKey(KeyCode.X);
        input[(int)InputFlag.Confirm] = Input.GetKey(KeyCode.Z);
        input[(int)InputFlag.Right] = Input.GetKey(KeyCode.RightArrow);
        input[(int)InputFlag.Left] = Input.GetKey(KeyCode.LeftArrow);
        input[(int)InputFlag.Down] = Input.GetKey(KeyCode.DownArrow);
        input[(int)InputFlag.Up] = Input.GetKey(KeyCode.UpArrow);

        //Add the current frame to the buffer
        _bufferFlags.Or(input);
    }

    /// <summary>
    /// Returns true if the given InputFlag was recorded on the given frame.
    /// Frame should be given as a number of frames before the current frame (0 for the current frame).
    /// </summary>
    public bool IsInputOnFrame(int frame, InputFlag input)
    {
        return _bufferFlags[frame * 8 + (int)input];
    }

    /// <summary>
    /// Returns true if the given InputFlags were recorded on the given frame.
    /// Frame should be given as a number of frames before the current frame (0 for the current frame).
    /// </summary>
    public bool IsCombinationOnFrame(int frame, InputFlag[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            if (!_bufferFlags[i * 8 + (int)inputs[i]])
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns all the inputs for a given frame as a BitArray.
    /// Frame should be given as a number of frames before the current frame (0 for the current frame).
    /// </summary>
    public BitArray GetFrame(int frame)
    {
        BitArray output = new BitArray(8);
        for (int i = 0; i < 8; i++)
        {
            output[i] = _bufferFlags[frame * 8 + i];
        }
        return output;
    }

    /// <summary>
    /// Returns true if the given InputFlag was recorded anywhere in the buffer.
    /// </summary>
    public bool IsInputInBuffer(InputFlag input)
    {
        for (int i = 0; i < k_bufferSize; i++)
        {
            if (_bufferFlags[i * 8 + (int)input])
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Returns true if the given InputFlags were recorded indepentently anywhere in the buffer (does not need to be on the same frame).
    /// </summary>
    public bool AreInputsInBuffer(InputFlag[] inputs)
    {
        foreach (InputFlag input in inputs)
        {
            if (!IsInputInBuffer(input))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// Returns true if the given InputFlags were recorded on the same frame anywhere in the buffer.
    /// </summary>
    public bool IsCombinationInBuffer(InputFlag[] inputs)
    {
        for (int i = 0; i < k_bufferSize; i++)
        {
            if (IsCombinationOnFrame(i, inputs))
            {
                return true;
            }
        }

        return false;
    }

    public BitArray GetFullBuffer()
    {
        return _bufferFlags;
    }

    public int GetBufferSize()
    {
        return k_bufferSize;
    }
}
