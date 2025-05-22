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

        if (numeroFaseReal == 1)
        {
            desbloqueado = true; // A primeira fase sempre começa desbloqueada
        }
        else
        {
            int estrelasDaAnterior = PlayerPrefs.GetInt("Stars_Fase_" + (numeroFaseReal - 1), 0);
            desbloqueado = estrelasDaAnterior >= 2;
        }

        botao.interactable = desbloqueado;
        imagemFase.sprite = desbloqueado ? spriteDesbloqueado : spriteBloqueado;
    }

    void CarregarFase()
    {
        SceneManager.LoadScene(nomeCena);
    }
}
