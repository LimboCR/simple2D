using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GraphicsSettingsManager : MonoBehaviour
{
    #region Variables
    [Header("Checkers")]
    public bool AllSet = false;

    [Space, Header("Dropdowns")]
    public TMP_Dropdown ResolutionDropdown;
    public TMP_Dropdown AAQualityDropdown;

    [Space, Header("Toggles")]
    public Toggle VsyncToogle;
    public Toggle PostProcToogle;
    public Toggle RenderShadows;

    [Space, Header("Cameras")]
    Resolution[] resolutions;
    public Camera MainCam;
    public UniversalAdditionalCameraData CamData;

    Coroutine SetCamera;

    #endregion

    #region Awake, Start and etc

    private void Awake()
    {
        resolutions = Screen.resolutions;
        MainCam = Camera.main;
        CamData = MainCam.GetUniversalAdditionalCameraData();
    }
    async void Start()
    {
        await PopulateRequiredFields();

        Debug.Log("[GraphicsSettingsManager] All Graphics Settings Fields Was Set. AllSet = true.");
        AllSet = true;
    }
    
    private async Task PopulateRequiredFields()
    {
        Task setupResolution = SetupResolutionSelector();
        Task setupToggles = SetupToggles();
        Task setupQuality = SetupQualityFields();
        Task loadSavedData = LoadSavedData();

        await Task.WhenAll(setupResolution, setupToggles, setupQuality, loadSavedData);
    }
    #endregion

    #region Initial Fields Setup
    private async Task SetupResolutionSelector()
    {
        await Task.Yield();

        var options = new List<string>();
        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(options);
        ResolutionDropdown.value = currentIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    private async Task SetupToggles()
    {
        await Task.Yield();

        VsyncToogle.isOn = QualitySettings.vSyncCount == 1 ? true : false;
        PostProcToogle.isOn = CamData.renderPostProcessing;
        RenderShadows.isOn = CamData.renderShadows;
    }

    private async Task SetupQualityFields()
    {
        await SetupAAQualityDropdown();
    }

    private async Task SetupAAQualityDropdown()
    {
        await Task.Yield();

        var options = new List<string>();
        int currentIndex = 0;

        var qualities = System.Enum.GetValues(typeof(AntialiasingQuality));
        int i = 0;

        foreach (AntialiasingQuality quality in qualities)
        {
            options.Add(quality.ToString());

            if (CamData.antialiasingQuality == quality)
                currentIndex = i;

            i++;
        }

        AAQualityDropdown.ClearOptions();
        AAQualityDropdown.AddOptions(options);
        AAQualityDropdown.value = currentIndex;
        AAQualityDropdown.RefreshShownValue();
    }

    private async Task LoadSavedData()
    {
        Task outbound = GlobalSettingsManager.s_GraphicsSettings.AsyncLoadGraphicsData(this);

        await Task.WhenAll(outbound);
    }
    #endregion

    #region CameraSetupAgain
    public void TrySetupCamera()
    {
        if(SetCamera == null)
            SetCamera = StartAndTrack(SetCameraCoroutine(), () => SetCamera = null);
    }

    public IEnumerator SetCameraCoroutine()
    {
        while (!AllSet) yield return null;

        MainCam = Camera.main;
        CamData = MainCam.GetUniversalAdditionalCameraData();
        GlobalSettingsManager.s_GraphicsSettings.LoadGraphicsData(this);

        yield break;
    }
    #endregion

    #region Screen Resolution
    public void SetResolution()
    {
        if (!AllSet) return;
        SetActualResolution(ResolutionDropdown.value);
    }

    public void SetActualResolution(int index, bool updateVisual = false)
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, true);
        //Debug.Log($"[ScreenSettings] Changed. New Screen.Resolution: {Screen.currentResolution}");

        if (updateVisual) ResolutionDropdown.value = index;
        else GlobalSettingsManager.s_GraphicsSettings.SaveGraphicsData(this);
    }
    #endregion

    #region Vsync
    public void SetVSync()
    {
        if (!AllSet) return;

        QualitySettings.vSyncCount = VsyncToogle.isOn ? 1 : 0;
        GlobalSettingsManager.s_GraphicsSettings.SaveGraphicsData(this);
        //Debug.Log($"[QualitySettings] Changed. New QualitySettings.vSyncCount = {QualitySettings.vSyncCount}");
    }
    #endregion

    #region AAQualityResolution
    public void SetPostProcessingCallback()
    {
        if (!AllSet) return;
        SetPostProcessing(PostProcToogle.isOn);
    }

    public void SetPostProcessing(bool value, bool updateVisual = false)
    {
        CamData.renderPostProcessing = value;

        if (updateVisual) PostProcToogle.isOn = value;
        else GlobalSettingsManager.s_GraphicsSettings.SaveGraphicsData(this);

        if (PostProcToogle.isOn)
            CamData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        else CamData.antialiasing = AntialiasingMode.None;
    }

    public void OnAAQualityChanged()
    {
        if (!AllSet) return;
        ChangeAAQuality(AAQualityDropdown.value);
    }

    public void ChangeAAQuality(int index, bool updateVisual = false)
    {
        CamData.antialiasingQuality = (AntialiasingQuality)index;

        if(updateVisual) AAQualityDropdown.value = index;
        else GlobalSettingsManager.s_GraphicsSettings.SaveGraphicsData(this);
    }

    public void OnAAModeChanged()
    {
        CamData.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing; // Or FXAA, depending on what you want
    }

    private int GetSampleCount(AntialiasingQuality quality)
    {
        switch (quality)
        {
            case AntialiasingQuality.Low: return 2;
            case AntialiasingQuality.Medium: return 4;
            case AntialiasingQuality.High: return 8;
            default: return 1;
        }
    }
    #endregion

    #region Rendering Shadows
    public void OnShadowRenderingChange()
    {
        if (!AllSet) return;
        SetRenderShadows(RenderShadows.isOn);
    }

    public void SetRenderShadows(bool value, bool updateVisual = false)
    {
        CamData.renderShadows = value;

        if (updateVisual) RenderShadows.isOn = value;
        else GlobalSettingsManager.s_GraphicsSettings.SaveGraphicsData(this);
    }
    #endregion

    #region Util
    public Coroutine StartAndTrack(IEnumerator routine, Action onComplete)
    {
        return StartCoroutine(Wrapper());

        IEnumerator Wrapper()
        {
            yield return StartCoroutine(routine);
            onComplete?.Invoke();
        }
    }
    #endregion
}