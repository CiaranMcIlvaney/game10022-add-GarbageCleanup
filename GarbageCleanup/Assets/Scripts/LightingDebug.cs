using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingDebug : MonoBehaviour
{
    void Start()
    {
        Debug.Log($"Skybox: {(RenderSettings.skybox ? RenderSettings.skybox.name : "None")}");
        Debug.Log($"AmbientIntensity: {RenderSettings.ambientIntensity}");
        Debug.Log($"AmbientLight: {RenderSettings.ambientLight}");
        Debug.Log($"Fog: {RenderSettings.fog}  FogColor: {RenderSettings.fogColor}");
        Debug.Log($"Quality Level: {QualitySettings.names[QualitySettings.GetQualityLevel()]}");
    }
}
