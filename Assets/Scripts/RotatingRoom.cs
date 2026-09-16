using PurrNet;
using UnityEngine;

public class RotatingRoom : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void Update()
    {
        if (!isServer)
            return;

        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
    }
}
