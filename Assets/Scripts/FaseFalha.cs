using UnityEngine;
using UnityEngine.SceneManagement;

public class FailureScreen : MonoBehaviour
{
    public void RetryLevel()
    {
        string nomeCena = PlayerPrefs.GetString("FaseParaRepetir", "");
        if (!string.IsNullOrEmpty(nomeCena))
        {
            Debug.Log("Recarregando fase: " + nomeCena);
            SceneManager.LoadScene(nomeCena);
        }
        else
        {
            Debug.LogError("Nenhuma fase salva para repetição!");
        }
    }
}
