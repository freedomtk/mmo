using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static loc_trung.Commons;

namespace loc_trung
{
    public partial class Form : System.Windows.Forms.Form
    {
        public volatile bool m_ToLower = false;
        public volatile bool m_Save = false;

        string userKey = null;
        public Form()
        {
            InitializeComponent();
            createSubFolder();
            initUI();
            this.MaximizeBox = false;
            // this.MinimizeBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            generateLicenseKey();
            checkLicenseKey();
           


        }


        /*
         * Tao ma SHA256 tu o Dia, Khi tạo key nên cộng thêm CPU info, Main , mac, ....
         * Demon chỉ lấy serial ổ C
         */
        private void checkLicenseKey()
        {
          
            List<UserLicense> list = Commons.GetLicenseKey();

            bool isLicenseOk = false;

            foreach (UserLicense user in list)
            {
                if (userKey.Equals(user.Key)) {
                    isLicenseOk = true;
                }
            }

            if (!isLicenseOk)
            {
                foreach (var button in this.Controls.OfType<Button>())
                {

                    button.Enabled = false;
                    button.Text = "?";
                }

            }
        }
        private void generateLicenseKey()
        {
            List<HardDrive> list = Commons.GetAllDiskDrives();

            string cDisk = list[0].ToString();
            string privateKey = "hacker007";
            String hash = Commons.sha256(privateKey + cDisk);
            userKey = hash;
            textBoxKey.Text = hash;
        }
        private void initUI()
        {
            this.Text = "Lọc Trùng";

            textBoxInput.Text = System.Environment.CurrentDirectory + "\\input";
            textBoxOutput.Text = System.Environment.CurrentDirectory + "\\output";
            textBoxData.Text = System.Environment.CurrentDirectory + "\\data";

            checkBoxToLower.Checked = true;
            checkBoxSaveData.Checked = true;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            String outputPath = textBoxOutput.Text;
            try
            {
                Process.Start("explorer", outputPath);
                // Runtime.getRuntime().exec("explorer " + outputPath);
            }
            catch
            {

            }

        }
        private void createSubFolder()
        {
            createSubFolder("data");
            createSubFolder("input");
            createSubFolder("output");
        }
        private void createSubFolder(String folderName)
        {
            if (!System.IO.File.Exists(folderName))
            {
                System.IO.Directory.CreateDirectory(folderName);
            }

        }
        private void btnRun_Click(object sender, EventArgs e)
        {
            m_ToLower = checkBoxToLower.Checked;
            m_Save = checkBoxSaveData.Checked;

            HashSet<string> dataSet = new HashSet<string>();
            HashSet<string> outputSet = new HashSet<string>();

            // DATA
            DirectoryInfo dataDirec = new DirectoryInfo("data");//Assuming Test is your Folder
            FileInfo[] fileDatas = dataDirec.GetFiles("*.txt.*"); //Getting Text files

            foreach (FileInfo file in fileDatas)
            {

                string[] lines = System.IO.File.ReadAllLines(file.FullName);
                foreach (string iline in lines)
                {
                    String compareLine = iline;
                    if (m_ToLower)
                    {
                        compareLine = iline.Trim().ToLower();
                    }
                    if (!dataSet.Contains(compareLine))
                    {
                        dataSet.Add(compareLine);
                    }
                }

            }

            labelTotalData.Text = dataSet.Count + "";


            DirectoryInfo inputDirec = new DirectoryInfo("input");//Assuming Test is your Folder
            FileInfo[] fileInputs = inputDirec.GetFiles("*.txt.*"); //Getting Text files

            foreach (FileInfo file in fileInputs)
            {
                richTextLog.AppendText(file.Name + "\n");
                /*
                string[] lines = System.IO.File.ReadAllLines(file.FullName);
                foreach (string iline in lines)
                {

                    String compareLine = iline;
                    if (m_ToLower)
                    {
                        compareLine = iline.Trim().ToLower();
                    }

                    if (!dataSet.Contains(compareLine))
                    {
                        outputSet.Add(compareLine);
                    }
                }
                */

                string line;

                // Read the file and display it line by line.  
                System.IO.StreamReader file1 =
                    new System.IO.StreamReader(file.FullName);
                while ((line = file1.ReadLine()) != null)
                {
                    //System.Console.WriteLine(line);

                    String compareLine = line;
                    if (m_ToLower)
                    {
                        compareLine = line.Trim().ToLower();
                    }

                    if (!dataSet.Contains(compareLine))
                    {
                        outputSet.Add(compareLine);
                    }


                }
                file1.Close();
            }

            var formattedDate = string.Format("{0:yyMMddhhmmss}", DateTime.Now);

            String pathOutput = $"output/KQ{formattedDate}.txt";

            if (outputSet.Count > 0)
            {
                using (StreamWriter sw = File.AppendText(pathOutput))
                {
                    foreach (String str in outputSet)
                    {
                        sw.WriteLine(str);
                    }
                }
            }
            labelTotalResult.Text = outputSet.Count + "";

            if (m_Save)
            {
                String pathData = $"data/KQ{formattedDate}.txt";

                if (outputSet.Count > 0)
                {
                    using (StreamWriter sw = File.AppendText(pathData))
                    {
                        foreach (String str in outputSet)
                        {
                            sw.WriteLine(str);
                        }
                    }
                }
            }
        }

        private void btnOpenInput_Click(object sender, EventArgs e)
        {
            String path = textBoxInput.Text;
            try
            {
                Process.Start("explorer", path);
                // Runtime.getRuntime().exec("explorer " + outputPath);
            }
            catch
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String path = textBoxData.Text;
            try
            {
                Process.Start("explorer", path);
                // Runtime.getRuntime().exec("explorer " + outputPath);
            }
            catch
            {

            }

        }

        private void buttonListDuplicate_Click(object sender, EventArgs e)
        {

            DirectoryInfo inputDirec = new DirectoryInfo("input");//Assuming Test is your Folder
            FileInfo[] fileInputs = inputDirec.GetFiles("*.txt.*"); //Getting Text files

            HashSet<string> outputSet = new HashSet<string>();

            List<String> listDup = new List<String>();
            int count = 0;


            foreach (FileInfo file in fileInputs)
            {
                richTextLog.AppendText(file.Name + "\n");

                string[] lines = System.IO.File.ReadAllLines(file.FullName);
                foreach (string iline in lines)
                {
                    if (iline != null && iline.Trim().Length > 0)
                    {
                        count++;
                    }

                    String compareLine = iline;
                    if (m_ToLower)
                    {
                        compareLine = iline.Trim().ToLower();
                    }

                    if (!outputSet.Contains(compareLine))
                    {
                        outputSet.Add(compareLine);
                    }
                    else
                    {
                        listDup.Add(compareLine);
                    }
                }
            }

            labelNumberLine.Text = count + "";
            labelNumberOutput.Text = outputSet.Count + "";
            if (listDup.Count > 0)
            {
                using (StreamWriter sw = File.AppendText("FileTrung.txt"))
                {
                    foreach (String str in listDup)
                    {
                        sw.WriteLine(str);
                    }
                }
            }
        }
    }
}
