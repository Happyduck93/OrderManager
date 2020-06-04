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
    public partial class Customer : UserControl
    {




        private void UpdateData(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
           

            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand("dbo.SP_UPDATE_CUTOMER_INFO", connection))
                {
                    //stored procedure
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerId", dr["CUSTOMER_ID"].ToString());
                    cmd.Parameters.AddWithValue("@name", dr["NAME"].ToString());
                    cmd.Parameters.AddWithValue("@address", dr["ADDRESS"].ToString());
                    cmd.Parameters.AddWithValue("@phone", dr["PHONE"].ToString());
                    cmd.Parameters.AddWithValue("@birthDate", dr["BIRTH_DATE"].ToString());
                    cmd.Parameters.AddWithValue("@point", dr["POINT"]);
                    

                    cmd.ExecuteNonQuery();
                }
            }

            connection.Close();
        }


        private void InsertData(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
            

            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand("dbo.SP_INSERT_CUTOMER_INFO", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerId", dr["CUSTOMER_ID"].ToString());
                    cmd.Parameters.AddWithValue("@productId", dr["PRODUCT_ID"].ToString());
                    cmd.Parameters.AddWithValue("@orderDate", dr["ORDER_DATE"].ToString());
                    cmd.Parameters.AddWithValue("@quantity", dr["QUANTITY"].ToString());
                    cmd.Parameters.AddWithValue("@totalPrice", dr["TOTAL_PRICE"].ToString());
                    cmd.Parameters.AddWithValue("@payType", dr["PAY_TYPE"]);
                    cmd.Parameters.AddWithValue("@orderID", dr["ORDER_ID"].ToString());

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
           

            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand("dbo.SP_DELETE_CUTOMER_INFO", connection))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@customerID", dr["CUSTOMER_ID", DataRowVersion.Original].ToString());

                    cmd.ExecuteNonQuery();
                }
            }

            connection.Close();


        }


        private void SearchData()
        {


            //접속
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);

          

            //명령 (param <- "procedure",connection)
            SqlCommand cmd = new SqlCommand("dbo.SP_SEARCH_CUTOMER_INFO", connection);

            //deault => text ||지정 => stored procedure 
            cmd.CommandType = CommandType.StoredProcedure;
            connection.Open();  

            DataTable dataSource = new DataTable();

            // object is valid only insde using brackets
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(dataSource);

            }

            connection.Close();

            grdCustomer.DataSource = dataSource;



        }







        public Customer()
        {
            InitializeComponent();
        }






        private void btnDelete_Click(object sender, EventArgs e)
        {
            for (int rowIndex = grdCustomer.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                //string
                var selected = grdCustomer.Rows[rowIndex].Cells["SELECT"].Value;
                if (selected != null && selected.Equals("true")) // string == "true"
                {
                    ((DataTable)grdCustomer.DataSource).Rows[rowIndex].Delete();
                }

            }
        }





        private void btnAdd_Click(object sender, EventArgs e)
        {
            //(DataTable)grdOrder.DateSource
            (grdCustomer.DataSource as DataTable).Rows.Add();


            //change the last row color
            grdCustomer.Rows[grdCustomer.RowCount - 1].DefaultCellStyle.BackColor = Color.Yellow;

        }







        private void btnSave_Click(object sender, EventArgs e)
        {

            //alt + shift to eddit multiple lines

            DataTable source = grdCustomer.DataSource as DataTable;
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
