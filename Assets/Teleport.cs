using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour
{
    [Tooltip("Name of the scene to load")]
    public string SceneToLoad;
    public GameObject CutSceneObj;

    public void PlayCutScene()
    {
        if(CutSceneObj != null)
        {
            if (CutSceneObj.TryGetComponent<CutSceneHandler>(out CutSceneHandler cutScene))
            {
                CutSceneObj.SetActive(true);
                cutScene.StartingFrame.TestFadeIn = true;
            }
        }
        else MoveToNewScene();

        GlobalEventsManager.ForceStopPlaying(AudioSourceType.Music);
        GameManager.ClearCombatNPCS();
    }

    public void MoveToNewScene()
    {
        if (!string.IsNullOrEmpty(SceneToLoad))
        {
            GameManager.PlayerIntializedLeavingScene();
            AudioManager.IntakeStopPlaying();
            SceneManager.LoadScene(SceneToLoad);
        }
        else
        {
            Debug.LogError("Teleport: Scene name is not set.");
        }
    }
}
