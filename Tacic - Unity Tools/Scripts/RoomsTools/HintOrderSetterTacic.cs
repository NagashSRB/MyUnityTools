// using System.Collections;
// using System.Collections.Generic;
// using UnityEditor;
// using UnityEngine;
//
// namespace  Tacic
// {
//     public class HintOrderSetterTacic : MonoBehaviour
//     {
//         public List<GameObject> hintOrder = new List<GameObject>();
//
//         [ContextMenu("Set Hint Order")]
//         public void SetHintOrder()
//         {
// #if UNITY_EDITOR
//             for (int i = 0; i < hintOrder.Count; i++)
//             {
//                 if (!hintOrder[i])
//                 {
//                     //Ako je prazno polje, preskacemo ga. To se podesava rucno
//                     //InventoryItem, MultipleIndicesTarget, Dismantle, Combine...
//                     continue;
//                 }
//             
//                 if (hintOrder[i].TryGetComponent<MiniGameItem>(out MiniGameItem miniGameItem))
//                 {
//                     miniGameItem.hintPriority = i;
//                 }
//                 else if (hintOrder[i].TryGetComponent<ActiveItem>(out ActiveItem activeItem))
//                 {
//                     activeItem.hintPriority = i;
//                 }
//                 else if (hintOrder[i].TryGetComponent<TargetItem>(out TargetItem targetItem))
//                 {
//                     targetItem.hintPriority = i;
//                 }
//                 else if (hintOrder[i].TryGetComponent<AnimationItem>(out AnimationItem animationItem))
//                 {
//                     animationItem.hintPriority = i;
//                 }
//                 else if (hintOrder[i].TryGetComponent<InventoryItem>(out InventoryItem combinedInventoryItem) && combinedInventoryItem.isCombinedItem && combinedInventoryItem.combineHint)
//                 {
//                     //set hint priority for the combined item
//                     combinedInventoryItem.combineHintPriority = i;
//
//                     //set hint priority for the items used to create the combined item
//                     foreach (GameObject item in combinedInventoryItem.itemsUsedToMakeThisItem)
//                     {
//                         InventoryItem currentItem = item.GetComponent<InventoryItem>();
//                         currentItem.combineHintPriority = i;
//
//                         //if the item is not marked as combine hint, mark it
//                         if (!currentItem.combineHint)
//                         {
//                             currentItem.combineHint = true;
//                         }
//                     }
//                 }
//                 else if (hintOrder[i].TryGetComponent<InventoryItem>(out InventoryItem dismantledInventoryItem) 
//                          && dismantledInventoryItem.isCombinedItem && dismantledInventoryItem.dismantleHint)
//                 {
//                     //set hint priority for the dismantle item
//                     dismantledInventoryItem.dismantleHintPriority = i;
//
//                     //set hint priority for items received after dismantling
//                     foreach (GameObject item in dismantledInventoryItem.itemsUsedToMakeThisItem)
//                     {
//                         InventoryItem currentItem = item.GetComponent<InventoryItem>();
//                         currentItem.dismantleHintPriority = i;
//
//                         //if the item is not marked as dismantle hint, mark it
//                         if (!currentItem.dismantleHint)
//                         {
//                             currentItem.dismantleHint = true;
//                         }
//                     }
//                 }            
//             }
//         
//             EditorUtility.SetDirty(gameObject);
// #endif
//         }
//   
//         [ContextMenu("Get All Components For Hints")]
//         public void GetAllHintComponents()
//         {
// #if UNITY_EDITOR
//             var children = transform.GetComponentsInChildren<Component>(true);
//             List<InventoryItem> inventoryItems = new List<InventoryItem>();
//             foreach (var child in children)
//             {
//                 if (GetSelectedGameObjectHintPriorityAttribute(child))
//                 {
//                     if (!hintOrder.Contains(child.gameObject))
//                     {
//                         hintOrder.Add(child.gameObject);
//                     }
//                 }
//             
//                 ActiveItem activeItem = child as ActiveItem;
//                 if (activeItem != null)
//                 {
//                     GameObject inventoryItem = activeItem.itemForInventory;
//                     if (inventoryItem.TryGetComponent<InventoryItem>(out InventoryItem inventoryItemComponent)
//                         && (inventoryItemComponent.dismantleHint || inventoryItemComponent.combineHint))
//                     {
//                         if (!hintOrder.Contains(inventoryItem))
//                         {
//                             hintOrder.Add(inventoryItem);
//                         }
//                     }
//                 }
//
//             }
// #endif
//         }
//     
//         static bool GetSelectedGameObjectHintPriorityAttribute(Component component)
//         {
//             SerializedObject serializedComponent = new SerializedObject(component);
//             serializedComponent.Update();
//             string hintPriorityPropertyName = "hintPriority";
//             SerializedProperty hintPriorityProperty = serializedComponent.FindProperty(hintPriorityPropertyName);
//        
//             if (hintPriorityProperty == null)
//             {
//                 Debug.Log($"HintPriority not found for object {component.name} / {component.ToString()}");
//                 return false;
//             }
//
//             Debug.Log($"HintPriority :Component: {component.name} / {component.ToString()}, Value:{hintPriorityProperty.intValue}");
//
//             return true;
//         }
//     }
// }