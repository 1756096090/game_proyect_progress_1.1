using Assets.Scripts.Stats;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private GameObject endMenuUI;
    [SerializeField] private FirstPlayerController player1;
    [SerializeField] private SecondPlayerController player2;

    private void Update()
    {
        if (player1.GetComponent<PlayerStats>().Health <= 0 || player2.GetComponent<PlayerStats>().Health <= 0)
        {
            Time.timeScale = 0f;
            endMenuUI.SetActive(true);
            return;
        }
    }

    public void Restart()
    {
        UnityEngine.SceneManagement.Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
