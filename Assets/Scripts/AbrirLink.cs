using UnityEngine;

public class OpenLink : MonoBehaviour 
{
    public void AbrirLink(string url)
    {
        Application.OpenURL(url);
    }
}
