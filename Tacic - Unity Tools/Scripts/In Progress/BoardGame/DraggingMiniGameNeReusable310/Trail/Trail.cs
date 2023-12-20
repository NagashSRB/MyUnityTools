using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace WebelinxGames.RoomsAndExits.Level310
{
    public class Trail : MonoBehaviour
    {
        [SerializeField] private List<TrailPart> previousTrailParts;
        [SerializeField] private GameObject trailPartPrefab;
        [SerializeField] private Transform trailParent;
        [SerializeField] private Color trailColor;
        public TrailPart currentTrailPart;
        public List<Transform> visitedPoints;

        public void Initialize(List<Transform> trailNodes = null)
        {
            previousTrailParts ??= new List<TrailPart>();
            visitedPoints ??= new List<Transform>();
        }

        public void AddTrailPart(BoardPath currentPath, Transform closestField, Transform startPoint = null)
        {
            if (startPoint == null)
            {
                startPoint = closestField;
            }

            if (currentTrailPart != null)
            {
                // Adding new trail part in addition to previous trail part.
                if (currentTrailPart.IsPartConnectedToPath(currentPath))
                {
                    startPoint = currentTrailPart.partEndPoint;
                }
                else
                {
                    // When changing path, remove previous trail part because you are taking alternative route
                    if (currentTrailPart.DoesPartStartFromSameField(currentPath))
                    {
                        startPoint = currentTrailPart.partStartPoint;
                        RemoveLastTrailPart();
                    }
                }
                currentTrailPart.DrawWholeTrailPart();
                previousTrailParts.Add(currentTrailPart);
            }
            
            GameObject trailPartPrefabRoot = Instantiate(trailPartPrefab, trailParent);
            TrailPart trailPart = trailPartPrefabRoot.GetComponent<TrailPart>();
            trailPart.SetTrailColor(trailColor);
            
            currentTrailPart = trailPart;
            currentTrailPart.Initialize(startPoint, currentPath, trailPartPrefabRoot);
        }

        public void RemoveLastTrailPart()
        {
            if (currentTrailPart != null)
            {
                if (visitedPoints.Contains(currentTrailPart.partStartPoint))
                {
                    visitedPoints.Remove(currentTrailPart.partStartPoint);
                }
                
                currentTrailPart.DestroyTrailPart();
            }

            if (previousTrailParts.Count > 0)
            {
                
                currentTrailPart = previousTrailParts[previousTrailParts.Count - 1];
                previousTrailParts.RemoveAt(previousTrailParts.Count - 1);
            }
            else
            {
                currentTrailPart = null;
            }
        }

        public void DrawTrail()
        {
            if (currentTrailPart != null)
            {
                currentTrailPart.DrawTrailPart(transform);
            }
        }

        public bool IsTrailGoingBackwards(BoardPath currentPath)
        {
            if (previousTrailParts == null || previousTrailParts.Count == 0)
            {
                return false;
            }

            return previousTrailParts[previousTrailParts.Count - 1].IsTrailPartOnPath(currentPath);
        }

        public bool IsTrailCurrentlyInPath(BoardPath currentPath) => currentTrailPart != null && currentTrailPart.IsTrailPartOnPath(currentPath);

        public bool IsTrailConnectedToPath(BoardPath currentPath) =>
            currentTrailPart == null || currentTrailPart.IsPartConnectedToPath(currentPath);
        
        public bool IsPathAlternativeToCurrentPart(BoardPath currentPath) => currentTrailPart.DoesPartStartFromSameField(currentPath);

        public void SetActive(bool isActive)
        {
            foreach (TrailPart trailPart in previousTrailParts)
            {
                trailPart.SetActive(isActive);
            }

            if (currentTrailPart != null)
            {
                currentTrailPart.SetActive(isActive);
            }
        }

        public void ResetTrail()
        {
            visitedPoints = new List<Transform>();
            while (previousTrailParts.Count > 0)
            {
                RemoveLastTrailPart();
            }
            
            currentTrailPart.DestroyTrailPart();
            currentTrailPart = null;
        }
    }
}