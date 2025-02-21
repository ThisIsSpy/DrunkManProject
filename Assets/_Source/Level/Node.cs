using Movement;
using UnityEngine;

namespace Level
{
    public class Node : MonoBehaviour
    {
        public bool canMoveLeft = false;
        public bool canMoveRight = false;
        public bool canMoveUp = false;
        public bool canMoveDown = false;

        public GameObject nodeLeft;
        public GameObject nodeRight;
        public GameObject nodeUp;
        public GameObject nodeDown;

        private void Start()
        {
            RaycastHit2D[] hitsDown;
            hitsDown = Physics2D.RaycastAll(transform.position, Vector2.down);
            for(int i = 0; i < hitsDown.Length; i++)
            {
                float distance = Mathf.Abs(hitsDown[i].point.y - transform.position.y);
                if (distance < 0.35f)
                {
                    canMoveDown = true;
                    nodeDown = hitsDown[i].collider.gameObject;
                }
            }

            RaycastHit2D[] hitsUp;
            hitsUp = Physics2D.RaycastAll(transform.position, Vector2.up);
            for (int i = 0; i < hitsUp.Length; i++)
            {
                float distance = Mathf.Abs(hitsUp[i].point.y - transform.position.y);
                if (distance < 0.35f)
                {
                    canMoveUp = true;
                    nodeUp = hitsUp[i].collider.gameObject;
                }
            }

            RaycastHit2D[] hitsRight;
            hitsRight = Physics2D.RaycastAll(transform.position, Vector2.right);
            for (int i = 0; i < hitsRight.Length; i++)
            {
                float distance = Mathf.Abs(hitsRight[i].point.x - transform.position.x);
                if (distance < 0.35f)
                {
                    canMoveRight = true;
                    nodeRight = hitsRight[i].collider.gameObject;
                }
            }

            RaycastHit2D[] hitsLeft;
            hitsLeft = Physics2D.RaycastAll(transform.position, Vector2.left);
            for (int i = 0; i < hitsLeft.Length; i++)
            {
                float distance = Mathf.Abs(hitsLeft[i].point.x - transform.position.x);
                if (distance < 0.35f)
                {
                    canMoveLeft = true;
                    nodeLeft = hitsLeft[i].collider.gameObject;
                }
            }
        }

        public GameObject GetNodeFromDirection(Direction direction)
        {
            if(direction == Direction.Up && canMoveUp) return nodeUp;
            else if(direction == Direction.Right && canMoveRight) return nodeRight;
            else if(direction == Direction.Down && canMoveDown) return nodeDown;
            else if (direction == Direction.Left && canMoveLeft) return nodeLeft;
            else return null;
        }
    }
}
