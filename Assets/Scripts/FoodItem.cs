using UnityEngine;

public class FoodItem : MonoBehaviour
{
    public float value; // Valor do alimento (agora com casas decimais)

    public void SetFoodValue(float newValue)
    {
        value = newValue;
    }
}
