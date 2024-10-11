// PaintingInteract.cs

using UnityEngine;
using UnityEngine.SceneManagement;

public class PaintingInteract : MonoBehaviour
{
    public string videoSceneName;

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Return))
        {
            // Load the video scene with transition
            FindObjectOfType<SceneTransitionManager>().TransitionToScene(videoSceneName);
        }
    }

    void OnGUI()
    {
        if (isPlayerNear)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 32;
            guiStyle.normal.textColor = Color.black;

            Vector2 size = guiStyle.CalcSize(new GUIContent("Press Enter to View"));
            GUI.Label(new Rect(Screen.width / 2 - size.x / 2, Screen.height - 100, size.x, size.y), "Press Enter to View", guiStyle);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
        }
    }
}
