using System.Collections.Generic;
using UnityEngine;

public class PlayerHistory : MonoBehaviour
{
    [Tooltip("Thời gian lưu lịch sử (giây)")]
    public float recordTime = 10f;

    private class PositionRecord
    {
        public Vector3 position;
        public float time;
    }

    private List<PositionRecord> positions = new List<PositionRecord>();

    void Update()
    {
        // Thêm vị trí hiện tại vào lịch sử
        positions.Add(new PositionRecord { position = transform.position, time = Time.time });

        // Xóa các vị trí quá cũ (trên 15 giây)
        float thresholdTime = Time.time - recordTime;
        positions.RemoveAll(p => p.time < thresholdTime);
    }

    // Lấy vị trí cách 'recordTime' giây trước
    public Vector3 GetPositionAgo()
    {
        if (positions.Count == 0) return transform.position;
        return positions[0].position; // vị trí cũ nhất trong 15 giây
    }
}
