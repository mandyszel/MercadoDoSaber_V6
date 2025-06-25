using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            // Arredondar valores para 2 casas decimais
            float valorPago = Mathf.Round(DragDropMoney.totalMoneyValue * 100f) / 100f;
            float valorEsperado = Mathf.Round(DragDropFood.totalValue * 100f) / 100f;

            float diferenca = Mathf.Abs(valorPago - valorEsperado);
            Debug.Log($"Valor pago arredondado: {valorPago}, Valor esperado arredondado: {valorEsperado}, Diferença: {diferenca}");

            if (diferenca <= 0.01f) // tolerância de 1 centavo
            {
                starSystem.VerifyPayment();

                // Log após verificar pagamento
                int estrelasAposVerificacao = starSystem.GetCurrentStars();
                Debug.Log($"Estrelas após VerifyPayment(): {estrelasAposVerificacao}");

                // Usa o número da fase vindo do componente FaseInfo
                FaseInfo info = FindFirstObjectByType<FaseInfo>();
                if (info != null)
                {
                    int numeroFase = info.numeroFase;
                    PlayerPrefs.SetInt("FaseAtual", numeroFase);

                    // SALVA AS ESTRELAS CONQUISTADAS NA FASE
                    int estrelasGanhas = starSystem.GetCurrentStars();
                    string key = "Stars_Fase_" + (numeroFase - 1); // Começa do índice 0
                    int estrelasAnteriores = PlayerPrefs.GetInt(key, 0);

                    Debug.Log("Fase atual: " + numeroFase);
                    Debug.Log("Salvando na chave: Stars_Fase_" + (numeroFase - 1));
                    Debug.Log($"Estrelas ganhas para salvar: {estrelasGanhas}");

                    // Salva as estrelas da última tentativa para exibir no menu
                    PlayerPrefs.SetInt(key, estrelasGanhas);

                    // Agora trata desbloqueio da próxima fase, com base no MAIOR progresso
                    int maiorFaseDesbloqueada = PlayerPrefs.GetInt("MaiorFaseDesbloqueada", 1);
                    if (info.numeroFase == maiorFaseDesbloqueada && estrelasGanhas >= 2)
                    {
                        PlayerPrefs.SetInt("MaiorFaseDesbloqueada", maiorFaseDesbloqueada + 1);
                        Debug.Log($"Desbloqueando Fase {maiorFaseDesbloqueada + 1}");
                    }

                    PlayerPrefs.Save();
                }

                // Limpa a fase
                dragDropFood.ResetSlots();
                DragDropMoney.totalMoneyValue = 0f;
                Debug.Log("Fase limpa!");

                int estrelasFinais = starSystem.GetCurrentStars();
                Debug.Log($"Estrelas finais para decidir cena: {estrelasFinais}");

                if (estrelasFinais >= 2)
{
    if (info != null && info.numeroFase == 12)
    {
        SceneManager.LoadScene("Scenes/UltimaFaseTela");
    }
    else
    {
        SceneManager.LoadScene("Scenes/FaseConcluida");
    }
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
                Debug.LogWarning($"Pagamento incorreto! Valor pago: {valorPago}, Valor esperado: {valorEsperado}");
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
