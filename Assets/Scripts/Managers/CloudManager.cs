using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public static CloudManager Instance;

    public GameObject[] clouds;

    static Transform mainCamTransform;

    Dictionary<CloudType, CloudInfo> cloudMap;

    void Awake()
    {
        if (!Instance)
            Instance = this;

        cloudMap = new Dictionary<CloudType, CloudInfo>
        {
            { CloudType.Light, new CloudInfo(50, 150, Color.white, 0, Color.clear) },
            { CloudType.Medium, new CloudInfo(200, 150, Color.white, 0, Color.clear) },
            { CloudType.Heavy, new CloudInfo(250, 100, new Color( .8f, .8f, .8f), 0, Color.clear) },
            { CloudType.Thunder, new CloudInfo(300, 75, Color.grey, .1f, Color.grey)  }
        };
    }

    public void SpawnClouds(CloudType cloudType)
    {
        var radius = cloudMap[cloudType].spawnRadius;
        SetCloudColor(cloudMap[cloudType].cloudColor);
        if (!mainCamTransform) mainCamTransform = Camera.main.transform;

        for (int i = 0; i < cloudMap[cloudType].cloudCount; i++)
            Instantiate(clouds[Random.Range(0, clouds.Length)], new Vector3(Random.Range(-radius, radius), 30, Random.Range(-radius, radius)), Quaternion.identity)
                .transform.LookAt(mainCamTransform);

        RenderSettings.fogColor = cloudMap[cloudType].fogColor;
        RenderSettings.fogDensity = cloudMap[cloudType].fogDensity;
    }

    public void SetCloudColor(Color color)
    {
        foreach (var cloud in clouds) cloud.GetComponent<SpriteRenderer>().color = color;
    }
}

public struct CloudInfo
{
    public int cloudCount;
    public int spawnRadius;
    public Color cloudColor;
    public float fogDensity;
    public Color fogColor;

    public CloudInfo(int cloudCount, int spawnRadius, Color cloudColor, float fogDensity, Color fogColor)
    {
        this.cloudCount = cloudCount;
        this.spawnRadius = spawnRadius;
        this.cloudColor = cloudColor;
        this.fogDensity = fogDensity;
        this.fogColor = fogColor;
    }
}

public enum CloudType
{
    Light, Medium, Heavy, Thunder
}
