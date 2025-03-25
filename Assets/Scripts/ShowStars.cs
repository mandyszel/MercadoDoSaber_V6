using UnityEngine;
using UnityEngine.UI; // Importante para trabalhar com UI

public class ShowStars : MonoBehaviour
{
    // Array para armazenar as 3 imagens de estrela
    public Image[] stars;  
    
    // Imagens das estrelas amarela e cinza
    public Sprite yellowStar;  
    public Sprite grayStar;    

    private int earnedStars;

    void Start()
    {
        earnedStars = PlayerPrefs.GetInt("EarnedStars", 0); // Carrega as estrelas ganhas do PlayerPrefs
        UpdateStarsDisplay(); // Atualiza a exibição das estrelas
    }

    void UpdateStarsDisplay()
    {
        for (int i = 0; i < stars.Length; i++)
        {
            // Se o número de estrelas ganhas for maior que o índice da estrela
            if (i < earnedStars)
                stars[i].sprite = yellowStar;  // Torna a estrela amarela
            else
                stars[i].sprite = grayStar;  // Torna a estrela cinza
        }
    }
}
