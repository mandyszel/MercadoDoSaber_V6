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

    void OnEnable()
    {
        AtualizarEstrelasMenu();
    }

    public void AtualizarEstrelasMenu()
    {
        int maiorFaseDesbloqueada = PlayerPrefs.GetInt("MaiorFaseDesbloqueada", 1); // Começa com Fase 1

        for (int i = 0; i < fasesEstrelas.Length; i++)
        {
            int numeroFaseReal = offsetFaseInicial + i + 1;

            if (numeroFaseReal > maiorFaseDesbloqueada)
            {
                foreach (Image estrela in fasesEstrelas[i].estrelas)
                {
                    if (estrela != null)
                        estrela.gameObject.SetActive(false);
                }
                continue;
            }

            string chave = "Stars_Fase_" + (numeroFaseReal - 1);
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

    // Chamada ao final da fase
    public void SalvarEstrelas(int numeroFase, int estrelasConquistadas)
    {
        string chaveEstrelas = "Stars_Fase_" + (numeroFase - 1);

        // Sempre salva a tentativa atual para refletir no menu
        PlayerPrefs.SetInt(chaveEstrelas, estrelasConquistadas);
        Debug.Log($"Fase {numeroFase}: nova tentativa com {estrelasConquistadas} estrela(s)");

        // Atualiza desbloqueio da próxima fase se o jogador ainda não tinha passado dela
        int maiorFaseDesbloqueada = PlayerPrefs.GetInt("MaiorFaseDesbloqueada", 1);

        // Se a fase atual for a última desbloqueada E o jogador foi bem
        if (numeroFase == maiorFaseDesbloqueada && estrelasConquistadas >= 2)
        {
            PlayerPrefs.SetInt("MaiorFaseDesbloqueada", maiorFaseDesbloqueada + 1);
            Debug.Log($"Desbloqueando Fase {maiorFaseDesbloqueada + 1}");
        }

        PlayerPrefs.Save();
    }
}
