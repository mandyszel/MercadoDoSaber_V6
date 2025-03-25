using UnityEngine;
using UnityEngine.SceneManagement;

public class FailureScreen : MonoBehaviour
{
    public void RetryLevel()
    {
        SceneManager.LoadScene("NomeDaFaseAtual"); // Troque pelo nome correto
    }
}
