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
    public CharacterController controller;

    public Animator anim;

    [SerializeField] float timeSinceLastHit;
    public float invulnerabilityTime = 4;

    public bool stunned;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        stunned = false;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        moveDirection = Vector3.right * x + Vector3.forward * z;

        //Apply stun
        if (stunned) moveDirection = Vector3.zero;

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

            //Animation Fall
            anim.SetTrigger("HitGround");

            if (Input.GetButton("Jump"))
            {
                //Reset animation Fall
                anim.ResetTrigger("HitGround");
                //anim.SetTrigger("Jump");
                //Play animation Jump (this way it doesnt have any time between Run and Idle)
                anim.Play("Jump");
                verticalDirection.y = Mathf.Sqrt(impulseJump * -2 * gravity);
            }
            if (verticalDirection.y == -2f)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Play animation Sopro (this way it doesnt have any time between Run and Idle)
                    anim.Play("Sopro");
                }
            }
        }

        controller.Move(moveDirection * speed * Time.deltaTime);

        controller.Move(verticalDirection * Time.deltaTime);

        //Set animation Run conditions
        if (moveDirection.x != 0 || moveDirection.z != 0) anim.SetBool("Running", true);
        else anim.SetBool("Running", false);

        timeSinceLastHit -= Time.deltaTime;
        if (timeSinceLastHit <= 3) stunned = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Floor") && hit.transform != actualFloor)
        {
            Debug.Log("Hit floor");
            actualFloor = hit.gameObject.transform;
        }
        else if (hit.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Hit enemy");
            if (hit.point.y < (transform.position.y - .85f) && hit.moveDirection.y < -.5f)
            {
                hit.gameObject.GetComponent<Enemy>().Kill();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && timeSinceLastHit <= 0)
        {
            Debug.Log("Enemy hit player");
            timeSinceLastHit = invulnerabilityTime;
            GameManager.instance.Hit();
            stunned = true;

            anim.SetTrigger("Damage");
        }
    }
}
