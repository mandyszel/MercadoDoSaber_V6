
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DragDropMoney : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector2 originalPosition;
    private Transform parentTransform;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static float totalMoneyValue = 0f;
    private MoneyItem moneyItem;
    private Transform currentSlot;
    private bool isSelected = false;
    public TextMeshProUGUI totalMoneyValueText;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalPosition = rectTransform.anchoredPosition; // CORRIGIDO
        parentTransform = transform.parent != null ? transform.parent : transform;
        if (parentTransform == transform)
        {
            Debug.LogWarning("parentTransform estava nulo, usando o próprio transform.");
        }

        moneyItem = GetComponent<MoneyItem>();
        currentSlot = null;
        UpdateTotalUI();
    }

    public void SetInitialPosition(Vector2 spawnPosition)
    {
        originalPosition = spawnPosition;
        rectTransform.anchoredPosition = originalPosition;
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
            rectTransform.anchoredPosition = originalPosition; // CORRIGIDO

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
Debug.Log("Comparando valores - Pago: " + DragDropMoney.totalMoneyValue.ToString("F10") + " | Esperado: " + DragDropFood.totalValue.ToString("F10"));
        }
    }

    private void UpdateTotalUI()
    {
        if (totalMoneyValueText != null)
        {
            totalMoneyValueText.text = totalMoneyValue.ToString("F2");
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

    public void ResetMoneySlots()
    {
        totalMoneyValue = 0f;
        UpdateTotalUI();

        transform.SetParent(parentTransform);
        rectTransform.anchoredPosition = originalPosition;
        isSelected = false;
        currentSlot = null;

        Debug.Log("Notas e moedas resetadas!");
    }
}
