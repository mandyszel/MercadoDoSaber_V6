using UnityEngine;
using UnityEngine.SceneManagement;

public class RepetirFase : MonoBehaviour
{
    public void Repetir()
    {
        string nomeCena = SceneManager.GetActiveScene().name.Trim();
        SceneManager.LoadScene(nomeCena);
    }
}
