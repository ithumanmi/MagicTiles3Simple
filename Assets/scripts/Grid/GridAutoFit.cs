using UnityEngine;

[RequireComponent(typeof(Grid))]
public class GridAutoFit : MonoBehaviour
{
    public int columnCount = 4;
    public int rowCount = 8;
    public float cellHeight = 2f;
    public Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        FitGridToScreen();
    }

    void FitGridToScreen()
    {
        if (mainCamera == null) return;
        float screenHeight = mainCamera.orthographicSize * 2f;
        float screenWidth = screenHeight * mainCamera.aspect;

        float cellWidth = screenWidth / columnCount;

        // Lấy GridLayout
        GridLayout gridLayout = GetComponent<GridLayout>();

        // Gợi ý: tự động căn giữa grid
        float gridWorldWidth = cellWidth * columnCount;
        float gridWorldHeight = cellHeight * rowCount;
        Vector3 gridCenter = new Vector3(0, 0, 0);
        transform.position = new Vector3(-screenWidth/2 + gridWorldWidth/2, -screenHeight/2 + gridWorldHeight/2, transform.position.z);
    }
} 