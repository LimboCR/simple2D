using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PersistentGraphicsSettings", menuName = "Game/Persistent Graphics Settings")]
public class GraphicsSettingsSO : ScriptableObject
{
    [Header("Boolean Varaiables")]
    public bool Vsync;
    public bool PostProcessing;
    public bool RenderShadows;

    [Space, Header("Option Variables")]
    public int ResolutionIndex;
    public int AAQualityIndex;

    public void SaveGraphicsData(GraphicsSettingsManager GSM)
    {
        Vsync = GSM.VsyncToogle.isOn;
        PostProcessing = GSM.PostProcToogle.isOn;
        RenderShadows = GSM.RenderShadows.isOn;

        ResolutionIndex = GSM.ResolutionDropdown.value;
        AAQualityIndex = GSM.AAQualityDropdown.value;
    }

    public void LoadGraphicsData(GraphicsSettingsManager GSM)
    {
        QualitySettings.vSyncCount = Vsync ? 1 : 0;
        GSM.VsyncToogle.isOn = Vsync;
        GSM.SetPostProcessing(PostProcessing, true);
        GSM.SetRenderShadows(RenderShadows, true);

        GSM.SetActualResolution(ResolutionIndex, true);
        GSM.ChangeAAQuality(AAQualityIndex, true);
    }

    public async Task AsyncLoadGraphicsData(GraphicsSettingsManager GSM)
    {
        await Task.Yield();

        QualitySettings.vSyncCount = Vsync ? 1 : 0;
        GSM.VsyncToogle.isOn = Vsync;
        GSM.SetPostProcessing(PostProcessing, true);
        GSM.SetRenderShadows(RenderShadows, true);

        GSM.SetActualResolution(ResolutionIndex, true);
        GSM.ChangeAAQuality(AAQualityIndex, true);
    }
}
