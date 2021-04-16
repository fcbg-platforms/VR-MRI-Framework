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
using UnityEngine;

public class _manager_scene_second_control_screen : MonoBehaviour {
	[Header("Progress bars configurations")]
	public Texture2D emptyProgressBar;
	public Texture2D fullProgressBar;
	public Texture2D progressBarBorder;
	public Color color_emptyProgressBar;
	public Color color_fullProgressBar;
	public Color color_progressBarBorder;
	public Color color_ProgresspercentageText;
	public Color color_InformationText;

	[Header("Progress bars informations")]
	public float progress_current_command;
	public float progress_current_experiment;
	public int i_total_command_nb;
	public int i_current_command_nb;
	public string s_current_command;
	public string s_current_command_duration;
	public string s_current_command_time_passed;
	public float f_current_command_duration;
	public float f_current_command_time_passed;
	public bool b_is_command_timer_active;
	public string s_current_experiment_time_passed;
	public float f_current_experiment_time_passed;
	public string s_current_experiment_time_passed_without_notime; // time passed "commands with duration" (excludes pause, notime commands, ...)
	public float f_current_experiment_time_passed_without_notime = -1;
	public float f_current_experiment_duration;
	public string s_current_experiment_duration;
	public bool b_has_experiment_start;

	public bool s_experiment_file_data_path_received;
	public bool s_log_file_data_path_received;

	[Header("Experiment Files Data Path")]
	public string s_experiment_file_data_path;
	public string s_log_file_data_path;
	public string s_experiment_exe_data_path = "\\_Datas\\_run_mri_experiment.bat";

	[Header("Pipes")]
	public GameObject GO_server_pipe;
	public GameObject GO_client_pipe;

	private bool b_has_mri_app_focus;
	private bool b_error_connection_cameras;
	private bool b_client_disconnected;
	private bool b_after_EOF_end_of_exp;

	float f_progress_loading_experiment = 0;
	float f_tick_time_progress_loading_experiment = 1f;

	float f_last_command_bypasstime_duration = 0;
	float f_current_experiment_time_passed_without_notime_before_bypasstime_command = 0;

	void OnGUI()
	{
		if (b_can_quit_want_quit)
		{
			if (GUI.Button(new Rect(Mathf.Ceil(Screen.width / 80), Mathf.Ceil(Screen.height / 60), Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 30)), "Quit"))
			{
				Application.Quit();
			}
		}


