/*  
    Copyright (C) <2020>  <Valentin Bourdon>
   
    Author: Valentin Bourdon -- <vr@fcbg.ch>
    Created: 9/12/2020
   
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
using System.IO;
using UnityEngine.UI;
using TMPro;
using Crosstales.FB;
using System.Linq;

public class CreateExperimentationFile : _Singleton<CreateExperimentationFile>
{
    protected CreateExperimentationFile() { }

    public GameObject contentExperience;
    public GameObject cell;
    public GameObject dragItem;

    GameObject currentCell;

    private void Awake()
    {
        InitBeginExperimentationFile();
    }

    /// <summary>
    /// Initialisation of the essential begin part of the experimentation file.
    /// All this commands are necessary and not changed between experimentation.
    /// </summary>
    public void InitBeginExperimentationFile()
    {
        AddBeginCells(cell);
    }

    /// <summary>
    /// Initialisation of the essential end part of the experimentation file.
    /// All this commands are necessary and not changed between experimentation.
    /// </summary>
    void InitEndExperimentationFile()
    {
        AddEndCells(cell);
    }


    /// <summary>
    /// Instantiate all begin cells
    /// </summary>
    /// <param name="_cell"></param>
    void AddBeginCells(GameObject _cell)
    {
        foreach (var key in Functionnalities.Instance.GetDictionnaryFuncionnalities().Keys)
        {
            switch (key)
            {
                case "comment":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("comment", currentCell, key,0, true);
                    break;

                case "load_scene_additive":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("load_scene_additive", currentCell, key,1, true);
                    break;
                case "get_mri_references":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("get_mri_references", currentCell, key,2, true);
                    break;
                case "init":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("init", currentCell, key,3, true);
                    break;
                case "set_MRI_length_rb":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("set_MRI_length_rb", currentCell, key,4, true);
                    break;
                case "pause":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);
                    //Init / Create Cell
                    CreateCell("pause", currentCell, key, 5, true);

                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);
                    //Init / Create Cell
                    CreateCell("pause", currentCell, key, 7, true);
                    break;
                case "move_inside":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("move_inside", currentCell, key, 6, true);
                    break;

                default:
                    break;
            }
            
        }

        SetSiblingIndex();
    }

    /// <summary>
    /// Instantiate all begin cells
    /// </summary>
    /// <param name="_cell"></param>
    void AddEndCells(GameObject _cell)
    {
        foreach (var key in Functionnalities.Instance.GetDictionnaryFuncionnalities().Keys)
        {
            switch (key)
            {
                
                case "load_scene_single":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("load_scene_single", currentCell, key, 1, true);
                    break;
                case "get_mri_references":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("get_mri_references", currentCell, key, 2, true);
                    break;
                case "avatar_inside_mri":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("avatar_inside_mri", currentCell, key, 3, true);
                    break;
                case "move_outside":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("move_outside", currentCell, key, 4, true);
                    break;
                case "pause":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("pause", currentCell, key, 5, true);
                    break;
                case "EOF":
                    //Instantiate new cell
                    currentCell = Instantiate(_cell, contentExperience.transform);

                    //Init / Create Cell
                    CreateCell("EOF", currentCell, key, 5,true);
                    break;

                default:
                    break;
            }

        }
    }

    /// <summary>
    /// Create all component of the cell : Title, InputFields, DragAndDropItem
    /// </summary>
    /// <param name="_title"></param>
    /// <param name="_cell"></param>
    /// <param name="_key"></param>
    public void CreateCell(string _title, GameObject _cell, string _key, int _indexHierarchy, bool _fixed)
    {
        //Fixe cell
        _cell.GetComponent<DragAndDropCell>().isFixed = _fixed;

        //Change color Header
        if (_fixed)
            _cell.transform.GetChild(0).GetComponent<Image>().color = Color.black;
        else
            _cell.transform.GetChild(0).GetComponent<Image>().color = _cell.GetComponent<DragAndDropCell>().full;

        //Add txt component
        _cell.transform.GetChild(0).transform.Find("Text").transform.gameObject.AddComponent<TextMeshProUGUI>();
        _cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().text = _title;
        _cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().fontSize = 30;

        if (_fixed)
            _cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().color = Color.white;
        else
            _cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().color = Color.black;

        _cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        _cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;

        //Add DragAndDroItem component
        _cell.transform.GetChild(0).transform.Find("Text").transform.gameObject.AddComponent<DragAndDropItem>();
        _cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<DragAndDropItem>().dragItem = dragItem;


        
        //Add InputFields - Depend of number of parameters the command needed
        int listLength = Functionnalities.Instance.GetDictionnaryFuncionnalities()[_key].Length;

        if (listLength == 0)
        {
            _cell.transform.GetChild(1).gameObject.SetActive(false);
            _cell.transform.GetChild(0).Find("Shrink").gameObject.SetActive(false);
            _cell.transform.GetChild(0).Find("Expand").gameObject.SetActive(false);
        }
        else
        {
           
            for (int i = 0; i < listLength; i++)
            {
                GameObject inputFiled = Instantiate(ManagerGenerator.Instance.inputFieldParameters, _cell.transform.GetChild(1).transform);
                inputFiled.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = Functionnalities.Instance.GetDictionnaryFuncionnalities()[_key][i];
            }

            _cell.transform.GetChild(1).gameObject.SetActive(false);
            _cell.transform.GetChild(0).Find("Shrink").gameObject.SetActive(false);
            _cell.transform.GetChild(0).Find("Expand").gameObject.SetActive(true);
        }

        _cell.GetComponent<DragAndDropCell>().siblingIndex = _indexHierarchy;


    }

    void SetSiblingIndex()
    {
        DragAndDropCell[] listCells = contentExperience.transform.GetComponentsInChildren<DragAndDropCell>();

        for (int i = 0; i < listCells.Length; i++)
        {
            listCells[i].transform.SetSiblingIndex(listCells[i].GetComponent<DragAndDropCell>().siblingIndex);
        }
    }


    /// <summary>
    /// Generate an experimentation file (txt format) with all commands included in the experimentation viewport
    /// </summary>
    /// <param name="path"></param>
    public void GenerateFile(string path)
    {
        //Take all cells
        DragAndDropCell[] obj = contentExperience.GetComponentsInChildren<DragAndDropCell>();
        List<DragAndDropCell> cells = new List<DragAndDropCell>();
        for (int i = 0; i < obj.Length; i++)
        {
            cells.Add(obj[i]);
        }

        StreamWriter SW_writer = new StreamWriter(path, false);

        for (int i = 0; i < cells.Count; i++)
        {
            //Try to find is a logic game exist
            if (cells[i].gameObject.GetComponent<GameLogicCell>())
            {
                //Take all cells
                DragAndDropCell[] gameLogicCells = cells[i].gameObject.transform.Find("Item Content").GetComponentsInChildren<DragAndDropCell>();

                //Find the type of game logic
                switch (cells[i].gameObject.GetComponent<GameLogicCell>().typeOfLogic)
                {
                    case GameLogicCell.TypeOfLogic.Loop://For

                        //Find size loop
                        int nbLoop = cells[i].transform.GetChild(1).GetChild(0).GetComponent<SetLengthLoop>().loopSize;

                        //Write content
                        for (int j = 0; j < nbLoop; j++)
                        {
                            for (int k = 0; k < gameLogicCells.Length; k++)
                            {
                                WriteContentCell(SW_writer, gameLogicCells[k]);
                            }
                        }

                        break;
                    case GameLogicCell.TypeOfLogic.Switch:
                        break;
                    case GameLogicCell.TypeOfLogic.While:
                        break;
                    default:
                        break;
                }

                //Remove cells used in the game logic to the main list of cells
                cells.RemoveRange(i, gameLogicCells.Length);
            }
            else
            {
                WriteContentCell(SW_writer, cells[i]);               
            }           
        }

        SW_writer.Close();
    }

    void WriteContentCell(StreamWriter sw, DragAndDropCell cell)
    {
        sw.WriteLine(cell.transform.GetChild(0).Find("Text").GetComponent<TextMeshProUGUI>().text);

        int lengthParameters = cell.transform.GetChild(1).GetComponentsInChildren<TMP_InputField>().Length;

        string parameters = "";
        for (int j = 0; j < lengthParameters - 1; j++)//Write all parameters / not the duration of the command
        {
            parameters += cell.transform.GetChild(1).transform.GetChild(j).GetComponent<TMP_InputField>().text + " ";

        }

        //Write parameters
        sw.WriteLine(parameters);

        //Find if duration command exist
        int childCount = cell.transform.GetChild(1).transform.childCount;
        bool durationCommandExist = false;
        foreach (var key in Functionnalities.Instance.GetDictionnaryFuncionnalities().Keys)
        {
            if (key == cell.transform.GetChild(0).Find("Text").GetComponent<TextMeshProUGUI>().text)
            {
                for (int j = 0; j < childCount; j++)
                {
                    if (cell.transform.GetChild(1).transform.GetChild(j).Find("Name command").GetComponent<TextMeshProUGUI>().text == "Duration of the command (ms) :")
                    {
                        durationCommandExist = true;
                    }
                }
            }
        }

        //Write duration of the command
        if (durationCommandExist)
        {
            sw.WriteLine(cell.transform.GetChild(1).transform.GetChild(childCount - 1).GetComponent<TMP_InputField>().text + " ");

            //Write a separation between commands
            sw.WriteLine("");
        }
    }


    public void SaveFile()
    {
        string extensions = "txt";

        string path = FileBrowser.SaveFile("Save Experimentation File", "", "MySaveFile", extensions);

        GenerateFile(path);
    }
}
