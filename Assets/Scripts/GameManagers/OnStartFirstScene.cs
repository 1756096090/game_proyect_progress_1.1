using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnStartFirstScene : MonoBehaviour
{
    [SerializeField] private GameObject countdownParent;
    [SerializeField] private TextMeshProUGUI countdownText;

    private void Start()
    {
        // InitializeScene();
    }

    private void InitializeScene()
    {
        // StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        // Pausar el tiempo del juego
        Time.timeScale = 0f;

        countdownParent.SetActive(true);

        int countdown = 1;
        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            // Utilizar WaitForSecondsRealtime para esperar en tiempo real
            yield return new WaitForSecondsRealtime(0.1f);
            countdown--;
        }

        countdownText.text = "Fight!";
        yield return new WaitForSecondsRealtime(0.1f);

        countdownParent.SetActive(false);

        // Reanudar el tiempo del juego
        Time.timeScale = 1f;
    }
}
