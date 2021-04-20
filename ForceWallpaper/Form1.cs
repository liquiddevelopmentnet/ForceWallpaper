using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ForceWallpaper
{
    public partial class ForceWallpaper : Form
    {

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(uint uiAction, uint uiParam, String pvParam, uint fWinIni);

        private const uint SPI_SETDESKWALLPAPER = 0x14;
        private const uint SPIF_UPDATEINIFILE = 0x1;
        private const uint SPIF_SENDWININICHANGE = 0x2;

        public ForceWallpaper()
        {
            InitializeComponent();
        }

        private void E_Load(object sender, EventArgs e)
        {
            
        }

        private void DisplayPicture(string file_name, bool update_registry)
        {
            try
            {
                uint flags = 0;
                if (update_registry) flags = SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE;

                if (!SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, file_name, flags))
                {
                    MessageBox.Show("SystemParametersInfo failed.",
                        "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying picture " +
                    file_name + ".\n" + ex.Message,
                    "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
            }
        }

        private void FileBrowser_Click(object sender, MouseEventArgs e)
        {
            string filePath = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "C:\\";
                openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath = openFileDialog.FileName;

                    try
                    {
                        _ = openFileDialog.OpenFile();
                    }
                    catch (FileNotFoundException)
                    {
                        MessageBox.Show("The Specified File does not exist.", "ForceWallpaper", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                }
            }
            this.textBox1.Text = filePath;
        }

        private void Change_Click(object sender, EventArgs e)
        {
            DisplayPicture(this.textBox1.Text, this.checkBox1.Checked);
        }
    }
}
