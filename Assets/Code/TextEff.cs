using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextEff : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;
    [SerializeField] GameObject objectToDestroy; // Tham chiếu đến GameObject cần xóa
    public string[] stringArray;
    [SerializeField] float timeBtwnChars;
    [SerializeField] float timeBtwnWords;
    int i = 0;

    void Start()
    {
        // Kiểm tra lỗi để tránh crash
        if (_textMeshPro == null || stringArray == null || stringArray.Length == 0)
        {
            Debug.LogWarning("TextMeshProUGUI or stringArray is not set or empty!");
            gameObject.SetActive(false); // Tắt GameObject nếu có lỗi
            return;
        }
        EndCheck();
    }

    public void EndCheck()
    {
        if (i <= stringArray.Length - 1)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TextVisible());
        }
        else
        {
            // Khi tất cả văn bản đã hiển thị, bắt đầu coroutine để xóa GameObject sau 1 giây
            StartCoroutine(DestroyObjectAfterDelay());
        }
    }

    private IEnumerator TextVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalVisibleCharacters = _textMeshPro.textInfo.characterCount;
        int counter = 0;

        while (true)
        {
            int visibleCount = counter % (totalVisibleCharacters + 1);
            _textMeshPro.maxVisibleCharacters = visibleCount;

            if (visibleCount >= totalVisibleCharacters)
            {
                i += 1;
                Invoke("EndCheck", timeBtwnWords);
                break;
            }

            counter += 1;
            yield return new WaitForSeconds(timeBtwnChars);
        }
    }

    private IEnumerator DestroyObjectAfterDelay()
    {
        // Chờ 1 giây trước khi xóa
        yield return new WaitForSeconds(1f);

        // Kiểm tra xem objectToDestroy có tồn tại không trước khi xóa
        if (objectToDestroy != null)
        {
            Destroy(objectToDestroy);
        }

        // Tùy chọn: Tắt hoặc xóa GameObject chứa script này
        gameObject.SetActive(false); // Tắt GameObject
        // Hoặc dùng Destroy(gameObject); để xóa hoàn toàn
    }
}