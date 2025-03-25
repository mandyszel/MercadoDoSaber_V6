//só deixa usar o botao de verificar se os 3 slots tiverem preenchidos e se tiver pelo menos uma nota/moeda no payment slot


using UnityEngine;
using UnityEngine.UI;  // Para referenciar o botão

public class CheckPayment : MonoBehaviour
{
    public StarSystem starSystem; // Referência ao StarSystem
    public Button verifyButton; // Referência ao botão de verificação
    public DragDropFood dragDropFood; // Referência ao script DragDropFood
    public string paymentSlotTag = "PaymentSlot"; // Tag do paymentSlot

    void Start()
    {
        // Adiciona o evento OnClick para o botão
        verifyButton.onClick.AddListener(OnVerifyPayment);
        
        // Desativa o botão inicialmente caso os slots não estejam preenchidos
        verifyButton.interactable = false;
    }

    void Update()
    {
        // Verifica se todos os 3 slots estão preenchidos e se o paymentSlot tem valor
        if (dragDropFood.AllSlotsFilled() && IsPaymentSlotNotEmpty())
        {
            // Se os slots estiverem preenchidos e o paymentSlot tiver valor, o botão pode ser clicado
            verifyButton.interactable = true;
        }
        else
        {
            // Caso contrário, o botão fica desativado
            verifyButton.interactable = false;
        }
    }

    // Método para verificar se o paymentSlot contém algum valor (se há filhos no GameObject)
    bool IsPaymentSlotNotEmpty()
    {
        // Encontra o GameObject com a tag "PaymentSlot"
        GameObject paymentSlot = GameObject.FindGameObjectWithTag(paymentSlotTag);

        if (paymentSlot != null)
        {
            // Verifica se o paymentSlot tem filhos (significando que algum valor foi colocado)
            return paymentSlot.transform.childCount > 0;
        }
        
        // Se não encontrar o GameObject com a tag "PaymentSlot"
        return false;
    }

    // Método chamado quando o botão é pressionado
    void OnVerifyPayment()
    {
        if (dragDropFood.AllSlotsFilled() && IsPaymentSlotNotEmpty())
        {
            // Se os slots estão preenchidos e o paymentSlot tem valor, chama o método VerifyPayment() do StarSystem
            starSystem.VerifyPayment();
            Debug.Log("Pagamento confirmado!");
        }
        else
        {
            // Se não estiverem preenchidos ou o paymentSlot não tiver valor, mostra uma mensagem no debug
            Debug.Log("Por favor, preencha todos os slots e adicione o valor ao paymentSlot antes de confirmar o pagamento.");
        }
    }
}
