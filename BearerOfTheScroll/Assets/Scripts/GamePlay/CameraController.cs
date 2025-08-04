using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Follow")]
    [SerializeField] private Transform target;
    [SerializeField] private float followSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);

    [Header("Pan (drag)")]
    [SerializeField] private float dragSpeed = 0.5f;

    private Vector3 lastPanPosition;
    private bool isDragging = false;

    private void Update()
    {
        HandleDrag();
    }

    private void LateUpdate()
    {
        if (!isDragging && target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            //transform.LookAt(target);
        }
    }

    private void HandleDrag()
    {
        // For PC
        if (Input.GetMouseButtonDown(0))
        {
            lastPanPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastPanPosition;
            PanCamera(delta);
            lastPanPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // For mobile version
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                lastPanPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 delta = (Vector3)touch.position - lastPanPosition;
                PanCamera(delta);
                lastPanPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    private void PanCamera(Vector3 delta)
    {
        Vector3 pan = new Vector3(-delta.x, 0, -delta.y) * dragSpeed * Time.deltaTime;
        transform.Translate(pan, Space.World);
    }

    //Its for my GameProcess.cs
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
