using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ManagerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject[] pointSpawn;
    [SerializeField] private float timeBetweenSpawn;
    [SerializeField] private float startTimer;
    [SerializeField] private EnemyCar carInstance;
    private readonly List<GameObject> _spawnObjects = new List<GameObject>();
    private readonly List<GameObject> _spawnedObjects = new List<GameObject>();
    private bool _isStartedSpawn = false;
    private Coroutine _spawnCoroutine;

    private void Start()
    {
        Time.timeScale = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (Time.time >= startTimer && !_isStartedSpawn)
        { 
            Spawn();
            _isStartedSpawn = true;
        }

        if (_isStartedSpawn && _spawnCoroutine == null && _spawnedObjects.Count == 0)
        {
            _spawnCoroutine = StartCoroutine(SpawnerWait());
            
        }
    }

    private IEnumerator SpawnerWait()
    {
        yield return new WaitForSeconds(timeBetweenSpawn);
        Spawn();
        StopCoroutine(_spawnCoroutine);
        _spawnCoroutine = null;
    }
    private void Spawn()
    {
        int count = 0;
        while (count != 2)
        {
            var spawnObject = pointSpawn[Random.Range(0, pointSpawn.Length)];
            if (!_spawnObjects.Contains(spawnObject))
            {
                _spawnObjects.Add(spawnObject);
                count++;
            }
        }

        foreach (var obj in _spawnObjects.ToList())
        {
            var car = Instantiate(carInstance, obj.transform.position, Quaternion.identity);
            car.destroyAction += DestroyCar;
            _spawnObjects.Remove(obj);
            _spawnedObjects.Add(car.gameObject);
        }
    }
    private void DestroyCar(GameObject obj)
    {
        _spawnedObjects.Remove(obj);
    }
}
