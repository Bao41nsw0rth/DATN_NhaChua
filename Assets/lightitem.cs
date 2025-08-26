using UnityEngine;

public class LightOnProximityOrHover : MonoBehaviour
{
    private Light objectLight;
    private Transform player;
    public float activationDistance = 5f; // Khoảng cách để bật đèn khi người chơi đến gần

    void Start()
    {
        // Lấy component Light
        objectLight = GetComponent<Light>();
        if (objectLight == null)
        {
            objectLight = gameObject.AddComponent<Light>();
            objectLight.enabled = false; // Tắt đèn mặc định
            objectLight.type = LightType.Point; // Có thể thay đổi loại đèn (Point, Spot, etc.)
            objectLight.color = Color.yellow; // Màu đèn, có thể tùy chỉnh
            objectLight.intensity = 10f; // Độ sáng đèn
            objectLight.range = 15f; // Phạm vi chiếu sáng
        }

        // Tìm player (giả sử player có tag "Player")
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Kiểm tra khoảng cách từ game object đến player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= activationDistance)
        {
            objectLight.enabled = true; // Bật đèn khi người chơi đến gần
        }
        else
        {
            // Chỉ tắt đèn nếu không hover chuột
            if (!IsMouseOver())
            {
                objectLight.enabled = false;
            }
        }
    }

    void OnMouseEnter()
    {
        // Bật đèn khi chuột hover
        objectLight.enabled = true;
    }

    void OnMouseExit()
    {
        // Tắt đèn khi chuột rời, nhưng chỉ nếu ngoài khoảng cách đến player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer > activationDistance)
        {
            objectLight.enabled = false;
        }
    }

    private bool IsMouseOver()
    {
        // Kiểm tra xem chuột có đang hover trên object không
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }
        return false;
    }
}