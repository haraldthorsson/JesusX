using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO.Ports;

public class menu : MonoBehaviour
{

    public Dropdown Ddplayer1;
    public Dropdown Ddplayer2;

    public Slider slider;

    SerialPort serialPort = new SerialPort();



    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;

        string[] ports = SerialPort.GetPortNames();

        PopulateDropdown(Ddplayer1, ports);
        PopulateDropdown(Ddplayer2, ports);

        serialPort.BaudRate = 9600;
    }

    // Update is called once per frame
    void Update()
    {
        if (serialPort.IsOpen)
        {
            slider.value = float.Parse(serialPort.ReadLine());
        }
    }

    public void Play()
    {
        SceneManager.LoadScene("game");
    }

    public void Exit()
    {
        Application.Quit();
    }


    void PopulateDropdown(Dropdown dropD, string[] ports)
    {
        List<string> options = new List<string>();
        Debug.Log(options);

        foreach (var option in ports)
        {
            options.Add(option);
        }
        dropD.AddOptions(options);
    }

    public void ChangePort(Dropdown dropD)
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }

        serialPort.PortName = dropD.options[dropD.value].text;

        serialPort.Open();
    }

    public void P1Character(int id)
    {
        PlayerPrefs.SetInt("p1ID", id);
    }

    public void P2Character(int id)
    {
        PlayerPrefs.SetInt("p2ID", id);
    }


}