using Enemies;
using Level;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementView : MonoBehaviour
    {
        public GameObject CurrentNodeObject { get; private set; }
        private List<Sprite> sprites;
        private SpriteRenderer spriteRenderer;
        private MovementModel model;
        private bool isGhost;

        [SerializeField] private Direction direction;
        private Direction lastMovingDirection;

        private bool isConstructed = false;

        public Direction LastMovingDirection { get { return lastMovingDirection; }
            private set 
            {
                if (lastMovingDirection == value) return;
                lastMovingDirection = value;
                UpdateSprite();
            }
        }

        public void Construct(List<Sprite> sprites, SpriteRenderer spriteRenderer, GameObject startingNode, MovementModel model, bool isGhost)
        {
            this.sprites = sprites;
            this.spriteRenderer = spriteRenderer;
            CurrentNodeObject = startingNode;
            this.model = model;
            isConstructed = true;
            this.isGhost = isGhost;
        }

        private void Update()
        {
            if (!isConstructed) return;

            Node currentNode = CurrentNodeObject.GetComponent<Node>();

            if (LastMovingDirection != direction)
            {
                if (direction == Direction.Up && LastMovingDirection == Direction.Down && currentNode.CanMoveUp)
                {
                    CurrentNodeObject = currentNode.NodeUp;
                    currentNode = CurrentNodeObject.GetComponent<Node>();
                    LastMovingDirection = direction;
                }
                else if (direction == Direction.Right && LastMovingDirection == Direction.Left && currentNode.CanMoveRight)
                {
                    CurrentNodeObject = currentNode.NodeRight;
                    currentNode = CurrentNodeObject.GetComponent<Node>();
                    LastMovingDirection = direction;
                }
                else if (direction == Direction.Down && LastMovingDirection == Direction.Up && currentNode.CanMoveDown)
                {
                    CurrentNodeObject = currentNode.NodeDown;
                    currentNode = CurrentNodeObject.GetComponent<Node>();
                    LastMovingDirection = direction;
                }
                else if (direction == Direction.Left && LastMovingDirection == Direction.Right && currentNode.CanMoveLeft)
                {
                    CurrentNodeObject = currentNode.NodeLeft;
                    currentNode = CurrentNodeObject.GetComponent<Node>();
                    LastMovingDirection = direction;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, CurrentNodeObject.transform.position, model.Speed * Time.deltaTime);

            if(transform.position.x == CurrentNodeObject.transform.position.x
                && transform.position.y == CurrentNodeObject.transform.position.y)
            {
                if (isGhost)
                {
                    GetComponent<EnemyHandler>().OnReachingCenterOfNode(currentNode);
                }

                GameObject newNode = currentNode.GetNodeFromDirection(direction);
                if(newNode != null)
                {
                    CurrentNodeObject = newNode;
                    LastMovingDirection = direction;
                }
                else
                {
                    newNode = currentNode.GetNodeFromDirection(LastMovingDirection);
                    if (newNode != null)
                    {
                        CurrentNodeObject = newNode;
                    }
                }
            }
        }

        public void SetDirection(Direction newDirection)
        {
            direction = newDirection;
        }


        private void UpdateSprite()
        {
            switch(LastMovingDirection)
            {
                case Direction.Up:
                    spriteRenderer.sprite = sprites[0];
                    break;
                case Direction.Right:
                    spriteRenderer.sprite = sprites[1];
                    break;
                case Direction.Down:
                    spriteRenderer.sprite = sprites[2];
                    break;
                case Direction.Left:
                    spriteRenderer.sprite = sprites[3];
                    break;
            }
        }
    }
    
}
