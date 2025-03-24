using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelPrefab: MonoBehaviour
{
    [SerializeField]
    private Vector2Int size;

    [SerializeField]
    private Tilemap collisions, backgrounds;

    [SerializeField]
    private Vector2Int entrancePoint, exitPoint;

    //todo: add platforms and objects

    public Vector2Int Size => size;

    public Tilemap Collisions => collisions;

    public Tilemap Backgrounds => backgrounds;

    public Vector2Int Entrance => entrancePoint;

    public Vector2Int Exit => exitPoint;
}
