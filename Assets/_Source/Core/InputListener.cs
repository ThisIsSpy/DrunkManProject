using Movement;
using UnityEngine;

namespace Core{
    
    public class InputListener : MonoBehaviour
    {
        private MovementController controller;

        public void Construct(MovementController controller)
        {
            this.controller = controller;
        }

        private void Update()
        {
            ListenForMovementInput();
        }

        private void ListenForMovementInput()
        {
            if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                controller.InvokeSetDirection(Direction.Up);
                return;
            }
            if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                controller.InvokeSetDirection(Direction.Right);
                return;
            }
            if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                controller.InvokeSetDirection(Direction.Down);
                return;
            }
            if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                controller.InvokeSetDirection(Direction.Left);
                return;
            }
        }
    }
}