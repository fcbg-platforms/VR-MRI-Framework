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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class _logs_in_GUI : MonoBehaviour
{
    string s_logs;
    Queue Q_logs = new Queue();
    Text t_debug_text;

    private void Start()
    {
        t_debug_text = GameObject.Find("Canvas/DebugTextScrollView/Viewport/Content/DebugTextZone").GetComponent<Text>();
        t_debug_text.color = Color.green;
    }

    void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    void HandleLog(string logString, string stackTrace, LogType type)
    {
        s_logs = logString;
        string s_new_log = "\n [" + type + "] : " + s_logs;
        Q_logs.Enqueue(s_new_log);
        if (type == LogType.Exception)
        {
            s_new_log = "\n" + stackTrace;
            Q_logs.Enqueue(s_new_log);
        }
        if (type == LogType.Error)
        {
            t_debug_text.color = Color.red;
        }
        if (type == LogType.Warning)
        {
            t_debug_text.color = new Color(1.0f, 0.55f, 0, 1);
        }
        s_logs = string.Empty;
        foreach (string s_log in Q_logs)
        {
            s_logs += s_log;
        }

        t_debug_text.text += s_new_log;

        if (logString == "clear_logs")
        {
            clearLogs();
        }
    }

    public void clearLogs()
    {
        t_debug_text.text = "";
        t_debug_text.color = Color.green;
    }
}