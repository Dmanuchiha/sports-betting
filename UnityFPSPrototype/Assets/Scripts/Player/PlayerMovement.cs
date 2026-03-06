using UnityEngine;

namespace FPSPrototype.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform cameraRoot;

        [Header("Movement")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float sprintSpeed = 8f;
        [SerializeField] private float crouchSpeed = 3f;
        [SerializeField] private float acceleration = 14f;
        [SerializeField] private float deceleration = 12f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -24f;

        [Header("Crouch")]
        [SerializeField] private float standingHeight = 1.8f;
        [SerializeField] private float crouchHeight = 1.1f;
        [SerializeField] private float crouchTransitionSpeed = 10f;

        [Header("Head Bob + Camera Sway")]
        [SerializeField] private float headBobFrequency = 8f;
        [SerializeField] private float headBobAmplitude = 0.035f;
        [SerializeField] private float sprintBobMultiplier = 1.4f;
        [SerializeField] private float swayAmount = 2.5f;
        [SerializeField] private float swaySmooth = 8f;

        public Vector3 Velocity => velocity;
        public bool IsGrounded => characterController.isGrounded;

        private CharacterController characterController;
        private Vector3 velocity;
        private Vector3 horizontalVelocity;
        private float bobTimer;
        private Vector3 cameraInitialLocalPosition;
        private bool isCrouched;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            cameraInitialLocalPosition = cameraRoot.localPosition;
            characterController.height = standingHeight;
        }

        private void Update()
        {
            HandleMove();
            HandleJumpAndGravity();
            HandleCrouch();
            HandleHeadBob();
            HandleSway();
        }

        private void HandleMove()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            Vector3 inputDir = (transform.right * x + transform.forward * z).normalized;

            bool sprinting = Input.GetKey(KeyCode.LeftShift) && z > 0.1f && !isCrouched;
            float targetSpeed = isCrouched ? crouchSpeed : sprinting ? sprintSpeed : walkSpeed;
            Vector3 targetVelocity = inputDir * targetSpeed;

            float accelRate = inputDir.sqrMagnitude > 0.01f ? acceleration : deceleration;
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, targetVelocity, accelRate * Time.deltaTime);

            Vector3 move = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
            characterController.Move(move * Time.deltaTime);
        }

        private void HandleJumpAndGravity()
        {
            if (characterController.isGrounded && velocity.y < 0f)
            {
                velocity.y = -2f;
            }

            if (Input.GetButtonDown("Jump") && characterController.isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
        }

        private void HandleCrouch()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                isCrouched = !isCrouched;
            }

            float targetHeight = isCrouched ? crouchHeight : standingHeight;
            characterController.height = Mathf.Lerp(characterController.height, targetHeight, crouchTransitionSpeed * Time.deltaTime);

            Vector3 center = characterController.center;
            center.y = characterController.height * 0.5f;
            characterController.center = center;
        }

        private void HandleHeadBob()
        {
            Vector3 horizontal = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z);
            if (!characterController.isGrounded || horizontal.magnitude < 0.1f)
            {
                bobTimer = 0f;
                cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, cameraInitialLocalPosition, Time.deltaTime * 8f);
                return;
            }

            float bobMultiplier = Input.GetKey(KeyCode.LeftShift) ? sprintBobMultiplier : 1f;
            bobTimer += Time.deltaTime * headBobFrequency * bobMultiplier;
            float bobOffset = Mathf.Sin(bobTimer) * headBobAmplitude;

            Vector3 targetPos = cameraInitialLocalPosition + new Vector3(0f, bobOffset, 0f);
            cameraRoot.localPosition = Vector3.Lerp(cameraRoot.localPosition, targetPos, Time.deltaTime * 12f);
        }

        private void HandleSway()
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            Quaternion target = Quaternion.Euler(-mouseY * swayAmount, mouseX * swayAmount, mouseX * swayAmount * 0.35f);
            cameraRoot.localRotation = Quaternion.Slerp(cameraRoot.localRotation, target, swaySmooth * Time.deltaTime);
        }
    }
}
