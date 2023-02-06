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



    // Start is called before the first frame update
    void Start()
    {
        string[] ports = SerialPort.GetPortNames();

        PopulateDropdown(Ddplayer1, ports);
        PopulateDropdown(Ddplayer2, ports);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        
        foreach(var option in ports)
        {
            options.Add(option);
        }
        dropD.AddOptions(options);
    }


}
