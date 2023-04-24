using HellsysLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPMMaker
{
    public partial class SettingsForm : Form
    {

        const string SECTION1 = "DATABASE";
        public SettingsForm()
        {
            InitializeComponent();
            InitSettings();
        }

        #region 이벤트
        private void dbopen_btn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                dbPath_txt.Text = fbd.SelectedPath;
                SaveSettings("DBPath", dbPath_txt.Text.ToString().Trim());
            }
        }
        #endregion
        #region 메서드
        private void InitSettings()
        {
            if(Helper.EPIIni.Read(SECTION1, "DBPath") != "")
            {
                dbPath_txt.Text = Helper.EPIIni.Read(SECTION1, "DBPath");
            }
        }
        private void SaveSettings(string key, string value)
        {
            Helper.EPIIni.Write(SECTION1, key, value);
        }
        #endregion
    }
}
