using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [LabelText("CountDown Parent")]
    [SerializeField] private GameObject countdownParent;
    [LabelText("CountDown Text")]
    [SerializeField] private TextMeshProUGUI countdownText;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        countdownParent.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause();
            else StartCoroutine(Resume());
        }
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        isPaused = true;
        Time.timeScale = 0f;
    }

    private IEnumerator Resume()
    {

        if (countdownParent == null || countdownText == null)
        {
            yield break;
        }

        pauseMenuUI.SetActive(false);
        countdownParent.SetActive(true);

        int countdown = 3;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSecondsRealtime(1);
            countdown--;
        }

        countdownText.text = "0";
        yield return new WaitForSecondsRealtime(1);

        countdownParent.SetActive(false);
        isPaused = false;
        Time.timeScale = 1f;
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
