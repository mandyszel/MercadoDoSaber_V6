using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximaFase : MonoBehaviour
{
    public void IrParaProximaFase()
    {
        // Se não existir nada salvo, assume "Fase1" como padrão
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
                    // Limpa todos os dados salvos antes de carregar a próxima fase
                    PlayerPrefs.DeleteAll();
                    PlayerPrefs.Save();

                    // Salva a nova fase como a fase anterior
                    PlayerPrefs.SetString("FaseAnterior", nomeProxima);
                    PlayerPrefs.Save();

                    // Carrega a próxima fase
                    SceneManager.LoadScene(nomeProxima);
                }
                else
                {
                    Debug.LogWarning("Próxima fase não encontrada: " + nomeProxima);
                }
            }
            else
            {
                Debug.LogError("Número da fase anterior não foi reconhecido: " + numeroStr);
            }
        }
        else
        {
            Debug.LogError("Fase anterior inválida: " + faseAnterior);
        }
    }
}
