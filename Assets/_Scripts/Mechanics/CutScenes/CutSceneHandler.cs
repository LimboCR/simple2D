using UnityEngine;

public class CutSceneHandler : MonoBehaviour
{
    public TestComicsFrame StartingFrame;
    public TestComicsFrame EndingFrame;

    public GameObject ContinueButton;
    private void Update()
    {
        if(EndingFrame.FrameLogicFinished == true) ContinueButton.SetActive(true);
    }
}
