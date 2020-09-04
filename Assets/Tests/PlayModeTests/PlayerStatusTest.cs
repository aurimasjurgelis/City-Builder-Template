using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class PlayerStatusTest
    {

        UIController uiController;
        GameManager gameManagerComponent;

        [SetUp]
        public void Init()
        {
            GameObject gameManagerObject = new GameObject();
            var cameraMovementComponent = gameManagerObject.AddComponent<CameraMovement>();

            uiController = gameManagerObject.AddComponent<UIController>();
            GameObject buildButtonObject = new GameObject();
            GameObject cancelButtonObject = new GameObject();
            GameObject cancelPanel = new GameObject();
            uiController.cancelActionButton = cancelButtonObject.AddComponent<Button>();
            var buttonBuildComponent = buildButtonObject.AddComponent<Button>();

            uiController.buildResidentialAreaButton = buttonBuildComponent;
            uiController.cancelActionPanel = cancelButtonObject;
            uiController.demolishButton = uiController.cancelActionButton;

            gameManagerComponent = gameManagerObject.AddComponent<GameManager>();
            gameManagerComponent.cameraMovement = cameraMovementComponent;
            gameManagerComponent.uiController = uiController;
        }

        [UnityTest]
        public IEnumerator PlayerStatusBuildingSingleStructureStateTestWithEnumeratorPasses()
        {


            yield return new WaitForEndOfFrame(); //waits to execute awake method
            yield return new WaitForEndOfFrame(); //=> executes start method
            uiController.buildResidentialAreaButton.onClick.Invoke();
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(gameManagerComponent.State is PlayerBuildingSingleStructureState);
        }

        [UnityTest]
        public IEnumerator PlayerStatusSelectionStateTestWithEnumeratorPasses()
        {


            yield return new WaitForEndOfFrame(); //waits to execute awake method
            yield return new WaitForEndOfFrame(); //=> executes sstart method
            Assert.IsTrue(gameManagerComponent.State is PlayerSelectionState);
        }


    }
}
