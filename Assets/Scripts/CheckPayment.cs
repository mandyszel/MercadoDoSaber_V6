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
    public GameObject alertPanel; // Imagem ou painel da mini tela
    public Button alertRetryButton; // Botão dentro da mini tela

    void Start()
    {
        verifyButton.onClick.AddListener(OnVerifyPayment);
        verifyButton.interactable = false;

        if (alertPanel != null)
            alertPanel.SetActive(false); // Esconde o alerta de erro

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
                // Limpar a fase ao acertar o pagamento
                dragDropFood.ResetSlots(); // Limpa os slots de alimentos
                DragDropMoney.totalMoneyValue = 0f; // Reseta o valor do dinheiro
                Debug.Log("Fase limpa!");
            }
            else
            {
                StarSystem.errors++; // Conta o erro
                ShowAlert(); // Mostra a mini tela de alerta
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
