using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximaFase : MonoBehaviour
{
    public void IrParaProximaFase()
    {
        // Se n�o existir nada salvo, assume "Fase1" como padr�o
        string faseAnterior = PlayerPrefs.GetString("FaseAnterior", "Fase1");

        if (faseAnterior.StartsWith("Fase"))
        {
            string numeroStr = faseAnterior.Substring(4).Trim();

            if (int.TryParse(numeroStr, out int numeroFaseAtual))
            {
                int numeroProxima = numeroFaseAtual + 1;
                string nomeProxima = "Fase" + numeroProxima;

                if (Application.CanStreamedLevelBeLoaded(nomeProxima))
                {
                    // Limpa todos os dados salvos antes de carregar a pr�xima fase
                    PlayerPrefs.DeleteAll();
                    PlayerPrefs.Save();

                    // Salva a nova fase como a fase anterior
                    PlayerPrefs.SetString("FaseAnterior", nomeProxima);
                    PlayerPrefs.Save();

                    // Carrega a pr�xima fase
                    SceneManager.LoadScene(nomeProxima);
                }
                else
                {
                    Debug.LogWarning("Pr�xima fase n�o encontrada: " + nomeProxima);
                }
            }
            else
            {
                Debug.LogError("N�mero da fase anterior n�o foi reconhecido: " + numeroStr);
            }
        }
        else
        {
            Debug.LogError("Fase anterior inv�lida: " + faseAnterior);
        }
    }
}
