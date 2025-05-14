using UnityEngine;
using UnityEngine.SceneManagement;

public class FailureScreen : MonoBehaviour
{
    public void RetryLevel()
    {
        string nomeCena = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(nomeCena);
    }
}
