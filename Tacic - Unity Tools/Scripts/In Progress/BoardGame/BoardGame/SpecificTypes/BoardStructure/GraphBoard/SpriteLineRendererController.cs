using System.Collections;
using System.Collections.Generic;
using Firebase.Analytics;
using UnityEngine;
using WebelinxGames.RoomsAndExits.Level215;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    public class SpriteLineRendererController : MonoBehaviour
    {
        [SerializeField] protected List<SpriteLineRenderer> lineRenderers;
        protected List<Transform> points;

        public void SetUpPoints(List<Transform> linePoints = null)
        {
            points = linePoints;
            for (int i = 0; i < points.Count - 1; i++)
            {
                lineRenderers[i].Initialize(points[i], points[i + 1]);
            }
        }

        public void DrawLines()
        {
            foreach (SpriteLineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.DrawLine();
            }
        }

        public void SetActive(bool isActive)
        {
            foreach (SpriteLineRenderer lineRenderer in lineRenderers)
            {
                lineRenderer.SetActive(isActive);
            }
        }
    }
}