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
    public partial class Order : UserControl
    {
        public Order()
        {
            InitializeComponent();
        }



        private void SearchData()
        {


            //connection
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);


            //string query = "select * from TB_ORDER_INFO";

            //command
            SqlCommand cmd = new SqlCommand("dbo.SP_SEARCH_ORDER_INFO", connection);
            cmd.CommandType = CommandType.StoredProcedure;

            connection.Open();

            // DataTable dataSource = new DataTable();
            var table = new DataTable();
            // object is valid only insde using brackets.
            using (var da = new SqlDataAdapter(cmd))
            {
                da.Fill(table);

            }

            connection.Close();

            grdOrder.DataSource = table;


        }






        private void UpdateData(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
            string query = "UPDATE TB_ORDER_INFO" +
                           "   SET CUSTOMER_ID       = @customerId" +
                           "      ,PRODUCT_ID        = @productId" +
                           "      ,ORDER_DATE        = @orderDate" +
                           "      ,QUANTITY          = @quantity" +
                           "      ,TOTAL_PRICE       = @totalPrice" +
                           "      ,PAY_TYPE          = @payType" +
                           " WHERE ORDER_ID    = @orderId";

            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

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


        private void InsertData(DataTable dt)
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
       

           
            SqlCommand cmd = new SqlCommand("dbo.SP_INSERT_ORDER_INFO", connection)
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add(new SqlParameter("@InsertTable", dt));

            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();

        }
        private void DeleteData(DataTable dt)
        {
            //HOMEWORK.. 
            // cmd.Parameters.AddWithValue("@productID", dr["PRODUCT_ID", DataRowVersion.Original].ToString());

            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DemoDatabase"].ConnectionString);
            string query = @"DELETE FROM TB_ORDER_INFO
                                WHERE ORDER_ID =@orderID";


            connection.Open();
            foreach (DataRow dr in dt.Rows)
            {
                //use this object only insde using bracket
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {

                    cmd.Parameters.AddWithValue("@orderID", dr["ORDER_ID", DataRowVersion.Original].ToString());

                    cmd.ExecuteNonQuery();
                }
            }

            connection.Close();


        }







        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData();
        }
        private void grdOrder_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //(DataTable)grdOrder.DateSource
            (grdOrder.DataSource as DataTable).Rows.Add();


            //change the last row color
            grdOrder.Rows[grdOrder.RowCount - 1].DefaultCellStyle.BackColor = Color.Yellow;

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            for (int rowIndex = grdOrder.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                //string-(true/false)
                var selected = grdOrder.Rows[rowIndex].Cells["SELECT"].Value;
                if (selected != null && selected.Equals("true")) // string == "true"
                {
                    ((DataTable)grdOrder.DataSource).Rows[rowIndex].Delete();
                }

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //alt + shift to eddit multiple lines

            DataTable source = grdOrder.DataSource as DataTable;
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

    }
}
