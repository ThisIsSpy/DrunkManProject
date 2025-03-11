using Movement;
using UnityEngine;

namespace Level
{
    public class Node : MonoBehaviour
    {
        [field: SerializeField] public bool IsGhostNode { get; private set; } = false;

        [field: SerializeField] public bool CanMoveLeft { get; private set; } = false;
        [field: SerializeField] public bool CanMoveRight { get; private set; } = false;
        [field: SerializeField] public bool CanMoveUp { get; private set; } = false;
        [field: SerializeField] public bool CanMoveDown { get; private set; } = false;

        [field: SerializeField] public GameObject NodeLeft { get; private set; }
        [field: SerializeField] public GameObject NodeRight { get; private set; }
        [field: SerializeField] public GameObject NodeUp { get; private set; }
        [field: SerializeField] public GameObject NodeDown { get; private set; }

        [SerializeField] private bool visualizeNodes;

        private void Start()
        {
            RaycastHit2D[] hitsDown;
            hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down);
            for(int i = 0; i < hitsDown.Length; i++)
            {
                float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
                if (distance < 1.05f && hitsDown[i].collider.TryGetComponent(out Node _))
                {
                    CanMoveDown = true;
                    NodeDown = hitsDown[i].collider.gameObject;
                    break;
                }
            }

            RaycastHit2D[] hitsUp;
            hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up);
            for (int i = 0; i < hitsUp.Length; i++)
            {
                float distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
                if (distance < 1.05f && hitsUp[i].collider.TryGetComponent(out Node _))
                {
                    CanMoveUp = true;
                    NodeUp = hitsUp[i].collider.gameObject;
                    break;
                }
            }

            RaycastHit2D[] hitsRight;
            hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right);
            for (int i = 0; i < hitsRight.Length; i++)
            {
                float distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
                if (distance < 1.05f && hitsRight[i].collider.TryGetComponent(out Node _))
                {
                    CanMoveRight = true;
                    NodeRight = hitsRight[i].collider.gameObject;
                    break;
                }
            }

            RaycastHit2D[] hitsLeft;
            hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left);
            for (int i = 0; i < hitsLeft.Length; i++)
            {
                float distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
                if (distance < 1.05f && hitsLeft[i].collider.TryGetComponent(out Node _))
                {
                    CanMoveLeft = true;
                    NodeLeft = hitsLeft[i].collider.gameObject;
                    break;
                }
            }

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (visualizeNodes) spriteRenderer.enabled = true;
            else spriteRenderer.enabled = false;
        }

        public GameObject GetNodeFromDirection(Direction direction)
        {
            if(direction == Direction.Up && CanMoveUp) return NodeUp;
            else if(direction == Direction.Right && CanMoveRight) return NodeRight;
            else if(direction == Direction.Down && CanMoveDown) return NodeDown;
            else if (direction == Direction.Left && CanMoveLeft) return NodeLeft;
            else return null;
        }
    }
}
