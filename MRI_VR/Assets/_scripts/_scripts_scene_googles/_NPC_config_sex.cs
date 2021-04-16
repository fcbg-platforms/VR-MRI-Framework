/*  
    Copyright (C) <2020>  <Louis Albert>
   
    Author: Louis Albert -- <vr@fcbg.ch>
   
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

#pragma warning disable 0618 //'UnityEngine.GameObject.active' is obsolete

public class _NPC_config_sex : MonoBehaviour
{
    private GameObject _Character_Child_Male;
    private GameObject _Character_Child_Female;

    // Use this for initialization
    void Start () {
        InitialiseReferences();
		SetNoSkin();
    }
	
    public void SetMaleSex()
    {
        _Character_Child_Male.SetActive(true);

        _Character_Child_Male.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Male.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Male.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Male.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Male.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Male.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;

        _Character_Child_Female.SetActive(false);
    }

    public void SetFemaleSex()
    {
        _Character_Child_Female.SetActive(true);

        _Character_Child_Female.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Female.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Female.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Female.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Female.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        _Character_Child_Female.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;

        _Character_Child_Male.SetActive(false);
    }

    void InitialiseReferences()
    {
        _Character_Child_Male = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(0).gameObject;
        _Character_Child_Female = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(1).gameObject;
    }

    public void SetNoSkin()
    {
        if (_Character_Child_Male.active && !_Character_Child_Female.active)
        {
            _Character_Child_Male.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Male.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Male.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Male.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Male.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Male.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
        else if (_Character_Child_Female.active && !_Character_Child_Male.active)
        {
            _Character_Child_Female.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Female.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Female.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Female.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Female.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
            _Character_Child_Female.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
        }
		else if (_Character_Child_Male.active && _Character_Child_Female.active)
		{
			_Character_Child_Male.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Male.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Male.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Male.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Male.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Male.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;

			_Character_Child_Female.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Female.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Female.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Female.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Female.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
			_Character_Child_Female.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = false;
		}
    }

    public void SetSkin()
    {
        if (_Character_Child_Male.active && !_Character_Child_Female.active)
        {
            _Character_Child_Male.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Male.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Male.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Male.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Male.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Male.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
        else if (_Character_Child_Female.active && !_Character_Child_Male.active)
        {
            _Character_Child_Female.transform.GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Female.transform.GetChild(2).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Female.transform.GetChild(3).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Female.transform.GetChild(4).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Female.transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
            _Character_Child_Female.transform.GetChild(6).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
        }
    }

    public void setIsBox()
    {
        SetNoSkin();
    }
}
