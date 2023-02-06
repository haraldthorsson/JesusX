using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputVisual : MonoBehaviour
{
    public Image[] TouchPad;
    public Image[] dPad;

    public Color nonPressedColor;
    public Color PressedColor;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // NUMPAD
        if (Input.GetKey(KeyCode.Keypad7)) TouchPad[0].color = PressedColor; else TouchPad[0].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad8)) TouchPad[1].color = PressedColor; else TouchPad[1].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad9)) TouchPad[2].color = PressedColor; else TouchPad[2].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad4)) TouchPad[3].color = PressedColor; else TouchPad[3].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad5)) TouchPad[4].color = PressedColor; else TouchPad[4].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad6)) TouchPad[5].color = PressedColor; else TouchPad[5].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad1)) TouchPad[6].color = PressedColor; else TouchPad[6].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad2)) TouchPad[7].color = PressedColor; else TouchPad[7].color = nonPressedColor;
        if (Input.GetKey(KeyCode.Keypad3)) TouchPad[8].color = PressedColor; else TouchPad[8].color = nonPressedColor;
        
        // DPAD
        if (Input.GetKey(KeyCode.UpArrow)    || Input.GetKey(KeyCode.W)) dPad[0].color = PressedColor; else dPad[0].color = nonPressedColor;
        if (Input.GetKey(KeyCode.LeftArrow)  || Input.GetKey(KeyCode.A)) dPad[1].color = PressedColor; else dPad[1].color = nonPressedColor;
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) dPad[2].color = PressedColor; else dPad[2].color = nonPressedColor;
        if (Input.GetKey(KeyCode.DownArrow)  || Input.GetKey(KeyCode.S)) dPad[3].color = PressedColor; else dPad[3].color = nonPressedColor;
    }
}
