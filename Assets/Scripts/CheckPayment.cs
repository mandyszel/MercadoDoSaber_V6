//s� deixa usar o botao de verificar se os 3 slots tiverem preenchidos e se tiver pelo menos uma nota/moeda no payment slot


using UnityEngine;
using UnityEngine.UI;  // Para referenciar o bot�o

public class CheckPayment : MonoBehaviour
{
    public StarSystem starSystem; // Refer�ncia ao StarSystem
    public Button verifyButton; // Refer�ncia ao bot�o de verifica��o
    public DragDropFood dragDropFood; // Refer�ncia ao script DragDropFood
    public string paymentSlotTag = "PaymentSlot"; // Tag do paymentSlot

    void Start()
    {
        // Adiciona o evento OnClick para o bot�o
        verifyButton.onClick.AddListener(OnVerifyPayment);
        
        // Desativa o bot�o inicialmente caso os slots n�o estejam preenchidos
        verifyButton.interactable = false;
    }

    void Update()
    {
        // Verifica se todos os 3 slots est�o preenchidos e se o paymentSlot tem valor
        if (dragDropFood.AllSlotsFilled() && IsPaymentSlotNotEmpty())
        {
            // Se os slots estiverem preenchidos e o paymentSlot tiver valor, o bot�o pode ser clicado
            verifyButton.interactable = true;
        }
        else
        {
            // Caso contr�rio, o bot�o fica desativado
            verifyButton.interactable = false;
        }
    }

    // M�todo para verificar se o paymentSlot cont�m algum valor (se h� filhos no GameObject)
    bool IsPaymentSlotNotEmpty()
    {
        // Encontra o GameObject com a tag "PaymentSlot"
        GameObject paymentSlot = GameObject.FindGameObjectWithTag(paymentSlotTag);

        if (paymentSlot != null)
        {
            // Verifica se o paymentSlot tem filhos (significando que algum valor foi colocado)
            return paymentSlot.transform.childCount > 0;
        }
        
        // Se n�o encontrar o GameObject com a tag "PaymentSlot"
        return false;
    }

    // M�todo chamado quando o bot�o � pressionado
    void OnVerifyPayment()
    {
        if (dragDropFood.AllSlotsFilled() && IsPaymentSlotNotEmpty())
        {
            // Se os slots est�o preenchidos e o paymentSlot tem valor, chama o m�todo VerifyPayment() do StarSystem
            starSystem.VerifyPayment();
            Debug.Log("Pagamento confirmado!");
        }
        else
        {
            // Se n�o estiverem preenchidos ou o paymentSlot n�o tiver valor, mostra uma mensagem no debug
            Debug.Log("Por favor, preencha todos os slots e adicione o valor ao paymentSlot antes de confirmar o pagamento.");
        }
    }
}
