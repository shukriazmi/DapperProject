using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;

namespace DapperProject
{
    public partial class FormContact : Form
    {
        SqlConnection sqlCon = new SqlConnection(@"Data Source=localhost\SQLEXPRESS;initial Catalog=ContactDB;Integrated Security= True;");
        int contactId = 0;

        public FormContact()
        {
            InitializeComponent();
        }

        private void FormContact_Load(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled)) //to validate textBoxName
            {
                try
                {
                    if (sqlCon.State == ConnectionState.Closed)
                        sqlCon.Open();
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@ContactID", contactId);
                    param.Add("@Name", textBoxName.Text.Trim());
                    param.Add("@PhoneNo", textBoxPhoneNo.Text.Trim());
                    param.Add("@Address", textBoxAddress.Text.Trim());

                    sqlCon.Execute("ContactAddOrEdit", param, commandType: CommandType.StoredProcedure);
                    if (contactId == 0)
                        MessageBox.Show("Saved Successfully", "Contact Form");
                    else
                        MessageBox.Show("Updated Successfully", "Contact Form");
                    FillDataGridView();
                    Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {

                    sqlCon.Close();
                }                
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@ContactID", contactId);
                sqlCon.Execute("ContactDeleteByID", param, commandType: CommandType.StoredProcedure);
                Clear();
                FillDataGridView();
                MessageBox.Show("Deleted Successfully", "Contact Form");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void dataGridViewContact_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewContact.CurrentRow.Index != -1)
                {
                    contactId = Convert.ToInt32(dataGridViewContact.CurrentRow.Cells[0].Value.ToString());
                    textBoxName.Text = dataGridViewContact.CurrentRow.Cells[1].Value.ToString();
                    textBoxPhoneNo.Text = dataGridViewContact.CurrentRow.Cells[2].Value.ToString();
                    textBoxAddress.Text = dataGridViewContact.CurrentRow.Cells[3].Value.ToString();
                    buttonDelete.Enabled = true;
                    buttonSave.Text = "Edit";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            try
            {
                FillDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void FillDataGridView()
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@SearchText", textBoxSearch.Text.Trim());

            List<Contact> list = sqlCon.Query<Contact>
                ("ContactViewAllOrSearch", param, commandType: CommandType.StoredProcedure).ToList<Contact>();

            dataGridViewContact.DataSource = list;
            dataGridViewContact.Columns[0].Visible = false;
        }

        void Clear()
        {
            textBoxName.Text = textBoxPhoneNo.Text = textBoxAddress.Text = "";
            contactId = 0;
            buttonSave.Text = "Save";
            buttonDelete.Enabled = false;
        }

        private void textBoxName_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                e.Cancel = true;
                textBoxName.Focus();
                errorProviderContact.SetError(textBoxName, "Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProviderContact.SetError(textBoxName, "");
            }
        }

        private void FormContact_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Determine if text in the textbox not null.
            if (!string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                // Display a MsgBox asking the user to save changes or abort.
                if (MessageBox.Show("Do you want to save changes to your Contact?", "Contact Form",
                   MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    // Cancel the Closing event from closing the form.
                    e.Cancel = true;
                    // Call method to save file
                    buttonSave_Click(textBoxName.Text,e);
                }
                else
                {
                    //Closing the form.
                    e.Cancel = false;
                }
            }
            else
            {
                //Closing the form.
                e.Cancel = false;
            }
        }
    }
}
