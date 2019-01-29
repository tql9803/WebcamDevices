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
        }

        private void StopButtonClick(object sender, EventArgs e)
        {
            StopCamera();
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
        }

        private void StopCamera()
        {
            videoSourcePlayer1.SignalToStop();
            videoSourcePlayer1.WaitForStop();
        }

        
    }
}
