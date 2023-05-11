using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace _Scripts.Cameras
{
    public class AnimatedCamera : MonoBehaviour
    {
        [SerializeField] private int currentPoint = 0;
        [SerializeField] private bool resetAtStart;
        [SerializeField, Range(0, 0.2f)] private float moveSpeed = 0.1f;
        [SerializeField, Range(0, 0.2f)] private float rotateSpeed = 0.01f;
        [SerializeField] private List<CameraWayPoint> wayPoints = new List<CameraWayPoint>();

        private void Awake()
        {
            if (wayPoints.Count <= 0 || currentPoint >= wayPoints.Count || currentPoint < 0)
                return;

            if (resetAtStart)
            {
                transform.position = wayPoints[currentPoint].position;
                transform.eulerAngles = wayPoints[currentPoint].eulers;
            }
        }

        private void FixedUpdate()
        {
            if (wayPoints.Count <= 0 || currentPoint >= wayPoints.Count)
                return;

            GoToWayPoint(wayPoints[currentPoint]);
        }

        private void GoToWayPoint(CameraWayPoint wayPoint)
        {
            if (Vector3.Distance(transform.position, wayPoint.position) > 0.1f)
            {
                float x = Mathf.Lerp(transform.position.x, wayPoint.position.x, moveSpeed);
                float y = Mathf.Lerp(transform.position.y, wayPoint.position.y, moveSpeed);
                float z = Mathf.Lerp(transform.position.z, wayPoint.position.z, moveSpeed);
                transform.position = new Vector3(x, y , z);
            }

            if (transform.eulerAngles != wayPoint.eulers)
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(wayPoint.eulers), rotateSpeed);
        }

        public void SetWayPoint(int value)
        {
            value = Mathf.Clamp(value, 0, wayPoints.Count);
            currentPoint = value;
        }

        [Button("Add from Transform")]
        private void AddFromTransform()
        {
            CameraWayPoint instance = new CameraWayPoint();
            instance.position = transform.position;
            instance.eulers = transform.eulerAngles;
            wayPoints.Add(instance);
        }
    }

    [System.Serializable]
    public struct CameraWayPoint
    {
        public Vector3 position;
        public Vector3 eulers;
    }
}
