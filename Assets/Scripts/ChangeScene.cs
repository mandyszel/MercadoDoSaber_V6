using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    // M�todo para trocar de cena
    public void TrocarCena(string nomeCena)
    {
        SceneManager.LoadScene(nomeCena);
    }
}
