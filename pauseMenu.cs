using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenu : MonoBehaviour
{

    bool paused = false;
    public GameObject pauseObj;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Resume();
            }
            else if (!paused)
            {
                Pause();
            }
        }
    }


    public void Resume()
    {
        Time.timeScale = 1;
        pauseObj.SetActive(false);
        paused = false;
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseObj.SetActive(true);
        paused = true;
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit()
    {
        SceneManager.LoadScene("menu");
    }
}
