using UnityEngine;
using UnityEngine.UI;

public class CartManager : MonoBehaviour
{
    public Text totalValueText; // Texto na UI para mostrar o total
    private int totalValue = 0; // Valor total do carrinho

    private Transform[] foodSlots; // Os três slots onde os alimentos serão colocados
    private int[] foodValues; // Valores dos alimentos nos slots

    void Start()
    {
        // Inicializa os slots e os valores
        foodSlots = new Transform[3];
        foodValues = new int[3];

        // Adapte para capturar os slots que estão na cena
        foodSlots[0] = GameObject.Find("FoodSlot1").transform; // Nome dos slots na sua cena
        foodSlots[1] = GameObject.Find("FoodSlot2").transform;
        foodSlots[2] = GameObject.Find("FoodSlot3").transform;
    }

    // Atualiza o valor total do carrinho
    public void UpdateTotal()
    {
        totalValue = 0;
        
        for (int i = 0; i < foodValues.Length; i++)
        {
            totalValue += foodValues[i];
        }

        totalValueText.text = "Total: " + totalValue.ToString();
    }

    // Adiciona um alimento ao carrinho
    public void AddFood(int slotIndex, int value)
    {
        foodValues[slotIndex] = value;
        UpdateTotal(); // Atualiza o total após adicionar o alimento
    }

    // Remove um alimento do carrinho
    public void RemoveFood(int slotIndex)
    {
        foodValues[slotIndex] = 0;
        UpdateTotal(); // Atualiza o total após remover o alimento
    }
}
