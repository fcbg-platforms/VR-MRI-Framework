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
using System.IO;
using System.Collections.Generic;
using System.Threading;

public class _camera_screenshot_threaded : MonoBehaviour
{
    bool b_take_screenshot;
    List<string> l_s_screenshot_filename = new List<string>();
    List<Texture2D> l_T2D_screenshot = new List<Texture2D>();

    public void take_screenshot(string s_filename)
    {
        l_s_screenshot_filename.Add(s_filename);
        b_take_screenshot = true;
    }

    public void save_taken_screenshots()
    {
        //Thread t = new Thread(new ThreadStart(Thread_IE_save_taken_screenshots));
        //t.Start();
        Thread_IE_save_taken_screenshots();
    }

    void Thread_IE_save_taken_screenshots()
    {
        //foreach (RenderTexture Rend_text in l_RenderTexture_screenshot) {
        for (int i = 0; i < l_T2D_screenshot.Count; i++)
        {
            File.WriteAllBytes(l_s_screenshot_filename[i] + ".png", ImageConversion.EncodeToPNG(l_T2D_screenshot[i]));

            Destroy(l_T2D_screenshot[i]);

        }
        l_s_screenshot_filename.Clear();
        l_T2D_screenshot.Clear();
        l_s_screenshot_filename = new List<string>();
        l_T2D_screenshot = new List<Texture2D>();
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (b_take_screenshot)
        {
            var tempRT = RenderTexture.GetTemporary(source.width, source.height);
            Graphics.Blit(source, tempRT);

            var tempTex = new Texture2D(source.width, source.height, TextureFormat.RGBA32, false);
            tempTex.ReadPixels(new Rect(0, 0, source.width, source.height), 0, 0, false);
            tempTex.Apply();

            l_T2D_screenshot.Add(tempTex);

            RenderTexture.ReleaseTemporary(tempRT);

            b_take_screenshot = false;
        }

        Graphics.Blit(source, destination);
    }
}

