using PurrNet;
using UnityEngine;

public class CubeSpawner : NetworkBehaviour
{
    public GameObject cubePrefab;
    protected override void OnSpawned(bool asServer)
    {
        base.OnSpawned(asServer);

        enabled = isOwner;
    }

    /*
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(cubePrefab, transform.position + transform.forward, transform.rotation);
        }
    }
    */
}
