using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MiniGames
{
    public class WiringGame : MonoBehaviour
    {
        public RectTransform[] leftEndpoints;
        public RectTransform[] rightEndpoints;

        public GameObject wireHandlePrefab;
        public RectTransform wiresParent;

        private List<WireHandle> handles = new List<WireHandle>();
        internal Dictionary<int, RectTransform> rightMap = new Dictionary<int, RectTransform>();
        private int solvedCount = 0;

        public GameObject winningMessage;

        void Start()
        {
            // Color left endpoints
            for (int i = 0; i < leftEndpoints.Length; i++)
            {
                var img = leftEndpoints[i]?.GetComponent<Image>();
                if (img != null)
                    img.color = GetColor(i);
            }

            // Shuffle and color right endpoints
            ShuffleEndpoints(rightEndpoints);
            for (int i = 0; i < rightEndpoints.Length; i++)
            {
                rightMap[i] = rightEndpoints[i];
                var img = rightEndpoints[i].GetComponent<Image>();
                if (img != null)
                    img.color = GetColor(i);
            }

            // Instantiate handles
            for (int i = 0; i < leftEndpoints.Length; i++)
            {
                if (leftEndpoints[i] == null) continue;
                var handleGO = Instantiate(wireHandlePrefab, wiresParent);
                var handle = handleGO.GetComponent<WireHandle>();
                handle.Initialize(i, leftEndpoints[i], HandleMatched, GetColor(i), this, wiresParent);
                handles.Add(handle);
            }
        }

        void ShuffleEndpoints(RectTransform[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                var temp = array[i];
                int rand = Random.Range(i, array.Length);
                array[i] = array[rand];
                array[rand] = temp;
            }
        }

        Color GetColor(int index)
        {
            Color[] palette = { Color.red, Color.green, Color.blue, Color.yellow, Color.magenta, Color.cyan, new Color(1f, 0.5f, 0f), new Color(0.5f, 0f, 1f), Color.white, new Color(0.5f, 0.25f, 0f)};
            return palette[index % palette.Length];
            
        }

        void HandleMatched(int handleId)
        {
            solvedCount++;
            if (solvedCount >= leftEndpoints.Length)
            {
                winningMessage.SetActive(true);
            }
        }
    }
}