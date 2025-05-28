using UnityEngine;
using UnityEngine.UI;

public class StarDisplay : MonoBehaviour
{
    [System.Serializable]
    public class FaseEstrelas
    {
        public Image[] estrelas; // As 3 estrelas da fase
    }

    [Header("Lista de estrelas de cada fase")]
    public FaseEstrelas[] fasesEstrelas;

    [Header("Sprites das estrelas")]
    public Sprite estrelaCheia;
    public Sprite estrelaVazia;

    void Start()
    {
        AtualizarEstrelasMenu();
    }

    void AtualizarEstrelasMenu()
    {
        for (int i = 0; i < fasesEstrelas.Length; i++)
        {
            string chave = "Stars_Fase_" + i;
            int estrelasSalvas = PlayerPrefs.GetInt(chave, 0);
            Debug.Log($"Fase {i + 1}: {estrelasSalvas} estrela(s)");

            for (int j = 0; j < fasesEstrelas[i].estrelas.Length; j++)
            {
                if (fasesEstrelas[i].estrelas[j] != null)
                {
                    fasesEstrelas[i].estrelas[j].sprite = (j < estrelasSalvas) ? estrelaCheia : estrelaVazia;
                }
            }
        }
    }
}
