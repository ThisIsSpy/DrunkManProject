using Level;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class MovementSetup : MonoBehaviour
    {
        [field : SerializeField] public bool IsPlayer { get; private set; }
        [SerializeField, Range(0,10)] private int speed;
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private Node startingNode;

        private MovementModel model;
        private MovementView view;
        private MovementController controller;

        public void Setup()
        {
            model = new(speed);
            view = GetComponent<MovementView>();
            if(startingNode == null) startingNode = FindFirstObjectByType<Node>();
            gameObject.transform.position = startingNode.transform.position;
            view.Construct(sprites, GetComponent<SpriteRenderer>(), startingNode.gameObject, model, !IsPlayer);
            controller = new(view);
        }

        public MovementController GetController()
        {
            return controller;
        }
    }
}