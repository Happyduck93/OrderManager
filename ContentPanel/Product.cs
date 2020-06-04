using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace OrderManager.ContentPanel
{
    public partial class Product : UserControl
    {
        public Product()
        {
            InitializeComponent();
        }



        private void defaultSetting()
        {
            //접속
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
            
            //명령(왔다갔다 하는 사람)
            SqlCommand cmd = new SqlCommand("DBO.SP_SEARCH_CODE_PRODUCT_TYPE", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();

            // 비쥬얼 스튜디오에서 보여지는 테이블 (데이타베이스에서 가져온걸 복붙)
            DataTable comboData = new DataTable();

            // object is valid only insde using brackets
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(comboData);

            }

            connection.Close();

            (grdProduct.Columns["CATEGORY"] as DataGridViewComboBoxColumn).DataSource = comboData;
            (grdProduct.Columns["CATEGORY"] as DataGridViewComboBoxColumn).DisplayMember = "CODE_DESCR";
            (grdProduct.Columns["CATEGORY"] as DataGridViewComboBoxColumn).ValueMember = "CODE_ID";




        }



        private void InsertData(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
          

            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand("DBO.SP_INSERT_PRODUCT_INFO", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productName", dr["PRODUCT_NAME"].ToString());
                    cmd.Parameters.AddWithValue("@color", dr["COLOR"].ToString());
                    cmd.Parameters.AddWithValue("@category", dr["CATEGORY"].ToString());
                    cmd.Parameters.AddWithValue("@arrivalDate", dr["ARRIVAL_DATE"].ToString());
                    cmd.Parameters.AddWithValue("@origin", dr["ORIGIN"].ToString());
                    cmd.Parameters.AddWithValue("@price", dr["PRICE"]);
                    cmd.Parameters.AddWithValue("@stock", dr["STOCK"]);
                    cmd.Parameters.AddWithValue("@productId", dr["PRODUCT_ID"].ToString());

                    cmd.ExecuteNonQuery();
                }
            }

            connection.Close();
        }
        private void DeleteData(DataTable dt)
        {
            //HOMEWORK.. 
            // cmd.Parameters.AddWithValue("@productID", dr["PRODUCT_ID", DataRowVersion.Original].ToString());

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
            string query = @"DELETE FROM TB_PRODUCT_INFO
                                WHERE PRODUCT_ID =@productID";


            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@productID", dr["PRODUCT_ID", DataRowVersion.Original].ToString());

                    cmd.ExecuteNonQuery();
                }
            }

            connection.Close();


        }



        private void UpdateData(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
            

            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand("DBO.SP_UPDATE_PRODUCT_INFO", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productName", dr["PRODUCT_NAME"].ToString());
                    cmd.Parameters.AddWithValue("@color", dr["COLOR"].ToString());
                    cmd.Parameters.AddWithValue("@category", dr["CATEGORY"].ToString());
                    cmd.Parameters.AddWithValue("@arrivalDate", dr["ARRIVAL_DATE"].ToString());
                    cmd.Parameters.AddWithValue("@origin", dr["ORIGIN"].ToString());
                    cmd.Parameters.AddWithValue("@price", dr["PRICE"]);
                    cmd.Parameters.AddWithValue("@stock", dr["STOCK"]);
                    cmd.Parameters.AddWithValue("@productId", dr["PRODUCT_ID"].ToString());

                    cmd.ExecuteNonQuery();
                }
            }

            connection.Close();
        }


        

        private void SearchData()
        {

            //접속
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);

        

            //명령
            SqlCommand cmd = new SqlCommand("DBO.SP_SEARCH_PRODUCT_INFO", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();

            DataTable dataSource = new DataTable();

            // object is valid only insde using brackets
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(dataSource);

            }

            connection.Close();

            grdProduct.DataSource = dataSource;

        }





































        private void fpnlSearch_Paint(object sender, PaintEventArgs e)
        {

        }

        private void grdProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Product_Load(object sender, EventArgs e)
        {
            defaultSetting();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //(DataTable)grdProduct.DateSource
            (grdProduct.DataSource as DataTable).Rows.Add();


            //change the last row color
            grdProduct.Rows[grdProduct.RowCount - 1].DefaultCellStyle.BackColor = Color.Yellow;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int rowIndex = grdProduct.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                //string
                var selected = grdProduct.Rows[rowIndex].Cells["SELECT"].Value;
                if (selected != null && selected.Equals("true")) // string == "true"
                {
                    ((DataTable)grdProduct.DataSource).Rows[rowIndex].Delete();
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //alt + shift 

            DataTable source = grdProduct.DataSource as DataTable;
            //source.GetChanges();
            DataTable updateDt = source.GetChanges(DataRowState.Modified);
            DataTable insertDt = source.GetChanges(DataRowState.Added);
            DataTable deleteDt = source.GetChanges(DataRowState.Deleted);

            //updateDt != null
            if (updateDt != null && updateDt.Rows.Count > 0)
            {
                UpdateData(updateDt);
            }
            if (insertDt != null && insertDt.Rows.Count > 0)
            {
                InsertData(insertDt);
            }
            if (deleteDt != null && deleteDt.Rows.Count > 0)
            {
                DeleteData(deleteDt);
            }
            SearchData();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }
    }
}
