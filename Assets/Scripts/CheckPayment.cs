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
            if (DragDropMoney.totalMoneyValue == DragDropFood.totalValue)
            {
                starSystem.VerifyPayment();

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
    Debug.Log("Salvando na chave: Stars_Fase_" + numeroFase);


                    if (estrelasGanhas > estrelasAnteriores)
                    {
                        PlayerPrefs.SetInt(key, estrelasGanhas);
                        PlayerPrefs.Save();
                    }
                }
                else
                {
                    Debug.LogWarning("Script FaseInfo não encontrado na cena!");
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
