using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximaFase : MonoBehaviour
{
    public void IrParaProximaFase()
    {
        string faseAnterior = PlayerPrefs.GetString("FaseAnterior", "");

        if (faseAnterior.StartsWith("Fase"))
        {
            string numeroStr = faseAnterior.Substring(4).Trim();

            if (int.TryParse(numeroStr, out int numeroFaseAtual))
            {
                int numeroProxima = numeroFaseAtual + 1;
                string nomeProxima = "Fase" + numeroProxima;

                if (Application.CanStreamedLevelBeLoaded(nomeProxima))
                {
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
