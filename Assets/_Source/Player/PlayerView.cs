using System.Collections.Generic;
using Movement;
using UnityEngine;

namespace Player
{
    public class PlayerView
    {
        private readonly GameObject player;
        private readonly Rigidbody2D rb;
        private readonly SpriteRenderer spriteRenderer;
        private readonly List<Sprite> playerSprites;
        private readonly PlayerModel model;
        private Direction currentDirection;

        public PlayerView(GameObject player, List<Sprite> playerSprites, PlayerModel model)
        {
            this.player = player;
            this.playerSprites = playerSprites;
            this.model = model;
            rb = player.GetComponent<Rigidbody2D>();
            spriteRenderer = rb.GetComponent<SpriteRenderer>();
        }

        public void UpdatePlayerDirection(Direction direction)
        {
            if (currentDirection != Direction.Null && currentDirection == direction) return;
            switch(direction)
            {
                case Direction.Up:
                    spriteRenderer.sprite = playerSprites[0];
                    rb.velocity = new(0, model.MovementSpeed);
                    break;
                case Direction.Right:
                    spriteRenderer.sprite = playerSprites[1];
                    rb.velocity = new(model.MovementSpeed, 0);
                    break;
                case Direction.Down:
                    spriteRenderer.sprite = playerSprites[2];
                    rb.velocity = new(0, -model.MovementSpeed);
                    break;
                case Direction.Left:
                    spriteRenderer.sprite = playerSprites[3];
                    rb.velocity = new(-model.MovementSpeed, 0);
                    break;
            }
            currentDirection = direction;
        }
    }
}