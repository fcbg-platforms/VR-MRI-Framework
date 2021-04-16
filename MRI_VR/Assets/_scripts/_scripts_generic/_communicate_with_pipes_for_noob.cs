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
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

public class _communicate_with_pipes_for_noob : MonoBehaviour
{

	NamedPipeClientStream client;
	NamedPipeServerStream server;

	StreamReader client_reader;
	StreamWriter client_writer;

	StreamReader server_reader;
	StreamWriter server_writer;

	IEnumerator coroutine_pipe_server;

	/*CancellationTokenSource cancellation_token_source_pipe_server;
	CancellationToken cancellation_token_pipe_server;

	CancellationTokenSource cancellation_token_source_pipe_client;
	CancellationToken cancellation_token_pipe_client;*/

	[Header("Pipe config")]
	public string pipe_name = "ILovePipes";
	[Header("Pipe init")]
	public bool b_start_pipe_server;
	public bool b_start_pipe_client;
	[Header("Pipe send")]
	public string text_pipe_client;
	public bool b_send_pipe_client;
	[Header("Pipe stop")]
	public bool b_stop_pipe_server;
	public bool b_stop_pipe_client;
	[Header("Received string")]
	public string s_received_string;
    public List<String> l_string;
    [Header("Pipe stop")]
	public bool b_is_pipe_server_active;
	public bool b_is_pipe_client_active;

	private Coroutine coroutine_server;
	private Coroutine coroutine_client;



	/// <summary>
	public bool b_check_for_number_of_commands;
	public int i_number_of_commands;
	/// </summary>



	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (b_start_pipe_server)
		{
			StartPipeServer(pipe_name);
			b_start_pipe_server = false;
		}
		if (b_start_pipe_client)
		{
			StartPipeClient(pipe_name);
			b_start_pipe_client = false;
		}
		if (b_send_pipe_client)
		{
			PipeClientSend(text_pipe_client);
			b_send_pipe_client = false;
		}
		if (b_stop_pipe_server)
		{
			stop_pipe_server();
			b_stop_pipe_server = false;
		}
		if (b_stop_pipe_client)
		{
			stop_pipe_client();
			b_stop_pipe_client = false;
		}
	}

	//////public void start_pipe_on_client()
	//////{
	//////	StartPipeClient(pipe_name);
	//////}

	//////public void start_pipe_on_server()
	//////{
	//////	StartPipeServer(pipe_name);
	//////}

	public void stop_pipe_server()
	{
        if (b_is_pipe_server_active)
        {
            //server.Disconnect();
            if (server_reader != null)
            {
                server_reader.Dispose();
            }
            if (server_writer != null)
            {
                server_writer.Dispose();
            }
            server.Dispose();
            // Debug.Log("server pipe stop");
            //cancellation_token_source_pipe_server.Cancel();

            b_is_pipe_server_active = false;

            if (b_is_pipe_client_active == false)
            {
                this.gameObject.SetActive(false);
            }
        }
	}

	public void stop_pipe_client()
	{
        if (b_is_pipe_client_active)
        {
            //client.Disconnect();
            /*if (client_reader != null)
            {
                client_reader.Dispose();
            }
            if (client_writer != null)
            {
                client_writer.Dispose();
            }
            client.Dispose();
            Debug.Log("client pipe stop");*/
            //cancellation_token_source_pipe_client.Cancel();

            if (client_reader != null)
            {
                client_reader.Dispose();
            }
            if (client_writer != null)
            {
                client_writer.Dispose();
            }
            client.Dispose();
            Debug.Log("client pipe stop");

            b_is_pipe_client_active = false;

            if (b_is_pipe_server_active == false)
            {
                this.gameObject.SetActive(false);
            }
        }
	}




    //[HideInInspector] // FOR DEBUG PURPOSE ONLY
    [Header("Start app alone")]
    public bool b_debug_discard_named_pipes_start_experiment_alone = false;

    public void start_pipe_on_client()
    {
        if (!b_debug_discard_named_pipes_start_experiment_alone)
        {
            StartPipeClient(pipe_name);
        }
    }

    public void start_pipe_on_server()
    {
        if (!b_debug_discard_named_pipes_start_experiment_alone)
        {
            StartPipeServer(pipe_name);
        }
    }

    void StartPipeClient(string pipe_name)
    {
        if (!b_debug_discard_named_pipes_start_experiment_alone)
        {
            //Client
            client = new NamedPipeClientStream(pipe_name);
            client.Connect();
            client_reader = new StreamReader(client);
            client_writer = new StreamWriter(client);
            Debug.Log("client pipe start");

            b_is_pipe_client_active = true;
        }
    }

    public void PipeClientSend(string input)
    {
        if (!b_debug_discard_named_pipes_start_experiment_alone)
        {
            client_writer.WriteLine(input);
            client_writer.Flush();
            Debug.Log(pipe_name + " client_writer : " + input);
        }
    }



    void StartPipeServer(string pipe_name)
	{
		Task.Factory.StartNew(() =>
		{
			Debug.Log("server pipe start");
			b_is_pipe_server_active = true;

			server = new NamedPipeServerStream(pipe_name);
			server.WaitForConnection();
			server_reader = new StreamReader(server);
			server_writer = new StreamWriter(server);
			while (true)
			{
				string line = server_reader.ReadLine();
				s_received_string = line;
                l_string.Add(line);
                //Debug.Log(pipe_name + " server_reader : <" + line +">");

				if (b_check_for_number_of_commands)
				{
					string next_read_line = s_received_string;
					char[] separators_next_read_line = new char[] { ' ' };
					string[] result_next_read_line = new string[0];
					try
					{
						result_next_read_line = next_read_line.Split(separators_next_read_line, StringSplitOptions.RemoveEmptyEntries);


						if (result_next_read_line.Length > 0)
						{
							switch (result_next_read_line[0])
							{
								case "[command]":
									i_number_of_commands++;

									if (result_next_read_line[1] == "forced_pause")
									{
										// paused during the experiment. Not count as a command
										i_number_of_commands--;
									}

									if (result_next_read_line[1] == "forced_stop")
									{
										// stop the experiment. Not count as a command
										i_number_of_commands--;
									}

									break;
							}
						}
					}
					catch (NullReferenceException)
					{

					}
				}
				//Debug.Log(":: b_new_command_waiting_to_be_processed : " + s_received_string);
			}
		});
	}
}
