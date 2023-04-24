using BPMMaker.DataBase;
using HellsysLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPMMaker
{
    public partial class Form1 : Form
    {
        #region 데이터베이스멤버
        private static string SECTION1 = "DATABASE";
        private static string DbName = "MyDB,db";
        private static string MyDBPath = System.IO.Path.Combine(Helper.EPIIni.Read(SECTION1, "DBPath"), DbName);
        private static string conString = "Data Source=" + MyDBPath;
        private static BindingSource bindingSrc;

        private static SQLiteConnection connection = new SQLiteConnection(conString);
        private static SQLiteCommand command = new SQLiteCommand("", connection);
        private static string sql;
        private static string dbCommand;
        DBManager dbm = new DBManager();
        #endregion



        SQLiteConnection sqlCon;
        SQLiteCommand sqlCmd;
        SQLiteDataReader sqlReader;
        public Form1()
        {
            InitializeComponent();
            InitForms();
            //Create_DB();
            Rayout_Setting();
            //Data_Show();
        }

        private void InitForms()
        {
            
            dbm.Create_DB();
            UpdateDatabinding();
        }
        private void displayPosition()
        {
            VendorPosition_lbl.Text = "Posion: " + Convert.ToString(bindingSrc.Position + 1) + "/" + bindingSrc.Count.ToString();
        }
        #region 레이아웃
        private void Rayout_Setting()
        {
            this.main_dgv.Font = new Font("tahoma", 11);
            this.subVendor_dgv.Font = new Font("tahoma", 11);
        }
        #endregion
        #region DB 메서드

        private void UpdateDatabinding(SQLiteCommand cmd = null)
        {
            try
            {
                TextBox tb;
                foreach (Control c in Groupsub2.Controls)
                {
                    if (c.GetType() == typeof(TextBox))
                    {
                        tb = (TextBox)c;
                        tb.DataBindings.Clear();
                        tb.Text = "";
                    }
                    dbCommand = "SELECT";

                    sql = "SELECT * FROM Vendor ORDER BY vendorcode ASC;";
                    if (cmd == null)
                    {
                        command.CommandText = sql;
                    }
                    else
                    {
                        command = cmd;
                    }
                }
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                DataSet dataSt = new DataSet();
                adapter.Fill(dataSt, "Vendor");
                bindingSrc = new BindingSource();
                bindingSrc.DataSource = dataSt.Tables["Vendor"];

                //simple databinding

                subVendorCode_txt.DataBindings.Add("Text", bindingSrc, "vendorcode");
                subVendor_txt.DataBindings.Add("Text", bindingSrc, "vendorname");
                subVendorPerson_txt.DataBindings.Add("Text", bindingSrc, "vendorperson");
                subVendorPersonCellPhone_txt.DataBindings.Add("Text", bindingSrc, "vendorpersoncellphone");
                subVendorProduct_txt.DataBindings.Add("Text", bindingSrc, "vendorproduct");
                subVendorNote_txt.DataBindings.Add("Text", bindingSrc, "vendornote");

                subVendor_dgv.DataSource = bindingSrc;

                subVendor_dgv.AutoResizeColumns((DataGridViewAutoSizeColumnsMode)DataGridViewAutoSizeColumnsMode.AllCells);
                subVendor_dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                subVendor_dgv.Columns[0].Width = 70;//AutoID
                displayPosition();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Data Binding Error: " + ex.Message.ToString(), "Error Message : son", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addCmdParameters()
        {
            command.Parameters.Clear();
            command.CommandText = sql;

            command.Parameters.AddWithValue("vendorcode", subVendorCode_txt.Text.Trim());
            command.Parameters.AddWithValue("vendorname", subVendor_txt.Text.Trim());
            command.Parameters.AddWithValue("vendorperson", subVendorPerson_txt.Text.Trim());
            command.Parameters.AddWithValue("vendorpersoncellphone", subVendorPersonCellPhone_txt.Text.Trim());
            command.Parameters.AddWithValue("vendorproduct", subVendorProduct_txt.Text.Trim());
            command.Parameters.AddWithValue("vendornote", subVendorNote_txt.Text.Trim());

            //if (dbCommand.ToUpper() == "UPDATE")
            //{
            //    command.Parameters.AddWithValue("AutoID", AutoID_txt.Text.Trim());
            //}
        }
        private void Clear_AllTextbox(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                else
                {
                    Clear_AllTextbox(c);
                }
            }
        }
        #endregion
        #region 이벤트
        private void subVendorSave_btn_Click(object sender, EventArgs e)
        {
            //addCmdParameters();
            if (string.IsNullOrEmpty(subVendorCode_txt.Text.Trim()) ||
                string.IsNullOrEmpty(subVendor_txt.Text.Trim()))
            {
                MessageBox.Show("Please fill in the required fields.", "Add New Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }
            OpenConnection();
            try
            {
                if (subVendorAddNew_btn.Text == "Add New")
                {
                    if (subVendorCode_txt.Text.Trim() == "" ||
                        string.IsNullOrEmpty(subVendorCode_txt.Text.Trim()))
                    {
                        MessageBox.Show("Plese select an item.");
                        return;
                    }
                    if (MessageBox.Show("ID: " + subVendorCode_txt.Text.Trim() +
                        " -- Do you want to update the slected record?",
                        "Visual C# and SQLite (UPDATE)",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                    {
                        return;
                    }
                    dbCommand = "UPDATE";
                    sql = "UPDATE Vendor SET vendorcode = @vendorcode, vendorname = @vendorname, vendorperson = @vendorperson, ";
                    sql += "vendorpersoncellphone = @vendorpersoncellphone, vendorproduct=@vendorproduct, vendornote=@vendornote Where vendorcode = @vendorcode";
                    addCmdParameters();
                }
                else if (subVendorAddNew_btn.Text.Equals("Cancel"))
                {
                    DialogResult result;
                    result = MessageBox.Show("Do you want to add a new Vendor record?) (Y/N)",
                        "데이터 추가(INSERT) ",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        dbCommand = "INSERT";
                        sql = "INSERT INTO Vendor(vendorcode, vendorname, vendorperson, vendorpersoncellphone, vendorproduct, vendornote)" +
                            "VALUES(@vendorcode, @vendorname, @vendorperson, @vendorpersoncellphone, @vendorproduct, @vendornote)";

                        addCmdParameters();
                    }
                    else
                    {
                        return;
                    }
                }
                int executeResult = command.ExecuteNonQuery();
                if (executeResult == -1)
                {
                    MessageBox.Show("Data was not Saved", "Fail to save Data",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);

                }
                else
                {
                    MessageBox.Show("Your SQL " + dbCommand + "Query has been excuted succes");
                    UpdateDatabinding();
                    subVendorAddNew_btn.Text = "Add New";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString(), "Save Data : ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                dbCommand = "";
                CloseConnection();
            }
        }
        #region 이벤트
        private void moveFirst_btn_Click(object sender, EventArgs e)
        {
            bindingSrc.MoveFirst();
            displayPosition();
        }
        private void movePreview_btn_Click(object sender, EventArgs e)
        {
            bindingSrc.MovePrevious();
            displayPosition();
        }
        private void moveNext_btn_Click(object sender, EventArgs e)
        {
            bindingSrc.MoveNext();
            displayPosition();
        }
        private void moveLast_btn_Click(object sender, EventArgs e)
        {
            bindingSrc.MoveLast();
            displayPosition();
        }
        #endregion

        //데이터 지우기
        private void subVendorDelete_btn_Click(object sender, EventArgs e)
        {
            if (subVendorAddNew_btn.Text == "Cancel")
            {
                return;
            }
            if (subVendorCode_txt.Text.Trim() == "" ||
                string.IsNullOrEmpty(subVendorCode_txt.Text.Trim()))
            {
                MessageBox.Show("Please select an item from the list",
                    "Delete Data : ",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            OpenConnection();
            try
            {
                dbCommand = "DELETE";
                sql = "DELETE FROM Vendor WHERE vendorcode = @vendorcode";

                command.Parameters.Clear();
                command.CommandText = sql;
                command.Parameters.AddWithValue("vendorcode", subVendorCode_txt.Text.Trim());

                int executeResult = command.ExecuteNonQuery();
                if (executeResult == 1)
                {
                    UpdateDatabinding();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString());
            }
            finally
            {
                dbCommand = "";
                CloseConnection();
            }
        }
        private void subVendor_dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (subVendor_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    subVendor_dgv.CurrentRow.Selected = true;
                    subVendor_txt.Text = subVendor_dgv.Rows[e.RowIndex].Cells["subVendor_col"].FormattedValue.ToString();
                    subVendorCode_txt.Text = subVendor_dgv.Rows[e.RowIndex].Cells["subVendorCode_col"].FormattedValue.ToString();
                    subVendorPerson_txt.Text = subVendor_dgv.Rows[e.RowIndex].Cells["subVendorPerson_col"].FormattedValue.ToString();
                    subVendorPersonCellPhone_txt.Text = subVendor_dgv.Rows[e.RowIndex].Cells["subVendorPersonCellPhone_col"].FormattedValue.ToString();
                    subVendorProduct_txt.Text = subVendor_dgv.Rows[e.RowIndex].Cells["subVendorProduct_col"].FormattedValue.ToString();
                    subVendorNote_txt.Text = subVendor_dgv.Rows[e.RowIndex].Cells["subVendorNote_col"].FormattedValue.ToString();
                }
            }
            catch (Exception)
            {
                return;
            }

        }
        /// <summary>
        /// Setting Form 열기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm frm = new SettingsForm();
            frm.ShowDialog();
        }
        #endregion
        #region 메서드
        public void OpenConnection()
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Clone();
            }
        }
        private void NumberOnly(object sender, KeyPressEventArgs e)
        {
            int asciiCode = Convert.ToInt32(e.KeyChar);
            if ((asciiCode != 8))
            {
                if ((asciiCode >= 48 && asciiCode <= 57))
                {
                    e.Handled = false;
                }
                else
                {
                    MessageBox.Show("숫자만 입력하세요");
                    e.Handled = true;
                }
            }
        }
        #endregion

        private void subVendorAddNew_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (subVendorAddNew_btn.Text == "Add New")
                {
                    subVendorAddNew_btn.Text = "Cancel";
                    VendorPosition_lbl.Text = "Position: 0/0";
                    subVendor_dgv.ClearSelection();
                    subVendor_dgv.Enabled = false;
                }
                else
                {
                    subVendorAddNew_btn.Text = "Add New";
                    UpdateDatabinding();
                    return;
                }
                TextBox txt;
                foreach (Control c in Groupsub2.Controls)
                {
                    if (c.GetType() == typeof(TextBox))
                    {
                        txt = (TextBox)c;
                        txt.DataBindings.Clear();
                        txt.Text = "";
                        if (txt.Name.Equals("subVendorCode_txt"))
                        {
                            if (txt.CanFocus)
                            {
                                txt.Focus();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void subSearch_btn_Click(object sender, EventArgs e)
        {
            if (subVendorAddNew_btn.Text == "Cancel")
            {
                return;
            }

            OpenConnection();
            try
            {
                if (string.IsNullOrEmpty(subKeyword_txt.Text.Trim()))
                {
                    UpdateDatabinding();
                    return;
                }
                sql = "SELECT * FROM Vendor ";
                sql += "WHERE vendorcode LIKE @Keyword2 ";
                sql += "OR vendorname LIKE @Keyword2 ";
                sql += "OR vendorperson LIKE @Keyword2 ";
                sql += "OR vendorpersoncellphone = @Keyword1 ";
                sql += "OR vendorproduct LIKE @Keyword2 ";
                sql += "OR vendornote LIKE @Keyword2 ";
                sql += "ORDER BY vendorcode ASC";

                command.CommandType = CommandType.Text;
                command.CommandText = sql;
                command.Parameters.Clear();

                string keyword = string.Format("%{0}%", subKeyword_txt.Text);
                command.Parameters.AddWithValue("Keyword1", subKeyword_txt.Text);
                command.Parameters.AddWithValue("Keyword2", keyword);

                UpdateDatabinding(command);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Search Error: " + ex.Message.ToString(),
                    "Error Message : ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
                subKeyword_txt.Focus();
            }
        }
    }
}
