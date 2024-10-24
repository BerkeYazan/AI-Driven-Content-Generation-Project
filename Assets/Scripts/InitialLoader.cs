using UnityEngine;
using UnityEngine.SceneManagement;

public class InitialLoader : MonoBehaviour
{
    void Start()
    {
        // Load the main scene additively so the PersistentScene isn't unloaded
        SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
    }
}
