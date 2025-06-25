using UnityEngine;

public class BotaoSair : MonoBehaviour
{
    public void SairJogo()
    {
        Debug.Log("Saindo do jogo...");

        // Isso fecha o jogo no build (Windows, Android, etc.)
        Application.Quit();

        // No editor da Unity, isso não funciona — mas o Debug mostra que o botão foi clicado
    }
}
