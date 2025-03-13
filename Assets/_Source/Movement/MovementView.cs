using Core;
using Enemies;
using Level;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementView : MonoBehaviour
    {
        public GameObject CurrentNodeObject;
        private GameManager gameManager;
        private List<Sprite> sprites;
        private SpriteRenderer spriteRenderer;
        public MovementModel Model { get; private set; }
        private Animator animator;
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

        public void Construct(List<Sprite> sprites, SpriteRenderer spriteRenderer, GameObject startingNode, MovementModel model, Animator animator, GameManager gameManager, bool isGhost)
        {
            this.sprites = sprites;
            this.spriteRenderer = spriteRenderer;
            this.animator = animator;
            this.gameManager = gameManager;
            CurrentNodeObject = startingNode;
            this.Model = model;
            this.isGhost = isGhost;
            direction = Direction.Null;
            if(animator!=null) animator.SetInteger("Direction", -1);
            isConstructed = true;
        }

        private void Update()
        {
            if (!isConstructed || !gameManager.GameIsRunning) return;

            Node currentNode = CurrentNodeObject.GetComponent<Node>();

            if (!isGhost && LastMovingDirection != direction)
            {
                CheckForMidMovementChange(Direction.Up, Direction.Down, currentNode.CanMoveUp, currentNode, out Node newCurrentNodeUp);
                currentNode = newCurrentNodeUp;
                CheckForMidMovementChange(Direction.Right, Direction.Left, currentNode.CanMoveRight, currentNode, out Node newCurrentNodeRight);
                currentNode = newCurrentNodeRight;
                CheckForMidMovementChange(Direction.Down, Direction.Up, currentNode.CanMoveDown, currentNode, out Node newCurrentNodeDown);
                currentNode = newCurrentNodeDown;
                CheckForMidMovementChange(Direction.Left, Direction.Right, currentNode.CanMoveLeft, currentNode, out Node newCurrentNodeLeft);
                currentNode = newCurrentNodeLeft;
            }

            transform.position = Vector2.MoveTowards(transform.position, CurrentNodeObject.transform.position, Model.CurrentSpeed * Time.deltaTime);

            if(transform.position.x == CurrentNodeObject.transform.position.x
                && transform.position.y == CurrentNodeObject.transform.position.y)
            {
                if (isGhost)
                {
                    GetComponent<EnemyHandler>().OnReachingCenterOfNode(currentNode);
                }

                if(currentNode.IsWrapNode)
                {
                    transform.position = currentNode.WrapNode.transform.position;
                    CurrentNodeObject = currentNode.WrapNode;
                    currentNode = CurrentNodeObject.GetComponent<Node>();
                }

                GameObject newNode = currentNode.GetNodeFromDirection(direction);
                if(newNode != null)
                {
                    if((TryGetComponent(out EnemyHandler enemyHandler) && enemyHandler.GhostNodeState != GhostNodeState.MovingInNodes && newNode.GetComponent<Node>().IsGhostNode) 
                        || !newNode.GetComponent<Node>().IsGhostNode)
                    {
                        CurrentNodeObject = newNode;
                        LastMovingDirection = direction;
                    }
                }
                else
                {
                    newNode = currentNode.GetNodeFromDirection(LastMovingDirection);
                    if (newNode != null)
                    {
                        if((TryGetComponent(out EnemyHandler enemyHandler) && enemyHandler.GhostNodeState != GhostNodeState.MovingInNodes && newNode.GetComponent<Node>().IsGhostNode) 
                            || !newNode.GetComponent<Node>().IsGhostNode)
                        {
                            CurrentNodeObject = newNode;
                        }
                    }
                }
                if (CurrentNodeObject.GetComponent<Node>().IsGhostNode && !GetComponent<EnemyHandler>().IsRespawning && GetComponent<EnemyHandler>().GhostNodeState == GhostNodeState.MovingInNodes)
                {
                    LastMovingDirection = Direction.Up;
                    direction = Direction.Up;
                }
            }
        }

        public void SetDirection(Direction newDirection)
        {
            direction = newDirection;
        }

        public void ChangeMovementSpeed(float speed)
        {
            Model.CurrentSpeed = speed;
        }

        public void CheckForMidMovementChange(Direction neededDirection, Direction lastDirection, bool neededCellIsPossible, Node currentNode, out Node newCurrentNode)
        {
            newCurrentNode = CurrentNodeObject.GetComponent<Node>();
            if (direction == neededDirection && lastMovingDirection == lastDirection && neededCellIsPossible)
            {
                switch(neededDirection)
                {
                    case Direction.Up:
                        if(!currentNode.NodeUp.GetComponent<Node>().IsGhostNode)
                            CurrentNodeObject = currentNode.NodeUp;
                        break;
                    case Direction.Right:
                        if (!currentNode.NodeRight.GetComponent<Node>().IsGhostNode)
                            CurrentNodeObject = currentNode.NodeRight;
                        break;
                    case Direction.Down:
                        if (!currentNode.NodeDown.GetComponent<Node>().IsGhostNode)
                            CurrentNodeObject = currentNode.NodeDown;
                        break;
                    case Direction.Left:
                        if (!currentNode.NodeLeft.GetComponent<Node>().IsGhostNode)
                            CurrentNodeObject = currentNode.NodeLeft;
                        break;
                }
                newCurrentNode = CurrentNodeObject.GetComponent<Node>();
                LastMovingDirection = direction;
            }
        }


        private void UpdateSprite()
        {
            if (animator == null) spriteRenderer.sprite = sprites[LastMovingDirection.GetHashCode()];
            else animator.SetInteger("Direction", LastMovingDirection.GetHashCode());
        }
    }
    
}
