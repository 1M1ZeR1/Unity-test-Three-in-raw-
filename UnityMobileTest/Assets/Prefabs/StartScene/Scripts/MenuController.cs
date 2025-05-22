using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject textTaker;
    private TextMeshProUGUI textMeshPro;

    [SerializeField] private GameObject scoreTextTaker;
    private TextMeshProUGUI resultTextMeshPro;

    private void Start()
    {
        textTaker.TryGetComponent(out textMeshPro);
        scoreTextTaker.TryGetComponent(out resultTextMeshPro);

        string gameResult = PlayerPrefs.GetString("GameResult","");
        int score = PlayerPrefs.GetInt("Score", 0);

        if (gameResult!= null && gameResult == "Win")
        {
            textMeshPro.text = $"Congratulations\nYou win!";

            resultTextMeshPro.text = $"Points scored:{score}";
        }
        else if(gameResult != null && gameResult == "Lose")
        {
            textMeshPro.text = $"Fail\nTry again!";

            resultTextMeshPro.text = $"Points scored:{score}";
        }
        else
        {
            textMeshPro.text = $"Try to clear all field!";
        }
    }
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("GameResult", "");
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.Save();
    }
    public void StartGame() { SceneManager.LoadScene("GameScene"); }
}
