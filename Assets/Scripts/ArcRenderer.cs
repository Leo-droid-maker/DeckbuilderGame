using System.Collections.Generic;
using UnityEngine;

public class ArcRenderer : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject dotPrefab;
    public int dotPoolSize = 50;
    private List<GameObject> dotPool = new List<GameObject>();
    private GameObject arrowHeadInstance;

    public float dotsSpacing = 50;
    public float arrowAngleAdjustment = 180;
    public int dotsToSkip = 1;
    private Vector3 arrowDirection;

    void Start()
    {
        arrowHeadInstance = Instantiate(original: arrowPrefab, parent: transform);
        arrowHeadInstance.transform.localPosition = Vector3.zero;
        InitializeDotPool(count: dotPoolSize);
    }

    void InitializeDotPool(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject dotInstance = Instantiate(original: dotPrefab, position: Vector3.zero, rotation: Quaternion.identity, parent: transform);
            dotInstance.SetActive(false);
            dotPool.Add(dotInstance);
        }
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        mousePosition.z = 0;
        Vector3 startPosition = transform.position;
        Vector3 midPoint = CalculateMidPoint(start: startPosition, end: mousePosition);

        UpdateArc(startPosition, midPoint, mousePosition);
        PositionAndRotateArrow(position: mousePosition);
    }

    void UpdateArc(Vector3 start, Vector3 mid, Vector3 end)
    {
        int numDots = Mathf.CeilToInt(f: Vector3.Distance(a: start, b: end) / dotsSpacing);

        for (int i = 0; i < numDots && i < dotPool.Count; i++)
        {
            float t = i / (float)numDots;
            t = Mathf.Clamp(value: t, min: 0f, max: 1f);

            Vector3 position = QuadraticBezierPoint(start, mid, end, t);

            if (i != numDots - dotsToSkip)
            {
                dotPool[i].transform.position = position;
                dotPool[i].SetActive(true);
            }
            if (i == numDots - (dotsToSkip + 1) && i - dotsToSkip + 1 >= 0)
            {
                arrowDirection = dotPool[i].transform.position;
            }
        }
    }

    Vector3 CalculateMidPoint(Vector3 start, Vector3 end)
    {
        Vector3 midPoint = (start + end) / 2;
        float arcHeight = Vector3.Distance(a: start, b: end) / 3f;
        midPoint.y += arcHeight;
        return midPoint;
    }

    Vector3 QuadraticBezierPoint(Vector3 start, Vector3 control, Vector3 end, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = uu * start;
        point += 2 * u * t * control;
        point += tt * end;
        return point;
    }

    void PositionAndRotateArrow(Vector3 position)
    {
        arrowHeadInstance.transform.position = position;
        Vector3 direction = arrowDirection - position;
        float angle = Mathf.Atan2(y: direction.y, x: direction.x) * Mathf.Rad2Deg;
        angle += arrowAngleAdjustment;
        arrowHeadInstance.transform.rotation = Quaternion.AngleAxis(angle: angle, axis: Vector3.forward);
    }
}
