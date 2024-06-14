using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;

public class StartGame : MonoBehaviour
{
    [SerializeField] private GameObject countdownDisplay;
    [SerializeField] private TextMeshProUGUI countdownText;

    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //Time.timeScale = 1f;
    }

    public void Exit()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
