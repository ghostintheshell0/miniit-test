using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using VContainer;

namespace miniIT.Arcanoid
{
    public class PlayerInput : MonoBehaviour
    {

        public event Action OnFire = default;
        public event Action<Vector2> OnPointerPosition = default;


        [SerializeField] 
        private InputAction fire = default;
        [Inject]
        private EventSystem eventSystem = default;

        private bool isPressed = false;


        private void Start()
        {
            fire.performed += FirePerformListener;
            fire.Enable();    
        }

        private void FirePerformListener(InputAction.CallbackContext context)
        {
            isPressed = true;
            
        }

        private void Update()
        {
            OnPointerPosition?.Invoke(Mouse.current.position.value);
            if(isPressed)
            {
                isPressed = false;
                if(!eventSystem.IsPointerOverGameObject())
                {
                    OnFire?.Invoke();
                }
            }
        }
    }
}