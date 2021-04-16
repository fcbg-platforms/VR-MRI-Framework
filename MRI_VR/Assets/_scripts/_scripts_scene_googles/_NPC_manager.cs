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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class _NPC_manager : MonoBehaviour {

    [HideInInspector]
	public bool TrialRunningEnableInput;
    private string SaveFilePath = "Results/Sequence1Results.txt";

	[Header("Light Mode")]
    [HideInInspector]
    public bool b_light_mode;
    private GameObject LightRH;
    private GameObject LightRH1;
    private GameObject LightLH;
    private GameObject LightLH1;

	[Header("Bracelet Mode")]
    [HideInInspector]
    public bool b_bracelet_mode;
    private GameObject GO_bracelet_right_hand_male;
    private GameObject GO_bracelet_left_hand_male;
    private GameObject GO_bracelet_right_hand_female;
    private GameObject GO_bracelet_left_hand_female;

	[Header("Others Auto")]
    private _main_experiment_manager _script_main_experiment_manager; // for the input feedback
    [HideInInspector]
    public int i_avatar_sex = -1; // -1 -> not initialised | 1 -> male | 2 -> female



	// Use this for initialization
	void Start () {
		LightRH = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(2).gameObject;
		LightLH = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(3).gameObject;
		LightRH1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(4).gameObject;
		LightLH1 = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(5).gameObject;

		LightRH.SetActive(false);
		LightLH.SetActive(false);
		LightRH1.SetActive(false);
		LightLH1.SetActive(false);


		GO_bracelet_right_hand_male = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
		GO_bracelet_left_hand_male = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
		GO_bracelet_right_hand_female = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;
		GO_bracelet_left_hand_female = _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.transform.GetChild(1).GetChild(0).GetChild(2).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(5).gameObject;

		GO_bracelet_right_hand_male.GetComponent<MeshRenderer>().enabled = false;
		GO_bracelet_left_hand_male.GetComponent<MeshRenderer>().enabled = false;
		GO_bracelet_right_hand_female.GetComponent<MeshRenderer>().enabled = false;
		GO_bracelet_left_hand_female.GetComponent<MeshRenderer>().enabled = false;

        _script_main_experiment_manager = _class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager;
	}
	
	// Update is called once per frame
	void Update () {


		// if input allowed (that means between the start of a trial and the end of a trial/first input of trial)
		if (TrialRunningEnableInput)
		{
			CheckAnswerInputKeyboard();
		}
	}

	public void CheckAnswerInputKeyboard()
	{
		if (Input.GetButtonDown("Button1"))
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			writeInFilePSpace(timestamp + " - [NPC_OBT] [1] - input :" + " 1");

			TrialRunningEnableInput = false;

			_script_main_experiment_manager.s_last_input_answer = "answer_1";
			_script_main_experiment_manager.s_last_input_command = "NPC_OBT";
			Debug.Log("answer 1");
		}
		else if (Input.GetButtonDown("Button2"))
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			writeInFilePSpace(timestamp + " - [NPC_OBT] [1] - input :" + " 2");

			TrialRunningEnableInput = false;

			_script_main_experiment_manager.s_last_input_answer = "answer_2";
			_script_main_experiment_manager.s_last_input_command = "NPC_OBT";
		}
		else if (Input.GetButtonDown("Button3"))
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			writeInFilePSpace(timestamp + " - [NPC_OBT] [1] - input :" + " 3");

			TrialRunningEnableInput = false;

			_script_main_experiment_manager.s_last_input_answer = "answer_3";
			_script_main_experiment_manager.s_last_input_command = "NPC_OBT";
		}
		else if (Input.GetButtonDown("Button6"))
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			writeInFilePSpace(timestamp + " - [NPC_OBT] [1] - input :" + " 4");

			TrialRunningEnableInput = false;

			_script_main_experiment_manager.s_last_input_answer = "answer_4";
			_script_main_experiment_manager.s_last_input_command = "NPC_OBT";
		}
		else if (Input.GetButtonDown("Button7"))
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			writeInFilePSpace(timestamp + " - [NPC_OBT] [1] - input :" + " 5");

			TrialRunningEnableInput = false;

			_script_main_experiment_manager.s_last_input_answer = "answer_5";
			_script_main_experiment_manager.s_last_input_command = "NPC_OBT";
		}
		else if (Input.GetButtonDown("Button8"))
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			writeInFilePSpace(timestamp + " - [NPC_OBT] [1] - input :" + " 6");

			TrialRunningEnableInput = false;

			_script_main_experiment_manager.s_last_input_answer = "answer_6";
			_script_main_experiment_manager.s_last_input_command = "NPC_OBT";
		}

	}

	public void writeInFile(string s_toWrite)
	{
		// Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(SaveFilePath, true);
		writer.WriteLine(s_toWrite);

		writer.Close();
	}
	public void writeInFilePSpace(string s_toWrite)
	{
		// Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(SaveFilePath, true);
		writer.WriteLine(s_toWrite);
		writer.WriteLine("");

		writer.Close();
	}

	public void setAnswerFilePath(string FilePath)
	{
		SaveFilePath = FilePath;
	}










	/// <summary>
	/// 
	/// </summary>
	/// <param name="part_name"></param>
	public void NPC_highligh_part(string part_name, string hex_light_color, float intensity)
	{
		Color lights_color = GetColorFromHex(hex_light_color);

		if (part_name == "none")
		{
			if (b_light_mode)
			{
				LightRH.SetActive(false);
				LightLH.SetActive(false);
				LightRH1.SetActive(false);
				LightLH1.SetActive(false);
			}

			if (b_bracelet_mode)
			{
				GO_bracelet_right_hand_male.GetComponent<MeshRenderer>().enabled = false;
				GO_bracelet_left_hand_male.GetComponent<MeshRenderer>().enabled = false;
				GO_bracelet_right_hand_female.GetComponent<MeshRenderer>().enabled = false;
				GO_bracelet_left_hand_female.GetComponent<MeshRenderer>().enabled = false;
			}
		}
		else if (part_name == "right_hand")
		{
			if (b_light_mode)
			{
				LightRH.SetActive(true);
				LightRH1.SetActive(true);

				LightRH.GetComponent<Light>().color = lights_color;
				LightRH1.GetComponent<Light>().color = lights_color;
				LightRH.GetComponent<Light>().intensity = intensity;
				LightRH1.GetComponent<Light>().intensity = intensity;
			}
			if (b_bracelet_mode)
			{
				if (i_avatar_sex == 1)
				{
					GO_bracelet_right_hand_male.GetComponent<MeshRenderer>().enabled = true;
					Color lights_color_2 = new Color(lights_color.r, lights_color.g, lights_color.b, Mathf.Clamp(intensity,0,1));
					GO_bracelet_right_hand_male.GetComponent<Renderer>().material.color = lights_color_2;
				}
				else if (i_avatar_sex == 2)
				{
					GO_bracelet_right_hand_female.GetComponent<MeshRenderer>().enabled = true;
					Color lights_color_2 = new Color(lights_color.r, lights_color.g, lights_color.b, Mathf.Clamp(intensity, 0, 1));
					GO_bracelet_right_hand_female.GetComponent<Renderer>().material.color = lights_color_2;
				}
			}

		}
		else if (part_name == "left_hand")
		{
			if (b_light_mode)
			{
				LightLH.SetActive(true);
				LightLH1.SetActive(true);

				LightLH.GetComponent<Light>().color = lights_color;
				LightLH1.GetComponent<Light>().color = lights_color;
				LightLH.GetComponent<Light>().intensity = intensity;
				LightLH1.GetComponent<Light>().intensity = intensity;
			}
			if (b_bracelet_mode)
			{
				if (i_avatar_sex == 1)
				{
					GO_bracelet_left_hand_male.GetComponent<MeshRenderer>().enabled = true;
					Color lights_color_2 = new Color(lights_color.r, lights_color.g, lights_color.b, Mathf.Clamp(intensity, 0, 1));
					GO_bracelet_left_hand_male.GetComponent<Renderer>().material.color = lights_color_2;
				}
				else if (i_avatar_sex == 2)
				{
					GO_bracelet_left_hand_female.GetComponent<MeshRenderer>().enabled = true;
					Color lights_color_2 = new Color(lights_color.r, lights_color.g, lights_color.b, Mathf.Clamp(intensity, 0, 1));
					GO_bracelet_left_hand_female.GetComponent<Renderer>().material.color = lights_color_2;
				}
			}

		}
		else
		{
			Debug.Log("[NPC_OBT] - error highlight part argument : " + part_name + " not recognized");
		}
	}

	public Color GetColorFromHex(string Hex)
	{

		Color Body_Color_Selected = new Color();

		if (Hex[0] == '#')
		{
			if (Hex.Length == 7)
				ColorUtility.TryParseHtmlString(Hex, out Body_Color_Selected);
		}
		else
		{
			if (Hex.Length == 6)
				ColorUtility.TryParseHtmlString("#" + Hex, out Body_Color_Selected);
		}

		return Body_Color_Selected;
	}


	public void ExpyVRStopDisplayTrial()
	{
		if (TrialRunningEnableInput)
		{
			string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			writeInFilePSpace(timestamp + " - [NPC_OBT] [1] - input :" + " -1");

			_script_main_experiment_manager.s_last_input_answer = "answer_none";
			_script_main_experiment_manager.s_last_input_command = "NPC_OBT";
		}
        _class_all_references_scene_mri_compatible_googles.Instance.GO_NPC_Character.GetComponent<_NPC_config_sex>().SetNoSkin();

		LightRH.SetActive(false);
		LightLH.SetActive(false);
		LightRH1.SetActive(false);
		LightLH1.SetActive(false);

		GO_bracelet_right_hand_male.GetComponent<MeshRenderer>().enabled = false;
		GO_bracelet_left_hand_male.GetComponent<MeshRenderer>().enabled = false;
		GO_bracelet_right_hand_female.GetComponent<MeshRenderer>().enabled = false;
		GO_bracelet_left_hand_female.GetComponent<MeshRenderer>().enabled = false;

		TrialRunningEnableInput = false;
	}
}
