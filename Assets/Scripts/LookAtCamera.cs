using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Transform cam;
    private Quaternion initialRotation;

    private void Start()
    {
        cam = FindAnyObjectByType<Camera>().GetComponent<Transform>();
        initialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(cam);
    }

    private void LateUpdate()
    {
        transform.rotation = initialRotation;
    }
}
