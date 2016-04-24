using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AForge.Video;
using AForge.Video.DirectShow;
using Newtonsoft.Json;
using System.Reflection;
using System.Linq.Expressions; 
namespace NASAChallenge
{
    public partial class Form1 : Form
    {
        int count = 10;
        bool start = false; 
        String lat = "00";
        String lft = "00";
        String lht = "00";
        String lnt = "00";
        String lst = "00";
        String lct = "00"; 
        public Form1()
        {
            InitializeComponent();

            timer1.Start();
        }
        //Create webcam object
        VideoCaptureDevice videoSource;
 
        private void Form1_Load(object sender, EventArgs e)
        {
            //List all available video sources. (That can be webcams as well as tv cards, etc)
            FilterInfoCollection videosources = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            //Check if atleast one video source is available
            if (videosources != null )
            {
                //For example use first video device. You may check if this is your webcam.
                videoSource = new VideoCaptureDevice(videosources[0].MonikerString);

                try
                {
                    //Check if the video device provides a list of supported resolutions
                    if (videoSource.VideoCapabilities.Length > 0)
                    {
                        string highestSolution = "0;0";
                        //Search for the highest resolution
                        for (int i = 0; i < videoSource.VideoCapabilities.Length; i++)
                        {
                            if (videoSource.VideoCapabilities[i].FrameSize.Width > Convert.ToInt32(highestSolution.Split(';')[0]))
                                highestSolution = videoSource.VideoCapabilities[i].FrameSize.Width.ToString() + ";" + i.ToString();
                        }
                        //Set the highest resolution as active
                        videoSource.VideoResolution = videoSource.VideoCapabilities[Convert.ToInt32(highestSolution.Split(';')[1])];
                    }
                }
                catch { }

                //Create NewFrame event handler
                //(This one triggers every time a new frame/image is captured
                videoSource.NewFrame += new AForge.Video.NewFrameEventHandler(videoSource_NewFrame);

                //Start recording
                videoSource.Start();
            }

        }

        void videoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            //Cast the frame as Bitmap object and don't forget to use ".Clone()" otherwise
            //you'll probably get access violation exceptions
            if (start) { 
            pictureBoxVideo.BackgroundImage = (Bitmap)eventArgs.Frame.Clone();
            var img = (Bitmap)eventArgs.Frame.Clone();
            byte[] byteArray = null;
            count--;
            if (count < 0)
            {
                count = 20; 
                using (MemoryStream stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    stream.Close();

                    byteArray = stream.ToArray();
                }

                var res = EMO.getEmo(byteArray);
                if(res.Length > 10){
                    dynamic data = JsonConvert.DeserializeObject(res);
                    lat = "" + data[0].scores.anger;
                    lht = "" + data[0].scores.happiness;
                    lnt = "" + data[0].scores.neutral;
                    lst = "" + data[0].scores.sadness;
                    lct = "" + data[0].scores.contempt;
                    lft = "" + data[0].scores.fear;

                    if (lat.Contains("E")) lat = "000";
                    if (lht.Contains("E")) lht = "000";
                    if (lnt.Contains("E")) lnt = "000";
                    if (lst.Contains("E")) lst = "000";
                    if (lct.Contains("E")) lct = "000";
                    if (lft.Contains("E")) lft = "000"; 
                }
            }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Stop and free the webcam object if application is closing
            if (videoSource != null && videoSource.IsRunning)
            {
                videoSource.SignalToStop();
                videoSource = null;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
       

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                this.la.Text = ("" + float.Parse(lat) * 100).Substring(0, 2) + " %";
            }
            catch
            {
                this.la.Text = " 0 %";
            }
            try
            {
                this.lh.Text = ("" + float.Parse(lht) * 100).Substring(0, 2) + " %";
            }
            catch
            {
                this.lh.Text = " 0 %";
            }
            try
            {
                this.ls.Text = ("" + float.Parse(lst) * 100).Substring(0, 2) + " %";
            }
            catch
            {
                this.ls.Text = " 0 %";
            }
            try
            {
                this.lc.Text = ("" + float.Parse(lct) * 100).Substring(0, 2) + " %";
            }
            catch
            {
                this.lc.Text = " 0 %";
            }
            try
            {
                this.lf.Text = ("" + float.Parse(lft) * 100).Substring(0, 2) + " %";
            }
            catch
            {
                this.lf.Text = " 0 %";
            }
            try
            {
                this.ln.Text = ("" + float.Parse(lnt) * 100).Substring(0, 2) + " %";
            }
            catch
            {
                this.ln.Text = " 0 %";
            }
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = (button1.Text == "Start") ? "Stop" : "Start";
            if (button1.Text == "Stop")
            {
                start = true;
            }
            else
            {
                start = false;
            }
        }
       
    }
}
