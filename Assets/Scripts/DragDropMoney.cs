//a principio tudo certto
// as vezes a nota da uma travadinha na borda do payment slot

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

    public static int totalMoneyValue = 0;
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

            // Obtém as dimensões do PaymentSlot
            RectTransform slotRect = targetSlot.GetComponent<RectTransform>();
            Vector2 slotSize = slotRect.rect.size;

            // Define um retângulo menor dentro do PaymentSlot
            
            
            float padding = 20f; // Ajuste conforme necessário
            float minX = -slotSize.x / 2 + padding;
            float maxX = slotSize.x / 2 - padding;
            float minY = -slotSize.y / 2 + padding;
            float maxY = slotSize.y / 2 - padding;

            // Garante que o dinheiro fique dentro do retângulo menor
            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(rectTransform.anchoredPosition.x, minX, maxX),
                Mathf.Clamp(rectTransform.anchoredPosition.y, minY, maxY)
            );

            // Ajuste para permitir a movimentação dentro do limite
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

        // Verifica o valor do pagamento
        if (totalMoneyValue == DragDropFood.totalValue)
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
            totalMoneyValueText.text = totalMoneyValue.ToString();
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
       // float padding = 20f; 
        Vector3 slotCenter = slot.rect.center;
        float clampedX = Mathf.Clamp(rectTransform.anchoredPosition.x, slotCenter.x - slot.rect.width / 4, slotCenter.x + slot.rect.width / 4);
        float clampedY = Mathf.Clamp(rectTransform.anchoredPosition.y, slotCenter.y - slot.rect.height / 4, slotCenter.y + slot.rect.height / 4);
        rectTransform.anchoredPosition = new Vector2(clampedX, clampedY);
    }
}
