using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MiniGames
{
    public class WireHandle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int id;
        private RectTransform handleRect;
        private RectTransform startPoint;
        private System.Action<int> onMatched;
        private Image img;
        private bool matched = false;
        private RectTransform lineRect;
        private Image lineImg;
        private WiringGame parentGame;
        private RectTransform canvasTransform;

        /// <summary>
        /// Initializes the handle and creates a UI-based line.
        /// </summary>
        public void Initialize(int handleId, RectTransform start, System.Action<int> callback, Color color, WiringGame parent, RectTransform wiresParent)
        {
            id = handleId;
            startPoint = start;
            onMatched = callback;
            parentGame = parent;
            canvasTransform = wiresParent;

            handleRect = GetComponent<RectTransform>();
            img = GetComponent<Image>();


            // Set handle color and position
            handleRect.position = start.position;

            // Create UI Image for line
            GameObject lineGO = new GameObject($"WireLine_{id}", typeof(RectTransform), typeof(Image));
            lineGO.transform.SetParent(canvasTransform, false);
            lineRect = lineGO.GetComponent<RectTransform>();
            lineImg = lineGO.GetComponent<Image>();
            lineImg.color = color;
            lineImg.raycastTarget = false;

            // Initialize line as zero-length
            lineRect.sizeDelta = Vector2.zero;
            lineRect.anchoredPosition = canvasTransform.InverseTransformPoint(start.position);
            lineRect.localRotation = Quaternion.identity;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (matched) return;
            UpdateLine(eventData.position);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (matched) return;
            handleRect.position = eventData.position;
            UpdateLine(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (matched || parentGame == null) return;
            if (!parentGame.rightMap.TryGetValue(id, out RectTransform target) || target == null)
            {
                ResetHandle();
                return;
            }

            float dist = Vector2.Distance(handleRect.position, target.position);
            if (dist < 50f)
            {
                matched = true;
                handleRect.position = target.position;
                UpdateLine(target.position);
                img.enabled = false;
                onMatched?.Invoke(id);
            }
            else
            {
                ResetHandle();
            }
        }

        private void ResetHandle()
        {
            if (startPoint != null && handleRect != null)
            {
                handleRect.position = startPoint.position;
                UpdateLine(startPoint.position);
            }
        }

        private void UpdateLine(Vector3 worldPosition)
        {
            if (lineRect == null) return;
            Vector2 localStart = canvasTransform.InverseTransformPoint(startPoint.position);
            Vector2 localEnd = canvasTransform.InverseTransformPoint(worldPosition);
            Vector2 direction = localEnd - localStart;
            float length = direction.magnitude;
            lineRect.sizeDelta = new Vector2(length, 40f);
            lineRect.anchoredPosition = localStart + direction * 0.5f;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            lineRect.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
