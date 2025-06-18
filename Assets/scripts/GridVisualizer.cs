using UnityEngine;

public class GridVisualizer : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject gridCellPrefab; // Prefab đơn giản để hiển thị grid
    public bool showGrid = true;
    
    void Start()
    {
        if (showGrid && gridCellPrefab != null)
        {
            CreateVisualGrid();
        }
    }
    
    void CreateVisualGrid()
    {
        Grid grid = gridManager.GetComponent<Grid>();
        
        for (int x = 0; x < gridManager.gridWidth; x++)
        {
            for (int y = 0; y < gridManager.gridHeight; y++)
            {
                Vector3Int gridPos = new Vector3Int(x, y, 0);
                Vector3 worldPos = grid.CellToWorld(gridPos);
                
                GameObject cell = Instantiate(gridCellPrefab, worldPos, Quaternion.identity);
                cell.transform.parent = transform;
                cell.name = $"GridCell_{x}_{y}";
            }
        }
    }
    
    // Tạo grid cell đơn giản nếu không có prefab
    public void CreateSimpleGridCell()
    {
        GameObject cell = GameObject.CreatePrimitive(PrimitiveType.Quad);
        cell.transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        
        // Tạo material đơn giản với outline
        Material mat = new Material(Shader.Find("Standard"));
        mat.color = new Color(1f, 1f, 1f, 0.1f);
        cell.GetComponent<Renderer>().material = mat;
        
        gridCellPrefab = cell;
    }
} 