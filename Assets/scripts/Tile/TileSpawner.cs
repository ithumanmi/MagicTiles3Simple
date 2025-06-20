using UnityEngine;
using System.Collections.Generic;

public class TileSpawner : MonoBehaviour
{
    public static TileSpawner Instance;
    public GridManager gridManager;
    public float holdIntervalMin = 1f;
    public float holdIntervalMax = 2f;
    public AudioSource musicSource;
    public int laneCount = 4;
    private bool[] laneOccupied;

    private float timer = 0f;
    private float holdTimer = 0f;
    private float holdInterval;
    private bool canSpawnHold = true; // Biến để kiểm soát việc spawn hold tile

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        laneOccupied = new bool[laneCount];
        for (int i = 0; i < laneCount; i++) laneOccupied[i] = false;
        holdInterval = Random.Range(holdIntervalMin, holdIntervalMax);
        if (musicSource != null)
            musicSource.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;
        holdTimer += Time.deltaTime;

        // Spawn tile thường
        if (timer >= GameManager.Instance.beatInterval)
        {
            SpawnTile(false);

            timer -= GameManager.Instance.beatInterval;
        }

        //Spawn tile hold sau mỗi holdInterval
        if (holdTimer >= holdInterval && canSpawnHold)
        {
            SpawnTile(true);
            holdTimer = 0f;
            holdInterval = Random.Range(holdIntervalMin, holdIntervalMax); // random lại cho lần sau
            canSpawnHold = false; // Tạm thời không cho spawn hold tile
            Invoke("EnableHoldSpawn", GameManager.Instance.beatInterval * 2); // Cho phép spawn hold tile sau 2 beat
        }
    }

    void EnableHoldSpawn()
    {
        canSpawnHold = true;
    }

    int GetFreeLane()
    {
        List<int> freeLanes = new List<int>();
        for (int i = 0; i < laneCount; i++)
        {
            if (!laneOccupied[i]) freeLanes.Add(i);
        }
        if (freeLanes.Count == 0) return -1;
        return freeLanes[Random.Range(0, freeLanes.Count)];
    }

    void SpawnTile(bool spawnHold = false)
    {
        int lane = GetFreeLane();
        if (lane == -1) return; // Không còn lane trống, không spawn

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
        float spawnY = topY + tileHeight + tileHeight / 2;
        Vector3 spawnPos = gridManager.GetColumnCenter(lane, spawnY);

        tileObj.transform.position = spawnPos;
        tileObj.transform.rotation = Quaternion.identity;

        // Đánh dấu lane đã bị chiếm
        laneOccupied[lane] = true;

        // Gán laneIndex cho tile để khi tile biến mất sẽ trả lại lane
        var tileController = tileObj.GetComponent<TileController>();
        if (tileController != null) tileController.SetLaneIndex(lane);
        var tileHold = tileObj.GetComponent<TileControllerHoldToTop>();
        if (tileHold != null) tileHold.SetLaneIndex(lane);
    }

    public void SetLaneFree(int lane)
    {
        if (lane >= 0 && lane < laneCount)
            laneOccupied[lane] = false;
    }
} 