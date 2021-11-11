using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Azure.CognitiveServices.Vision.Face.Models;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CognitiveServicesSO
{
    public partial class Form1 : Form
    {
        private string videoPath = null;
        private const string IMAGES_FOLDER = @".\images";
        private VisionAPI visionAPI;
        private FaceAPI faceAPI;

        private int adultFrames;
        private int maleFrames;
        private int femaleFrames;
        private int totalFrames;
        private float videoFPS;

        public Form1()
        {
            InitializeComponent();
            InitializeDetectionAPI();
            dgvMaleFemale.BackgroundColor = Color.White;
            dgvMaleFemale.RowHeadersVisible = false;
            dgvAdult.BackgroundColor = Color.White;
            dgvAdult.RowHeadersVisible = false;
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                videoPath = openFileDialog.FileName;
                lblFile.Text = Path.GetFileName(videoPath);
            }
        }

        private void InitializeDetectionAPI()
        {
            List<VisualFeatureTypes?> visionFeatures = new List<VisualFeatureTypes?>()
            {
                VisualFeatureTypes.Adult
            };
            List<FaceAttributeType> faceAttributes = new List<FaceAttributeType>()
            {
                FaceAttributeType.Gender
            };
            visionAPI = new VisionAPI(visionFeatures);
            faceAPI = new FaceAPI(faceAttributes);
        }

        private async void btnRun_Click(object sender, EventArgs e)
        {
            if (videoPath != null)
            {
                videoFPS = GetFpsInVideo();
                PrepareImagesFolder();
                DecodeKeyImages();

                adultFrames = maleFrames = femaleFrames = totalFrames = 0;
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                await ProcessImages(false);
                stopwatch.Stop();
                TimeSpan ts = stopwatch.Elapsed;
                lblSecuential.Text = string.Format("{0:00}:{1:00}:{2:00}.{3}",
                        ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

                dgvAdult.Rows.Clear();
                dgvMaleFemale.Rows.Clear();

                adultFrames = maleFrames = femaleFrames = 0;
                stopwatch = new Stopwatch();
                stopwatch.Start();
                await ProcessImages(true);
                stopwatch.Stop();
                ts = stopwatch.Elapsed;
                lblParallel.Text = string.Format("{0:00}:{1:00}:{2:00}.{3}",
                        ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
            }
        }

        private void PrepareImagesFolder()
        {
            if (Directory.Exists(IMAGES_FOLDER))
            {
                Directory.Delete(IMAGES_FOLDER, true);
            }
            Directory.CreateDirectory(IMAGES_FOLDER);
        }
        private void DecodeKeyImages()
        {
            string command = $"/C ffmpeg -i {videoPath} -vf \"select = gt(scene\\, 0.5), scale = 640:360\" -vsync vfr -frame_pts 1 {IMAGES_FOLDER}\\%03d.png";
            Process proc = new Process();
            proc.StartInfo.FileName = "CMD.exe";
            proc.StartInfo.Arguments = command;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.Start();
            proc.WaitForExit();
        }

        private async Task ProcessImages(bool async)
        {
            string[] fileEntries = Directory.GetFiles(".\\images");
            totalFrames = fileEntries.Length;
            if (async)
            {
                // TODO: process by batches instead
                var tasks = fileEntries.Select(id => AnalyzeImage(id));
                await Task.WhenAll(tasks);
            }
            else
            {
                foreach (string fileName in fileEntries)
                {
                    await AnalyzeImage(fileName);
                }
            }
        }

        private async Task AnalyzeImage(string imagePath)
        {
            int frame = int.Parse(Path.GetFileName(imagePath).Split('.')[0]);
            int seconds = FramesToSeconds(frame, videoFPS);
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            using (Stream imageVision = File.OpenRead(imagePath))
            {
                using (Stream imageFace = File.OpenRead(imagePath))
                {
                    ImageAnalysis results = await visionAPI.StartRecognizing(imageVision);
                    IList<DetectedFace> detectedFaces = await faceAPI.StartRecognizing(imageFace);
                    if ((results.Adult.IsAdultContent || results.Adult.IsRacyContent || results.Adult.IsGoryContent))
                    {
                        adultFrames++;
                        dgvAdult.Rows.Add(time.ToString(@"hh\:mm\:ss\:fff"));
                        ProccessVisionOutput();
                    }
                    foreach (DetectedFace face in detectedFaces)
                    {
                        if (face.FaceAttributes.Gender == 0)
                        {
                            maleFrames++;
                            dgvMaleFemale.Rows.Add(time.ToString(@"hh\:mm\:ss\:fff"), "Hombre");
                        }
                        else
                        {
                            femaleFrames++;
                            dgvMaleFemale.Rows.Add(time.ToString(@"hh\:mm\:ss\:fff"), "Mujer");
                        }
                        ProccessFaceOutput();
                    }
                }
            }
        }

        private void ProccessFaceOutput()
        {
            chartMaleFemale.Titles.Clear();
            chartMaleFemale.Series["Series1"].Points.Clear();

            string[] names = { "Hombres", "Mujeres" };
            int[] values = { maleFrames, femaleFrames };
            chartMaleFemale.Titles.Add("Cantidad de Hombres y Mujeres");
            for (int i = 0; i < names.Length; i++)
            {
                chartMaleFemale.Series["Series1"].Points.AddXY(names[i] + " = " + values[i], values[i]);
            }
        }

        private void ProccessVisionOutput()
        {
            chartAdult.Titles.Clear();
            chartAdult.Series["Series1"].Points.Clear();

            string[] names = { "+18", "No +18" };
            int[] values = { adultFrames, totalFrames - adultFrames };
            chartAdult.Titles.Add("Cantidad de escenas para adultos");
            for (int i = 0; i < names.Length; i++)
            {
                chartAdult.Series["Series1"].Points.AddXY(names[i] + " = " + values[i], values[i]);
            }
        }

        private float GetFpsInVideo()
        {
            ShellFile shellFile = ShellFile.FromFilePath(videoPath);
            return ((float)shellFile.Properties.System.Video.FrameRate.Value) / 1000;
        }

        private int FramesToSeconds(int frame, float fps)
        {
            return (int) (frame / fps);
        }
    }
}
