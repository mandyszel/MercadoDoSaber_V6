using UnityEngine;

public class ProximaFase : MonoBehaviour
{
    public static int tentativas;

    void Awake()
    {
        tentativas = 0;

        // Aqui voc� pode resetar outras vari�veis, como o total do carrinho, se necess�rio:
        DragDropFood.totalValue = 0f;
        DragDropFood.selectedCount = 0;

        Debug.Log("Vari�veis reiniciadas para nova fase.");
    }
}
