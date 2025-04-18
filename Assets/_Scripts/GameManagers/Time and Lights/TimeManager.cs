using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public int hours = CurrentHours;
    public int minutes = CurrentMinutes;
    public float timeScale = 96f; // 24 hours × 60 minutes / 15 minutes real-time

    //private float currentTime; // in-game minutes since 00:00

    public static float CurrentTimeRaw;
    public static int CurrentHours;
    public static int CurrentMinutes;

    [Space]
    [Header("Testing time change")]
    [Space]
    [Header("Set initial game time")]
    public bool SetInitialTime;
    public int SetIHours, SetIMinutes;
    [Space, Header("Change time in game")]
    public bool ChangeTime;
    public int SetHours, SetMinutes;


    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if(SetInitialTime)
            SetTime(SetIHours > 0 ? SetIHours : 0, SetIMinutes > 0 ? SetIMinutes : 0);
    }

    void Update()
    {
        if (ChangeTime)
        {
            ChangeTime = false;
            SetTime(SetHours > 0 ? SetHours : 0, SetMinutes > 0 ? SetMinutes : 0);
            SetHours = 0; SetMinutes = 0;
        }
    }

    void FixedUpdate()
    {
        // Advance game time scaled by real time
        CurrentTimeRaw += Time.fixedDeltaTime * timeScale;

        // Wrap around after 24 hours
        if (CurrentTimeRaw >= 1440f) CurrentTimeRaw -= 1440f;

        CurrentHours = Mathf.FloorToInt(CurrentTimeRaw / 60f);
        CurrentMinutes = Mathf.FloorToInt(CurrentTimeRaw % 60f);

        LightsManager.Instance.UpdateLighting(CurrentHours, CurrentMinutes);
    }

    public void SetTime(int hours, int minutes)
    {
        CurrentTimeRaw = (hours * 60) + minutes;
    }
}
