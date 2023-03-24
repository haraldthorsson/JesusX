using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class controllerinterpreter : MonoBehaviour
{

    public string port = "COM5";

    public static SerialPort serialport;

    public Text text;
    public Text AttackText;

    public Color color;
    public Color activatedColor;

    public Image[] panels;
    public bool[] button = { false, false, false, false, false, false, false, false, false };

    public float timer;
    float time;



    List<int[]> attacks = new List<int[]>
    {
        new int[] { 4, 5, 6 },       //[0]PUNCH
        new int[] { 1, 2, 3 },       //[1]PUCHDOWN
        new int[] { 7, 8, 9 },       //[2]PUNCHUP
        new int[] { 1, 5, 3, 5, 7 }, //[3]PUNCHUP

    };


    int Touch = 0;
    int lastTouch = 0;
    bool valueChanged = false;



    List<int> history = new List<int>();


    // Start is called before the first frame update
    void Start()
    {
        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);

        serialport = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
        serialport.PortName = port;
        serialport.BaudRate = 9600;

        serialport.Open();


    }

    // Update is called once per frame
    void Update()
    {

        string[] input = serialport.ReadLine().Split(',');
        serialport.ReadTimeout = 25;




        Touch = int.Parse(input[0]);


        if(Touch != lastTouch)
        {
            valueChanged = true;
        }
        lastTouch = Touch;

        //only once (buttonDown)
        //if (Input.GetKeyDown(KeyCode.Q)) button[0] = true; else button[0] = false;
        //if (Input.GetKeyDown(KeyCode.W)) button[1] = true; else button[1] = false;
        //if (Input.GetKeyDown(KeyCode.E)) button[2] = true; else button[2] = false;
        //if (Input.GetKeyDown(KeyCode.A)) button[3] = true; else button[3] = false;
        //if (Input.GetKeyDown(KeyCode.S)) button[4] = true; else button[4] = false;
        //if (Input.GetKeyDown(KeyCode.D)) button[5] = true; else button[5] = false;
        //if (Input.GetKeyDown(KeyCode.Z)) button[6] = true; else button[6] = false;
        //if (Input.GetKeyDown(KeyCode.X)) button[7] = true; else button[7] = false;
        //if (Input.GetKeyDown(KeyCode.C)) button[8] = true; else button[8] = false;

        if (Touch == 1) button[0] = true; else button[0] = false;
        if (Touch == 2) button[1] = true; else button[1] = false;
        if (Touch == 3) button[2] = true; else button[2] = false;
        if (Touch == 4) button[3] = true; else button[3] = false;
        if (Touch == 5) button[4] = true; else button[4] = false;
        if (Touch == 6) button[5] = true; else button[5] = false;
        if (Touch == 7) button[6] = true; else button[6] = false;
        if (Touch == 8) button[7] = true; else button[7] = false;
        if (Touch == 9) button[8] = true; else button[8] = false;

        //
        //if (button[0]) { panels[0].color = activatedColor; AddHistory(1); } else panels[0].color = color;
        //if (button[1]) { panels[1].color = activatedColor; AddHistory(2); } else panels[1].color = color;
        //if (button[2]) { panels[2].color = activatedColor; AddHistory(3); } else panels[2].color = color;
        //if (button[3]) { panels[3].color = activatedColor; AddHistory(4); } else panels[3].color = color;
        //if (button[4]) { panels[4].color = activatedColor; AddHistory(5); } else panels[4].color = color;
        //if (button[5]) { panels[5].color = activatedColor; AddHistory(6); } else panels[5].color = color;
        //if (button[6]) { panels[6].color = activatedColor; AddHistory(7); } else panels[6].color = color;
        //if (button[7]) { panels[7].color = activatedColor; AddHistory(8); } else panels[7].color = color;
        //if (button[8]) { panels[8].color = activatedColor; AddHistory(9); } else panels[8].color = color;


        if (button[0] && valueChanged) { panels[0].color = activatedColor; AddHistory(1); valueChanged = false; } else panels[0].color = color;
        if (button[1] && valueChanged) { panels[1].color = activatedColor; AddHistory(2); valueChanged = false; } else panels[1].color = color;
        if (button[2] && valueChanged) { panels[2].color = activatedColor; AddHistory(3); valueChanged = false; } else panels[2].color = color;
        if (button[3] && valueChanged) { panels[3].color = activatedColor; AddHistory(4); valueChanged = false; } else panels[3].color = color;
        if (button[4] && valueChanged) { panels[4].color = activatedColor; AddHistory(5); valueChanged = false; } else panels[4].color = color;
        if (button[5] && valueChanged) { panels[5].color = activatedColor; AddHistory(6); valueChanged = false; } else panels[5].color = color;
        if (button[6] && valueChanged) { panels[6].color = activatedColor; AddHistory(7); valueChanged = false; } else panels[6].color = color;
        if (button[7] && valueChanged) { panels[7].color = activatedColor; AddHistory(8); valueChanged = false; } else panels[7].color = color;
        if (button[8] && valueChanged) { panels[8].color = activatedColor; AddHistory(9); valueChanged = false; } else panels[8].color = color;



        for (int i = 0; i < attacks.Count; i++)
        {
            int attack = Check(attacks[i], i);
            if (attack >= 0)
            {
                Debug.Log(attack);
                AttackText.text = attack.ToString();

            }
        }
           
           
           
    }      

    int Check(int[] attack, int attackId)
    {
        //Debug.Log("check");

        bool pattern = false;


        Array.Reverse(attack);

        for (int i = 0; i < attack.Length; i++)
        {
            if(!(history[4-i] == attack[i]))
            {
                return -1;
            }
        }

        Clear();
        return attackId;
    }

    void AddHistory(int number)
    {
        history.RemoveAt(0);
        history.Add(number);
        


        text.text = (history[0] + " " + history[1] + " " + history[2] + " " + history[3] + " " + history[4]);
        
    }

    void Clear()
    {
        history.Clear();
        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);

        text.text = (history[0] + " " + history[1] + " " + history[2] + " " + history[3] + " " + history[4]);
    }


    void OnApplicationQuit()
    {
        if (serialport != null)
            serialport.Close();
    }
}

