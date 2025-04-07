using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightsManager : MonoBehaviour
{
    public static LightsManager Instance;

    //Global Light
    public Light2D globalLight;
    public static float Intensity = 1f;
    public static Light2D GlobalLight;

    //Lights in game
    public bool LightsOn = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        GlobalLight = globalLight;
    }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void UpdateLighting(int hours, int minutes)
    {
        if (hours >= 14 && hours < 20)
        {
            if(hours >= 18) LightsOn = true;
            else LightsOn = false;
            // 14:00 > 20:00 > fade 1 > 0.05
            float t = (hours - 14 + minutes / 60f) / 6f;
            Intensity = Mathf.Lerp(1f, 0.05f, t);
        }
        else if (hours >= 20 || hours < 5)
        {
            // 20:00 > 05:00 > stays at 0.05
            LightsOn = true;
            Intensity = 0.05f;
        }
        else if (hours >= 5 && hours < 12)
        {
            if (hours >= 8) LightsOn = false;
            else LightsOn = true;
            // 05:00 > 12:00 > fade 0.05 > 1
            float t = (hours - 5 + minutes / 60f) / 7f;
            Intensity = Mathf.Lerp(0.05f, 1f, t);
        }
        else
        {
            // 12:00 > 14:00 > stays at 1
            LightsOn = false;
            Intensity = 1f;
        }

        GlobalLight.intensity = Intensity;
    }
}
