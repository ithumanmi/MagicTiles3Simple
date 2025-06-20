using UnityEngine;

// Lưu ý: Hãy thiết lập cellSize và cellGap trực tiếp trong Inspector của GridLayout!
public class GridManager : MonoBehaviour
{
    public int gridWidth = 4;  // Số cột
    public int gridHeight = 8; // Số hàng
    // Không thể thay đổi cellSize và cellGap bằng code, hãy setup trong Inspector

    private Grid grid;
    private float[] columnCenters;

    void Awake()
    {
        grid = GetComponent<Grid>();
        CalculateColumnCenters();
    }

    // Tính toán vị trí center X của từng cột (world position)
    void CalculateColumnCenters()
    {
        columnCenters = new float[gridWidth];
        // Lấy thông tin camera
        Camera cam = Camera.main;
        float screenHeight = cam.orthographicSize * 2f;
        float screenWidth = screenHeight * cam.aspect;
        float cellWidth = screenWidth / gridWidth;
        float xStart = -screenWidth / 2f + cellWidth / 2f;
        for (int i = 0; i < gridWidth; i++)
        {
            columnCenters[i] = xStart + i * cellWidth;
        }
    }

    // Lấy vị trí center của cột thứ idx (0-3)
    public Vector3 GetColumnCenter(int idx, float y = 0f)
    {
        if (columnCenters == null || idx < 0 || idx >= columnCenters.Length)
            return Vector3.zero;
        return new Vector3(columnCenters[idx], y, 0f);
    }

    // Lấy vị trí spawn cho tile (ở đầu cột, có offset từ top camera)
    public Vector3 GetSpawnPosition(int lane, float offsetY = 0f)
    {
        // Spawn ở top camera trừ offsetY
        Camera cam = Camera.main;
        float y = cam.orthographicSize - offsetY;
        return GetColumnCenter(lane, y);
    }

    // Kiểm tra vị trí có hợp lệ không
    public bool IsValidPosition(Vector3Int gridPos)
    {
        return gridPos.x >= 0 && gridPos.x < gridWidth &&
               gridPos.y >= 0 && gridPos.y < gridHeight;
    }

    // Lấy grid position từ world position
    public Vector3Int WorldToGridPosition(Vector3 worldPos)
    {
        return grid.WorldToCell(worldPos);
    }

    // Lấy world position từ grid position
    public Vector3 GridToWorldPosition(Vector3Int gridPos)
    {
        return grid.CellToWorld(gridPos);
    }
} 