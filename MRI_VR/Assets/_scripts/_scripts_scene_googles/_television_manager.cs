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
using UnityEngine.Video;

public class _television_manager : MonoBehaviour {

    public string path;
    public bool setTVPicture; // input required
	public bool setTVPictureWithQuestion;
	public bool setTVPictureAfterQuestion;
	public bool setTVPictureOnly; // no input
	public bool setTVPictureFeedbackLastCommand; // for feedback picture
	public Material TVScreenMaterial;
    public Texture TVOff;
    public bool TrialRunningEnableInput;
    public string SaveFilePath = "Results/xxxResults.txt";
    public bool activateTV;
    public bool desactivateTV;
    public bool b_video_ended;

	// Use this for initialization
	/*void Start () {
        InitializeGameObjects();
	}

    void InitializeGameObjects()
    {
        _class_all_references_scene_mri_compatible_googles.Instance.GO_television_TVScreen.SetActive(false);
    }*/
	
	// Update is called once per frame
	void Update () {
		if (setTVPicture)
        {
            //string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            //writeInFile(timestamp + " - [displayTV] [0] - " + path);
            StartCoroutine(DisplayPicture());
            setTVPicture = false;
        }

		if (setTVPictureWithQuestion)
		{
			//string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			//writeInFile(timestamp + " [question_with_TV] [-1] - " + path);
			StartCoroutine(DisplayPictureOnly());
			setTVPictureWithQuestion = false;
		}

		if (setTVPictureAfterQuestion)
		{
			//string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
			//writeInFile(timestamp + " [question_after_TV] [-1] - " + path);
			StartCoroutine(DisplayPictureOnly());
			setTVPictureAfterQuestion = false;
		}

		if (setTVPictureOnly)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [displayTV] [0] - " + path);
            StartCoroutine(DisplayPictureOnly());
            setTVPictureOnly = false;
        }

		if (setTVPictureFeedbackLastCommand)
		{
			StartCoroutine(DisplayPictureOnly());
			setTVPictureFeedbackLastCommand = false;
		}

        if (activateTV)
        {
            _class_all_references_scene_mri_compatible_googles.Instance.GO_television_TVScreen.SetActive(true);
            activateTV = false;
        }
        if (desactivateTV)
        {
            _class_all_references_scene_mri_compatible_googles.Instance.GO_television_TVScreen.SetActive(false);
            desactivateTV = false;
        }

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
            writeInFilePSpace(timestamp + " - [displayTV] [1] - input :" + " 1");

            TrialRunningEnableInput = false;

			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_answer = "answer_1";
			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_command = "displayTV";
		}
        else if (Input.GetButtonDown("Button2"))
		{
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [displayTV] [1] - input :" + " 2");

            TrialRunningEnableInput = false;

			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_answer = "answer_2";
			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_command = "displayTV";
		}
        else if (Input.GetButtonDown("Button3"))
		{
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [displayTV] [1] - input :" + " 3");

            TrialRunningEnableInput = false;

			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_answer = "answer_3";
			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_command = "displayTV";
		}
        else if (Input.GetButtonDown("Button6"))
		{
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [displayTV] [1] - input :" + " 4");

            TrialRunningEnableInput = false;

			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_answer = "answer_4";
			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_command = "displayTV";
		}
        else if (Input.GetButtonDown("Button7"))
		{
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [displayTV] [1] - input :" + " 5");

            TrialRunningEnableInput = false;

			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_answer = "answer_5";
			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_command = "displayTV";
		}
        else if (Input.GetButtonDown("Button8"))
		{
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [displayTV] [1] - input :" + " 6");

            TrialRunningEnableInput = false;

			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_answer = "answer_6";
			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_command = "displayTV";
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

    // Use this for initialization
    IEnumerator DisplayPicture()
    {
        TrialRunningEnableInput = true;

        if (path == "")
        {
            stopDisplayOnTVScreen();
        }
        else
        {
            WWW www = new WWW("file:///" + path);
            while (!www.isDone)
                yield return null;
            TVScreenMaterial.mainTexture = www.texture;
            path = "";
        }
    }

    // Use this for initialization
    IEnumerator DisplayPictureOnly()
    {
        if (path == "")
        {
            stopDisplayOnTVScreen();
        }
        else
        {
            WWW www = new WWW("file:///" + path);
            while (!www.isDone)
                yield return null;
            TVScreenMaterial.mainTexture = www.texture;
            path = "";
        }
    }

    public void stopDisplayOnTVScreen()
    {
        if (TrialRunningEnableInput)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
            writeInFilePSpace(timestamp + " - [displayTV] [1] - input :" + " -1");

			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_answer = "answer_none";
			_class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager.s_last_input_command = "displayTV";
		}
        TVScreenMaterial.mainTexture = TVOff;
        TrialRunningEnableInput = false;
	}

	public void stopDisplayOnTVScreenWithQuestions()
	{
		TVScreenMaterial.mainTexture = TVOff;
	}

	public void setAnswerFilePath(string FilePath)
    {
        SaveFilePath = FilePath;
    }

    public void play_video(string s_path)
    {
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        writeInFilePSpace(timestamp + " - [set_display_TV_video] - [START] - " + s_path);

        var video_player = _class_all_references_scene_mri_compatible_googles.Instance.GO_television_TVScreen.GetComponent<VideoPlayer>();

        video_player.url = Application.dataPath + "/../" + "/" + s_path;
        video_player.loopPointReached += EndReached;
        video_player.Play();
    }

    void EndReached(VideoPlayer vp)
    {
        vp.Stop();
        b_video_ended = true;
        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        writeInFilePSpace(timestamp + " - [set_display_TV_video] - [END]");
    }
}