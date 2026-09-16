using PurrNet;
using UnityEngine;

public class PlayerPhysicsMovement : NetworkBehaviour
{
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float bounceForce = 10f;
    [SerializeField] private Rigidbody rb;

    private bool _willJump;

    protected override void OnSpawned(bool asServer)
    {
        base.OnSpawned(asServer);
        if (asServer)
            return;

        rb.isKinematic = !isServer;

        enabled = isOwner;

        if (isOwner)
        {
            networkManager.onTick += OnTick;
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        networkManager.onTick -= OnTick;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _willJump = true;
        }
    }

    private void OnTick(bool asServer)
    {
        if (asServer)
            return;



        var input = new InputData()
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            jump = _willJump
        };

        _willJump = false;

        Move(input);
    }

    [ServerRpc]
    private void Move(InputData inputData)
    {
        var movement = new Vector3(inputData.input.x, 0, inputData.input.y) * moveForce;
        
        rb.AddForce(movement);
        
        if (inputData.jump)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isServer)
            return;

        if (!other.gameObject.TryGetComponent(out PlayerPhysicsMovement otherPlayer))
            return;

        var direction = (transform.position - other.transform.position).normalized;
        rb.AddForce(direction * bounceForce, ForceMode.Impulse);
    }

    private struct InputData
    {
        public Vector2 input;
        public bool jump;
    }
}
