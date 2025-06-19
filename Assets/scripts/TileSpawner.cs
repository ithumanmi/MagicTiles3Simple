using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public static TileSpawner Instance;
    public GridManager gridManager;
    public float beatInterval = 0.5f; // Thời gian giữa các nhịp (giây)
    public float holdIntervalMin = 1f;
    public float holdIntervalMax = 2f;
    public AudioSource musicSource;
    public int laneCount = 4;
    private bool[] laneOccupied;

    private float timer = 0f;
    private float holdTimer = 0f;
    private float holdInterval;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        laneOccupied = new bool[laneCount];
        holdInterval = Random.Range(holdIntervalMin, holdIntervalMax);
        if (musicSource != null)
            musicSource.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;
        holdTimer += Time.deltaTime;

        // Spawn tile thường
        if (timer >= beatInterval)
        {
            SpawnTile();
            timer -= beatInterval;
        }

        //// Spawn tile hold sau mỗi holdInterval
        //if (holdTimer >= holdInterval)
        //{
        //    SpawnTile(true);
        //    holdTimer = 0f;
        //    holdInterval = Random.Range(holdIntervalMin, holdIntervalMax); // random lại cho lần sau
        //}
    }

    void SpawnTile(bool spawnHold = false)
    {
        int lane = Random.Range(0, gridManager.gridWidth);
        float tileHeight = 1f;
        GameObject tileObj = null;
        if (spawnHold && TilePooler.HoldInstance != null)
        {
            tileObj = TilePooler.HoldInstance.GetTile();
        }
        else
        {
            tileObj = TilePooler.Instance.GetTile();
        }
        if (TilePooler.Instance.tilePrefab != null)
        {
            var sr = TilePooler.Instance.tilePrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
                tileHeight = sr.bounds.size.y;
        }
        float topY = Camera.main.orthographicSize;
        float spawnY = topY + tileHeight+ tileHeight/2;
        Vector3 spawnPos = gridManager.GetColumnCenter(lane, spawnY);

        tileObj.transform.position = spawnPos;
        tileObj.transform.rotation = Quaternion.identity;
    }

    public void SetLaneFree(int lane)
    {
        if (lane >= 0 && lane < laneCount) laneOccupied[lane] = false;
    }
} 