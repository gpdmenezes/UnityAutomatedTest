using System.Collections;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CharacterTests
    {
        private Camera mainCamera;
        private CharacterInputController characterInputController;

        [OneTimeSetUp]
        public void Setup ()
        {
            mainCamera = new GameObject("Main Camera").AddComponent<Camera>();
            mainCamera.gameObject.AddComponent<AudioListener>();

            GameObject character = new GameObject("Character");
            character.AddComponent<Rigidbody>().useGravity = false;
            character.AddComponent<BoxCollider>();
            CharacterCollider characterCollider = character.AddComponent<CharacterCollider>();

            characterInputController = character.AddComponent<CharacterInputController>();
            characterInputController.characterCollider = characterCollider;
            characterCollider.controller = characterInputController;

            GameObject blobShadow = new GameObject("BlobShadow");
            characterInputController.blobShadow = blobShadow;

            GameObject trackManager = new GameObject("TrackManager");
            characterInputController.trackManager = trackManager.AddComponent<TrackManager>();
            characterInputController.trackManager.DisableTrackManagerUpdate();

            characterInputController.StartMoving();
        }

        [UnityTest, Order(1)]
        public IEnumerator IsCoinPickupAddingValue()
        {
            yield return null;

            PlayerData.Create();
            int initialCoins = PlayerData.instance.coins;
            Assert.IsTrue(PlayerData.instance.coins == 0, "PlayerData 'coins' was not properly initialized.");

            GameObject coinPickupPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Pickup.prefab");
            Coin.coinPool = new Pooler(coinPickupPrefab, 256);
            GameObject coinPickup = Coin.coinPool.Get();
            coinPickup.transform.position = characterInputController.transform.position;

            yield return null;

            Assert.IsTrue(PlayerData.instance.coins == 1, "PlayerData 'coins' was not properly updated. (" + PlayerData.instance.coins + ")");
        }

        [UnityTest, Order(2)]
        public IEnumerator IsLeftMovementOnScreen ()
        {
            yield return null;

            int amountOfClicks = 10;
            float delayBetweenClicks = 0.25f;

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