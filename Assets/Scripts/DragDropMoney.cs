using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragDropMoney : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 originalPosition;
    private Transform parentTransform;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static float totalMoneyValue = 0f; // ALTERADO para float
    private MoneyItem moneyItem;
    private Transform currentSlot;
    private bool isSelected = false;
    public TextMeshProUGUI totalMoneyValueText;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.position;
        parentTransform = transform.parent;
        moneyItem = GetComponent<MoneyItem>();
        currentSlot = null;
        UpdateTotalUI();
    }

    public void SetInitialPosition(Vector3 spawnPosition)
    {
        originalPosition = spawnPosition;
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y);
        transform.SetParent(parentTransform);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isSelected && currentSlot != null)
        {
            totalMoneyValue = Mathf.Max(0, totalMoneyValue - moneyItem.value);
            isSelected = false;
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

        if (targetSlot != null)
        {
            if (currentSlot != null && currentSlot != targetSlot)
            {
                totalMoneyValue = Mathf.Max(0, totalMoneyValue - moneyItem.value);
            }

            transform.SetParent(targetSlot);

            RectTransform slotRect = targetSlot.GetComponent<RectTransform>();
            Vector2 slotSize = slotRect.rect.size;

            float padding = 20f;
            float minX = -slotSize.x / 2 + padding;
            float maxX = slotSize.x / 2 - padding;
            float minY = -slotSize.y / 2 + padding;
            float maxY = slotSize.y / 2 - padding;

            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(rectTransform.anchoredPosition.x, minX, maxX),
                Mathf.Clamp(rectTransform.anchoredPosition.y, minY, maxY)
            );

            rectTransform.anchoredPosition = clampedPosition;

            if (!isSelected)
            {
                totalMoneyValue += moneyItem.value;
                isSelected = true;
            }

            currentSlot = targetSlot;
        }
        else
        {
            transform.SetParent(parentTransform);
            rectTransform.anchoredPosition = originalPosition - parentTransform.position;

            if (isSelected)
            {
                totalMoneyValue = Mathf.Max(0, totalMoneyValue - moneyItem.value);
                isSelected = false;
            }

            currentSlot = null;
        }

        UpdateTotalUI();

        if (Mathf.Approximately(totalMoneyValue, DragDropFood.totalValue))
        {
            Debug.Log("Valor correto! Pagamento concluído.");
        }
        else
        {
            Debug.Log("Valor incorreto!");
        }
    }

    private void UpdateTotalUI()
    {
        if (totalMoneyValueText != null)
        {
            totalMoneyValueText.text = totalMoneyValue.ToString("F2"); // Mostra com duas casas decimais
        }
    }

    private RectTransform GetValidSlot(PointerEventData eventData)
    {
        foreach (GameObject slot in GameObject.FindGameObjectsWithTag("PaymentSlot"))
        {
            RectTransform slotRect = slot.GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, Input.mousePosition, canvas.worldCamera))
            {
                return slotRect;
            }
        }
        return null;
    }

    private void ClampToSlot(RectTransform slot)
    {
        Vector3 slotCenter = slot.rect.center;
        float clampedX = Mathf.Clamp(rectTransform.anchoredPosition.x, slotCenter.x - slot.rect.width / 4, slotCenter.x + slot.rect.width / 4);
        float clampedY = Mathf.Clamp(rectTransform.anchoredPosition.y, slotCenter.y - slot.rect.height / 4, slotCenter.y + slot.rect.height / 4);
        rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
    }

    // Método para resetar os slots de dinheiro
    public void ResetMoneySlots()
    {
        totalMoneyValue = 0f;
        UpdateTotalUI();

        // Reseta a posição do item e o estado de seleção
        transform.SetParent(parentTransform);
        rectTransform.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y);
        isSelected = false;
        currentSlot = null;

        Debug.Log("Notas e moedas resetadas!");
    }
}
