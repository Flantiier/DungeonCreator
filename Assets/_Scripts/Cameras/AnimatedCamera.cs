using UnityEngine;

namespace _Scripts.Cameras
{
    public class AnimatedCamera : MonoBehaviour
    {
        [SerializeField] private int currentPoint = 0;
        [SerializeField] private bool resetAtStart;
        [SerializeField, Range(0, 0.2f)] private float moveSpeed = 0.1f;
        [SerializeField, Range(0, 0.2f)] private float rotateSpeed = 0.01f;
        [SerializeField] private CameraWayPoint[] wayPoints;

        private void Awake()
        {
            if (wayPoints.Length <= 0 || currentPoint >= wayPoints.Length || currentPoint < 0)
                return;

            if (resetAtStart)
                transform.position = wayPoints[currentPoint].position;
        }

        private void FixedUpdate()
        {
            if (wayPoints.Length <= 0 || currentPoint >= wayPoints.Length)
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
            value = Mathf.Clamp(value, 0, wayPoints.Length);
            currentPoint = value;
        }
    }

    [System.Serializable]
    public struct CameraWayPoint
    {
        public Vector3 position;
        public Vector3 eulers;
    }
}
