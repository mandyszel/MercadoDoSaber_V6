using UnityEngine;

public class OpenLink : MonoBehaviour 
{
    public void OpenSite(string url)
    {
        Application.OpenURL(url);
    }
}
