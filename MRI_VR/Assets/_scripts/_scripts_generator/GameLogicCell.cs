/*  
    Copyright (C) <2020>  <Valentin Bourdon>
   
    Author: Valentin Bourdon -- <vr@fcbg.ch>
    Created: 9/14/2020
   
    This file is part of VR-MRI Framework.

    VR-MRI Framework is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    VR-MRI Framework is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with Foobar.  If not, see<https://www.gnu.org/licenses/>.

*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.EventSystems;

public class GameLogicCell : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public enum TypeOfLogic
    {
        Loop,
        Switch,
        While
    }

    public TypeOfLogic typeOfLogic;
    GameObject listFonctionnalitiesContent;
    public GameObject experimentationCell;
    public bool isOver = false;
    public bool overIsOverrideByItem = false;
    
    DragAndDropItem[] listItems;
    DragAndDropItem currentItem;

    private void Start()
    {
        listFonctionnalitiesContent = GameObject.Find("Drag_List").transform.GetChild(0).transform.GetChild(0).Find("Content").transform.gameObject;
        listItems = listFonctionnalitiesContent.transform.GetComponentsInChildren<DragAndDropItem>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        isOver = true;
    }

    public void OnPointerExit(PointerEventData data)
    {
        isOver = false;
    }


    public void Update()
    {
        if (isOver)
        {
            DragAndDropCell[] childListCells;
            childListCells = transform.Find("Item Content").GetComponentsInChildren<DragAndDropCell>();
            Debug.Log("childListCells = " + childListCells.Length);

            for (int i = 0; i < childListCells.Length; i++)
            {
                if (childListCells[i].GetComponent<DragAndDropCell>().isOver == true)
                {
                    overIsOverrideByItem = true;
                    break;
                }
                else
                {
                    overIsOverrideByItem = false;
                }
            }
        }
        else
        {

        }

        if (overIsOverrideByItem)
        {
            GetComponent<DragAndDropCell>().isOver = false;
            GetComponent<DragAndDropCell>().ChangeOverColorToBasicColor();
            Debug.Log("PUT GAME LOGIC OVER FALSE");
        }
        else
        {
            if (isOver)
            {
                GetComponent<DragAndDropCell>().isOver = true;
                GetComponent<DragAndDropCell>().ChangeBasicColorToOverColor();
            }

            Debug.Log("PUT GAME LOGIC OVER TRUE");
        }
    }

    public void OnDrop(PointerEventData data)
    {
        bool itemIsDrag = false;

        Debug.Log("OnDrop");
        if (isOver)
        {
            //Find if an item is drag
            for (int i = 0; i < listItems.Length; i++)
            {
                //Add item in the game logic container. Only if this item from the main list.
                if (listItems[i].GetDraggedItem() != null && listItems[i].GetDraggedItem().transform.parent.parent.parent.parent.parent.parent.name != "Swap_Experience" && listItems[i].GetDraggedItem().transform.parent.parent.parent.parent.GetComponent<GameLogicCell>() == null)
                {             
                    currentItem = listItems[i].GetDraggedItem();
                    itemIsDrag = true;
                }
                //Move item from Experence List to this Game logic container
                else if (listItems[i].GetDraggedItem() != null && listItems[i].GetDraggedItem().transform.parent.parent.parent.parent.parent.parent.name == "Swap_Experience")
                {
                    currentItem = listItems[i].GetDraggedItem();
                    currentItem.transform.parent.parent.gameObject.transform.SetParent(this.transform.Find("Item Content").transform);
                    Debug.Log("MOVE TO LOOP");
                }
            }

            //Add new cell when drop item in the viewport
            if (itemIsDrag)
            {
                Debug.Log("Add Cell in game logic 1");
                AddCellInGameLogic(experimentationCell);
            }
        }
    }


    void AddCellInGameLogic(GameObject _cell)
    {
        Debug.Log("Add Cell in game logic 2");
        //Create new cell
        GameObject cell_ = Instantiate(_cell, transform.Find("Item Content").transform);
        cell_.GetComponent<DragAndDropCell>().AddItem(currentItem, cell_.transform.Find("Header").transform);

        //Fixe cell
        cell_.GetComponent<DragAndDropCell>().isFixed = false;

        MouseOver mouseOver = GameObject.FindObjectOfType<MouseOver>();
        mouseOver.GetComponent<Image>().color = Color.white;

        //Add parameters
        int listLength = Functionnalities.Instance.GetDictionnaryFuncionnalities().Values.ElementAt(currentItem.keyFunctionnality).Length;

        if (listLength == 0)//If no parameters - Don't show the expand viewport
        {
            cell_.transform.GetChild(1).gameObject.SetActive(false);
            cell_.transform.GetChild(0).Find("Shrink").gameObject.SetActive(false);
            cell_.transform.GetChild(0).Find("Expand").gameObject.SetActive(false);
        }
        else//If parameters - Add inputFields
        {
            for (int i = 0; i < listLength; i++)
            {
                //Create a new inputField
                GameObject inputFiled = Instantiate(ManagerGenerator.Instance.inputFieldParameters, cell_.transform.GetChild(1).transform);
                inputFiled.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Functionnalities.Instance.GetDictionnaryFuncionnalities().Values.ElementAt(currentItem.keyFunctionnality)[i];


                //Find is command is a notime command
                //If true --> Fixed time at -1 and freeze inputField of the duration command
                foreach (var key in Functionnalities.Instance.GetDictionnaryFuncionnalities().Keys)
                {
                    if (key == cell_.transform.GetChild(0).Find("Text").GetComponent<TextMeshProUGUI>().text)
                    {
                        for (int j = 0; j < cell_.transform.GetChild(1).transform.childCount; j++)
                        {
                            if (cell_.transform.GetChild(1).transform.GetChild(j).Find("Name command").GetComponent<TextMeshProUGUI>().text == "Duration of the command (ms) :")
                            {
                                if (key.Contains("notime"))
                                {
                                    inputFiled.GetComponent<TMP_InputField>().placeholder.GetComponent<TextMeshProUGUI>().text = "-1";
                                    inputFiled.GetComponent<TMP_InputField>().interactable = false;
                                }
                            }
                        }
                    }
                }
            }

            cell_.transform.GetChild(1).gameObject.SetActive(false);
            cell_.transform.GetChild(0).Find("Shrink").gameObject.SetActive(false);
            cell_.transform.GetChild(0).Find("Expand").gameObject.SetActive(true);
        }
    }

}
