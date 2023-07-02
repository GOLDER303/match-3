using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemGridVisual : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;

    private GemGridPosition gemGridPosition;
    private Vector3 startPosition;
    private float sinTime;

    private void Start()
    {
        startPosition = transform.position;
    }

    public void Setup(GemGridPosition gemGridPosition)
    {
        this.gemGridPosition = gemGridPosition;
        gemGridPosition.OnGemGridPositionDestroyed += OnGemGridPositionDestroyedVisual;
    }

    private void Update()
    {
        Vector3 targetPosition = gemGridPosition.worldPosition;

        gemGridPosition.isMoving = targetPosition != transform.position;

        if (!gemGridPosition.isMoving)
        {
            startPosition = transform.position;
            sinTime = 0;
        }
        else
        {
            sinTime += Time.deltaTime * moveSpeed;
            sinTime = Mathf.Clamp(sinTime, 0, Mathf.PI);
            float t = evaluate(sinTime);
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }

    }

    private float evaluate(float x)
    {
        return .5f * Mathf.Sin(x - Mathf.PI / 2f) + .5f;
    }

    private void OnGemGridPositionDestroyedVisual()
    {
        Instantiate(gemGridPosition.gemSO.destroyParticleSystem, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        gemGridPosition.OnGemGridPositionDestroyed -= OnGemGridPositionDestroyedVisual;
    }
}
