using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;


public class MouseOver : MonoBehaviour
{
    public bool viewportIsOver = false;

    public DummyControlUnit dummyControlExperience;
    public GameObject cell;
    public GameObject item;
    public Color overColor;
    DragAndDropItem currentItem;

    public GameObject listConent;
    public DragAndDropItem[] listItems;

    private void Start()
    {
        listItems = listConent.transform.GetComponentsInChildren<DragAndDropItem>();
    }
    public void OverEnter()
    {
        viewportIsOver = true;

        //Change background color when overlay with item
        for (int i = 0; i < listItems.Length; i++)
        {
            if (listItems[i].GetDraggedItem() != null)
            {
                GetComponent<Image>().color = overColor;            
            }
        }
    }

    public void OverExit()
    {
        viewportIsOver = false;

        //Reset background color when exit
        for (int i = 0; i < listItems.Length; i++)
        {
            if (listItems[i].GetDraggedItem() != null)
            {
                GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void OverDrop()
    {
        bool itemIsDrag = false;
        bool itemIsGameLogic = false;
        GameObject gameLogicPrefab = null;

        if (viewportIsOver)
        {
            //Find if an item is drag
            for (int i = 0; i < listItems.Length; i++)
            {
                if (listItems[i].GetDraggedItem() != null && listItems[i].GetDraggedItem().transform.parent.parent.parent.parent.parent.parent.name != "Swap_Experience" && listItems[i].GetDraggedItem().transform.parent.parent.parent.parent.GetComponent<GameLogicCell>() == null)
                {
                    if (listItems[i].GetDraggedItem().transform.parent.parent.GetComponent<GameLogicContainer>() != null)
                    {
                        itemIsGameLogic = true;

                        switch (listItems[i].GetDraggedItem().transform.parent.parent.GetComponent<GameLogicContainer>().myGameLogic)
                        {
                            case GameLogicContainer.GameLogic.For:
                                gameLogicPrefab = listItems[i].GetDraggedItem().transform.parent.parent.GetComponent<GameLogicContainer>().for_cell;
                                break;
                            case GameLogicContainer.GameLogic.While:
                                gameLogicPrefab = listItems[i].GetDraggedItem().transform.parent.parent.GetComponent<GameLogicContainer>().while_cell;
                                break;
                            case GameLogicContainer.GameLogic.Switch:
                                gameLogicPrefab = listItems[i].GetDraggedItem().transform.parent.parent.GetComponent<GameLogicContainer>().switch_cell;
                                break;
                            default:
                                break;
                        }

                        Debug.Log("GAME LOGIC");
                    }
                    
                    currentItem = listItems[i].GetDraggedItem();
                    itemIsDrag = true;
                }     
            }

            //Add new cell when drop item in the viewport
            if (itemIsDrag && !itemIsGameLogic)
            {
                AddCell(cell);
            }
            else if (itemIsDrag && itemIsGameLogic)//Add new cell, type of game logic in the viewport
            {
                if(gameLogicPrefab != null)
                    AddGameLogic(gameLogicPrefab);
            }
        }
    }

    //Create a new cell 
    void AddCell(GameObject _cell)
    {
        //Create new cell
        GameObject cell_ = Instantiate(_cell, transform.Find("Content").transform);
        cell_.GetComponent<DragAndDropCell>().AddItem(currentItem, cell_.transform.Find("Header").transform);
 
        //Fixe cell
        cell_.GetComponent<DragAndDropCell>().isFixed = false;

        GetComponent<Image>().color = Color.white;

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
                                    inputFiled.GetComponent<TMP_InputField>().text = "-1";
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

        currentItem = null;
    }

    void AddGameLogic(GameObject _cell)
    {
        //Create new cell
        GameObject cell = Instantiate(_cell, transform.Find("Content").transform);
        cell.GetComponent<DragAndDropCell>().AddItem(currentItem, cell.transform.Find("Header").transform);

        //Fixe cell
        cell.GetComponent<DragAndDropCell>().isFixed = false;

        GetComponent<Image>().color = Color.white;
    }

}
