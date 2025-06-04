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

    [Header("Número da primeira fase neste menu (ex: 0 para Fase 1, 6 para Fase 7)")]
    public int offsetFaseInicial = 0;

    void Start()
    {
        AtualizarEstrelasMenu();
    }

    public void AtualizarEstrelasMenu()
    {
        int faseAtualDesbloqueada = PlayerPrefs.GetInt("FaseAtual", 1); // Começa na Fase 1

        for (int i = 0; i < fasesEstrelas.Length; i++)
        {
            int numeroFaseReal = offsetFaseInicial + i + 1; // Ex: 6 + 0 + 1 = Fase 7

            if (numeroFaseReal > faseAtualDesbloqueada)
            {
                foreach (Image estrela in fasesEstrelas[i].estrelas)
                {
                    if (estrela != null)
                        estrela.gameObject.SetActive(false);
                }
                continue;
            }

            string chave = "Stars_Fase_" + (numeroFaseReal - 1); // Ex: Fase 7 ? Stars_Fase_6
            int estrelasSalvas = PlayerPrefs.GetInt(chave, 0);
            Debug.Log($"Fase {numeroFaseReal}: {estrelasSalvas} estrela(s)");

            for (int j = 0; j < fasesEstrelas[i].estrelas.Length; j++)
            {
                if (fasesEstrelas[i].estrelas[j] != null)
                {
                    fasesEstrelas[i].estrelas[j].gameObject.SetActive(true);
                    fasesEstrelas[i].estrelas[j].sprite = (j < estrelasSalvas) ? estrelaCheia : estrelaVazia;
                }
            }
        }
    }

    // ? Esta função deve ser chamada ao final da fase, passando o número da fase e as estrelas conquistadas
    public void SalvarEstrelas(int numeroFase, int estrelasConquistadas)
    {
        string chaveEstrelas = "Stars_Fase_" + (numeroFase - 1);
        PlayerPrefs.SetInt(chaveEstrelas, estrelasConquistadas); // Sempre sobrescreve
        Debug.Log($"Estrelas atualizadas na Fase {numeroFase}: {estrelasConquistadas}");

        // Desbloquear próxima fase se esta for a última desbloqueada e o jogador foi bem (2 ou mais)
        int faseAtualDesbloqueada = PlayerPrefs.GetInt("FaseAtual", 1);

        if (numeroFase == faseAtualDesbloqueada && estrelasConquistadas >= 2)
        {
            PlayerPrefs.SetInt("FaseAtual", faseAtualDesbloqueada + 1);
            Debug.Log($"Fase {faseAtualDesbloqueada + 1} desbloqueada!");
        }

        PlayerPrefs.Save();
    }
}
