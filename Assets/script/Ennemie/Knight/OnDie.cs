using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnDie : MonoBehaviour
{
    private void OnDestroy()
    {
        SceneManager.LoadScene(0);
    }
}
