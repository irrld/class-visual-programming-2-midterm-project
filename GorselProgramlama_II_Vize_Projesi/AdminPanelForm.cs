using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GorselProgramlama_II_Vize_Projesi
{
    public partial class AdminPanelForm : Form
    {
        public AdminPanelForm()
        {
            InitializeComponent();
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            AddUserForm form = new AddUserForm();
            form.ShowDialog();
        }

        private void signOutButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            new LoginForm().Show();
        }

        private void AdminPanelForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void deleteUserButton_Click(object sender, EventArgs e)
        {
            RemoveUserForm form = new RemoveUserForm();
            form.ShowDialog();
        }

        private void updateFoodButton_Click(object sender, EventArgs e)
        {
            UpdatePriceForm form = new UpdatePriceForm();
            form.ShowDialog();
        }
    }
}
