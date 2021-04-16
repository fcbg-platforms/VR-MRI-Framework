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
using UnityEngine.UI;

public class _3d_questions_after_TV : MonoBehaviour {

    private GameObject QuestionSceneText;

    [Header("Save / Load Avatar Configuration")]
    private string SaveFilePath = "Results/AnswersAfterTV.txt";

    [Header("Question Time Parameters (ms)")]
    [Tooltip("Time No Text Before Displaying Question")]
    private float TimeNoTextBeforeDisplayQuestion = 1000;
    [Tooltip("Time During Which the selected symbol stays visible")]
    private float TimeSelectedSymbolDisplay = 500;

    private float eventTimeNoTextBeforeDisplayQuestion;
    private float eventTimeSelectedSymbolDisplay;
    [HideInInspector]
    public bool IsAnswerTrue; // variable to answer Positivly to a question
    [HideInInspector]
    public bool IsAnswerFalse; // variable to answer Negativly to a question

    private bool started; // variable to know if the question sequence is performed
    private bool expyVRstarted; // variable to know if the expyVR question is performed
    private string expyVRquestion; // the current expyVR question
    [HideInInspector]
    public bool expyVRcurrentQuestionAnswered; // has the current expyVR question been answered
    [HideInInspector]
    public bool expyVRcurrentQuestionAnsweredcanGoNext;
    private int expyVRintAnswer = -1; // Answer given by the select mecanism

    private GameObject Questions_SelectorMecanism_AnswerTrue;
    private GameObject Questions_SelectorMecanism_AnswerFalse;
	
	// save these infos for logs
	//string saved_question;
	string saved_left_text;
	string saved_right_text;
	int saved_i_TimeNoTextBeforeDisplayQuestion;
	int saved_i_TimeSelectedSymbolDisplay;
	string saved_path_of_the_picture;

    private _main_experiment_manager _script_main_experiment_manager;



    private void Awake()
    {
        eventTimeNoTextBeforeDisplayQuestion = float.PositiveInfinity;
        eventTimeSelectedSymbolDisplay = float.PositiveInfinity;
    }

