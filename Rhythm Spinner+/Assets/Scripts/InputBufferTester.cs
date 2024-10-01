using UnityEngine;
using UnityEngine.UI;

public class InputBufferTester : MonoBehaviour
{
    public GameObject testerImagePrefab;

    private bool paused = false;

    private Image[,] testerImages;

    private GridLayoutGroup layoutGroup;

    // Start is called before the first frame update
    void Start()
    {
        if (InputBuffer.Instance == null)
        {
            Debug.LogError("Scene has no Input Manager.");
            Application.Quit();
        }

        layoutGroup = GetComponent<GridLayoutGroup>();

        testerImages = new Image[8, InputBuffer.Instance.GetBufferSize()];

        for (int i = 0; i < testerImages.GetLength(0); i++)
        {
            for (int j = 0; j < testerImages.GetLength(1); j++)
            {
                testerImages[i, j] = Instantiate(testerImagePrefab, layoutGroup.transform).GetComponent<Image>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!paused)
        {
            for (int i = 0; i < testerImages.GetLength(0); i++)
            {
                for (int j = 0; j < testerImages.GetLength(1); j++)
                {
                    testerImages[j, i].enabled = InputBuffer.Instance.IsInputOnFrame(j, (InputBuffer.InputFlag)i);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
        }
    }
}
