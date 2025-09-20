using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class OBJSpawner : MonoBehaviour
{
    [SerializeField] private OBJAttributeHolder attributeHolder;
    [SerializeField] private OBJ[] objPrefabs;
    [SerializeField] private Vector2 sizeRange = new Vector2(0.5f, 2f);
    [SerializeField] private Vector2 spawnIntervalRange = new Vector2(0.5f, 1.5f);
    [SerializeField] private float fieldSize = 1.0f;
    [SerializeField] private float objLifeTime = 5.0f;

    private Vector2 spawnRange = new Vector2(-1.0f, 1.0f);
    private Coroutine spawnCoroutine;

    public void SetUp(Vector2 sizeRange = default, float fieldSize = default, Vector2 spawnIntervalRange = default, float objLifeTime = default)
    {
        this.sizeRange = sizeRange == default ? this.sizeRange : sizeRange;
        this.fieldSize = fieldSize == default ? this.fieldSize : fieldSize;
        this.spawnIntervalRange = spawnIntervalRange == default ? this.spawnIntervalRange : spawnIntervalRange;
        spawnRange = new Vector2(-this.fieldSize / 2, this.fieldSize / 2);
    }

    public void StartSpawning()
    {
        if (spawnCoroutine == null)
        {
            spawnCoroutine = StartCoroutine(SpawnOBJRoutine());
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnOBJRoutine()
    {
        while (true)
        {
            SpawnOBJ();
            yield return new WaitForSeconds(Random.Range(spawnIntervalRange.x, spawnIntervalRange.y));
        }
    }

    public void SpawnOBJ()
    {
        OBJ objPrefab = objPrefabs[Random.Range(0, objPrefabs.Length)];
        OBJAttribute attribute = (OBJAttribute)Random.Range(0, System.Enum.GetValues(typeof(OBJAttribute)).Length);
        float size = Random.Range(sizeRange.x, sizeRange.y);
        Vector3 position = new Vector3(Random.Range(spawnRange.x, spawnRange.y), this.transform.position.y, Random.Range(spawnRange.x, spawnRange.y));
        Vector3 rotation = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        OBJ objInstance = Instantiate(objPrefab, position, Quaternion.Euler(rotation), this.transform);
        objInstance.SetUp(attribute, attributeHolder.GetMaterial(attribute), size, objLifeTime);
    }
}