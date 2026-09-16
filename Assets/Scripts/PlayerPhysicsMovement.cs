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

        //All clients set it to kinematic, so only the server runs physics!
        rb.isKinematic = !isServer;
        //Only the owner has it enabled, as to run Update()
        enabled = isOwner;

        //Only the owner runs OnTick to send input to the server
        if (isOwner)
        {
            networkManager.onTick += OnTick;
        }
        Debug.Log($"{name} | isOwner: {isOwner} | enabled: {enabled}");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        //Unsubcribing again for cleanup
        networkManager.onTick -= OnTick;
    }

    private void Update()
    {
        //We have to store the input to be used during the next tick
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _willJump = true;
        }
    }

    private void OnTick(bool asServer)
    {
        //In case of a host setup, we don't want this to run twice.
        if (asServer)
            return;

        //We generate the input struct that will be sent to the server
        var input = new InputData()
        {
            movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")),
            jump = _willJump
        };

        _willJump = false;

        //We send the input to the server
        Move(input);
    }

    [ServerRpc]
    private void Move(InputData inputData)
    {
        var movement = new Vector3(inputData.movement.x, 0, inputData.movement.y) * moveForce;
        
        rb.AddForce(movement);
        
        if (inputData.jump)
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision other)
    {
        //Other than the if-statement here, this is single-player code from the
        //perspective of the server
        if (!isServer)
            return;

        if (!other.gameObject.TryGetComponent(out PlayerPhysicsMovement otherPlayer))
            return;

        var direction = (transform.position - other.transform.position).normalized;
        rb.AddForce(direction * bounceForce, ForceMode.Impulse);
    }

    private struct InputData
    {
        public Vector2 movement;
        public bool jump;
    }
}
