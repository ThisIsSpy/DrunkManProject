﻿namespace Movement
{
    public class MovementController
    {
        private readonly MovementView view;

        public MovementController(MovementView view)
        {
            this.view = view;
        }

        public void InvokeSetDirection(Direction direction)
        {
            view.SetDirection(direction);
        }
    }
}