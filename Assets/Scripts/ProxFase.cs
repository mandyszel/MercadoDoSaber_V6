using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class VictoryScreen : MonoBehaviour
{
    public Image[] stars;
    public Sprite starFilled;
    public Sprite starEmpty;
    public TextMeshProUGUI resultText;

    void Start()
    {
        int earnedStars = PlayerPrefs.GetInt("EarnedStars", 0);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].sprite = i < earnedStars ? starFilled : starEmpty;
        }
    }

    public void NextLevel()
    {
        SceneManager.LoadScene("NomeDaProximaFase"); // Troque pelo nome correto
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene("NomeDaFaseAtual"); // Troque pelo nome correto
    }
}
