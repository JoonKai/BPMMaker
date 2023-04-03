using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPMMaker
{
    public partial class Form1 : Form
    {
        //데이터 베이스 경로
        string myDB = "MyDB.db";
        string myDBPath = @"URI=file:" + Application.StartupPath + "\\MyDB.db";

        SQLiteConnection sqlCon;
        SQLiteCommand sqlCmd;
        SQLiteDataReader sqlReader;
        public Form1()
        {
            InitializeComponent();
            Create_DB();
            Rayout_Setting();
            Data_Show();
        }
        #region 레이아웃
        private void Rayout_Setting()
        {
            this.main_dgv.Font = new Font("tahoma", 9);
            this.subVendor_dgv.Font = new Font("tahoma", 9);
        }

        #endregion
        #region 메서드
        //데이터베이스 파일 및 목록 생성
        private void Create_DB()
        {
            if (!System.IO.File.Exists(myDB))
            {
                SQLiteConnection.CreateFile(myDB);
                using (var sqlite = new SQLiteConnection(@"Data Source=" + myDB))
                {
                    sqlite.Open();
                    string sql = "create table Vendor(" +
                        "vendornumber varchar(20)," +
                        "vendorname varchar(12)," +
                        "vendorcode varchar(12)," +
                        "vendorperson varchar(12)," +
                        "vendorpersoncellphone varchar(12)," +
                        "vendorproduct varchar(12)," +
                        "vendornote varchar(12))";
                    SQLiteCommand command = new SQLiteCommand(sql, sqlite);
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                Console.WriteLine("Database cannot crate");
                return;
            }
        }
        //데이터 불러오기
        private void Data_Show()
        {
            var con = new SQLiteConnection(myDBPath);
            con.Open();

            string stm = "SELECT * FROM Vendor";
            var cmd = new SQLiteCommand(stm, con);
            sqlReader = cmd.ExecuteReader();
            while (sqlReader.Read())
            {
                subVendor_dgv.Rows.Insert(0,
                    sqlReader.GetString(0), 
                    sqlReader.GetString(1),
                    sqlReader.GetString(2),
                    sqlReader.GetString(3),
                    sqlReader.GetString(4),
                    sqlReader.GetString(5),
                    sqlReader.GetString(6));
            }
        }
        //텍스트 박스 모두 지우기
        private void Clear_AllTextbox(Control con)
        {
            foreach (Control c in con.Controls)
            {
                if(c is TextBox)
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
            var con = new SQLiteConnection(myDBPath);
            con.Open();
            var cmd = new SQLiteCommand(con);

            try
            {
                cmd.CommandText = "INSERT INTO " +
                    "Vendor(vendornumber,vendorname,vendorcode,vendorperson,vendorpersoncellphone,vendorproduct,vendornote) " +
                    "VALUES(@vendornumber, @vendorname, @vendorcode, @vendorperson, @vendorpersoncellphone,@vendorproduct, @vendornote)";

                string VENDORNO = "";
                string VENDORNAME = subVendor_txt.Text;
                string VENDORCODE = subVendorCode_txt.Text;
                string VENDORPERSON = subVendorPerson_txt.Text;
                string VENDORPERSONCELLPHONE = subVendorPersonCellPhone_txt.Text;
                string VENDORPRODUCT = subVendorProduct_txt.Text;
                string VENDORNOTE = subVendorNote_txt.Text;

                cmd.Parameters.AddWithValue("@vendornumber", VENDORNO);
                cmd.Parameters.AddWithValue("@vendorname", VENDORNAME);
                cmd.Parameters.AddWithValue("@vendorcode", VENDORCODE);
                cmd.Parameters.AddWithValue("@vendorperson", VENDORPERSON);
                cmd.Parameters.AddWithValue("@vendorpersoncellphone", VENDORPERSONCELLPHONE);
                cmd.Parameters.AddWithValue("@vendorproduct", VENDORPRODUCT);
                cmd.Parameters.AddWithValue("@vendornote", VENDORNOTE);
                //subVendor_dgv.ColumnCount = 6;
                //subVendor_dgv.Columns[0].Name = "업체명";
                //subVendor_dgv.Columns[1].Name = "업체코드";
                //subVendor_dgv.Columns[2].Name = "업체담당자";
                //subVendor_dgv.Columns[3].Name = "연락처";
                //subVendor_dgv.Columns[4].Name = "업체취급품목";
                //subVendor_dgv.Columns[5].Name = "기타";

                string[] row = new string[] { VENDORNO, VENDORNAME, VENDORCODE, VENDORPERSON, VENDORPERSONCELLPHONE,VENDORPRODUCT, VENDORNOTE };
                subVendor_dgv.Rows.Add(row);

                cmd.ExecuteNonQuery();

            }
            catch (Exception)
            {
                Console.WriteLine("데이터를 입력하지 못하였습니다.");
                return;
            }
        }

        

        private void subVendorUpdate_btn_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(myDBPath);
            con.Open();

            var cmd = new SQLiteCommand(con);
            try
            {
                //cmd.CommandText = "UPDATE Vendor Set id=@Id where name = @Name";
                //cmd.Prepare();
                //cmd.Parameters.AddWithValue("@vendornumber", subNo_col.);
                //cmd.Parameters.AddWithValue("@vendorname", VENDORNAME);
                //cmd.Parameters.AddWithValue("@vendorcode", VENDORCODE);
                //cmd.Parameters.AddWithValue("@vendorperson", VENDORPERSON);
                //cmd.Parameters.AddWithValue("@vendorpersoncellphone", VENDORPERSONCELLPHONE);
                //cmd.Parameters.AddWithValue("@vendorproduct", VENDORPRODUCT);
                //cmd.Parameters.AddWithValue("@vendornote", VENDORNOTE);

                cmd.ExecuteNonQuery();
                subVendor_dgv.Rows.Clear();
                Data_Show();

            }
            catch (Exception)
            {
                Console.WriteLine("cannot update data");
                return;
            }
        }
        //데이터 지우기
        private void subVendorDelete_btn_Click(object sender, EventArgs e)
        {
            var con = new SQLiteConnection(myDBPath);
            con.Open();
            var cmd = new SQLiteCommand(con);

            try
            {
                cmd.CommandText = "DELETE FROM Vendor where VENDORNAME = @vendorname";
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@vendorname", subVendor_txt.Text);
                cmd.ExecuteNonQuery();
                subVendor_dgv.Rows.Clear();
                Data_Show();
            }
            catch (Exception)
            {
                Console.WriteLine("cannot delete data");
                return;
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
        #endregion

        private void NumberOnly(object sender, KeyPressEventArgs e)
        {
            int asciiCode = Convert.ToInt32(e.KeyChar);
            if((asciiCode != 8))
            {
                if((asciiCode >=48 && asciiCode <= 57))
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
    }
}
