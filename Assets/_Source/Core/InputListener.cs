using Movement;
using Player;
using UnityEngine;

namespace Core{
    
    public class InputListener : MonoBehaviour
    {
        [SerializeField] private bool TurnOnDebugInputs;
        private MovementController controller;
        private PlayerLivesController livesController;

        public void Construct(MovementController controller)
        {
            this.controller = controller;
        }

        public void Construct(PlayerLivesController livesController)
        {
            this.livesController = livesController;
        }

        private void Update()
        {
            ListenForMovementInput();
            if(TurnOnDebugInputs) ListenForDebugInput();
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

        private void ListenForDebugInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                livesController.InvokeLivesUpdate(1);
                return;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                livesController.InvokeLivesUpdate(-1);
            }
        }
    }
}