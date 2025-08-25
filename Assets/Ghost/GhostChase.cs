using System.Collections;
using UnityEngine;

public class GhostChase : MonoBehaviour
{
    public float moveSpeed = 3f;           // Tốc độ di chuyển của ma
    public Transform player;               // Tham chiếu tới người chơi
    private Vector3 startPosition;         // Vị trí ban đầu của ma
    private bool isChasing = false;        // Trạng thái truy đuổi
    private Coroutine returnCoroutine;     // Lưu Coroutine quay về

    void Start()
    {
        startPosition = transform.position; // Lưu vị trí gốc của ma
    }

    void Update()
    {
        if (isChasing && player != null)
        {
            // Truy đuổi người chơi
            transform.position = Vector3.MoveTowards(
                transform.position,
                player.position,
                moveSpeed * Time.deltaTime
            );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
            player = other.transform;

            // Nếu đang trong trạng thái chuẩn bị quay về thì hủy
            if (returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine);
                returnCoroutine = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;

            // Bắt đầu đếm thời gian quay về
            returnCoroutine = StartCoroutine(ReturnAfterDelay(5f));
        }
    }

    IEnumerator ReturnAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Quay về vị trí ban đầu
        while (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPosition,
                moveSpeed * Time.deltaTime
            );
            yield return null;
        }

        // Khi về tới chỗ cũ
        returnCoroutine = null;
    }
}
