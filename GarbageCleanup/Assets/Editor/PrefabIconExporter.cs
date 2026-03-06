/*
 
- Prefab Icon Exporter

Purpose:
- Batch renders selected prefabs to transparent PNG icons for use in UI.

Implementation:
This tool was created with assistance from AI and adapted for this project. It uses standard Unity editor scripting techniques
such as PrefabUtility, RenderTexture rendering, and EditorWindow tools.

Project: Garbage Cleanup (Mini Capstone)
Author: Ciaran McIlvaney

 */

using System.IO;
using UnityEditor;
using UnityEngine;

public class PrefabIconExporter : EditorWindow
{
    private int iconSize = 256;
    private Vector3 cameraOffset = new Vector3(1.8f, 1.2f, -1.8f);
    private Color backgroundColor = new Color(0, 0, 0, 0); // transparent
    private string outputFolder = "Assets/ExportedIcons";

    [MenuItem("Tools/Prefab Icon Exporter")]
    public static void ShowWindow()
    {
        GetWindow<PrefabIconExporter>("Prefab Icon Exporter");
    }

    private void OnGUI()
    {
        GUILayout.Label("Batch Export Transparent Prefab Icons", EditorStyles.boldLabel);

        iconSize = EditorGUILayout.IntField("Icon Size", iconSize);
        cameraOffset = EditorGUILayout.Vector3Field("Camera Offset", cameraOffset);
        backgroundColor = EditorGUILayout.ColorField("Background Color", backgroundColor);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        GUILayout.Space(10);

        if (GUILayout.Button("Export Selected Prefabs"))
        {
            ExportSelectedPrefabs();
        }
    }

    private void ExportSelectedPrefabs()
    {
        Object[] selectedObjects = Selection.objects;

        if (selectedObjects.Length == 0)
        {
            Debug.LogWarning("No prefabs selected.");
            return;
        }

        if (!AssetDatabase.IsValidFolder(outputFolder))
        {
            string[] split = outputFolder.Split('/');
            string currentPath = split[0];

            for (int i = 1; i < split.Length; i++)
            {
                string nextPath = currentPath + "/" + split[i];
                if (!AssetDatabase.IsValidFolder(nextPath))
                {
                    AssetDatabase.CreateFolder(currentPath, split[i]);
                }
                currentPath = nextPath;
            }
        }

        foreach (Object obj in selectedObjects)
        {
            GameObject prefab = obj as GameObject;
            if (prefab == null)
                continue;

            ExportPrefabIcon(prefab);
        }

        AssetDatabase.Refresh();
        Debug.Log("Finished exporting prefab icons.");
    }

    private void ExportPrefabIcon(GameObject prefab)
    {
        GameObject tempInstance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        if (tempInstance == null)
        {
            Debug.LogWarning($"Could not instantiate prefab: {prefab.name}");
            return;
        }

        // Reset transform
        tempInstance.transform.position = Vector3.zero;
        tempInstance.transform.rotation = Quaternion.identity;

        // Get bounds of all renderers
        Renderer[] renderers = tempInstance.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning($"No renderers found on prefab: {prefab.name}");
            DestroyImmediate(tempInstance);
            return;
        }

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds);
        }

        Vector3 center = bounds.center;
        float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

        // Create temporary camera
        GameObject camGO = new GameObject("TempIconCamera");
        Camera cam = camGO.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = backgroundColor;
        cam.orthographic = false;
        cam.fieldOfView = 30f;
        cam.nearClipPlane = 0.01f;
        cam.farClipPlane = 100f;

        // Camera position
        cam.transform.position = center + cameraOffset.normalized * maxSize * 2.2f;
        cam.transform.LookAt(center);

        // Create temporary light
        GameObject lightGO = new GameObject("TempIconLight");
        Light lightComp = lightGO.AddComponent<Light>();
        lightComp.type = LightType.Directional;
        lightComp.intensity = 1.2f;
        lightGO.transform.rotation = Quaternion.Euler(40f, -30f, 0f);

        // Render texture
        RenderTexture rt = new RenderTexture(iconSize, iconSize, 24);
        cam.targetTexture = rt;
        RenderTexture.active = rt;

        cam.Render();

        Texture2D tex = new Texture2D(iconSize, iconSize, TextureFormat.RGBA32, false);
        tex.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);
        tex.Apply();

        byte[] pngBytes = tex.EncodeToPNG();
        string filePath = Path.Combine(outputFolder, prefab.name + ".png");
        File.WriteAllBytes(filePath, pngBytes);

        // Cleanup
        RenderTexture.active = null;
        cam.targetTexture = null;
        DestroyImmediate(rt);
        DestroyImmediate(tex);
        DestroyImmediate(camGO);
        DestroyImmediate(lightGO);
        DestroyImmediate(tempInstance);

        Debug.Log($"Exported icon: {filePath}");
    }
}