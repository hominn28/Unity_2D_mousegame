using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{

    public float minSize = 10f;
    public float maxSize = 20f;
    public float minSpeed = 100f;
    public float maxSpeed = 350f;

    public float maxSpinSpeed = 10f;

    public Sprite[] frames;      // 슬라이스된 6개 프레임을 여기로 드래그
    public float fps = 8f;       // 초당 프레임 (6~10 추천)
    private SpriteRenderer sr;
    private float animTimer;
    private int frameIndex;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        float randomSize = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(randomSize, randomSize, 1);

        float randomSpeed = Random.Range(minSpeed, maxSpeed);
        Vector2 randomDirection = Random.insideUnitCircle;

        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(randomDirection * randomSpeed);

        float randomTorque = Random.Range(-maxSpinSpeed, maxSpinSpeed);
        rb.AddTorque(randomTorque);

        sr = GetComponent<SpriteRenderer>();
        if (sr != null && frames != null && frames.Length > 0)
        {
            frameIndex = Random.Range(0, frames.Length); // 동시 생성시 싱크 어긋나게
            sr.sprite = frames[frameIndex];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (sr != null && frames != null && frames.Length > 0 && fps > 0f)
        {
            animTimer += Time.deltaTime;
            float frameTime = 1f / fps;
            if (animTimer >= frameTime)
            {
                animTimer -= frameTime;
                frameIndex = (frameIndex + 1) % frames.Length;
                sr.sprite = frames[frameIndex];
            }
        }
    }
}
