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
using System.IO;
using UnityEngine.UI;
using TMPro;
using Crosstales.FB;
using System.Linq;
using System;

public class LoadExperimentationFile : MonoBehaviour
{

    public GameObject experimentationContent;
    public DragAndDropCell[] childs;

    private StreamReader reader;
    public void OpenFile()
    {
        string extensions = "txt";

        string path = FileBrowser.OpenSingleFile("Open File", "", extensions);

        reader = new StreamReader(path);

        RemoveAllExperimentationList();
        Readfile();
        // rebuildList(path);
    }

    private void Start()
    {
       

        // RemoveAllExperimentationList();

        //GetComponent<CreateExperimentationFile>().InitBeginExperimentationFile();
    }

    void RemoveAllExperimentationList()
    {
        childs = experimentationContent.GetComponentsInChildren<DragAndDropCell>();

        foreach (var item in childs)
        {
            Destroy(item.gameObject);
        }
    }

    void Readfile()
    {
        string value;
        int index = 0;

        bool readFile = true;

        while (readFile)
        {
            if (!reader.EndOfStream)
            {
                value = reader.ReadLine();

                Debug.Log(value);

                char[] separators = new char[] { ' ' };
                string[] result = value.Split(separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var key in Functionnalities.Instance.GetDictionnaryFuncionnalities().Keys)
                {
                    if (result.Length != 0 && result[0] == key)
                    {
                        Debug.Log("CREATE CELL - OPEN FILE");
                        //Instantiate new cell
                        GameObject currentCell = Instantiate(GetComponent<CreateExperimentationFile>().cell, experimentationContent.transform);

                        //Init / Create Cell
                        GetComponent<CreateExperimentationFile>().CreateCell(result[0], currentCell, key, index, false);

                        index++;
                    }

                }
            }
            else
            {
                readFile = false;
            }
        }
       
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
