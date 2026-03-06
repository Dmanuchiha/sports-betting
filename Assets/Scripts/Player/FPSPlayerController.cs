using UnityEngine;

namespace FPSPrototype.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSPlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform playerCamera;
        [SerializeField] private Transform cameraHolder;

        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8.5f;
        [SerializeField] private float crouchSpeed = 2.8f;
        [SerializeField] private float acceleration = 20f;
        [SerializeField] private float deceleration = 16f;
        [SerializeField] private float jumpHeight = 1.5f;
        [SerializeField] private float gravity = -24f;

        [Header("Crouch")]
        [SerializeField] private float standingHeight = 1.8f;
        [SerializeField] private float crouchHeight = 1.1f;
        [SerializeField] private float crouchLerpSpeed = 10f;

        [Header("Mouse Look")]
        [SerializeField] private float mouseSensitivity = 1.4f;
        [SerializeField] private float minPitch = -80f;
        [SerializeField] private float maxPitch = 85f;

        [Header("Camera Feel")]
        [SerializeField] private float headBobFrequency = 10f;
        [SerializeField] private float headBobAmplitude = 0.05f;
        [SerializeField] private float sprintBobMultiplier = 1.35f;
        [SerializeField] private float swayAmount = 3f;
        [SerializeField] private float swaySmooth = 8f;

        public float MouseSensitivity
        {
            get => mouseSensitivity;
            set => mouseSensitivity = Mathf.Clamp(value, 0.1f, 5f);
        }

        private CharacterController controller;
        private Vector3 velocity;
        private Vector3 planarVelocity;
        private float pitch;
        private bool isCrouching;
        private Vector3 cameraLocalStart;
        private float headBobTimer;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            cameraLocalStart = cameraHolder.localPosition;
            controller.height = standingHeight;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            Look();
            Move();
            ApplyHeadBob();
            ApplyCameraSway();
        }

        private void Look()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);
            pitch -= mouseY;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
            playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void Move()
        {
            bool grounded = controller.isGrounded;
            if (grounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && grounded && !isCrouching)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouching = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                isCrouching = false;
            }

            float targetHeight = isCrouching ? crouchHeight : standingHeight;
            controller.height = Mathf.Lerp(controller.height, targetHeight, Time.deltaTime * crouchLerpSpeed);

            float speed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed);
            Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
            Vector3 desiredVelocity = (transform.right * input.x + transform.forward * input.z) * speed;

            float accel = input.sqrMagnitude > 0f ? acceleration : deceleration;
            planarVelocity = Vector3.MoveTowards(planarVelocity, desiredVelocity, accel * Time.deltaTime);

            velocity.y += gravity * Time.deltaTime;
            Vector3 movement = planarVelocity + Vector3.up * velocity.y;
            controller.Move(movement * Time.deltaTime);
        }

        private void ApplyHeadBob()
        {
            if (!controller.isGrounded || planarVelocity.magnitude < 0.1f)
            {
                cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, cameraLocalStart, Time.deltaTime * 8f);
                return;
            }

            bool sprinting = Input.GetKey(KeyCode.LeftShift) && !isCrouching;
            float bobSpeed = headBobFrequency * (sprinting ? sprintBobMultiplier : 1f);
            headBobTimer += Time.deltaTime * bobSpeed;

            float xOffset = Mathf.Cos(headBobTimer * 0.5f) * headBobAmplitude * 0.5f;
            float yOffset = Mathf.Sin(headBobTimer) * headBobAmplitude;
            cameraHolder.localPosition = cameraLocalStart + new Vector3(xOffset, yOffset, 0f);
        }

        private void ApplyCameraSway()
        {
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");
            Quaternion target = Quaternion.Euler(-mouseY * swayAmount, mouseX * swayAmount, 0f);
            cameraHolder.localRotation = Quaternion.Slerp(cameraHolder.localRotation, target, Time.deltaTime * swaySmooth);
        }
    }
}
