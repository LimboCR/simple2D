using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightScript : MonoBehaviour
{
    [Header("Light Sources & Related Objects")]
    [SerializeField] private List<GameObject> objectLights = new();
    [SerializeField] private List<GameObject> relatedObjects = new();

    private bool IsLightOn;

    private void Awake()
    {
        
    }

    private void Start()
    {
        IsLightOn = LightsManager.Instance.LightsOn;
        HandleLights(IsLightOn);
    }

    void Update()
    {
        if (LightsManager.Instance.LightsOn && !IsLightOn)
        {
            IsLightOn = true;
            HandleLights(IsLightOn);
        }
        if(!LightsManager.Instance.LightsOn && IsLightOn)
        {
            IsLightOn = false;
            HandleLights(IsLightOn);
        }
    }

    private void HandleLights(bool lightsOn)
    {
        if (objectLights.Count > 0)
        {
            foreach (GameObject obj in objectLights)
            {
                if(obj.TryGetComponent<Light2D>(out Light2D source))
                    source.enabled = lightsOn;
            }
        }

        if(relatedObjects.Count > 0)
        {
            foreach(GameObject obj in relatedObjects)
            {
                if(obj.activeSelf != lightsOn) obj.SetActive(lightsOn);
            }
        }
    }
}
