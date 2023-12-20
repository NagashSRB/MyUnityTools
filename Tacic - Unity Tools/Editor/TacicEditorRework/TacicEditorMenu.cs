using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Tacic.Tacic___Unity_Tools.Editor.TacicEditorRework
{
    public class TacicEditorMenu : EditorWindow
    {
        // TODO: Dodati jos jedan layer za rooms i generalne fje? Menu i pozivi za utils odvojeni za rooms i unity? 2 klase nove
        [MenuItem("GameObject/Talenzzo/Dodaj brojeve pored imena - Parent", false, 0)]
        static void AddNumbersToGameObjectNameMenuItem()
        {
            TacicEditorRooms.AddNumbersToGameObjectNameMenu();
        }

        [MenuItem("Talenzzo/Sortiraj pozicije objekata po hijerarhiji - Select", false, 0)]
        static void SortObjectsByPositionMenuItem()
        {
            // TODO : Da ima editor window da se unesu dimenzije matrice/liste. Da li je potrebno?
            // TODO : Proveriti za dodavanje i rotacije (dodeljivanje objektu)
            TacicEditorRooms.SortObjectsByPositionMenu();
        }
        
        [MenuItem("GameObject/Talenzzo/Napravi Position Holdere za child objekte - Parent", false, 0)]
        static void CreateHoldersForChildObjectMenuItem()
        {
            TacicEditorRooms.CreateHoldersForChildObjectMenu();
        }
        
        [MenuItem("Talenzzo/Zaokruzi poziciju na 4 decimale - Select", false, 0)]
        static void RoundBy4MenuItem()
        {
            TacicEditorRooms.RoundPosition(4);
        }

        [MenuItem("GameObject/Talenzzo/Podesi pozicije childova kao u PositionHolders - Parent", false, 0)]
        static void SetObjectPositionAsHolderMenuItem()
        {
            TacicEditorRooms.SetObjectPositionAsHolderMenu();
        }
        
        [MenuItem("Talenzzo/Swap 2 Selected object transforms", false, 0)]
        static void SwapSelectedObjectTransformMenuItem()
        {
            TacicEditorRooms.SwapSelectedObjectTransformMenu();
        } 
        
        // TODO: Fix. All children or selected only. 
        [MenuItem("Talenzzo/Increase order in layer from first to last", false, 0)]
        static void IncreaseOrderInLayerFromFirstToLastMenuItem()
        {
            TacicEditorRooms.IncreaseOrderInLayerFromFirstToLastMenu();
        }

        // TODO: Swap 2 objects in hierarchy
    }
}
