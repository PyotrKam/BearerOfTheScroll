using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;           
    [SerializeField] private Vector3 offset = new Vector3(0, 8f, -6f);  
    [SerializeField] private float smoothSpeed = 5f;

    [SerializeField] private float dragSpeed = 0.5f;

    private Vector3 lastPanPosition;
    private bool isDragging = false;

    private void Update()
    {
        HandleDragInput();
    }

    private void LateUpdate()
    {
        if (target == null || isDragging) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    private void HandleDragInput()
    {
        // PC
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

        // Phone
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
                Vector3 delta = (Vector3)touch.position - (Vector3)lastPanPosition;
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


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
