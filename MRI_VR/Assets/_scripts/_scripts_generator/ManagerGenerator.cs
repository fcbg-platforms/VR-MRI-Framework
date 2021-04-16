/*  
    Copyright (C) <2020>  <Valentin Bourdon>
   
    Author: Valentin Bourdon -- <vr@fcbg.ch>
    Created: 9/08/2020
   
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
using TMPro;
using System.Linq;

public class ManagerGenerator : _Singleton<ManagerGenerator>
{

    protected ManagerGenerator() { }

    public GameObject contentListFunctionnalities;
    public GameObject cell;

    public GameObject  dragItem;
    public GameObject inputFieldParameters;

    // Start is called before the first frame update
    void Awake()
    {
        // create instances of Functionnalities
        bool b_functionnalities = Functionnalities.Instance.isActiveAndEnabled;

        CreateCells();
    }

    private void Init()//Init the begin and end of the generator file. Always the same
    {

    }

    void CreateCells()
    {
        Debug.Log(Functionnalities.Instance.GetDictionnaryFuncionnalities().Count);
        for (int i = 0; i < Functionnalities.Instance.GetDictionnaryFuncionnalities().Count; i++)
        {
            AddCell(cell, Functionnalities.Instance.GetDictionnaryFuncionnalities().Keys.ElementAt(i), i);
        }
    }

    void AddCell(GameObject _cell, string _txt, int index)
    {
        GameObject cell = Instantiate(_cell, contentListFunctionnalities.transform);

        //Add txt component
        cell.transform.GetChild(0).transform.Find("Text").transform.gameObject.AddComponent<TextMeshProUGUI>();
        cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().text = _txt;
        cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().fontSize = 30;
        cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().color = Color.black;
        cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Midline;

        //Add DragAndDroItem component
        cell.transform.GetChild(0).transform.Find("Text").transform.gameObject.AddComponent<DragAndDropItem>();
        cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<DragAndDropItem>().dragItem = dragItem;
        cell.transform.GetChild(0).transform.Find("Text").transform.GetComponent<DragAndDropItem>().keyFunctionnality = index;

       //
        cell.transform.GetChild(0).transform.Find("Shrink").transform.gameObject.SetActive(false);
        cell.transform.GetChild(0).transform.Find("Expand").transform.gameObject.SetActive(false);
        cell.transform.GetChild(1).transform.gameObject.SetActive(false);
    }

   
}
