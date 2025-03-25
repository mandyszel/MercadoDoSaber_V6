using UnityEngine;
using UnityEngine.SceneManagement;

public class StarSystem : MonoBehaviour
{
    public static int errors = 0; // Conta os erros do jogador
    private int earnedStars = 0;  // Número de estrelas ganhas

    // Chama quando o jogador clica para verificar o pagamento
    public void VerifyPayment()
    {
        // Verifica se o pagamento está correto
        if (DragDropMoney.totalMoneyValue == DragDropFood.totalValue)
        {
            CalculateStars();  // Calcula as estrelas com base nos erros
            PlayerPrefs.SetInt("EarnedStars", earnedStars); // Salva as estrelas
            PlayerPrefs.SetInt("CanProceed", earnedStars >= 2 ? 1 : 0); // 1 = pode avançar, 0 = não pode
            SceneManager.LoadScene("FaseConcluida"); // Vai para a cena de vitória
        }
        else
        {
            errors++; // Aumenta o número de erros
            PlayerPrefs.SetInt("EarnedStars", 0); // Zera as estrelas se errar
            SceneManager.LoadScene("FaseFalha"); // Vai para a cena de falha
        }
    }

    // Calcula as estrelas com base no número de erros
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
