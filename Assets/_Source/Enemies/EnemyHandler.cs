using Movement;
using Level;
using UnityEngine;
using Core;
using System.Collections;
using System.Collections.Generic;

namespace Enemies
{
    public class EnemyHandler : MonoBehaviour
    {
        private const float DIST_BETWEEN_NODES = 0.35f;

        [field: SerializeField] public GhostNodeState GhostNodeState { get; private set; }
        [SerializeField] private GhostNodeState respawnState;
        [field: SerializeField] public GhostType GhostType { get; private set; }

        public bool ReadyToLeaveHome = false;
        public bool IsRespawning = false;
        public bool IsAfraid = false;
        [field: SerializeField] public bool LeftHomeBefore { get; private set; } = false;

        [field: SerializeField] private float awakeTime = 1f;
        [SerializeField] private int scatterNodeIndex = 0;
        [SerializeField] private Node[] scatterNodes;
        [SerializeField] private GameManager gameManager;

        private MovementView movementView;

        private void Awake()
        {
            movementView = GetComponent<MovementView>();
            switch (GhostType)
            {
                case GhostType.Red:
                    GhostNodeState = GhostNodeState.StartNode;
                    respawnState = GhostNodeState.CenterNode;
                    ReadyToLeaveHome = true;
                    break;
                case GhostType.Blue:
                    GhostNodeState = GhostNodeState.LeftNode;
                    respawnState = GhostNodeState.LeftNode;
                    break;
                case GhostType.Pink:
                    GhostNodeState = GhostNodeState.CenterNode;
                    respawnState = GhostNodeState.CenterNode;
                    ReadyToLeaveHome = true;
                    break;
                case GhostType.Orange:
                    GhostNodeState = GhostNodeState.RightNode;
                    respawnState = GhostNodeState.RightNode;
                    break;
            }
            IsRespawning = false;
        }

        private void Update()
        {
            if(IsRespawning)
            {
                GhostNodeState = GhostNodeState.Respawning;
                ReadyToLeaveHome = false;
                IsRespawning = false;
                IsAfraid = false;
                GetComponent<MovementView>().ChangeMovementSpeed(GetComponent<MovementView>().Model.NormalSpeed*2);
                GetComponent<Collider2D>().enabled = false;
            }
        }

        public void OnReachingCenterOfNode(Node node)
        {
            switch(GhostNodeState)
            {
                case GhostNodeState.MovingInNodes:
                    if(gameManager.GhostMode == GhostMode.Scatter && !IsAfraid) 
                    {
                        DetermineGhostScatterModeDirection();
                    }
                    else if (IsAfraid)
                    {
                        Direction direction = GetRandomDirection();
                        movementView.SetDirection(direction);
                    }
                    else
                    {
                        switch (GhostType)
                        {
                            case GhostType.Red:
                                DetermineRedGhostDirection();
                                break;
                            case GhostType.Pink:
                                DeterminePinkGhostDirection();
                                break;
                            case GhostType.Blue:
                                DetermineBlueGhostDirection();
                                break;
                            case GhostType.Orange:
                                DetermineOrangeGhostDirection();
                                break;
                        }
                    }
                    break;
                case GhostNodeState.Respawning:
                    if (transform.position.x == GetComponent<MovementSetup>().StartingNode.transform.position.x &&
                        transform.position.y == GetComponent<MovementSetup>().StartingNode.transform.position.y)
                    {
                        GhostNodeState = respawnState;
                        GetComponent<MovementView>().ChangeMovementSpeed(GetComponent<MovementView>().Model.NormalSpeed);
                        GetComponent<Collider2D>().enabled = true;
                        StartCoroutine(GhostAwakeningCoroutine());
                    }
                    else
                    {
                        Direction direction = GetClosestDirection(GetComponent<MovementSetup>().StartingNode.transform.position);
                        movementView.SetDirection(direction);
                    }
                    break;
                default:
                    if(ReadyToLeaveHome)
                    {
                        if(!LeftHomeBefore) LeftHomeBefore = true;
                        switch (GhostNodeState)
                        {
                            case GhostNodeState.LeftNode:
                                GhostNodeState = GhostNodeState.CenterNode;
                                movementView.SetDirection(Direction.Right);
                                break;
                            case GhostNodeState.RightNode:
                                GhostNodeState = GhostNodeState.CenterNode;
                                movementView.SetDirection(Direction.Left);
                                break;
                            case GhostNodeState.CenterNode:
                                GhostNodeState = GhostNodeState.StartNode;
                                movementView.SetDirection(Direction.Up);
                                break;
                            case GhostNodeState.StartNode:
                                GhostNodeState = GhostNodeState.MovingInNodes;
                                movementView.SetDirection(Direction.Up);
                                break;
                        }
                    }
                    break;
            }
        }

        private void DetermineRedGhostDirection()
        {
            Direction direction = GetClosestDirection(gameManager.Player.transform.position);
            movementView.SetDirection(direction);
        }

