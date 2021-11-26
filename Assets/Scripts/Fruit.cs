using UnityEngine;
using UnityEngine.Tilemaps;

public class Fruit : MonoBehaviour
{
    private Transform _pacman123;
    private Tilemap _level;
    private FruitSpawn _spawnPoint;

    void Start()
    {
        _pacman123 = GameObject.Find("Pacman").transform;
        _level = GameObject.Find("Pacman").GetComponent<PacmanMoving>().TileMap;
        _spawnPoint = GameObject.Find("FruitSpawnPoint").GetComponent<FruitSpawn>();
    }

    void FixedUpdate()
    {
        if (_level.WorldToCell(transform.position) == _level.WorldToCell(_pacman123.position))
        {
            _spawnPoint.SendScore();
            gameObject.SetActive(false);
        }
    }
}
