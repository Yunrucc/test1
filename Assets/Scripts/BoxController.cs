using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoxController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 offset;
    private float zCoord;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 按下滑鼠
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform == transform)
                {
                    isDragging = true;
                    zCoord = Camera.main.WorldToScreenPoint(transform.position).z;
                    offset = transform.position - GetMouseWorldPos();
                    rb.useGravity = false; // 拖曳時先暫時關掉重力
                }
            }
        }

        // 放開滑鼠
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rb.useGravity = true; // 恢復重力
        }
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 targetPos = GetMouseWorldPos() + offset;

            // 限制 Y 高度（避免拖曳時掉進地板）
            targetPos.y = Mathf.Max(targetPos.y, 0.5f); // 依照你的地板高度調整

            rb.MovePosition(targetPos);
        }
    }

    Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
