using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public void TrocarCena(string nomeCena)
    {
        string cenaAtual = SceneManager.GetActiveScene().name;

        // Se estiver em uma fase de Fase1 a Fase12 e for voltar para o Menu
        if (nomeCena == "Menu" && cenaAtual.StartsWith("Fase"))
        {
            FaseInfo faseInfo = FindFirstObjectByType<FaseInfo>();
            if (faseInfo != null && faseInfo.numeroFase >= 1 && faseInfo.numeroFase <= 12)
            {
                // Resetar variáveis estáticas
                StarSystem.errors = 0;
                DragDropMoney.totalMoneyValue = 0f;
                DragDropFood.totalValue = 0f;

                // Se quiser, você também pode resetar os slots diretamente (opcional)
                DragDropFood drag = FindFirstObjectByType<DragDropFood>();
                if (drag != null)
                    drag.ResetSlots();

                // Agora sim, volta para o menu
                SceneManager.LoadScene(nomeCena);
                return;
            }
        }

        // Caso contrário, troca normal de cena
        SceneManager.LoadScene(nomeCena);
    }
}
