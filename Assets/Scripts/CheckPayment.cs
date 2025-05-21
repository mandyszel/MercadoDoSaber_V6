using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class CheckPayment : MonoBehaviour
{
    public StarSystem starSystem;
    public Button verifyButton;
    public DragDropFood dragDropFood;
    public string paymentSlotTag = "PaymentSlot";

    [Header("Alerta de Erro")]
    public GameObject alertPanel;
    public Button alertRetryButton;

    void Start()
    {
        // Zera tentativas ao iniciar a fase
        StarSystem.errors = 0;

        verifyButton.onClick.AddListener(OnVerifyPayment);
        verifyButton.interactable = false;

        if (alertPanel != null)
            alertPanel.SetActive(false);

        if (alertRetryButton != null)
            alertRetryButton.onClick.AddListener(HideAlert);
    }

    void Update()
    {
        verifyButton.interactable = dragDropFood.AllSlotsFilled() && IsPaymentSlotNotEmpty();
    }

    bool IsPaymentSlotNotEmpty()
    {
        GameObject paymentSlot = GameObject.FindGameObjectWithTag(paymentSlotTag);
        return paymentSlot != null && paymentSlot.transform.childCount > 0;
    }

    void OnVerifyPayment()
    {
        if (dragDropFood.AllSlotsFilled() && IsPaymentSlotNotEmpty())
        {
            if (DragDropMoney.totalMoneyValue == DragDropFood.totalValue)
            {
                starSystem.VerifyPayment();

                // Pega o nome da cena atual e tenta extrair o número da fase
                string currentSceneName = SceneManager.GetActiveScene().name;
                Match match = Regex.Match(currentSceneName, @"Fase(\d+)");

                if (match.Success)
                {
                    int numeroFase = int.Parse(match.Groups[1].Value);
                    PlayerPrefs.SetInt("FaseAtual", numeroFase);

                    // SALVA AS ESTRELAS CONQUISTADAS NA FASE
                    int estrelasGanhas = starSystem.GetCurrentStars();
                    string key = "Stars_Fase_" + (numeroFase - 1); // Supondo que a fase 1 é índice 0
                    int estrelasAnteriores = PlayerPrefs.GetInt(key, 0);

                    if (estrelasGanhas > estrelasAnteriores)
                    {
                        PlayerPrefs.SetInt(key, estrelasGanhas);
                    }

                    PlayerPrefs.Save(); // Garante que tudo seja salvo
                }
                else
                {
                    Debug.LogWarning("Cena atual não segue o padrão 'FaseN': " + currentSceneName);
                }

                // Limpa a fase
                dragDropFood.ResetSlots();
                DragDropMoney.totalMoneyValue = 0f;
                Debug.Log("Fase limpa!");

                int estrelasFinais = starSystem.GetCurrentStars();

                if (estrelasFinais >= 2)
                {
                    SceneManager.LoadScene("Scenes/FaseConcluida");
                }
                else
                {
                    // Salva o nome da cena atual para poder repetir depois
                    string nomeFaseAtual = SceneManager.GetActiveScene().name;
                    PlayerPrefs.SetString("FaseParaRepetir", nomeFaseAtual);
                    PlayerPrefs.Save();

                    SceneManager.LoadScene("Scenes/FaseFalha");
                }

            }
            else
            {
                StarSystem.errors++;
                ShowAlert();
            }
        }
    }

    void ShowAlert()
    {
        if (alertPanel != null)
            alertPanel.SetActive(true);
    }

    public void HideAlert()
    {
        if (alertPanel != null)
            alertPanel.SetActive(false);
    }
}
