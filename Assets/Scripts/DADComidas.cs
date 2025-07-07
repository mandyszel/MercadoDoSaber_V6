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
    public static float totalValue = 0f;

    private FoodItem foodItem;
    private RectTransform currentSlot;

    private static bool[] slotOccupied = new bool[3]; // Três slots

    void Awake()
    {
        // Resetar variáveis estáticas
    totalValue = 0f;
    selectedCount = 0;
    slotOccupied = new bool[3];

        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        foodItem = GetComponent<FoodItem>();

        // Atribuição segura do parentTransform
        parentTransform = transform.parent;
        if (parentTransform == null)
        {
            Debug.LogWarning("parentTransform estava nulo, tentando usar o próprio transform.");
            parentTransform = transform;
        }

        originalPosition = transform.position;
        currentSlot = null;

        UpdateTotalUI();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSelected && currentSlot != null)
        {
            totalValue = Mathf.Max(0f, totalValue - foodItem.value);
            selectedCount--;
            isSelected = false;
            slotOccupied[GetSlotIndex(currentSlot)] = false;
            currentSlot = null;
            UpdateTotalUI();
        }

        transform.SetAsLastSibling();
        canvasGroup.alpha = 0.9f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling();

        Vector3 globalPosition;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out globalPosition))
        {
            transform.position = globalPosition;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        RectTransform targetSlot = GetValidSlot(eventData);

        if (targetSlot != null && !IsSlotOccupied(targetSlot))
        {
            if (currentSlot != null && currentSlot != targetSlot)
            {
                totalValue = Mathf.Max(0f, totalValue - foodItem.value);
                selectedCount--;
                slotOccupied[GetSlotIndex(currentSlot)] = false;
            }

            transform.SetParent(targetSlot);
            rectTransform.anchoredPosition = Vector2.zero;

            if (!isSelected)
            {
                totalValue += foodItem.value;
                selectedCount++;
                isSelected = true;
                slotOccupied[GetSlotIndex(targetSlot)] = true;
            }

            currentSlot = targetSlot;
        }
        else
        {
            transform.SetParent(parentTransform);
            rectTransform.anchoredPosition = originalPosition - parentTransform.position;

            if (isSelected)
            {
                totalValue = Mathf.Max(0f, totalValue - foodItem.value);
                selectedCount--;
                isSelected = false;
            }

            currentSlot = null;
        }

        UpdateTotalUI();
        Debug.Log("Total Atual: R$ " + totalValue.ToString("F2"));
    }

    private void UpdateTotalUI()
    {
        if (totalValueText != null)
        {
            totalValueText.text = "R$ " + totalValue.ToString("F2");
        }
        else
        {
            Debug.LogWarning("totalValueText não foi atribuído no inspector.");
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

    private bool IsSlotOccupied(RectTransform slot)
    {
        return slotOccupied[GetSlotIndex(slot)];
    }

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

    public bool AllSlotsFilled()
    {
        foreach (bool isOccupied in slotOccupied)
        {
            if (!isOccupied)
            {
                return false;
            }
        }
        return true;
    }

    public void ResetSlots()
    {
        for (int i = 0; i < slotOccupied.Length; i++)
        {
            slotOccupied[i] = false;
        }
        totalValue = 0f;
        selectedCount = 0;
        UpdateTotalUI();
    }
}
