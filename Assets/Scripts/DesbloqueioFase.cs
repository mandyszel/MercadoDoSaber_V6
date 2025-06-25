using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FaseBotaoController : MonoBehaviour
{
    [Header("Configurações da fase")]
    public int numeroFaseReal; // Ex: Fase 1 = 1, Fase 2 = 2, ..., Fase 12 = 12
    public string nomeCena; // Nome da cena correspondente, ex: "Fase1"

    [Header("Sprites da fase")]
    public Sprite spriteDesbloqueado; // Colorida
    public Sprite spriteBloqueado;    // Preto e branco

    [Header("Referências")]
    public Button botao;
    public Image imagemFase;

    void Start()
    {
        AtualizarBotao();
        botao.onClick.AddListener(CarregarFase);
    }

    void AtualizarBotao()
    {
        bool desbloqueado = false;

        // Fase 1 sempre desbloqueada
        if (numeroFaseReal == 1)
        {
            desbloqueado = true;
        }
        else
        {
            // Checa se a fase anterior teve 2 ou mais estrelas
            int estrelasFaseAnterior = PlayerPrefs.GetInt("Stars_Fase_" + (numeroFaseReal - 2), 0);
            desbloqueado = estrelasFaseAnterior >= 2;
        }

        botao.interactable = desbloqueado;
        imagemFase.sprite = desbloqueado ? spriteDesbloqueado : spriteBloqueado;

        Debug.Log($"[Menu] Fase {numeroFaseReal} {(desbloqueado ? "DESBLOQUEADA" : "bloqueada")} (Estrelas Fase Anterior: {PlayerPrefs.GetInt("Stars_Fase_" + (numeroFaseReal - 2), 0)})");
    }

    void CarregarFase()
    {
        SceneManager.LoadScene(nomeCena);
    }
}
