using UnityEngine;
[AddComponentMenu("DangSon/PlayerMovement")]
public class PlayerMovement : MonoBehaviour
{
    [Header("Character")]
    public float speed = 12f;
    public float gravity = -0.981f * 2;
    public float jumHeight = 3f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public float groundDistance = 0.4f;
    private CharacterController controller;
    private Vector3 velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsCheckGround() && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move =(transform.right * x+transform.forward*z);
        controller.Move(move*speed*Time.deltaTime);
        if(Input.GetButtonDown("Jump")&&IsCheckGround()) 
            {
            velocity.y = Mathf.Sqrt(jumHeight - gravity * 2f);
            }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
    }
    bool IsCheckGround()
    {
        bool ground = Physics.CheckSphere(groundCheck.position,groundDistance,groundMask);
        return ground;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}
