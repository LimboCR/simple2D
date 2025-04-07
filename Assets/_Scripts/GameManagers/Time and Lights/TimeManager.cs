using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;

    public Light2D globalLight;

    public int hours = CurrentHours;
    public int minutes = CurrentMinutes;
    public float timeScale = 96f; // 24 hours × 60 minutes / 15 minutes real-time

    //private float currentTime; // in-game minutes since 00:00

    public static float CurrentTimeRaw;
    public static int CurrentHours;
    public static int CurrentMinutes;

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Update()
    {
        
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
