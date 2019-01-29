using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using AForge.Video;
using AForge.Video.DirectShow;

namespace WebcamTest
{
    public partial class WebcamTestApp : Form
    {
        FilterInfoCollection videoDevices;

        private Stopwatch fpsWatch;

        public WebcamTestApp()
        {
            InitializeComponent();
            UpdateVideoDevices();
        }

        private void UpdateVideoDevices()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            for (int i = 1, n = videoDevices.Count; i <= n; i++)
            {
                string cameraName = i + " : " + videoDevices[i - 1].Name;

                comboBox1.Items.Add(cameraName);                
            }

            button1.Enabled = true;
            button2.Enabled = false;

        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            StopCamera();

            button1.Enabled = true;
            button2.Enabled = false;
        }

        private void StartButtonClick(object sender, EventArgs e)
        {
            StartCameras();

            button1.Enabled = false;
            button2.Enabled = true;

        }

        private void StartCameras()
        {
            VideoCaptureDevice videoSource = new VideoCaptureDevice(videoDevices[comboBox1.SelectedIndex].MonikerString);

            videoSourcePlayer1.VideoSource = videoSource;
            videoSourcePlayer1.Start();

            fpsWatch = null;
            timer1.Start();
        }

        private void StopCamera()
        {
            timer1.Stop();

            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            IVideoSource videoSource = videoSourcePlayer1.VideoSource;

            int framesReceived = 0;

            // get number of frames for the last second
            if (videoSource != null)
            {
                framesReceived = videoSource.FramesReceived;
            }            

            if (fpsWatch == null)
            {
                fpsWatch = new Stopwatch();
                fpsWatch.Start();
            }
            else
            {
                fpsWatch.Stop();

                float fps1 = 1000.0f * framesReceived / fpsWatch.ElapsedMilliseconds;

                label2.Text = fps1.ToString("F2") + " fps";

                fpsWatch.Reset();
                fpsWatch.Start();
            }
        }
    }
}
