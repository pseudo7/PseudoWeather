using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindMill : MonoBehaviour
{
    public float speed = 100;

    void Update()
    {
        transform.GetChild(0).Rotate(0, 0, -speed * Time.deltaTime);
    }
}
