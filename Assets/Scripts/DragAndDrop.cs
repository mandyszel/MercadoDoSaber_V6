//dando certo porem as frutas desaparecer a partir da 2 vez
//dando certo, erro corrigido
//erro de conseguir colocar um item acima do  outro 
//arrumado

//so pode fazer a confirmação apos ter os tres slots completos


using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragDropFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform parentTransform;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static int selectedCount = 0;
    private bool isSelected = false;

    public TextMeshProUGUI totalValueText;
    public static int totalValue = 0;
    private FoodItem foodItem;
    private RectTransform currentSlot; // Alterado para RectTransform

    private static bool[] slotOccupied = new bool[3]; // Supondo 3 slots

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.position;
        parentTransform = transform.parent;
        foodItem = GetComponent<FoodItem>();
        currentSlot = null;
        UpdateTotalUI();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSelected && currentSlot != null)
        {
            // Se já está em um slot, tira o item de lá
            totalValue = Mathf.Max(0, totalValue - foodItem.value);
            selectedCount--;
            isSelected = false;
            slotOccupied[GetSlotIndex(currentSlot)] = false; // Marca o slot como desocupado
            currentSlot = null;
            UpdateTotalUI();
        }

        transform.SetAsLastSibling();
        canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out position
        );
        rectTransform.anchoredPosition = position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        RectTransform targetSlot = GetValidSlot(eventData);

        if (targetSlot != null && !IsSlotOccupied(targetSlot)) // Verifica se o slot não está ocupado
        {
            if (currentSlot != null && currentSlot != targetSlot)
            {
                // Se o item estava em outro slot, tira ele de lá
                totalValue = Mathf.Max(0, totalValue - foodItem.value);
                selectedCount--;
                slotOccupied[GetSlotIndex(currentSlot)] = false; // Marca o slot anterior como desocupado
            }

            // Atualiza o parentTransform corretamente para o slot, e coloca a posição correta
            transform.SetParent(targetSlot);
            rectTransform.anchoredPosition = Vector2.zero; // Garantir que o alimento vai para o centro do slot

            if (!isSelected)
            {
                totalValue += foodItem.value;
                selectedCount++;
                isSelected = true;
                slotOccupied[GetSlotIndex(targetSlot)] = true; // Marca o slot como ocupado
            }

            currentSlot = targetSlot;
        }
        else
        {
            // Quando o alimento não é colocado em um slot válido, ele volta para o parent original
            transform.SetParent(parentTransform);
            rectTransform.anchoredPosition = originalPosition - parentTransform.position; // Posição correta
            if (isSelected)
            {
                totalValue = Mathf.Max(0, totalValue - foodItem.value);
                selectedCount--;
                isSelected = false;
            }
            currentSlot = null;
        }

        UpdateTotalUI();
        Debug.Log("Total Atual: " + totalValue);
    }

    private void UpdateTotalUI()
    {
        if (totalValueText != null)
        {
            totalValueText.text = totalValue.ToString();
        }
    }

    private RectTransform GetValidSlot(PointerEventData eventData)
    {
        foreach (GameObject slot in GameObject.FindGameObjectsWithTag("FoodSlot"))
        {
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, Input.mousePosition, canvas.worldCamera))
            {
                return slotRect;
            }
        }
        return null;
    }

    // Função para verificar se o slot está ocupado
    private bool IsSlotOccupied(RectTransform slot)
    {
        return slotOccupied[GetSlotIndex(slot)];
    }

    // Função para obter o índice do slot baseado no objeto RectTransform
    private int GetSlotIndex(RectTransform slot)
    {
        int index = -1;
        GameObject[] slots = GameObject.FindGameObjectsWithTag("FoodSlot");
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].GetComponent<RectTransform>() == slot)
            {
                index = i;
                break;
            }
        }
        return index;
    }

    // Adiciona o método AllSlotsFilled() no DragDropFood
public bool AllSlotsFilled()
{
    foreach (bool isOccupied in slotOccupied)
    {
        if (!isOccupied)
        {
            return false; // Se algum slot não estiver ocupado, retorna false
        }
    }
    return true; // Se todos os slots estiverem ocupados, retorna true
}

}
