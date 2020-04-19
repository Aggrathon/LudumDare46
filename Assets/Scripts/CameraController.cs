using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public static CameraController active { get; protected set; }

    public CinemachineVirtualCamera topCamera;
    public CinemachineVirtualCamera shoulderCamera;
    public CinemachineBrain cameraBrain;

    public Transform activeUnit;

    Transform target;

    public float turnSpeed = 1f;
    public float moveSpeed = 10f;

    enum State {
        shoulder,
        top
    }

    State state;

    void Start()
    {
        var go = new GameObject();
        go.name = "Camera Target";
        target = go.transform;
        target.parent = transform;
        topCamera.Follow = target;
        shoulderCamera.Follow = target;
        ActivateTop();
        active = this;
    }

    void Update() {
        if (state == State.shoulder) {
            if (Input.GetMouseButton(1))
                shoulderCamera.transform.Rotate(Vector3.up, -Input.GetAxis("Mouse X") * turnSpeed);
            if (Input.GetKeyUp(KeyCode.Tab))
                ActivateTop();
            if (Input.GetKeyUp(KeyCode.Space))
                shoulderCamera.transform.Rotate(0f, 90f, 0f, Space.World);
        } else {
            if (Input.GetKeyUp(KeyCode.Tab))
                ActivateShoulder();
            if (Input.GetKeyUp(KeyCode.Space))
                ActivateTop();
            if (Input.GetMouseButton(1))
                topCamera.transform.Rotate(0f, -Input.GetAxis("Mouse X") * turnSpeed, 0f, Space.World);
            float rotation = topCamera.transform.rotation.eulerAngles.y;
            Vector3 dir = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            target.position += Quaternion.Euler(0f, rotation, 0f) * dir * (Time.deltaTime * moveSpeed);
        }
    }

    public void FocusOn(Transform tracker) {
        activeUnit = tracker;
        ActivateTop();
    }

    public void ActivateShoulder() {
        shoulderCamera.Follow = activeUnit;
        shoulderCamera.transform.rotation = Quaternion.Euler(0f, topCamera.transform.rotation.eulerAngles.y, 0f);
        topCamera.Priority = 0;
        shoulderCamera.Priority = 10;
        state = State.shoulder;
    }

    public void ActivateTop() {
        target.position = activeUnit.position;
        topCamera.Priority = 10;
        shoulderCamera.Priority = 0;
        state = State.top;
    }
}
