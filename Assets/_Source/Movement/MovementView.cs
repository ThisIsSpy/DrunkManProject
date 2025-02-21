using Level;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementView : MonoBehaviour
    {
        private GameObject currentNodeObject;
        private List<Sprite> sprites;
        private SpriteRenderer spriteRenderer;
        private MovementModel model;

        private Direction direction;
        private Direction lastMovingDirection;

        private bool isConstructed = false;

        private Direction Direction { get { return direction; }
            set 
            {
                if (direction == value) return;
                direction = value;
                UpdateSprite();
            }
        }

        public void Construct(List<Sprite> sprites, SpriteRenderer spriteRenderer, GameObject startingNode, MovementModel model)
        {
            this.sprites = sprites;
            this.spriteRenderer = spriteRenderer;
            currentNodeObject = startingNode;
            this.model = model;
            isConstructed = true;
        }

        private void Update()
        {
            if (!isConstructed) return;

            Node currentNode = currentNodeObject.GetComponent<Node>();

            transform.position = Vector2.MoveTowards(transform.position, currentNodeObject.transform.position, model.Speed * Time.deltaTime);

            if(transform.position.x == currentNodeObject.transform.position.x
                && transform.position.y == currentNodeObject.transform.position.y)
            {
                GameObject newNode = currentNode.GetNodeFromDirection(Direction);
                if(newNode != null)
                {
                    currentNodeObject = newNode;
                    lastMovingDirection = Direction;
                }
                else
                {
                    direction = lastMovingDirection;
                    newNode = currentNode.GetNodeFromDirection(Direction);
                    if (newNode != null)
                    {
                        currentNodeObject = newNode;
                    }
                }
            }
        }

        public void SetDirection(Direction newDirection)
        {
            Direction = newDirection;
        }

        private void UpdateSprite()
        {
            switch(Direction)
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
