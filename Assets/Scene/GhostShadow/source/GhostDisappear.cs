using UnityEngine;

public class GhostDisappear : MonoBehaviour
{
    public Transform player;
    public float disappearDistance = 180f;
    public AudioClip disappearSound;

    private bool hasDisappeared = false;

    void Update()
    {
        if (player == null || hasDisappeared) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < disappearDistance)
        {
            hasDisappeared = true;

            Vector3 ghostPos = transform.position;

            // T?o AudioSource "?o" ð? phát âm thanh t?i v? trí con ma
            if (disappearSound != null)
            {
                GameObject audioObj = new GameObject("GhostDisappearSound");
                audioObj.transform.position = ghostPos;

                AudioSource source = audioObj.AddComponent<AudioSource>();
                source.clip = disappearSound;
                source.Play();

                // H?y object âm thanh sau khi phát xong
                Destroy(audioObj, disappearSound.length);
            }

            // Xóa con ma ngay l?p t?c
            Destroy(gameObject);
        }
    }
}