		if (!b_has_experiment_start)
		{
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.fontSize = Mathf.Min(Screen.width / 7, Screen.height / 10);
			GUI.skin.label.normal.textColor = color_InformationText;

			if (f_progress_loading_experiment >= 5 * f_tick_time_progress_loading_experiment)
			{
				f_progress_loading_experiment = 0;
			}

			if ((f_progress_loading_experiment >= 0) && (f_progress_loading_experiment < f_tick_time_progress_loading_experiment)) {
				GUI.Label(new Rect(50, 4 * Mathf.Ceil(Screen.height / 6), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "Loading Experiment     ");
			}
			else if ((f_progress_loading_experiment >= f_tick_time_progress_loading_experiment) && (f_progress_loading_experiment < 2 * f_tick_time_progress_loading_experiment))
			{
				GUI.Label(new Rect(50, 4 * Mathf.Ceil(Screen.height / 6), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "Loading Experiment .   ");
			}
			else if ((f_progress_loading_experiment >= 2 * f_tick_time_progress_loading_experiment) && (f_progress_loading_experiment < 3 * f_tick_time_progress_loading_experiment))
			{
				GUI.Label(new Rect(50, 4 * Mathf.Ceil(Screen.height / 6), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "Loading Experiment ..  ");
			}
			else if ((f_progress_loading_experiment >= 3 * f_tick_time_progress_loading_experiment) && (f_progress_loading_experiment < 4 * f_tick_time_progress_loading_experiment))
			{
				GUI.Label(new Rect(50, 4 * Mathf.Ceil(Screen.height / 6), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "Loading Experiment ... ");
			}
			else if ((f_progress_loading_experiment >= 4 * f_tick_time_progress_loading_experiment) && (f_progress_loading_experiment < 5 * f_tick_time_progress_loading_experiment))
			{
				GUI.Label(new Rect(50, 4 * Mathf.Ceil(Screen.height / 6), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "Loading Experiment ....");
			}

			f_progress_loading_experiment += Time.deltaTime;
		}
		else if (b_has_experiment_start)
		{
			if (!b_has_mri_app_focus)
			{
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.skin.label.fontSize = Mathf.Min(Screen.width / 12, Screen.height / 18);
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), Mathf.Ceil(Screen.height / 12) + (3 / 2) * Mathf.Min(Screen.width / 12, Screen.height / 18), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "MRI app is not focused. No input received !");
			}
			/*else if (b_has_mri_app_focus)
			{
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.skin.label.fontSize = Mathf.Min(Screen.width / 9, Screen.height / 13);
				GUI.skin.label.normal.textColor = Color.green;
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), 2 * Mathf.Ceil(Screen.height / 6), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "MRI app have focus");
			}*/
			if (b_error_connection_cameras)
			{
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.skin.label.fontSize = Mathf.Min(Screen.width / 12, Screen.height / 18);
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), Mathf.Ceil(Screen.height / 12) + (4 / 2) * Mathf.Min(Screen.width / 12, Screen.height / 18), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "Motion Tracking system not detected !");
			}
			if (b_client_disconnected)
			{
				GUI.skin.label.alignment = TextAnchor.MiddleLeft;
				GUI.skin.label.fontSize = Mathf.Min(Screen.width / 12, Screen.height / 18);
				GUI.skin.label.normal.textColor = Color.red;
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), Mathf.Ceil(Screen.height / 12) + 3 * Mathf.Min(Screen.width / 12, Screen.height / 18), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), "MRI app disconnected !");
			}


			GUI.skin.label.alignment = TextAnchor.MiddleLeft;
			GUI.skin.label.fontSize = Mathf.Min(Screen.width / 9, Screen.height / 13);
			GUI.skin.label.normal.textColor = color_InformationText;
			GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), Mathf.Ceil(Screen.height / 12), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), String.Format("{0:0.00}", f_current_experiment_time_passed_without_notime) + " / " + String.Format("{0:0.00}", f_current_experiment_duration));

			GUI.skin.label.alignment = TextAnchor.MiddleRight;
			GUI.skin.label.fontSize = Mathf.Min(Screen.width / 9, Screen.height / 13);
			GUI.skin.label.normal.textColor = color_InformationText;
			GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), Mathf.Ceil(Screen.height / 12), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), String.Format("{0:0.00}", f_current_experiment_time_passed));



			// Current command progress
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) - Mathf.Ceil(Screen.height / 48), 3 * Mathf.Ceil(Screen.height / 7) - Mathf.Ceil(Screen.height / 48), 7 * Mathf.Ceil(Screen.width / 8) + 2 * Mathf.Ceil(Screen.height / 48), Mathf.Ceil(Screen.height / 48)), progressBarBorder);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) - Mathf.Ceil(Screen.height / 48), 5 * Mathf.Ceil(Screen.height / 7), 7 * Mathf.Ceil(Screen.width / 8) + 2 * Mathf.Ceil(Screen.height / 48), Mathf.Ceil(Screen.height / 48)), progressBarBorder);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) - Mathf.Ceil(Screen.height / 48), 3 * Mathf.Ceil(Screen.height / 7), Mathf.Ceil(Screen.height / 48), 2 * Mathf.Ceil(Screen.height / 7)), progressBarBorder);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) + 7 * Mathf.Ceil(Screen.width / 8), 3 * Mathf.Ceil(Screen.height / 7), Mathf.Ceil(Screen.height / 48), 2 * Mathf.Ceil(Screen.height / 7)), progressBarBorder);

			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16), 3 * Mathf.Ceil(Screen.height / 7), 7 * Mathf.Ceil(Screen.width / 8), 2 * Mathf.Ceil(Screen.height / 7)), emptyProgressBar);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16), 3 * Mathf.Ceil(Screen.height / 7), Mathf.Clamp(progress_current_command, 0, 100) * (7 * Mathf.Ceil(Screen.width / 8)) / 100, 2 * Mathf.Ceil(Screen.height / 7)), fullProgressBar);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.fontSize = Mathf.Min(Screen.width / 9, Screen.height / 13);
			GUI.skin.label.normal.textColor = color_ProgresspercentageText;
			if (progress_current_command == 0)
			{
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), 3 * Mathf.Ceil(Screen.height / 7), 7 * Mathf.Ceil(Screen.width / 8), 2 * Mathf.Ceil(Screen.height / 7)), string.Format(s_current_command + Environment.NewLine + " (" + s_current_command_time_passed + " / " + String.Format("{0:0.00}", f_current_command_duration) + ") {0:N0}%", Mathf.Clamp(progress_current_command, 0, 100)));
			}
			else if (progress_current_command == 100)
			{
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), 3 * Mathf.Ceil(Screen.height / 7), 7 * Mathf.Ceil(Screen.width / 8), 2 * Mathf.Ceil(Screen.height / 7)), string.Format(s_current_command + Environment.NewLine + " (" + s_current_command_time_passed + " / " + String.Format("{0:0.00}", f_current_command_duration) + ") {0:N0}%", Mathf.Clamp(progress_current_command, 0, 100)));
			}
			else
			{
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), 3 * Mathf.Ceil(Screen.height / 7), 7 * Mathf.Ceil(Screen.width / 8), 2 * Mathf.Ceil(Screen.height / 7)), string.Format(s_current_command + Environment.NewLine + " (" + s_current_command_time_passed + " / " + String.Format("{0:0.00}", f_current_command_duration) + ") {0:0.00}%", Mathf.Clamp(progress_current_command, 0, 100)));
			}


			// Current experiment progress
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) - Mathf.Ceil(Screen.height / 48), 9.25f * Mathf.Ceil(Screen.height / 12) - Mathf.Ceil(Screen.height / 48), 7 * Mathf.Ceil(Screen.width / 8) + 2 * Mathf.Ceil(Screen.height / 48), Mathf.Ceil(Screen.height / 48)), progressBarBorder);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) - Mathf.Ceil(Screen.height / 48), 11.25f * Mathf.Ceil(Screen.height / 12), 7 * Mathf.Ceil(Screen.width / 8) + 2 * Mathf.Ceil(Screen.height / 48), Mathf.Ceil(Screen.height / 48)), progressBarBorder);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) - Mathf.Ceil(Screen.height / 48), 9.25f * Mathf.Ceil(Screen.height / 12), Mathf.Ceil(Screen.height / 48), Mathf.Ceil(Screen.height / 6)), progressBarBorder);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16) + 7 * Mathf.Ceil(Screen.width / 8), 9.25f * Mathf.Ceil(Screen.height / 12), Mathf.Ceil(Screen.height / 48), Mathf.Ceil(Screen.height / 6)), progressBarBorder);

			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16), 9.25f * Mathf.Ceil(Screen.height / 12), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), emptyProgressBar);
			GUI.DrawTexture(new Rect(Mathf.Ceil(Screen.width / 16), 9.25f * Mathf.Ceil(Screen.height / 12), Mathf.Clamp(progress_current_experiment, 0, 100) * (7 * Mathf.Ceil(Screen.width / 8)) / 100, Mathf.Ceil(Screen.height / 6)), fullProgressBar);
			GUI.skin.label.alignment = TextAnchor.MiddleCenter;
			GUI.skin.label.fontSize = Mathf.Min(Screen.width / 9, Screen.height / 13);
			GUI.skin.label.normal.textColor = color_ProgresspercentageText;
			if (progress_current_experiment == 0)
			{
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), 9.25f * Mathf.Ceil(Screen.height / 12), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), string.Format("command n° " + i_current_command_nb + /*GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands +*/ " / " + i_total_command_nb + " ({0:N0}%)", Mathf.Clamp(progress_current_experiment, 0, 100)));
			}
			else if (progress_current_experiment == 100)
			{
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), 9.25f * Mathf.Ceil(Screen.height / 12), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), string.Format("command n° " + i_current_command_nb + /*GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands +*/ " / " + i_total_command_nb + " ({0:N0}%)", Mathf.Clamp(progress_current_experiment, 0, 100)));
			}
			else
			{
				GUI.Label(new Rect(Mathf.Ceil(Screen.width / 16), 9.25f * Mathf.Ceil(Screen.height / 12), 7 * Mathf.Ceil(Screen.width / 8), Mathf.Ceil(Screen.height / 6)), string.Format("command n° " + i_current_command_nb + /*GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands +*/ " / " + i_total_command_nb + " ({0:0.00}%)", Mathf.Clamp(progress_current_experiment, 0, 100)));
			}
		}
	}

	// Use this for initialization
	void Start () {

        // force focus on this app window and hide mouse cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;



        init_textures_with_color(color_emptyProgressBar, color_fullProgressBar, color_progressBarBorder);
		
		GO_server_pipe = GameObject.Find("GO_server_pipe");
		GO_client_pipe = GameObject.Find("GO_client_pipe");
		
		GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().start_pipe_on_server();

		i_total_command_nb = _static_informations.i_number_of_commands;
		f_current_experiment_duration = _static_informations.f_total_commands_time;
		f_current_experiment_duration = f_current_experiment_duration / 1000; // ms to seconds
		s_current_experiment_duration = string.Format("{0:0.00}", f_current_experiment_duration);
		i_current_command_nb = 0;
		f_current_command_time_passed = 0;
		b_has_experiment_start = false;

		///////
		GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().b_check_for_number_of_commands = true;
		GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands = 0;
		///////

		// get experiment + log file location
		s_experiment_file_data_path = _static_informations.s_experiment_file_data_path;
		s_log_file_data_path = _static_informations.s_log_file_data_path;
		s_experiment_exe_data_path = _static_informations.s_experiment_exe_data_path;
		// start the experiment app

		//Debug.Log(System.IO.Directory.GetCurrentDirectory() + s_experiment_exe_data_path);
		System.Diagnostics.Process.Start(System.IO.Directory.GetCurrentDirectory() + s_experiment_exe_data_path);
	}
	
	// Update is called once per frame
	void Update () {

		update_received_information_server_pipe();
		update_current_command_status();
		update_current_experiment_status();
		update_can_quit_app();

	}


	IEnumerator StopApp(float waitTime)
	{
		yield return new WaitForSeconds(waitTime);

		// force focus on this app and show mouse cursor
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;

		//Application.Quit();
	}



	void update_received_information_server_pipe()
	{
		if (GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().l_string.Count > 0)
		//if (GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().s_received_string != "")
		{
            string next_read_line = GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().l_string[0];
            GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().l_string.RemoveAt(0);

            // Parse
            GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().s_received_string = "";

			char[] separators_next_read_line = new char[] { ' ' };
			string[] result_next_read_line = new string[0];
			try
			{
				result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);
			}
			catch (NullReferenceException)
			{
				// bro app client disconnected -> this app disconnect client + server
				GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().stop_pipe_server();

				b_client_disconnected = true;
				b_after_EOF_end_of_exp = true;
				b_can_quit_want_quit = true;


				StartCoroutine(StopApp(2.5f));
			}

			if (result_next_read_line.Length > 0)
			{
                Debug.Log("pipe server received : " + string.Join(" ", result_next_read_line));
                switch (result_next_read_line[0])
				{
					case "[command]": // [command] text_command_name command_duration test_to_be_displayed

						i_current_command_nb++;

						if (i_current_command_nb == 1) // first experiment command
						{
							b_has_experiment_start = true;
							f_current_experiment_time_passed_without_notime = 0;

							// stop display loading circle and display control HUD
							GameObject.Find("Canvas/LoadingCircleProgress").SetActive(false);
						}
						f_current_experiment_time_passed_without_notime = f_current_experiment_time_passed_without_notime_before_bypasstime_command + f_last_command_bypasstime_duration;

						s_current_command = result_next_read_line[1];
						s_current_command_duration = result_next_read_line[2];
						float.TryParse(s_current_command_duration, out f_current_command_duration);
						if (f_current_command_duration == -1)
						{
							f_current_command_duration = -1;
							s_current_command_time_passed = string.Format("{0:0.00}", f_current_command_duration);
							b_is_command_timer_active = false;
						}
						else
						{
							f_current_command_duration = f_current_command_duration / 1000; // ms to seconds
							b_is_command_timer_active = true;
						}

						f_current_command_time_passed = 0;

						if (s_current_command == "forced_pause")
						{
							// paused during the experiment. Not count as a command
							i_current_command_nb--;
							s_current_command += " | " + "'9' to continue";
						}

						if (s_current_command == "free_move_mri_table")
						{
							// free move the table. Not count as a command
							i_current_command_nb--;
						}

						if (s_current_command == "forced_stop")
						{
							// stop the experiment. Not count as a command
							i_current_command_nb--;
							// start the processus of closing pipes
							GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().stop_pipe_client();
						}

						if (s_current_command == "pause")
						{
							s_current_command += " | " + "'9' to continue";
						}

						if (s_current_command == "pause_wait_for_MRI_pulse")
						{
							s_current_command += " | MRI pulse | or " + "'5' to continue";
						}

                        i_current_command_nb = GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands;

                        if (result_next_read_line.Length == 4)
                        {
                            int i_command_number;
                            if (int.TryParse(result_next_read_line[3], out i_command_number))
                            {
                                i_current_command_nb = i_command_number;
                                GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands = i_command_number;
                            }
                        }

                        if (result_next_read_line.Length == 5)
                        {
                            int i_command_number;
                            if (int.TryParse(result_next_read_line[3], out i_command_number))
                            {
                                i_current_command_nb = i_command_number;
                                GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands = i_command_number;
                            }


                            string s_current_command_replaced_underscore_arobase = result_next_read_line[4].Replace('_', ' ');

                            Char delimiter = '@';
                            String[] substrings = s_current_command_replaced_underscore_arobase.Split(delimiter);

                            for (int i = 0; i < substrings.Length; i++)
                            {
                                s_current_command += Environment.NewLine + substrings[i];
                            }
                        }

                        // hold time bypasstime commands
                        if (f_current_command_duration > 0)
						{
							f_last_command_bypasstime_duration = f_current_command_duration;
							f_current_experiment_time_passed_without_notime_before_bypasstime_command = f_current_experiment_time_passed_without_notime;
						}

						if (f_current_command_duration <= 0)
						{
							progress_current_command = 100;
						}

						if (i_total_command_nb > 0)
						{
							progress_current_experiment = ((float) /*i_current_command_nb*/ GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands) * 100.00f / ((float) i_total_command_nb);
						}						

						if (s_current_command == "EOF")
						{
							// start the processus of closing pipes
							GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().stop_pipe_client();
						}
						break;
					case "[action]":

						if (result_next_read_line[1] == "pause_forced_pb")
						{
                            s_current_command_duration = "-1";

							f_current_command_duration = -1;
							s_current_command_time_passed = string.Format("{0:0.00}", f_current_command_duration);
							b_is_command_timer_active = false;

							progress_current_command = 100;

							f_current_experiment_time_passed_without_notime = f_current_experiment_time_passed_without_notime_before_bypasstime_command + f_last_command_bypasstime_duration;

							s_current_command = "experimenter pause" + " | " + "'9' to continue";
							// paused during the experiment cause pb happened. Not count as a command
							s_current_command += Environment.NewLine + "'y'&'x' to change CP | 'c' to start from last CP";
						}
						else if (result_next_read_line[1] == "SelectCheckpointChanged")
						{
							s_current_command = "Selected CP" + (Int32.Parse(result_next_read_line[3])+1).ToString() + ": " + result_next_read_line[2] + " (l" + result_next_read_line[4] + ")";
							s_current_command += Environment.NewLine + "'y'&'x' to change CP | 'c' to validate";
						}
						else if (result_next_read_line[1] == "ResetSelectedCheckpoint")
						{
							int i_tryparse_number_commands;
							if (int.TryParse(result_next_read_line[2], out i_tryparse_number_commands))
							{
								GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands = i_tryparse_number_commands;
							}
							float f_tryparse_time_elapsed;
							if (float.TryParse(result_next_read_line[3], out f_tryparse_time_elapsed))
							{
								f_current_experiment_time_passed_without_notime = f_tryparse_time_elapsed;
								f_current_experiment_time_passed_without_notime_before_bypasstime_command = f_current_experiment_time_passed_without_notime;
								f_last_command_bypasstime_duration = 0;
							}
							progress_current_experiment = ((float) /*i_current_command_nb*/ GO_server_pipe.GetComponent<_communicate_with_pipes_for_noob>().i_number_of_commands) * 100.00f / ((float)i_total_command_nb);

							s_current_command = "Back to CP: " + result_next_read_line[4];
							s_current_command += Environment.NewLine + "'9' to start";
						}

						break;




					case "[our_server_started_connect_your_client]":
						GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().start_pipe_on_client();
						GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[our_client_connected_waiting_to_send_variables_values]");
						break;
					case "[waiting_s_experiment_file_data_path]":
						GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[s_experiment_file_data_path] " + s_experiment_file_data_path);
						break;
					case "[waiting_s_log_file_data_path]":
						GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[s_log_file_data_path] " + s_log_file_data_path);
						break;
					case "[all_variables_values_received]":
						GO_client_pipe.GetComponent<_communicate_with_pipes_for_noob>().PipeClientSend("[start_experiment]");
						break;
					case "[mri_app_has_focus]":
						b_has_mri_app_focus = true;
						break;
					case "[mri_app_has_not_focus]":
						b_has_mri_app_focus = false;
						break;
					case "[error_connection_to_cameras]":
						b_error_connection_cameras = true;
						break;
					case "null":
						Debug.Log("client_disconnected");
						break;
						;
				}
			}			
		}
	}

	void update_current_experiment_status()
	{
		if (b_has_experiment_start)
		{
			f_current_experiment_time_passed += Time.deltaTime;
			s_current_experiment_time_passed = string.Format("{0:0.00}", f_current_experiment_time_passed);
		}
	}

	void update_current_command_status()
	{
		if (b_is_command_timer_active)
		{
			f_current_command_time_passed += Time.deltaTime;
			s_current_command_time_passed = string.Format("{0:0.00}", f_current_command_time_passed);
			progress_current_command = (f_current_command_time_passed / f_current_command_duration) * 100;

			f_current_experiment_time_passed_without_notime += Time.deltaTime;
			s_current_experiment_time_passed_without_notime = string.Format("{0:0.00}", f_current_experiment_time_passed_without_notime);
		}
	}

	void init_textures_with_color(Color c_emptyProgressBar, Color c_fullProgressBar, Color c_progressBarBorder)
	{
		// Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
		emptyProgressBar = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

		// reset all picels color
		Color32 resetColor_c_emptyProgressBar = c_emptyProgressBar;
		Color32[] resetColorArray_c_emptyProgressBar = emptyProgressBar.GetPixels32();

		for (int i = 0; i < resetColorArray_c_emptyProgressBar.Length; i++)
		{
			resetColorArray_c_emptyProgressBar[i] = resetColor_c_emptyProgressBar;
		}

		emptyProgressBar.SetPixels32(resetColorArray_c_emptyProgressBar);
		emptyProgressBar.Apply();


		// Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
		fullProgressBar = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

		// reset all picels color
		Color32 resetColor_c_fullProgressBar = c_fullProgressBar;
		Color32[] resetColorArray_c_fullProgressBar = fullProgressBar.GetPixels32();

		for (int i = 0; i < resetColorArray_c_fullProgressBar.Length; i++)
		{
			resetColorArray_c_fullProgressBar[i] = resetColor_c_fullProgressBar;
		}

		fullProgressBar.SetPixels32(resetColorArray_c_fullProgressBar);
		fullProgressBar.Apply();


		// Create a new 2x2 texture ARGB32 (32 bit with alpha) and no mipmaps
		progressBarBorder = new Texture2D(1024, 1024, TextureFormat.ARGB32, false);

		// reset all picels color
		Color32 resetColor_c_progressBarBorder = c_progressBarBorder;
		Color32[] resetColorArray_c_progressBarBorder = progressBarBorder.GetPixels32();

		for (int i = 0; i < resetColorArray_c_progressBarBorder.Length; i++)
		{
			resetColorArray_c_progressBarBorder[i] = resetColor_c_progressBarBorder;
		}

		progressBarBorder.SetPixels32(resetColorArray_c_progressBarBorder);
		progressBarBorder.Apply();
	}

	bool b_can_quit_want_quit;

	// end of the experiment -> close exp
	public void update_can_quit_app()
	{
		if (b_after_EOF_end_of_exp)
		{
			if (Input.GetAxisRaw("QuitApp") == 1)
			{
				Application.Quit();
			}
		}
	}
}