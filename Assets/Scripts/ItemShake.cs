using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemShake : MonoBehaviour
{
    public bool isPlaying = false;
    float timer = 0f;
    float aniLength = 0.5f;

    float speed = 50f;
    float radius = 0.05f;

    Vector3 originalPos;
    Vector3 offset;

    private void Start()
    {
        originalPos = transform.position;
        offset = transform.position;
    }

    private void Update()
    {
        if (isPlaying)
        {
            timer += Time.deltaTime;

            if (timer >= aniLength)
            {
                isPlaying = false;
                timer = 0f;
            }

            transform.position = new Vector3((radius * Mathf.Cos(Time.time * speed)) + offset.x, offset.y, offset.z);
        }
        else
        {
            transform.position = originalPos;
        }
    }
}