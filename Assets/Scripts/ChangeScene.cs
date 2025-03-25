using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // Método para trocar de cena
    public void TrocarCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }
}
