using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MistManager : MonoBehaviour
{
    public static MistManager Instance;

    public GameObject[] mists;
    public Transform mistParent;

    static Transform mainCamTransform;

    Dictionary<MistType, MistInfo> mistMap;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        RenderSettings.fog = true;

        mistMap = new Dictionary<MistType, MistInfo>
        {
            { MistType.Light, new MistInfo(10, 5, 3.5f, 4.5f, Color.white, .01f) },
            { MistType.Medium, new MistInfo(20, 5, 4, 4.5f, Color.white, .1f) },
            { MistType.Heavy, new MistInfo(30, 5, 4, 5, new Color(.8f, .8f, .8f), .25f) },
        };

        if (!mainCamTransform) mainCamTransform = Camera.main.transform;
    }

    public void ShowMist(MistType mistType)
    {
        DestroyAllMists();
        SpawnMist(mistMap[mistType]);
    }

    void DestroyAllMists()
    {
        foreach (Transform mist in mistParent) Destroy(mist.gameObject);
    }

    void SpawnMist(MistInfo mistInfo)
    {
        int step = 360 / mistInfo.mistCount;

        for (int i = 0; i < mistInfo.mistCount; i++)
        {
            Vector3 spawnPos = new Vector3(Mathf.Cos(i * step) * mistInfo.spawnRadius, Random.Range(mistInfo.mistHeightLow, mistInfo.mistHeightHigh), Mathf.Sin(i * step) * mistInfo.spawnRadius);
            Instantiate(mists[Random.Range(0, mists.Length)], spawnPos, Quaternion.identity, mistParent)
                .transform.LookAt(mainCamTransform);
        }

        RenderSettings.fogColor = mistInfo.fogColor;
        RenderSettings.fogDensity = mistInfo.fogDensity;
    }
}

public struct MistInfo
{
    public int mistCount;
    public int spawnRadius;
    public float mistHeightLow;
    public float mistHeightHigh;
    public Color fogColor;
    public float fogDensity;

    public MistInfo(int mistCount, int spawnRadius, float mistHeightLow, float mistmistHeightHigh, Color fogColor, float fogDensity)
    {
        this.mistCount = mistCount;
        this.spawnRadius = spawnRadius;
        this.mistHeightLow = mistHeightLow;
        this.mistHeightHigh = mistmistHeightHigh;
        this.fogColor = fogColor;
        this.fogDensity = fogDensity;
    }
}

public enum MistType
{
    Light, Medium, Heavy
}
