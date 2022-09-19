using VideoLibrary;
/*  TODO !!
 *      -> Modularize the program.  
 *      The form should NOT be doing this processing!
 *      FIX IT FOO
 */

namespace YoutubeDownloaderWindowsGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var url = textBox1.Text;
            textBox1.Text = "Establishing connection to the provided URL..";
            int resolution = 720;   // default to 720
            int bitrate = 192;  // default
            if (checkBox2.CheckState == CheckState.Checked)
            {
                resolution = 360;
                bitrate = 96;
            }
            Task Download = Task.Run(() =>
            {
                try
                {
                    var youtube = YouTube.Default;
                    var videos = youtube.GetAllVideos(url);
                    int numberOfVideos = videos.Count();
                    int i = 1;
                    foreach (YouTubeVideo video in videos)
                    {
                        if (video.Resolution == resolution && video.AudioBitrate == bitrate)
                        {
                            var dir = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 14);
                            textBox1.Text = "Connection established, attempting download...";
                            richTextBox1.Text =
                                $"Title: \"{video.Title}\"\nResolution: \"{video.Resolution}\"\nAudioBitrate: \"{video.AudioBitrate}\"\n";
                            File.WriteAllBytes(dir + video.Title + ".mp4", video.GetBytes());
                            textBox1.Text = "Download successful: " + Directory.GetCurrentDirectory();
                            return;
                        }
                        else if (i == numberOfVideos)
                        {
                            textBox1.Text = "No suitable video file available at this address.";
                        }    
                        ++i;
                    }
                }
                catch
                {
                    textBox1.Text = "Unable to complete request.  Test a different URL.";
                }
            });
            await Download;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.CheckState == CheckState.Checked)
            {
                checkBox2.CheckState = CheckState.Unchecked;
            }
            else
            {
                checkBox2.CheckState = CheckState.Checked;
            }

            //checkBox2.CheckState = CheckState.Unchecked;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.CheckState == CheckState.Checked)
            {
                checkBox1.CheckState = CheckState.Unchecked;
            }
            else
            {
                checkBox1.CheckState |= CheckState.Checked;
            }
        }
    }
}