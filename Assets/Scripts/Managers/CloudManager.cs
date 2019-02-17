using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public static CloudManager Instance;

    public Transform cloudParent;
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
        if (!mainCamTransform) mainCamTransform = Camera.main.transform;
    }

    public void ShowClouds(CloudType cloudType)
    {
        DestroyAllClouds();
        SpawnClouds(cloudMap[cloudType].cloudColor, cloudMap[cloudType].cloudCount, cloudMap[cloudType].spawnRadius);

        RenderSettings.fogColor = cloudMap[cloudType].fogColor;
        RenderSettings.fogDensity = cloudMap[cloudType].fogDensity;
    }

    void SpawnClouds(Color cloudColor, int cloudCount, int radius)
    {
        SetCloudColor(cloudColor);
        for (int i = 0; i < cloudCount; i++)
            Instantiate(clouds[Random.Range(0, clouds.Length)], new Vector3(Random.Range(-radius, radius), 30, Random.Range(-radius, radius)), Quaternion.identity, cloudParent)
                .transform.LookAt(mainCamTransform);
    }

    public void SetCloudColor(Color color)
    {
        foreach (var cloud in clouds) cloud.GetComponent<SpriteRenderer>().color = color;
    }

    void DestroyAllClouds()
    {
        foreach (Transform cloud in cloudParent) Destroy(cloud.gameObject);
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
