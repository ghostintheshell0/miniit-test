using UnityEngine;
using UnityEngine.InputSystem;

namespace miniit.Arcanoid
{
    public class PlayerInput : MonoBehaviour
    {
        public InputAction Fire = default;
        public Vector2 PointerPosition
        {
            get
            {
                return Mouse.current.position.value;
            }
        }
    }
}