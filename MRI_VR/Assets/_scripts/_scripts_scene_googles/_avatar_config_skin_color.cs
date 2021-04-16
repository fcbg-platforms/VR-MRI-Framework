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
using UnityEngine.UI;

public class _avatar_config_skin_color : MonoBehaviour
{
    private GameObject _Character_Child_Male_Child_Body;
    private GameObject _Character_Child_Female_Child_Body;

    private Color color_body_color_selected;

    // Use this for initialization
    void Start () {
        InitialiseReferences();
        ButtonGenericColorHex("#1F5822");
    }

    void InitialiseReferences()
    {
        _Character_Child_Male_Child_Body = _class_all_references_scene_mri_compatible_googles.Instance.GO_Character.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject;
        _Character_Child_Female_Child_Body = _class_all_references_scene_mri_compatible_googles.Instance.GO_Character.transform.GetChild(1).gameObject.transform.GetChild(1).gameObject;
    }

	public void ButtonGenericColorHex(string Hex)
	{
		if (Hex[0] == '#')
		{
			if (Hex.Length == 7)
				ColorUtility.TryParseHtmlString(Hex, out color_body_color_selected);
		}
		else
		{
			if (Hex.Length == 6)
				ColorUtility.TryParseHtmlString("#" + Hex, out color_body_color_selected);
		}
		_Character_Child_Male_Child_Body.GetComponent<SkinnedMeshRenderer>().materials[0].color = color_body_color_selected;
		_Character_Child_Female_Child_Body.GetComponent<SkinnedMeshRenderer>().materials[0].color = color_body_color_selected;
	}
}
