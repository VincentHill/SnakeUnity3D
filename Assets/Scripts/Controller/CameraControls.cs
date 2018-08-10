using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour {

    //How quickly to rotate the object.
    float sensitivity = 50;

    void Update() {

        if (Input.GetMouseButton(0)) {
            float rotationX = Input.GetAxis("Mouse X") * sensitivity;
            //float rotationY = Input.GetAxis("Mouse Y") * sensitivity;

            transform.Rotate(transform.up, -Mathf.Deg2Rad * rotationX);
            //transform.Rotate(transform.right, -Mathf.Deg2Rad * rotationY);
            Vector3 euler = transform.rotation.eulerAngles;
            euler.z = 0f;
            transform.rotation = Quaternion.Euler(euler);
        }
    }

    public IEnumerator Idle() {
        float angle = 1f / 3f;
        while (true) {
            transform.Rotate(Vector3.up, angle);
            yield return null;
        }
    }
}