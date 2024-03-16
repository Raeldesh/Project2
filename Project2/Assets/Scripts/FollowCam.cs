using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.2f;
    private Vector3 velocity = Vector3.zero;
    private float boundary  = -2f;
    
    void LateUpdate()
    {
        if (target != null && target.position.y > boundary)
        {
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y + 0.5f, transform.position.z);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        else
        {
            this.enabled = false;
        }
    }
}
