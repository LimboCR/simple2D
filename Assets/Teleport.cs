using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string SceneToLoad;

    public void MoveToNewScene()
    {
        if (!string.IsNullOrEmpty(SceneToLoad))
        {
            GameManager.PlayerIntializedLeavingScene();
            SceneManager.LoadScene(SceneToLoad);
        }
        else
        {
            Debug.LogWarning("Teleport: Scene name is not set.");
        }
    }
}
