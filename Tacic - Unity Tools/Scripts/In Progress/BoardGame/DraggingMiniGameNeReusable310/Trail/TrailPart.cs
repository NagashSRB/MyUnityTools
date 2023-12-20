using UnityEngine;

namespace WebelinxGames.RoomsAndExits.Level310
{
    public class TrailPart : SpriteLineRenderer
    {
        private GameObject trailGO;
        private BoardPath path;
        public Transform partStartPoint;
        public Transform partEndPoint;
        
        private float drawingOffset = 0.3f;
        public void Initialize(Transform trailStartPoint, BoardPath currentPath, GameObject trailPrefabRoot)
        {
            trailGO = trailPrefabRoot;
            path = currentPath;
            partStartPoint = trailStartPoint;
            partEndPoint = partStartPoint == currentPath.startField.transform ? currentPath.endField.transform : currentPath.startField.transform;
        
            base.Initialize(partStartPoint, partEndPoint, drawingOffset);
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
            SetUpPoints(partStartPoint, targetPoint, drawingOffset);
            DrawLine();
        }

        public void DrawWholeTrailPart()
        {
            SetUpPoints(partStartPoint, partEndPoint, drawingOffset);
            DrawLine();
        }
        
        public void DestroyTrailPart()
        {
            Destroy(trailGO);
        }
    }
}