using PurrNet;
using UnityEngine;

public class KillZone : NetworkBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (!isServer)
            return;

        if (!other.gameObject.TryGetComponent(out PlayerPhysicsMovement otherPlayer))
            return;

        GameManager.killCounter++;
        Destroy(other.gameObject);
    }
}
