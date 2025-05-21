using UnityEngine;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        public Button button;
        public Image image;
        public Sprite unlockedSprite;
        public Sprite lockedSprite;
    }

    public LevelButton[] levelButtons;

    void Start()
    {
        UpdateLevelButtons();
    }

    void UpdateLevelButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            bool isUnlocked = false;

            if (i == 0)
            {
                // Primeira fase sempre desbloqueada
                isUnlocked = true;
            }
            else
            {
                // Verifica se a fase anterior teve 2 estrelas ou mais
                int previousStars = PlayerPrefs.GetInt("Stars_Fase_" + (i), 0); // Fase 1 é índice 0
                if (previousStars >= 2)
                    isUnlocked = true;
            }

            levelButtons[i].button.interactable = isUnlocked;
            levelButtons[i].image.sprite = isUnlocked ? levelButtons[i].unlockedSprite : levelButtons[i].lockedSprite;
        }
    }
}
