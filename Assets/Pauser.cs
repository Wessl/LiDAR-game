using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pauser : MonoBehaviour
{
    public GameObject pausePanel;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // open pause panel
            GameObject.FindObjectOfType<MouseLook>().lookEnabled = pausePanel.activeSelf;
            pausePanel.SetActive(!pausePanel.activeSelf);
        }
    }

    public void ExitGame()
    {
        Debug.Log("clicked");
        Application.Quit();
    }
}
