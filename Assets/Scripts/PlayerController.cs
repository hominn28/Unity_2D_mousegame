using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody2D))]

public class PlayerController : MonoBehaviour
{
    private float elapsedTime = 0f;
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;

    Rigidbody2D rb;

    public UIDocument uiDocument;
  

    private int highScore = 0;
    const string HIGH_SCORE_KEY = "HIGH_SCORE";

    private Label ScoreText;
    private Button RestartButton;
    private Label HighscoreText;

    public GameObject explosionEffect;

    public float maxSpeed = 5f;

    public GameObject boosterFlame;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ScoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        RestartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        RestartButton.style.display = DisplayStyle.None;
        RestartButton.clicked += ReloadScene;
        highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
        HighscoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel"); // 대소문자 정확히!
        if (HighscoreText != null)
            HighscoreText.style.display = DisplayStyle.None;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        Debug.Log("Score: " + score);
        if (ScoreText != null)
            ScoreText.text = "Score: " + score;
        if (Mouse.current.leftButton.isPressed)
        {
            // Calculate mouse direction
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;

            // Move player in direction of mouse
            transform.up = direction;
            rb.AddForce(direction * thrustForce);

            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
            
            }
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, transform.rotation);
        RestartButton.style.display = DisplayStyle.Flex;
        if (score > highScore)
        {
            highScore = (int)score; 
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
            PlayerPrefs.Save();
        }

        if (HighscoreText != null)
        {
            HighscoreText.text = "High Score: " + highScore;
            HighscoreText.style.display = DisplayStyle.Flex;  // 죽었을 때만 보이게
        }
        Destroy(gameObject);
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
