using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene(1);
    }
}
