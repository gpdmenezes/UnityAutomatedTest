using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CharacterMovementTests
    {
        private Camera mainCamera;
        private CharacterInputController characterInputController;
        private int amountOfClicks = 10;
        private float delayBetweenClicks = 0.5f;

        [OneTimeSetUp]
        public void Setup()
        {
            mainCamera = new GameObject().AddComponent<Camera>();
            mainCamera.gameObject.AddComponent<AudioListener>();

            GameObject character = new GameObject();
            character.AddComponent<BoxCollider>();
            CharacterCollider characterCollider = character.AddComponent<CharacterCollider>();

            characterInputController = character.AddComponent<CharacterInputController>();
            characterInputController.characterCollider = characterCollider;

            GameObject blobShadow = new GameObject();
            characterInputController.blobShadow = blobShadow;

            GameObject trackManager = new GameObject();
            characterInputController.trackManager = trackManager.AddComponent<TrackManager>();

            characterInputController.StartMoving();
        }

        [UnityTest]
        public IEnumerator IsLeftMovementOnScreen ()
        {
            yield return null;

            for (int i = 0; i < amountOfClicks; i++)
            {
                characterInputController.ChangeLane(-1);
                yield return new WaitForSeconds(delayBetweenClicks);
            }

            float positionX = characterInputController.characterCollider.transform.position.x;
            Assert.IsTrue(positionX == -characterInputController.trackManager.laneOffset, "Position X (" + positionX + ") is misaligned.");
            Assert.IsTrue(TestUtils.IsGameObjectWithinScreenBounds(characterInputController.gameObject, mainCamera), "Character is off-screen.");
        }
    }

}