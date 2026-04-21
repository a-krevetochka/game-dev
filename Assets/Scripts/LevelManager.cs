using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    [SerializeField] private Transform _spawnPoint;

    public static Transform SpawnPoint => instance._spawnPoint;

    void Start()
    {
        instance = this;
    }

}
