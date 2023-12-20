using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.MiniGame_Base.SwapElementsByDragging.v1___level_8
{
    public class DragElementsScript : MonoBehaviour
    {
        public List<int> originalPositionIndices;
        public int currentPositionIndex;

        public float draggingSpeed;

        public bool isHoldingItem;

        public Vector3 startPosition;

        public Vector3 offsetVector;

        public bool returnToStartPosition;

        public bool returningToStartPosition;

        public Vector3 tapStartPosition;

        // Promenljiva koja nam proverava da li smo tapnuli na objekat
        public bool isTap;

        public bool setOrderOnPick;
        public int orderOnPick;
        public int orderOnDrop;

        void Awake()
        {
            if (setOrderOnPick)
                orderOnDrop = GetComponent<SpriteRenderer>().sortingOrder;

            isHoldingItem = false;
            returningToStartPosition = false;
            isTap = false;
            StartCoroutine("SetStartPositionCoroutine");
        }

        /// <summary>
        /// Postavlja startnu poziciju elementa nakon 0.1 sekunde.
        /// </summary>
        IEnumerator SetStartPositionCoroutine()
        {
            // yield return new WaitForEndOfFrame();
            yield return new WaitForSeconds(0.1f);
            startPosition = transform.localPosition;// GetComponent<RectTransform>().anchoredPosition;
        }

        public void OnMouseDown()
        {
            if (GameplayManager.Instance.AreAllUIElementsClosed() && DragElementsToPlacesManager.Instance.gameStarted && !DragElementsToPlacesManager.Instance.CheckIfAllElementsAreSet())
            {
                if (setOrderOnPick)
                    GetComponent<SpriteRenderer>().sortingOrder = orderOnPick;
                else
                    GetComponent<SpriteRenderer>().sortingOrder = 3;

                isTap = true;

                tapStartPosition = Input.mousePosition;

                CancelInvoke("SetInputType");
                Invoke("SetInputType", 0.05f);

                isHoldingItem = true;
            }
        }

        public void OnMouseUp()
        {
            if (GameplayManager.Instance.AreAllUIElementsClosed() && DragElementsToPlacesManager.Instance.gameStarted && !DragElementsToPlacesManager.Instance.CheckIfAllElementsAreSet())
            {
                isHoldingItem = false;

                returningToStartPosition = true;

                Transform hit = RaycastFunction2D(Input.mousePosition, GameplayManager.Instance.zoomElementsCamera);

                if (hit != null)
                {
                    DragElementsToPlacesManager.Instance.SwapElements(hit.GetComponent<DragElementsScript>(), this);
                }

                DragElementsToPlacesManager.Instance.selectedElement = null;
            }
        }

        /// <summary>
        /// Funkcija koja resetuje isTap promenljivu kada nije tap u pitanju (swipe).
        /// </summary>
        public void SetInputType()
        {
            isTap = false;
        }

        public void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                isHoldingItem = false;

                returningToStartPosition = true;
            }
        }

        void Update()
        {
            if (isHoldingItem && DragElementsToPlacesManager.Instance.selectedElement == null && !isTap)
            {
                Vector3 screenPoint = (Vector3)Input.mousePosition/* + offsetVector*/;
                screenPoint.z = 100f;

                transform.position = Vector3.Lerp(transform.position, /*Camera.main.ScreenToWorldPoint(screenPoint)*/ GameplayManager.Instance.zoomElementsCamera.ScreenToWorldPoint(screenPoint) + offsetVector, draggingSpeed * Time.deltaTime);
            }

            if (!isHoldingItem && returnToStartPosition && returningToStartPosition)
            {
                Vector3 screenPoint = startPosition;
                screenPoint.z = 10f;

                transform.localPosition = Vector3.Lerp(transform.localPosition, screenPoint, draggingSpeed * Time.deltaTime / 1.5f);

                if (Vector3.Distance(transform.localPosition, screenPoint) < 0.2f)
                {
                    transform.localPosition = screenPoint;
                    returningToStartPosition = false;

                    // FIXME Ako se poklapaju indexi za sada cu da stavim samo da alfa bude manji
                    if (originalPositionIndices.Contains(currentPositionIndex))
                    {
                        // Pustamo zvuk kada se namesti puzzle deo
                        //SoundManager.Instance.Play_MosaicPartCorrect();

                        if (DragElementsToPlacesManager.Instance.CheckIfAllElementsAreSet() && DragElementsToPlacesManager.Instance.gameStarted)
                            DragElementsToPlacesManager.Instance.MiniGameFinished();
                    }

                    if (setOrderOnPick)
                        GetComponent<SpriteRenderer>().sortingOrder = orderOnDrop;
                    else
                        GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
            }
        }

        void OnTriggerEnter2D(Collider2D coll)
        {
            if (isHoldingItem && coll.GetComponent<DragElementsScript>() != null)
            {
                // Prvo zamenimo ova dva elementa
                DragElementsToPlacesManager.Instance.SwapElements(this, coll.GetComponent<DragElementsScript>());

                // Zatim iskljucimo da se ovaj objekat ne drzi vise
                isHoldingItem = false;
            }
        }

        Transform RaycastFunction2D(Vector3 pos, Camera cam)
        {
            RaycastHit2D[] hits;

            hits = Physics2D.RaycastAll(cam.ScreenToWorldPoint(pos), Vector2.zero);

            if (hits.Length > 0)
            {
                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].transform.GetComponent<DragElementsScript>() != null && hits[i].transform.GetComponent<DragElementsScript>() != this)
                        return hits[i].transform;

                }
            }

            return null;
        }
    }
}