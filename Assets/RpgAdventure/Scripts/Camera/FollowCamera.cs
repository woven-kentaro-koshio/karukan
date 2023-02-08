using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    // Update is called once per frame
    void LateUpdate()
    {
        if (!target)
        {
            return;
        }

        // current camera angle
        float currentRotationAngle = transform.eulerAngles.y;
        // current player angle
        float wantedRotationAngle = target.eulerAngles.y;

        // desired camera angle (interpolate for smooth movement of camera)
        float nextRotationAngle = Mathf.Lerp(currentRotationAngle, wantedRotationAngle, 0.5f);
        // desired camera rotation
        Quaternion nextRotation = Quaternion.Euler(0, nextRotationAngle, 0);
        // desired camera position
        transform.position = target.position - (nextRotation * Vector3.forward) * 10.0f + Vector3.up * 5.0f;

        // 
        transform.LookAt(target);







    }
}
