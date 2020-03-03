using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public float speed = 3.5f;
    private float gravity = 10f;
    private CharacterController controller;
    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        PlayerMovement();
    }
    void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical);
        Vector3 velocity = direction * speed;
        velocity = Camera.main.transform.TransformDirection(velocity);
        velocity.y -= gravity;
        controller.Move(velocity * Time.deltaTime);


    }
}