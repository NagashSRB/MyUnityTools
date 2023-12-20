using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Scripts.In_Progress.BoardGame.SpecificTypes.BoardStructure.GraphBoard
{
    public class TrailPart : SpriteLineRenderer
    {
        private GameObject trailGO;
        private BoardPath path;
        public Transform partStartPoint;
        public Transform partEndPoint;
        public void Initialize(Transform trailStartPoint, BoardPath currentPath, GameObject trailPrefabRoot)
        {
            trailGO = trailPrefabRoot;
            path = currentPath;
            partStartPoint = trailStartPoint;
            partEndPoint = partStartPoint == currentPath.startField.transform ? currentPath.endField.transform : currentPath.startField.transform;
        
            base.Initialize(partStartPoint, partEndPoint);
            DrawLine();
        }
        
        public void SetTrailColor(Color color)
        {
            spriteRenderer.color = color;
        }

        public bool IsTrailPartOnPath(BoardPath pathToCheck) => path == pathToCheck;
        
        public bool IsEndingPoint(Transform pointToCheck) => pointToCheck == partEndPoint;
        
        public bool IsStartingPoint(Transform pointToCheck) => pointToCheck == partStartPoint;
        

        public bool IsPartConnectedToPath(BoardPath pathToCheck)
        {
            return partEndPoint == pathToCheck.startField.transform || partEndPoint == pathToCheck.endField.transform;
        }

        public bool DoesPartStartFromSameField(BoardPath path)
        {
            return partStartPoint == path.startField.transform || partStartPoint == path.endField.transform;
        }


        public void DrawTrailPart(Transform targetPoint)
        {
            SetUpPoints(partStartPoint, targetPoint);
            DrawLine();
        }

        public void DrawWholeTrailPart()
        {
            SetUpPoints(partStartPoint, partEndPoint);
            DrawLine();
        }
        
        public void DestroyTrailPart()
        {
            Destroy(trailGO);
        }
    }
}