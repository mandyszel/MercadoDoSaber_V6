using UnityEngine;
using UnityEngine.SceneManagement;

public class StarSystem : MonoBehaviour
{
    public static int errors = 0; // Conta os erros do jogador
    private int earnedStars = 0;  // Número de estrelas ganhas

    // Chama quando o jogador acerta o pagamento
    public void VerifyPayment()
    {
        if (DragDropMoney.totalMoneyValue == DragDropFood.totalValue)
        {
            CalculateStars();
            PlayerPrefs.SetInt("EarnedStars", earnedStars);
            PlayerPrefs.SetString("LastPlayedLevel", SceneManager.GetActiveScene().name);

            if (earnedStars >= 2)
            {
                PlayerPrefs.SetInt("CanProceed", 1);
                SceneManager.LoadScene("FaseConcluida");
            }
            else
            {
                PlayerPrefs.SetInt("CanProceed", 0);
                SceneManager.LoadScene("FaseFalha");
            }
        }
        else
        {
            errors++;
            PlayerPrefs.SetString("LastPlayedLevel", SceneManager.GetActiveScene().name);
            PlayerPrefs.SetInt("EarnedStars", 0);
            PlayerPrefs.SetInt("CanProceed", 0);
            SceneManager.LoadScene("FaseFalha");
        }
    }

    private void CalculateStars()
    {
        if (errors <= 1)
            earnedStars = 3;
        else if (errors <= 2)
            earnedStars = 2;
        else if (errors <= 3)
            earnedStars = 1;
        else
            earnedStars = 0;
    }
    public int GetCurrentStars()
{
    return earnedStars;
}

}
