using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using Movement;
using Core;

namespace Tests
{
    
    public class MovementTest : InputTestFixture
    {
        GameObject player = Resources.Load<GameObject>("Player");
        GameObject tile = Resources.Load<GameObject>("Tile");
        GameObject instantiatedPlayer;
        Keyboard keyboard;
        Vector2 prevPos;

        [OneTimeSetUp]
        public void SetupMovementTest()
        {
            keyboard = InputSystem.AddDevice<Keyboard>();
            Quaternion q = Quaternion.Euler(0, 0, 180);
            GameObject instantiatedTile1 = Object.Instantiate(tile, new(1.5f, 2.5f), Quaternion.identity);
            GameObject instantiatedTile2 = Object.Instantiate(tile, new(1.5f, 1.5f), q);
            instantiatedPlayer = Object.Instantiate(player, Vector2.zero, Quaternion.identity);
            MovementSetup movementSetup = instantiatedPlayer.GetComponent<MovementSetup>();
            InputListener inputListener = instantiatedPlayer.AddComponent<InputListener>();
            movementSetup.Setup();
            inputListener.Construct(movementSetup.GetController());
        }

        [UnityTest]
        public IEnumerator MovementTest1()
        {
            MovementController controller = instantiatedPlayer.GetComponent<MovementSetup>().GetController();
            controller.InvokeSetDirection(Direction.Up);
            yield return new WaitForSeconds(1f);
            prevPos = instantiatedPlayer.transform.position;
            Assert.That(instantiatedPlayer.transform.position.y > 0, "it is done");
        }

        [UnityTest]
        public IEnumerator MovementTest2()
        {
            MovementController controller = instantiatedPlayer.GetComponent<MovementSetup>().GetController();
            controller.InvokeSetDirection(Direction.Down);
            yield return new WaitForSeconds(1.5f);
            Assert.That(instantiatedPlayer.transform.position.y < prevPos.y, "it is done twice");
        }
    }
    
}
