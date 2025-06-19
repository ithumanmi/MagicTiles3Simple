using System.Collections.Generic;
using UnityEngine;

public class TilePooler : MonoBehaviour
{
    public static TilePooler Instance;
    public static TilePooler HoldInstance;
    public GameObject tilePrefab;
    public int poolSize = 20;
    public bool isHoldPool = false;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Awake()
    {
        if (isHoldPool) HoldInstance = this;
        else Instance = this;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(tilePrefab);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public GameObject GetTile()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();
            obj.SetActive(true);
            
            // Reset TileController state
            var tileController = obj.GetComponent<TileController>();
            if (tileController != null)
            {
                tileController.ResetState();
            }
            
            // Reset TileControllerHoldToTop state
            var tileHold = obj.GetComponent<TileControllerHoldToTop>();
            if (tileHold != null)
            {
                tileHold.ResetState();
            }
            
            // Reset SpriteRenderer
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = Color.white;
            }
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(tilePrefab);
            return obj;
        }
    }

    public void ReturnTile(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
    }

    // Static API cho quản lý chung
    public static GameObject GetTile(bool isHold)
    {
        return isHold ? HoldInstance.GetTile() : Instance.GetTile();
    }
    public static void ReturnTile(GameObject obj, bool isHold)
    {
        if (isHold) HoldInstance.ReturnTile(obj);
        else Instance.ReturnTile(obj);
    }
} 