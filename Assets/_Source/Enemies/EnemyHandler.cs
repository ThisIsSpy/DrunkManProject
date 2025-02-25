using Movement;
using Level;
using UnityEngine;

namespace Enemies
{
    public class EnemyHandler : MonoBehaviour
    {
        [SerializeField] private GhostNodeState ghostNodeState;
        [SerializeField] private GhostType ghostType;
        [SerializeField] private bool readyToLeaveHome = false;
        [SerializeField] private GameObject player;

        private MovementView movementView;

        private void Awake()
        {
            movementView = GetComponent<MovementView>();
            switch (ghostType)
            {
                case GhostType.Red:
                    ghostNodeState = GhostNodeState.StartNode;
                    break;
                case GhostType.Blue:
                    ghostNodeState = GhostNodeState.LeftNode;
                    break;
                case GhostType.Pink:
                    ghostNodeState = GhostNodeState.CenterNode;
                    break;
                case GhostType.Orange:
                    ghostNodeState = GhostNodeState.RightNode;
                    break;
            }
        }
        public void OnReachingCenterOfNode(Node node)
        {
            switch(ghostNodeState)
            {
                case GhostNodeState.MovingInNodes:
                    //switch (ghostType)
                    //{
                    //    case GhostType.Blue:
                    //        DetermineRedGhostDirection();
                    //        break;
                    //    default:
                    //        break;
                    //}
                    DetermineRedGhostDirection();
                    break;
                case GhostNodeState.Respawning:
                    break;
                default:
                    if(readyToLeaveHome)
                    {
                        switch (ghostNodeState)
                        {
                            case GhostNodeState.LeftNode:
                                ghostNodeState = GhostNodeState.CenterNode;
                                movementView.SetDirection(Direction.Right);
                                break;
                            case GhostNodeState.RightNode:
                                ghostNodeState = GhostNodeState.CenterNode;
                                movementView.SetDirection(Direction.Left);
                                break;
                            case GhostNodeState.CenterNode:
                                ghostNodeState = GhostNodeState.StartNode;
                                movementView.SetDirection(Direction.Up);
                                break;
                            case GhostNodeState.StartNode:
                                ghostNodeState = GhostNodeState.MovingInNodes;
                                movementView.SetDirection(Direction.Up);
                                break;
                        }
                    }
                    break;
            }
        }

        private void DetermineRedGhostDirection()
        {
            Direction direction = GetClosestDirection(player.transform.position);
            movementView.SetDirection(direction);
        }

        private void DeterminePinkGhostDirection()
        {

        }

        private void DetermineBlueGhostDirection()
        {

        }

        private void DetermineOrangeGhostDirection()
        {

        }

        private Direction GetClosestDirection(Vector2 target)
        {
            float shortestDistance = 0;
            Direction lastMovingDirection = movementView.LastMovingDirection;
            Direction newDirection = Direction.Null;
            Node currentNode = movementView.CurrentNodeObject.GetComponent<Node>();

            if(currentNode.CanMoveUp && lastMovingDirection != Direction.Down)
            {
                GameObject nodeUp = currentNode.NodeUp;
                float distance = Vector2.Distance(nodeUp.transform.position, target);
                if(distance < shortestDistance || shortestDistance == 0)
                {
                    shortestDistance = distance;
                    newDirection = Direction.Up;
                }
            }
            if (currentNode.CanMoveRight && lastMovingDirection != Direction.Left)
            {
                GameObject nodeRight = currentNode.NodeRight;
                float distance = Vector2.Distance(nodeRight.transform.position, target);
                if (distance < shortestDistance || shortestDistance == 0)
                {
                    shortestDistance = distance;
                    newDirection = Direction.Right;
                }
            }
            if (currentNode.CanMoveDown && lastMovingDirection != Direction.Up)
            {
                GameObject nodeDown = currentNode.NodeDown;
                float distance = Vector2.Distance(nodeDown.transform.position, target);
                if (distance < shortestDistance || shortestDistance == 0)
                {
                    shortestDistance = distance;
                    newDirection = Direction.Down;
                }
            }
            if(currentNode.CanMoveLeft && lastMovingDirection != Direction.Right)
            {
                GameObject nodeLeft = currentNode.NodeLeft;
                float distance = Vector2.Distance(nodeLeft.transform.position, target);
                if (distance < shortestDistance || shortestDistance == 0)
                {
                    shortestDistance = distance;
                    newDirection = Direction.Left;
                }
            }
            return newDirection;
        }
    }

    public enum GhostNodeState
    {
        Respawning,
        LeftNode,
        RightNode,
        CenterNode,
        StartNode,
        MovingInNodes
    }

    public enum GhostType
    {
        Red,
        Blue,
        Pink,
        Orange
    }
}