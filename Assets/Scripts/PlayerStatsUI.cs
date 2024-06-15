using Assets.Scripts.Stats;
using TMPro;
using UnityEngine;
 
public class PlayerStatsUI : MonoBehaviour
{
    [SerializeField] private PlayerStats statsPlayer1;
    [SerializeField] private PlayerStats statsPlayer2;
    [SerializeField] private TextMeshProUGUI textPlayer1;
    [SerializeField] private TextMeshProUGUI textPlayer2;
 
 
    void Update()
    {
        textPlayer1.text = $"HP: {statsPlayer1.Health}\nATK: {statsPlayer1.Attack}\nDEF: {statsPlayer1.Defense}";
        textPlayer2.text = $"HP: {statsPlayer2.Health}\nATK: {statsPlayer2.Attack}\nDEF: {statsPlayer2.Defense}";
    }
}