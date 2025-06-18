using UnityEngine;

public class GridLinesDrawer : MonoBehaviour
{
    public int columnCount = 4;
    public Camera mainCamera;
    public Color lineColor = Color.white;
    public float lineWidth = 0.05f;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        DrawGridLines();
    }

    void DrawGridLines()
    {
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;
        float cellWidth = screenWidth / columnCount;

        float yMin = -screenHeight / 2f;
        float yMax = screenHeight / 2f;

        for (int i = 1; i < columnCount; i++)
        {
            float x = -screenWidth / 2f + i * cellWidth;
            GameObject lineObj = new GameObject("GridLine_" + i);
            lineObj.transform.parent = this.transform;
            var lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(x, yMin, 0));
            lr.SetPosition(1, new Vector3(x, yMax, 0));
            lr.startWidth = lineWidth;
            lr.endWidth = lineWidth;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = lineColor;
            lr.endColor = lineColor;
            lr.sortingOrder = 10; // Đảm bảo vẽ trên background
        }
    }
} 