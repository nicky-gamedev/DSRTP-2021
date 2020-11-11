using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerSimpleConfig : MonoBehaviour
{
    public Transform actualFloor;

    [SerializeField] float speed;
    [SerializeField] float impulseJump;
    [SerializeField] float gravity;

    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector3 verticalDirection;
    CharacterController controller;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = Vector3.right * x + Vector3.forward * z;
        var look = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, 0.05f);

        if (Input.GetButton("Jump") && controller.isGrounded)
        {
            verticalDirection.y = Mathf.Sqrt(impulseJump * -2 * gravity);
        }

        if (verticalDirection.y >= gravity * 2)
        {
            verticalDirection.y += gravity * Time.deltaTime;
        }

        controller.Move(moveDirection * speed * Time.deltaTime);
        controller.Move(verticalDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        actualFloor = hit.gameObject.transform;
    }
}
