using PurrNet;
using UnityEngine;

public class RotatingRoom : NetworkBehaviour
{
    [SerializeField] private float rotationSpeed;

    private void FixedUpdate()
    {
        if (!isServer)
            return;

        Rotate_Observer(Time.deltaTime * -rotationSpeed);
    }

    [ObserversRpc]
    private void Rotate_Observer(float speed)
    {
        transform.Rotate(0, 0, speed);
    }
}
