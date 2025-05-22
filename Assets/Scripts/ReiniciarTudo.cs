using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetProgress : MonoBehaviour
{
    public void ResetarProgresso()
    {
        PlayerPrefs.DeleteAll(); // Remove todas as chaves salvas
        PlayerPrefs.Save();
        Debug.Log("Progresso resetado!");

        // Se quiser reiniciar a cena do menu
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
