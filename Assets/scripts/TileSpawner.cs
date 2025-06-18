using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    public GridManager gridManager;
    public float beatInterval = 0.5f; // Thời gian giữa các nhịp (giây)
    public AudioSource musicSource;

    private float timer = 0f;

    void Start()
    {
        if (musicSource != null)
            musicSource.Play();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= beatInterval)
        {
            SpawnTile();
            timer -= beatInterval;
        }
    }

    void SpawnTile()
    {
        int lane = Random.Range(0, gridManager.gridWidth);
        float tileHeight = 1f;
        if (TilePooler.Instance.tilePrefab != null)
        {
            var sr = TilePooler.Instance.tilePrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
                tileHeight = sr.bounds.size.y;
        }
        float topY = Camera.main.orthographicSize;
        float spawnY = topY + tileHeight / 2f;
        Vector3 spawnPos = gridManager.GetColumnCenter(lane, spawnY);

        GameObject tileObj = TilePooler.Instance.GetTile();
        tileObj.transform.position = spawnPos;
        tileObj.transform.rotation = Quaternion.identity;
        tileObj.transform.localScale = Vector3.one;
    }
} 