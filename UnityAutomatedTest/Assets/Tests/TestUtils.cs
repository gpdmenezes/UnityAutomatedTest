using UnityEngine;

namespace Tests
{
    public static class TestUtils
    {
        public static bool IsGameObjectWithinScreenBounds (GameObject gameObject, Camera mainCamera)
        {
            Vector3 objectPosition = gameObject.transform.position;
            Vector3 screenPoint = mainCamera.WorldToScreenPoint(objectPosition);
            return (screenPoint.x >= 0 && screenPoint.x <= Screen.width) && (screenPoint.y >= 0 && screenPoint.y <= Screen.height);
        }
    }
}
