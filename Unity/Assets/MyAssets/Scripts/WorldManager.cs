using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public Camera mainCamera;
    public List<GameObject> projectilePrefabs;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        mainCamera = Camera.main;
    }

    public void Shoot(GameObject ignore, Vector3 projectileStartPos, Vector3 projectileDirection, int projectileIndex)
    {
        if (projectileIndex < projectilePrefabs.Count)
        {
            GameObject.Instantiate(projectilePrefabs[projectileIndex], projectileStartPos, new Quaternion()).GetComponent<Projectile>().Init(ignore, projectileDirection);;
        }
    }
}
