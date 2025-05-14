using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RepetirFase : MonoBehaviour
{
    public void Repetir()
    {
        // Chama a fun��o para resetar as vari�veis
        ResetGameVariables();
        
        // Inicia a Coroutine para recarregar a cena
        StartCoroutine(RecarregarCena());
    }

    private IEnumerator RecarregarCena()
    {
        // Adiciona uma pequena pausa para garantir que tudo seja resetado antes de carregar a cena novamente
        yield return new WaitForSeconds(0.1f);

        // Obt�m o nome da cena atual e recarrega a cena
        string nomeCena = SceneManager.GetActiveScene().name.Trim();
        Debug.Log("Recarregando a cena: " + nomeCena);
        SceneManager.LoadScene(nomeCena);
    }

    private void ResetGameVariables()
    {
        // Aqui voc� pode resetar as vari�veis do jogo, como o valor total e tentativas
        DragDropFood.totalValue = 0f; // Reseta o valor total de alimentos
        DragDropMoney.totalMoneyValue = 0f; // Reseta o valor total de dinheiro
        DragDropFood.selectedCount = 0; // Reseta o contador de alimentos selecionados

        // Adicione mais vari�veis que precisar resetar aqui
        // Exemplo: Resetando as vari�veis de slots, se necess�rio
        // CartManager.ResetSlots();
        // GameController.ResetProgress();
    }

    public void Mensagem()
    {
        Debug.Log("O Bot�o de repetir foi clicado!");
    }
}
