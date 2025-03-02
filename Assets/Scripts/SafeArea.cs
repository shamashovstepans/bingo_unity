using UnityEngine;

namespace BingoGame
{
    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        [SerializeField] private RectTransform rectTransform;
        private Rect lastSafeArea;

        void Awake()
        {
            ApplySafeArea();
        }

        void ApplySafeArea()
        {
            Rect safeArea = Screen.safeArea;

            if (safeArea == lastSafeArea) return; // No change, no need to update
            lastSafeArea = safeArea;

            Vector2 anchorMin = safeArea.position;
            Vector2 anchorMax = safeArea.position + safeArea.size;

            // Convert pixels to anchor values (0-1)
            anchorMin.x /= Screen.width;
            anchorMin.y /= Screen.height;
            anchorMax.x /= Screen.width;
            anchorMax.y /= Screen.height;

            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        void Update()
        {
            if (Screen.safeArea != lastSafeArea) // Check for orientation changes
                ApplySafeArea();
        }
    }
}