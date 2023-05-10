using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G04_Simon : MonoBehaviour
{
    enum Colors
    {
        BLUE,
        RED,
        YELLOW,
        GREEN
    }

    [SerializeField]
    private List<SimonButton> buttons;
    private List<Colors> displayPattern, guessPattern;

    // Start is called before the first frame update
    void Start()
    {
        buttons[0].Blue += () => CheckColor(Colors.BLUE);
        buttons[1].Red += () => CheckColor(Colors.RED);
        buttons[2].Yellow += () => CheckColor(Colors.YELLOW);
        buttons[3].Green += () => CheckColor(Colors.GREEN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CheckColor(Colors color)
    {
        Debug.Log(color);
    }
}