        private void DeterminePinkGhostDirection()
        {
            Direction playerDirection = gameManager.Player.GetComponent<MovementView>().LastMovingDirection;

            Vector2 target = gameManager.Player.transform.position;
            switch (playerDirection)
            {
                case Direction.Up:
                    target.y += DIST_BETWEEN_NODES * 2;
                    break;
                case Direction.Right:
                    target.x += DIST_BETWEEN_NODES * 2;
                    break;
                case Direction.Down:
                    target.y -= DIST_BETWEEN_NODES * 2;
                    break;
                case Direction.Left:
                    target.x -= DIST_BETWEEN_NODES * 2;
                    break;
            }
            Direction direction = GetClosestDirection(target);
            movementView.SetDirection(direction);
        }

        private void DetermineBlueGhostDirection()
        {
            Direction playerDirection = gameManager.Player.GetComponent<MovementView>().LastMovingDirection;
            

            Vector2 target = gameManager.Player.transform.position;
            switch (playerDirection)
            {
                case Direction.Up:
                    target.y += DIST_BETWEEN_NODES * 2;
                    break;
                case Direction.Right:
                    target.x += DIST_BETWEEN_NODES * 2;
                    break;
                case Direction.Down:
                    target.y -= DIST_BETWEEN_NODES * 2;
                    break;
                case Direction.Left:
                    target.x -= DIST_BETWEEN_NODES * 2;
                    break;
            }
            Vector2 redGhostPos = gameManager.GetGhost(GhostType.Red).transform.position;
            float xDist = target.x - redGhostPos.x;
            float yDist = target.y - redGhostPos.y;
            Vector2 blueTarget = new(target.x + xDist,target.y +  yDist);
            Direction direction = GetClosestDirection(blueTarget);
            movementView.SetDirection(direction);
        }

        private void DetermineOrangeGhostDirection()
        {
            float distance = Vector2.Distance(gameManager.Player.transform.position, transform.position);
            if (distance < 0) distance *= -1;
            if (distance <= DIST_BETWEEN_NODES * 8) DetermineRedGhostDirection();
            else DetermineGhostScatterModeDirection();
        }

        private void DetermineGhostScatterModeDirection()
        {
            if (transform.position == scatterNodes[scatterNodeIndex].transform.position) scatterNodeIndex++;
            if (scatterNodeIndex == scatterNodes.Length) scatterNodeIndex = 0;
            Direction direction = GetClosestDirection(scatterNodes[scatterNodeIndex].transform.position);
            movementView.SetDirection(direction);
        }

        private Direction GetClosestDirection(Vector2 target)
        {
            float shortestDistance = 0;
            Direction lastMovingDirection = movementView.LastMovingDirection;
            Direction newDirection = Direction.Null;
            Node currentNode = movementView.CurrentNodeObject.GetComponent<Node>();

            if (currentNode.CanMoveUp && lastMovingDirection != Direction.Down)
            {
                GameObject nodeUp = currentNode.NodeUp;
                float distance = Vector2.Distance(nodeUp.transform.position, target);
                if (distance < shortestDistance || shortestDistance == 0)
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
            if (currentNode.CanMoveLeft && lastMovingDirection != Direction.Right)
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

        public Direction GetRandomDirection()
        {
            List<Direction> possibleDirections = new();
            Node currentNode = movementView.CurrentNodeObject.GetComponent<Node>();
            if(currentNode.CanMoveUp && movementView.LastMovingDirection != Direction.Down) possibleDirections.Add(Direction.Up);
            if(currentNode.CanMoveRight && movementView.LastMovingDirection != Direction.Left) possibleDirections.Add(Direction.Right);
            if(currentNode.CanMoveDown && movementView.LastMovingDirection != Direction.Up) possibleDirections.Add(Direction.Down);
            if(currentNode.CanMoveLeft && movementView.LastMovingDirection != Direction.Right) possibleDirections.Add(Direction.Left);
            Direction direction = possibleDirections[Random.Range(0, possibleDirections.Count-1)];
            return direction;
        }

        public void ReturnToStartingPosition()
        {
            GhostNodeState = respawnState;
        }

        //public void DistanceFinder(bool isMovementPossible, Direction direction, Vector2 target, float shortestDistance, Direction previousNewDirection, out Direction newDirection, out float newShortestDistance)
        //{
        //    newShortestDistance = shortestDistance;
        //    newDirection = previousNewDirection;
        //    if (isMovementPossible && movementView.LastMovingDirection != direction)
        //    {
        //        GameObject newNode = movementView.CurrentNodeObject;
        //        Node currentNode = movementView.CurrentNodeObject.GetComponent<Node>();
        //        switch(direction)
        //        {
        //            case Direction.Up:
        //                newNode = currentNode.NodeDown;
        //                break;
        //            case Direction.Right:
        //                newNode = currentNode.NodeLeft;
        //                break;
        //            case Direction.Down:
        //                newNode = currentNode.NodeUp;
        //                break;
        //            case Direction.Left:
        //                newNode = currentNode.NodeRight;
        //                break;
        //        }
        //        float distance = Vector2.Distance(newNode.transform.position, target);
        //        if (distance < shortestDistance || shortestDistance == 0)
        //        {
        //            newShortestDistance = distance;
        //            newDirection = direction;
        //        }
        //    }
        //}

        private IEnumerator GhostAwakeningCoroutine()
        {
            yield return new WaitForSeconds(awakeTime);
            ReadyToLeaveHome = true;
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