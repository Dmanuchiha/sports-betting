using UnityEngine;

namespace FPSPrototype.Player
{
    public class MouseLook : MonoBehaviour
    {
        [SerializeField] private Transform playerBody;
        [SerializeField] private Transform pitchPivot;
        [SerializeField] private float sensitivity = 120f;
        [SerializeField] private float smoothTime = 0.02f;

        private float xRotation;
        private Vector2 currentDelta;
        private Vector2 deltaVelocity;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime;

            Vector2 targetDelta = new Vector2(mouseX, mouseY);
            currentDelta = Vector2.SmoothDamp(currentDelta, targetDelta, ref deltaVelocity, smoothTime);

            xRotation -= currentDelta.y;
            xRotation = Mathf.Clamp(xRotation, -88f, 88f);

            pitchPivot.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * currentDelta.x);
        }

        public void SetSensitivity(float newSensitivity)
        {
            sensitivity = Mathf.Clamp(newSensitivity, 30f, 300f);
        }
    }
}
