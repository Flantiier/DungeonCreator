using UnityEngine;

public class DragCardTest : MonoBehaviour
{
    #region Variables
    public Camera cam;
    private Vector3 _lastPosition;
    private Vector3 _mousePos;
    #endregion

    #region Builts_In
    private void Awake()
    {
        _lastPosition = transform.position;
    }

    private void OnMouseDown()
    {
    }

    private void OnMouseDrag()
    {
        Debug.Log("Drag");
        transform.position = GetMousePos();
    }

    private void OnMouseUp()
    {
        Debug.Log("Up");
        transform.position = _lastPosition;
    }
    #endregion

    #region Methods
    private Vector3 GetMousePos()
    {
        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(screenPos);
    }
    #endregion
}
