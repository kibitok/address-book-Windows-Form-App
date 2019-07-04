using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Address_Book
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.Font = new System.Drawing.Font("Helvetica", 9.75F,
              System.Drawing.FontStyle.Italic,
              System.Drawing.GraphicsUnit.Point,
              ((System.Byte)(0)));
            this.ForeColor = System.Drawing.Color.Blue;

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void TextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            try
            {
            panel1.Enabled = true;
            app.PhoneBook.AddPhoneBookRow(app.PhoneBook.NewPhoneBookRow());
            phoneBookBindingSource.MoveLast();
            txtPhoneNumber.Focus();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                app.PhoneBook.RejectChanges();
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            txtPhoneNumber.Focus();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            phoneBookBindingSource.ResetBindings(false);
            panel1.Enabled = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

            
            phoneBookBindingSource.EndEdit();
            app.PhoneBook.AcceptChanges();
            app.PhoneBook.WriteXml(string.Format("{0}//data.dat", Application.StartupPath));
            panel1.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                app.PhoneBook.RejectChanges();
            }
        }
        static Data db;
        protected static Data app
        {
            get
            {
                if (db == null)
                    db = new Data();
                return db;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string fileName = string.Format("{0}//data.dat", Application.StartupPath);
            if (File.Exists(fileName))
                app.PhoneBook.ReadXml(fileName);
            phoneBookBindingSource.DataSource = app.PhoneBook;
            panel1.Enabled = false;
        }

        private void DataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (MessageBox.Show("Are you sure you want to delete this record?","Message",
                    MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
                    phoneBookBindingSource.RemoveCurrent();
            }
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyValue== (char)13)
            {
            if (!string.IsNullOrEmpty(txtSearch.Text))
            {
                var query = from o in app.PhoneBook
                            where o.PhoneNumber == txtSearch.Text || o.Name.Contains(txtSearch.Text) || o.Email == txtSearch.Text
                            select o;
                dataGridView.DataSource = query.ToList();
            }
            else
            {
                dataGridView.DataSource = phoneBookBindingSource;
            }

            }
        }
    }
}
