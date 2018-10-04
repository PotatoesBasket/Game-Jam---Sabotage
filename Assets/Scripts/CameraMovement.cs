using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    GameManager gameManager;
    Camera cam;

    public bool isUpdating = false;
    public Vector3 currentPos;
    public Vector3 nextPos;

    float timer;
    float aniLength;
    float speed = 5f;

    private void Awake()
    {
        gameManager = GetComponent<GameManager>();
        cam = Camera.main;

        cam.transform.position = gameManager.cameraPositions[0].position;
    }

    private void Update()
    {
        if (isUpdating)
        {
            float distCovered = (Time.time - timer) * speed;
            float fracJourney = distCovered / aniLength;

            cam.transform.position += Vector3.Lerp(currentPos, nextPos, fracJourney);
        }
        
        if (aniLength >= 0f)
        {
            timer = 0f;
            isUpdating = false;
        }
    }

    public void UpdatePosition()
    {
        timer = Time.time;
        aniLength = Vector3.Distance(currentPos, nextPos);

        currentPos = cam.transform.position;
        nextPos = gameManager.currentCameraPos;

        isUpdating = true;
    }
}