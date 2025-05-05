using UnityEngine;
using UnityEngine.SceneManagement;

public class StarSystem : MonoBehaviour
{
    public static int errors = 0; // Conta os erros do jogador
    private int earnedStars = 0;  // N�mero de estrelas ganhas

    // Chama quando o jogador clica para verificar o pagamento
    public void VerifyPayment()
    {
        // Verifica se o pagamento est� correto
        if (DragDropMoney.totalMoneyValue == DragDropFood.totalValue)
        {
            CalculateStars();  // Calcula as estrelas com base nos erros
            PlayerPrefs.SetInt("EarnedStars", earnedStars); // Salva as estrelas

            // Salva o nome da fase atual para refer�ncia futura
            PlayerPrefs.SetString("LastPlayedLevel", SceneManager.GetActiveScene().name);

            if (earnedStars >= 2)
            {
                PlayerPrefs.SetInt("CanProceed", 1); // Pode avan�ar
                SceneManager.LoadScene("FaseConcluida");
            }
            else
            {
                PlayerPrefs.SetInt("CanProceed", 0); // N�o pode avan�ar
                SceneManager.LoadScene("FaseFalha");
            }
        }
        else
        {
            errors++;
            // Salva o nome da fase atual para refer�ncia futura
            PlayerPrefs.SetString("LastPlayedLevel", SceneManager.GetActiveScene().name);

            PlayerPrefs.SetInt("EarnedStars", 0);
            PlayerPrefs.SetInt("CanProceed", 0);

            SceneManager.LoadScene("FaseFalha");
        }
    }

    // Calcula as estrelas com base no n�mero de erros
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
}
