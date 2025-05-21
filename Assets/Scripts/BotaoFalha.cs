using UnityEngine;
using UnityEngine.SceneManagement;

public class BotaoRepetirFase : MonoBehaviour
{
    public void RepetirFase()
    {
        string faseParaRepetir = PlayerPrefs.GetString("FaseParaRepetir", "");

        if (!string.IsNullOrEmpty(faseParaRepetir))
        {
            Debug.Log("Voltando para a fase: " + faseParaRepetir);
            SceneManager.LoadScene(faseParaRepetir);
        }
        else
        {
            Debug.LogError("Nenhuma fase foi salva para repetir!");
        }
    }
}
