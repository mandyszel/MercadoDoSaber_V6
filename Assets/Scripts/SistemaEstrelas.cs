using UnityEngine;
using UnityEngine.SceneManagement;

public class StarSystem : MonoBehaviour
{
    public static int errors = 0;
    private int earnedStars = 0;

   public void VerifyPayment()
{
    float valorPago = Mathf.Round(DragDropMoney.totalMoneyValue * 100f) / 100f;
    float valorEsperado = Mathf.Round(DragDropFood.totalValue * 100f) / 100f;
    float diferenca = Mathf.Abs(valorPago - valorEsperado);

    if (diferenca <= 0.01f)
    {
        CalculateStars();
        PlayerPrefs.SetInt("EarnedStars", earnedStars);
        PlayerPrefs.SetString("LastPlayedLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("CanProceed", earnedStars >= 2 ? 1 : 0);

        Debug.Log($"[StarSystem] Pagamento correto. Estrelas calculadas: {earnedStars}");
    }
    else
    {
        errors++;
        earnedStars = 0;
        PlayerPrefs.SetInt("EarnedStars", 0);
        PlayerPrefs.SetString("LastPlayedLevel", SceneManager.GetActiveScene().name);
        PlayerPrefs.SetInt("CanProceed", 0);

        Debug.LogWarning($"[StarSystem] Pagamento errado. Erros: {errors}, Estrelas: {earnedStars}");
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