    // Use this for initialization
    void Start () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        if (expyVRstarted)
        {
            if (Time.time * 1000 >= eventTimeNoTextBeforeDisplayQuestion)
            {
                QuestionSceneText.GetComponent<TextMesh>().text = "";

                Char delimiter = '@';
                String[] substrings = expyVRquestion.Split(delimiter);
                foreach (var substring in substrings)
                {
                    if (QuestionSceneText.GetComponent<TextMesh>().text != "")
                    {
                        QuestionSceneText.GetComponent<TextMesh>().text += Environment.NewLine;
                    }
                    QuestionSceneText.GetComponent<TextMesh>().text += substring;

                }

                expyVRwriteQuestion();
                DisplayButtons();
                				
				// desactivate the TV
                _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>().stopDisplayOnTVScreenWithQuestions();
                
				eventTimeNoTextBeforeDisplayQuestion = float.PositiveInfinity;
            }

            if (Time.time * 1000 >= eventTimeSelectedSymbolDisplay)
            {
				expyVRcurrentQuestionAnsweredcanGoNext = true;

				eventTimeNoTextBeforeDisplayQuestion = float.PositiveInfinity;
				eventTimeSelectedSymbolDisplay = float.PositiveInfinity;

				expyVRstarted = false;

				expyVRquestion = "";
				expyVRintAnswer = -1;

                _class_all_references_scene_mri_compatible_googles.Instance.GO_television.GetComponent<_television_manager>().stopDisplayOnTVScreenWithQuestions();

				RemoveButtons();
			}

            if (IsAnswerTrue)
            {
                expyVRintAnswer = 1;
                eventTimeSelectedSymbolDisplay = Time.time * 1000 + TimeSelectedSymbolDisplay;

                IsAnswerTrue = false;
                IsAnswerFalse = false;

                expyVRcurrentQuestionAnswered = true;

				_script_main_experiment_manager.s_last_input_answer = "answer_left";
				_script_main_experiment_manager.s_last_input_command = "question_after_TV";
			}
            if (IsAnswerFalse)
            {
                expyVRintAnswer = 0;
                eventTimeSelectedSymbolDisplay = Time.time * 1000 + TimeSelectedSymbolDisplay;

                IsAnswerTrue = false;
                IsAnswerFalse = false;

                expyVRcurrentQuestionAnswered = true;

				_script_main_experiment_manager.s_last_input_answer = "answer_right";
				_script_main_experiment_manager.s_last_input_command = "question_after_TV";
			}

            if (expyVRcurrentQuestionAnswered == true)
            {
				expyVRcurrentQuestionAnswered = false;

				expyVRwriteAnswers(); ///////

				QuestionSceneText.GetComponent<TextMesh>().text = "";

				if (expyVRintAnswer == -1)
				{
					_script_main_experiment_manager.s_last_input_answer = "answer_none";
					_script_main_experiment_manager.s_last_input_command = "question_after_TV";
				}
			}
        }
    }



    public void ExpyVRsetanswerNone()
    {
        if(!IsAnswerTrue && !IsAnswerFalse)
        {
            expyVRintAnswer = -1;
            eventTimeSelectedSymbolDisplay = Time.time * 1000 + TimeSelectedSymbolDisplay;

            IsAnswerTrue = false;
            IsAnswerFalse = false;

            expyVRcurrentQuestionAnswered = true;
        }
    }




    public bool ExpyVRCheckAnswerFilesExistanceAndSetItAsCurrentAnswerFile(string FilePath)
    {
        SaveFilePath = FilePath;
        if (File.Exists(SaveFilePath))
        {
            Debug.Log("Answers File already exists");
            return false;
        }
        else
        {
            return true;
        }
    }


    public void setAnswerFilePath(string FilePath)
    {
        SaveFilePath = FilePath;
    }



    public void ExpyVRStartQuestion(string question, string left_text, string right_text, int i_TimeNoTextBeforeDisplayQuestion, int i_TimeSelectedSymbolDisplay, string path_of_the_picture)
    {
		TimeNoTextBeforeDisplayQuestion = i_TimeNoTextBeforeDisplayQuestion;
		TimeSelectedSymbolDisplay = i_TimeSelectedSymbolDisplay;

		string s_question_1 = question.Replace('_', ' ');
        QuestionSceneText.GetComponent<TextMesh>().text = "";
        eventTimeNoTextBeforeDisplayQuestion = Time.time * 1000 + TimeNoTextBeforeDisplayQuestion;
        expyVRquestion = s_question_1;

		string s_left_text = left_text.Replace('_', ' ');
		string s_right_text = right_text.Replace('_', ' ');
		Questions_SelectorMecanism_AnswerTrue.GetComponent<Hover.Core.Items.Types.HoverItemDataCheckbox>().Label = s_left_text;
		Questions_SelectorMecanism_AnswerFalse.GetComponent<Hover.Core.Items.Types.HoverItemDataCheckbox>().Label = s_right_text;


		// save these infos for logs
		//saved_question = s_question_1;
		saved_left_text = s_left_text;
		saved_right_text = s_right_text;
		saved_i_TimeNoTextBeforeDisplayQuestion = i_TimeNoTextBeforeDisplayQuestion;
		saved_i_TimeSelectedSymbolDisplay = i_TimeSelectedSymbolDisplay;
		saved_path_of_the_picture = path_of_the_picture;

		expyVRstarted = true;
    }

    public void expyVRwriteQuestion()
    {
        StreamWriter writer = new StreamWriter(SaveFilePath, true);

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
        writer.WriteLine(timestamp + " [question_after_TV] [0] - " + expyVRquestion + " - " + saved_left_text + " - " + saved_right_text + " - " + saved_i_TimeNoTextBeforeDisplayQuestion + " - " + saved_i_TimeSelectedSymbolDisplay + " - " + saved_path_of_the_picture);

		writer.Close();
    }

    public void expyVRwriteAnswers()
    {
        StreamWriter writer = new StreamWriter(SaveFilePath, true);

        string timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);
		writer.WriteLine(timestamp + " [question_after_TV] [1] - " + ((expyVRintAnswer == 0) ? "[left] " + saved_right_text : ((expyVRintAnswer == 1) ? "[right] " + saved_left_text : "[none] " + "No Answer")));
		writer.WriteLine("");

		writer.Close();
	}

    public void SetIsAnswerTrue()
    {
        IsAnswerTrue = true;
    }

    public void SetIsAnswerFalse()
    {
        IsAnswerFalse = true;
    }

    void Initialize()
    {
        QuestionSceneText = _class_all_references_scene_mri_compatible_googles.Instance.GO_question_scene_text;
        Questions_SelectorMecanism_AnswerTrue = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_selector_mecanism_AnswerTrue;
        Questions_SelectorMecanism_AnswerFalse = _class_all_references_scene_mri_compatible_googles.Instance.GO_questions_selector_mecanism_AnswerFalse;

        _script_main_experiment_manager = _class_all_references_scene_mri_compatible_googles.Instance.script_main_experiment_manager;
    }
    
	void DisplayButtons()
	{
		Questions_SelectorMecanism_AnswerTrue.transform.localPosition = new Vector3(0.0025f, -0.3716f, 0.32f);
		Questions_SelectorMecanism_AnswerFalse.transform.localPosition = new Vector3(0.1525f, -0.3716f, 0.32f);
	}

	void RemoveButtons()
	{
		Questions_SelectorMecanism_AnswerTrue.transform.localPosition = new Vector3(10000 + 0.0025f, 10000 + -0.3716f, 10000 + 0.32f);
		Questions_SelectorMecanism_AnswerFalse.transform.localPosition = new Vector3(10000 + 0.1525f, 10000 + -0.3716f, 10000 + 0.32f);
	}
}
