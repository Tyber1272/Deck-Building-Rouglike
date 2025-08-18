using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript : MonoBehaviour
{
    Vector2 mouseMove;
    [SerializeField] float rotationDecreaseX;
    [SerializeField] float rotationDecreaseY;
    [SerializeField] float rotationDelayX;
    [SerializeField] float rotationDelayY;
    void Start()
    {
        
    }

    void Update()
    {
        mouseMove.x = (Input.mousePosition.x / rotationDecreaseX) + rotationDelayX;
        mouseMove.y = (Input.mousePosition.y / rotationDecreaseY) + rotationDelayY;
        transform.localRotation = Quaternion.Euler(-mouseMove.y, mouseMove.x, 0);
    }
}
