using UnityEngine;
using cky.Pooling;
using cky.Managers;
using System.Collections;
using cky.Interfaces;

public class CubeSpawner : SingletonNonPersistent<CubeSpawner>
{
    [SerializeField] Transform cubePrefabTr;
    PoolManager _poolManager;
    LevelSettings _levelSettings;
    WaitForSeconds wfsSpawnInterval;
    CubeMaterialManager _cubeMaterialManager;

    private void Start()
    {
        _poolManager = PoolManager.Instance;
        _levelSettings = LevelManager.Instance.levelSettings;
        _cubeMaterialManager = CubeMaterialManager.Instance;

        EventManager.TriggerCubeSpawner += StartSpawning;
        EventManager.GameEnd += GameEnd;
    }

    private void GameEnd(bool obj) => StopAllCoroutines();

    private void StartSpawning()
    {
        wfsSpawnInterval = new WaitForSeconds(_levelSettings.spawnerInterval);

        StartCoroutine(Spawning());
    }

    IEnumerator Spawning()
    {
        while (true)
        {
            Spawn();

            yield return wfsSpawnInterval;
        }
    }

    private void Spawn()
    {
        var cube = _poolManager.Spawn(cubePrefabTr, transform.position, Quaternion.identity);

        if (cube.TryGetComponent<MeshRenderer>(out var mr) == true)
        {
            mr.material = _cubeMaterialManager.cubeMat;
        }

        if (cube.TryGetComponent<IForceable>(out var iForceable) == true)
        {
            iForceable.AddForce(RandomForceVector(), _levelSettings.spawnerThrowPower);
        }
    }

    private Vector3 RandomForceVector() =>
        new Vector3(Random(-_levelSettings.spawnerThrowingMaxValueXZ, _levelSettings.spawnerThrowingMaxValueXZ),
                    Random(_levelSettings.spawnerThrowingMinValueY, _levelSettings.spawnerThrowingMaxValueY),
                    Random(-_levelSettings.spawnerThrowingMaxValueXZ, _levelSettings.spawnerThrowingMaxValueXZ));
    private float Random(float min, float max) => UnityEngine.Random.Range(min, max);
}