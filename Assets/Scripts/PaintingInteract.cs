using UnityEngine;

public class PaintingInteract : MonoBehaviour
{
    [Tooltip("Name of the video scene to load when interacting with this painting.")]
    public string videoSceneName;

    private bool isPlayerNear = false;

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.Return))
        {
            if (SceneTransitionManager.instance != null)
            {
                SceneTransitionManager.instance.TransitionToScene(videoSceneName, true); 
            }
            else
            {
                Debug.LogError("SceneTransitionManager.instance not found in the scene.");
            }
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
