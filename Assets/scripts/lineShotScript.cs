using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineShotScript : MonoBehaviour
{
    LineRenderer lineRenderer;
    [SerializeField] float fadeSpeed;
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }
    void Update()
    {
        lineRenderer.startColor = new Color(lineRenderer.startColor.r, lineRenderer.startColor.g, lineRenderer.startColor.b, lineRenderer.startColor.a - fadeSpeed * Time.deltaTime);
        lineRenderer.endColor = new Color(lineRenderer.endColor.r, lineRenderer.endColor.g, lineRenderer.endColor.b, lineRenderer.endColor.a - fadeSpeed * Time.deltaTime);
    }
}
