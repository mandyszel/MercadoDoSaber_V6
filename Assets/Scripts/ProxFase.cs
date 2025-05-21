using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximaFase : MonoBehaviour
{
    public void IrParaProximaFase()
    {
        int faseAtual = PlayerPrefs.GetInt("FaseAtual", -1);

        if (faseAtual == -1)
        {
            Debug.LogError("Fase atual não foi salva corretamente!");
            return;
        }

        int proximaFase = faseAtual + 1;
        if (proximaFase <= 12)
        {
            string nomeProximaFase = "Fase" + proximaFase;
            Debug.Log("Indo para: " + nomeProximaFase);
            SceneManager.LoadScene(nomeProximaFase);
        }
        else
        {
            Debug.Log("Última fase concluída!");
        }
    }
}
