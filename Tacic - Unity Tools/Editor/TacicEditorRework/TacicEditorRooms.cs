using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework
{
    public class TacicEditorRooms : EditorWindow
    {
        #region Rooms
        
        //TODO: logerrors kod returna?

        /// <summary>
        /// Turn on all hotspots on or off the rooms holder
        /// </summary>
        /// <param name="roomsHolder">Rooms holder object</param>
        /// <param name="isHotspotActive">Should all hotspots be active or not</param>
        /// <param name="roomsHolderPath"></param>
        public static void SetActiveHotspots( bool isHotspotActive, string roomsHolderPath = null)
        {
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                roomsHolderPath = GetRoomsHolderAssetPath();
            }
            
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                Debug.LogError("Cannot find roomsholder object");
                return;
            }

            GameObject roomsHolder = StartEditingLevelPrefab(roomsHolderPath);
            
            foreach (Transform room in roomsHolder.transform)
            {
                var hotspots = room.GetComponentsInChildren<Hotspot>(true);
                Debug.Log($"Child counts: {hotspots.Length}");
                foreach (var hotspot in hotspots)
                {
                    hotspot.gameObject.SetActive(isHotspotActive);
                    hotspot.transform.GetChild(0).gameObject.SetActive(isHotspotActive);
                }
            }
            
            SaveLevelPrefab(roomsHolder, roomsHolderPath);
        }
    
        /// <summary>
        /// Dodaj brojeve pored imena
        /// </summary>
        public static void AddNumbersToGameObjectNameMenu()
        {
            GameObject selectedObject = TacicEditorUtils.GetOneSelectedObject();
            if (selectedObject == null)
            {
                return;
            }

            TacicEditorUtils.AddNumbersToChildsGameObjectName(selectedObject.transform);
        }
        
        /// <summary>
        /// Sortiraj pozicije objekata po hijerarhiji
        /// </summary>
        public static void SortObjectsByPositionMenu()
        {
            if (TacicEditorUtils.GetSelectedGameObjects() == null)
            {
                return;
            }

            List<GameObject> selectedObjects = TacicEditorUtils.GetSelectedGameObjects().ToList();
            TacicEditorUtils.SortGameObjectListBySiblingIndex(selectedObjects);
            TacicEditorUtils.SortObjectsByPosition(selectedObjects);
        }
        
        #endregion
        
        /// <summary>
        /// Napravi Position Holdere za child objekte
        /// </summary>
        public static void CreateHoldersForChildObjectMenu(string createdParentName = "PositionHolders", string createdObjectName = "PositionHolder")
        {
            GameObject fields = TacicEditorUtils.GetOneSelectedObject();
            if (fields == null)
            {
                return;
            }
            
            GameObject holders = TacicEditorUtils.CreateGameObject(createdParentName, fields.transform.parent);
            List<GameObject> holdersChildren =
                TacicEditorUtils.CreateGameObjects(createdObjectName, fields.transform.childCount, holders.transform);

            holders.transform.position = fields.transform.position;
            foreach (Transform field in fields.transform)
            {
                holdersChildren[field.GetSiblingIndex()].transform.position = field.position;
            }

            TacicEditorUtils.AddNumbersToChildsGameObjectName(holders.transform);
        }
        
        /// <summary>
        /// Round position
        /// </summary>
        /// <param name="digits">Rounding precision, number of digits</param>
        public static void RoundPosition(int digits = 2)
        {
            List<GameObject> selectedObject = TacicEditorUtils.GetSelectedGameObjects();
            if (selectedObject == null)
            {
                return;
            }

            TacicEditorUtils.RoundPositionOfGameObjects(selectedObject, digits);
        }

        /// <summary>
        /// Podesi pozicije childova kao u PositionHolders
        /// </summary>
        public static void SetObjectPositionAsHolderMenu()
        {
            GameObject selectedObject = TacicEditorUtils.GetOneSelectedObject();

            if (selectedObject == null)
            {
                return;
            }

            Transform elementsParent = selectedObject.transform;
            Transform holdersParent = elementsParent.transform.parent.Find("PositionHolders");

            if (elementsParent.childCount != holdersParent.childCount)
            {
                Debug.LogError("Elements and holders must have the same number of elements");
                return;
            }
            
            TacicEditorUtils.CopyChildPositions(holdersParent, elementsParent);
        }

        // Swap 2 Selected object transforms
        public static void SwapSelectedObjectTransformMenu()
        {
            List<GameObject> gameObjects = TacicEditorUtils.GetSelectedGameObjects();
            if (gameObjects != null && gameObjects.Count == 2)
            {
                TacicEditorUtils.SwapTwoTransforms(gameObjects[0].transform, gameObjects[1].transform);
            }
            else
            {
                Debug.LogError("You need to select exactly 2 objects");
            }
        }

        public static void SetLayersToAllZonesMenu(int zoomElementLayer, string roomsHolderPath = null)
        {
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                roomsHolderPath = GetRoomsHolderAssetPath();
            }
            
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                Debug.LogError("Cannot find roomsholder object");
                return;
            }

            GameObject roomsHolder = StartEditingLevelPrefab(roomsHolderPath);
            List<MiniGame> miniGames = GetAllMiniGames(roomsHolder.transform);
            List<ZoomZone> zoomZones = GetAllZoomZones(roomsHolder.transform);
            
            //TODO: zones posto se radi na sceni sve preko transforma ili go. transform bolje. moze concat

            foreach (MiniGame miniGame in miniGames)
            {
                DisableZonesAnimationHolder(miniGame.transform);
                TacicEditorUtils.SetLayerToChildren(miniGame.transform, zoomElementLayer, true);
            }
            
            foreach (ZoomZone zoomZone in zoomZones)
            {
                DisableZonesAnimationHolder(zoomZone.transform);
                TacicEditorUtils.SetLayerToChildren(zoomZone.transform, zoomElementLayer, true);
            }
            
            SaveLevelPrefab(roomsHolder, roomsHolderPath);
        }
        
        public static void CloseRoomsAndMiniGames(string roomsHolderPath = null)
        {
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                roomsHolderPath = GetRoomsHolderAssetPath();
            }
            
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                Debug.LogError("Cannot find roomsholder object");
                return;
            }

            GameObject roomsHolder = StartEditingLevelPrefab(roomsHolderPath);
            List<MiniGame> miniGames = GetAllMiniGames(roomsHolder.transform);
            List<ZoomZone> zoomZones = GetAllZoomZones(roomsHolder.transform);
            
            //TODO: zones posto se radi na sceni sve preko transforma ili go. transform bolje. moze concat

            foreach (MiniGame miniGame in miniGames)
            {
                DisableZonesAnimationHolder(miniGame.transform);
            }
            
            foreach (ZoomZone zoomZone in zoomZones)
            {
                DisableZonesAnimationHolder(zoomZone.transform);
            }

            EnableOnlyFirstRoom(roomsHolder.transform);
            
            SaveLevelPrefab(roomsHolder, roomsHolderPath);
        }
        
        public static void SetSizeForAllZones(string roomsHolderPath = null)
        {
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                roomsHolderPath = GetRoomsHolderAssetPath();
            }
            
            if (string.IsNullOrEmpty(roomsHolderPath))
            {
                Debug.LogError("Cannot find roomsholder object");
                return;
            }

            GameObject roomsHolder = StartEditingLevelPrefab(roomsHolderPath);
            List<MiniGame> miniGames = GetAllMiniGames(roomsHolder.transform);
            List<ZoomZone> zoomZones = GetAllZoomZones(roomsHolder.transform);
            
            //TODO: zones posto se radi na sceni sve preko transforma ili go. transform bolje. moze concat

            foreach (MiniGame miniGame in miniGames)
            {
                miniGame.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
                miniGame.transform.GetChild(0).localPosition = new Vector3(0f, 0.2f, 0f);
            }
            
            foreach (ZoomZone zoomZone in zoomZones)
            {
                zoomZone.transform.localScale = new Vector3(1.3f, 1.3f, 1f);
                zoomZone.transform.GetChild(0).localPosition = new Vector3(0f, 0.2f, 0f);
            }

            SaveLevelPrefab(roomsHolder, roomsHolderPath);
        }
        
        // TODO: Find workaround to work with both of them together. They don't inherit from same class 'Zone', that would be the best
        public static void CloseZonesAndRooms(Transform roomsHolder)
        {
            //Type type = typeof(ZoomZone);
            //inputGameObject.GetComponent(type);
            int zoomElementLayer = 8;

            List<MiniGame> miniGames = GetAllMiniGames(roomsHolder);
            List<ZoomZone> zoomZones = GetAllZoomZones(roomsHolder);
            
            //TODO: zones posto se radi na sceni sve preko transforma ili go. transform bolje. moze concat

            foreach (MiniGame miniGame in miniGames)
            {
                DisableZonesAnimationHolder(miniGame.transform);
                TacicEditorUtils.SetLayerToChildren(miniGame.transform, zoomElementLayer, true);
            }
            
            foreach (ZoomZone zoomZone in zoomZones)
            {
                DisableZonesAnimationHolder(zoomZone.transform);
                TacicEditorUtils.SetLayerToChildren(zoomZone.transform, zoomElementLayer, true);
            }
            
            EnableOnlyFirstRoom(roomsHolder);
        }

        /// <summary>
        /// Increase order in layer from first to last
        /// </summary>
        /// TODO: Fix
        public static void IncreaseOrderInLayerFromFirstToLastMenu()
        {
            List<GameObject> selectedGameObjects = TacicEditorUtils.GetSelectedGameObjects();
            if (selectedGameObjects == null || selectedGameObjects.Count == 0)
            {
                return;
            }

            if (!TacicEditorUtils.IsParentSameForObjects(selectedGameObjects))
            {
                Debug.LogError("Parents are not the same for selected objects");
            }
            
            Transform parent = selectedGameObjects[0].transform.parent;

            List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

            // TODO: Not working well. it goes trhough all children
            foreach (Transform selectedGameObject in parent)
            {
                if (selectedGameObject.GetComponent<SpriteRenderer>())
                {
                    spriteRenderers.Add(selectedGameObject.GetComponent<SpriteRenderer>());
                }
                else
                {
                    Debug.LogError("Nemaju svi sprite renderer");
                    return;
                }
            }

            int sortingOrder = spriteRenderers[0].sortingOrder;
            foreach (SpriteRenderer renderer in spriteRenderers)
            {
                Undo.RecordObject(renderer, "Sorting Order Changed");
                renderer.sortingOrder = sortingOrder;
                sortingOrder++;
            }
        }
        
        
        // ----------------------------------------------------------------
        // Rooms Helper Functions

        #region Rooms Helper Functions

        public static List<MiniGame> GetAllMiniGames(Transform roomsHolder)
        {
            List<MiniGame> miniGames = new List<MiniGame>();
            foreach (Transform room in roomsHolder)
            {
                foreach (Transform zone in room)
                {
                    MiniGame miniGame = zone.gameObject.GetComponent<MiniGame>();
                    if (miniGame)
                    {
                        miniGames.Add(miniGame);
                    }
                }
            }

            return miniGames;
        }
        
        public static List<ZoomZone> GetAllZoomZones(Transform roomsHolder)
        {
            List<ZoomZone> zoomZones = new List<ZoomZone>();
            foreach (Transform room in roomsHolder)
            {
                foreach (Transform zone in room)
                {
                    ZoomZone zoomZone = zone.gameObject.GetComponent<ZoomZone>();
                    if (zoomZone)
                    {
                        zoomZones.Add(zoomZone);
                    }
                }
            }

            return zoomZones;
        }

        public static List<Transform> GetAllRooms(Transform roomsHolder)
        {
            List<Transform> rooms = new List<Transform>();
            foreach (Transform room in roomsHolder)
            {
                rooms.Add(room);
            }

            return rooms;
        }

        public static void EnableOnlyFirstRoom(Transform roomsHolder)
        {
            List<Transform> rooms = GetAllRooms(roomsHolder);

            if (rooms.Count == 0)
            {
                Debug.LogError("No rooms found");
                return;
            }
            
            foreach (Transform room in rooms)
            {
                if (room.gameObject.name != "AudioObjects")
                {
                    room.gameObject.SetActive(false);
                }
            }
            
            rooms[0].gameObject.SetActive(true);
        }

        // If minigame and zoomzone would inherit from same class (zone), i could make lists of them together
        public static void DisableZonesAnimationHolder(Transform zone)
        {
            if (!zone.GetChild(0).gameObject.activeSelf)
            {
                return;
            }
            
            zone.GetChild(0).gameObject.SetActive(false);
            Debug.Log($"Disabled animation holder for zone: " + zone.name);
        }
        
        public static string GetRoomsHolderAssetPath()
        {
            PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
            if (stage != null && stage.prefabContentsRoot.name == "RoomsHolder")
            {
                return stage.assetPath;
            }
            
            GameObject sceneRoomsHolder = GameObject.Find("RoomsHolder");
            if (sceneRoomsHolder)
            {
                return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(sceneRoomsHolder);
            }

            return "";
        }
        
        public static GameObject StartEditingLevelPrefab(string roomsHolderPath) => PrefabUtility.LoadPrefabContents(roomsHolderPath);
    
        public static void SaveLevelPrefab(GameObject roomsHolder, string roomsHolderPath)
        {
            PrefabUtility.SaveAsPrefabAsset(roomsHolder, roomsHolderPath);
            PrefabUtility.UnloadPrefabContents(roomsHolder);
        }

        
        #endregion
    }
}

