using UnityEngine;
using UnityEngine.InputSystem;

namespace miniIT.Arcanoid
{
    public class ArcanoidDebugger : MonoBehaviour
    {
        public InputAction click = default;
        private bool clickPerformed = false;

        private void Start()
        {
            click.performed += ClickPerformListener;
            click.Enable();
        }

        private void ClickPerformListener(InputAction.CallbackContext context)
        {
            clickPerformed = true;
        }

        private void Update()
        {
            if(clickPerformed)
            {
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
                OnClick(worldPos);
                clickPerformed = false;
            }
        }

        private void OnClick(Vector3 worldPosition)
        {
            Collider2D collider2D = Physics2D.OverlapPoint(worldPosition);
            if(collider2D != default && collider2D.TryGetComponent(out Brick brick))
            {
                brick.Kill();
            }
        }
    }

}