using UnityEngine;

public class ProximaFase : MonoBehaviour
{
    public static int tentativas;

    void Awake()
    {
        tentativas = 0;

        // Aqui você pode resetar outras variáveis, como o total do carrinho, se necessário:
        DragDropFood.totalValue = 0f;
        DragDropFood.selectedCount = 0;

        Debug.Log("Variáveis reiniciadas para nova fase.");
    }
}
