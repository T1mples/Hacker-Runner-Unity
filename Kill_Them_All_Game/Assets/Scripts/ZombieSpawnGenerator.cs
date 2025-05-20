using UnityEngine;

public class ZombieSpawnGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;

    private Vector3[] _spawnPositions;

    private readonly float _yPositionSpawn = 0.0755757093f;
    private readonly float _xPositionSpawn = 0;

    private void Start()
    {
        InitializeSpawnPositions();
        SpawnPrefabs();
    }

    private void InitializeSpawnPositions()
    {
        _spawnPositions = new Vector3[13];
        _spawnPositions[0] = new Vector3(0, _yPositionSpawn, 45);

        for (int i = 1; i < _spawnPositions.Length; i++)
        {
            _spawnPositions[i] = new Vector3(_xPositionSpawn, _yPositionSpawn, 45 + (i * 20));
        }
    }

    private void SpawnPrefabs()
    {
        for (int i = 0; i < _spawnPositions.Length; i++)
        {
            int prefabIndex = Random.Range(0, _prefabs.Length);
            Instantiate(_prefabs[prefabIndex], _spawnPositions[i], Quaternion.identity);
        }
    }
}
