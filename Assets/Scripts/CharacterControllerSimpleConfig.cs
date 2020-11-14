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

        if (!moveDirection.Equals(Vector3.zero))
        {
            var look = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, 0.05f);
        }


        if (!controller.isGrounded)
        {
            verticalDirection.y += gravity * Time.deltaTime;
        }
        else
        {
            verticalDirection.y = -2f;

            if (Input.GetButton("Jump"))
            {
                verticalDirection.y = Mathf.Sqrt(impulseJump * -2 * gravity);
            }
        }

        controller.Move(moveDirection * speed * Time.deltaTime);

        controller.Move(verticalDirection * Time.deltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.gameObject == actualFloor) return;

        actualFloor = hit.gameObject.transform;
    }
}
