using Core;
using Enemies;
using Level;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementSetup : MonoBehaviour
    {
        [field : SerializeField] public bool IsPlayer { get; private set; }
        [SerializeField, Range(0,10)] private float speed;
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private GameManager gameManager;
        [field: SerializeField] public Node StartingNode { get; private set; }

        private MovementModel model;
        private MovementView view;
        private MovementController controller;

        public void Setup()
        {
            model = new(speed);
            view = GetComponent<MovementView>();
            if(StartingNode == null) StartingNode = FindFirstObjectByType<Node>();
            Animator animator;
            if (TryGetComponent(out Animator a)) animator = a;
            else animator = null;
            Debug.Log($"{animator==null}, {gameObject.name}");
            view.Construct(sprites, GetComponent<SpriteRenderer>(), StartingNode.gameObject, model, animator, gameManager, !IsPlayer);
            controller = new(view);
            ReturnToStartingNode();
        }

        public void ReturnToStartingNode()
        {
            gameObject.transform.position = StartingNode.transform.position;
            view.CurrentNodeObject = StartingNode.gameObject;
            if(!IsPlayer) GetComponent<EnemyHandler>().ReturnToStartingPosition();
        }

        public MovementController GetController()
        {
            return controller;
        }
    }
}