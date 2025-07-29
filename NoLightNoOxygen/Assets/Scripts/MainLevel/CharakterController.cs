using UnityEngine;
using UnityEngine.SceneManagement;


public class CharakterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera cam;
    public GameObject gameOver;

    //Oxygen System
    public int oxygen = 100;
    public float oxygenDrainRate = 1f;
    private float oxygenTimer = 0f;

    private Rigidbody rb;
    private Vector3 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // prevent physics from rotating the body
    }

    void Update()
    {
        // Get movement input (WASD or arrow keys)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        // Combine input into one vector (world-space direction)
        moveInput = new Vector3(moveX, 0f, moveZ).normalized;

        // Rotate to face the mouse
        RotateTowardsMouse();

        //Oxygen Timer
        oxygenTimer += Time.deltaTime;
        if (oxygenTimer >= 1f)
        {
            oxygen -= (int)oxygenDrainRate;
            //oxygen stayes at 0
            oxygen = Mathf.Max(oxygen, 0); 
            oxygenTimer = 0f;
            Debug.Log("OXygen:" + oxygen);
        }
    }

    void FixedUpdate()
    {
        // Move the player based on input
        rb.MovePosition(rb.position + moveInput * moveSpeed * Time.fixedDeltaTime);
    }

    void RotateTowardsMouse()
    {
        // Ray from camera through mouse position
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float distance))
        {
            Vector3 targetPoint = ray.GetPoint(distance);
            Vector3 direction = (targetPoint - transform.position).normalized;

            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                // Instantly rotate to look at the mouse
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameOver.SetActive(true);
            Debug.Log("Enemy Collison" + gameObject.name);

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                enemy.SetActive(false); // Or disable AI scripts if you prefer
            }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

}