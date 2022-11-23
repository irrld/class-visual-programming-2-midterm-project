using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace GorselProgramlama_II_Vize_Projesi
{
    public partial class LoginForm : Form
    {
        public static char KEY_ENTER = (char)13;
        public static string adminXmlPath = Application.StartupPath + @"\admin.xml";
        public static string usersXmlPath = Application.StartupPath + @"\users.xml";

        public LoginForm()
        {
            InitializeComponent();
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            DoLogin(usernameBox.Text, passwordBox.Text);
        }

        private void DoLogin(string username, string password)
        {
            XmlDocument documentAdmin = new XmlDocument();
            documentAdmin.Load(adminXmlPath);

            string adminUsername = documentAdmin.GetElementsByTagName("username")[0].InnerText;
            if (username == adminUsername)
            {
                string adminPassword = documentAdmin.GetElementsByTagName("password")[0].InnerText;
                if (password == adminPassword)
                {
                    this.Hide();
                    AdminPanelForm panel = new AdminPanelForm();
                    panel.Show();
                } else
                {
                    MessageBox.Show("Kullanıcı bilgileri eşleşmedi!");
                }
            } else
            {
                XmlDocument documentUsers = new XmlDocument();
                documentUsers.Load(usersXmlPath);

                foreach (var child in documentUsers.GetElementsByTagName("users")[0].ChildNodes) {
                    XmlElement element = (XmlElement)child;
                    var foundUsername = element.GetElementsByTagName("username")[0].InnerText;
                    if (username == foundUsername)
                    {
                        var foundPassword = element.GetElementsByTagName("password")[0].InnerText;
                        if (password == foundPassword)
                        {
                            this.Hide();
                            UserPanelForm panel = new UserPanelForm();
                            panel.Show();
                            return;
                        }
                    }
                }
                MessageBox.Show("Kullanıcı bilgileri eşleşmedi!");
            }
        }

        private void usernameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter tuşuna basıldığında şifre yazma kısmına geçiş
            if (e.KeyChar == KEY_ENTER)
            {
                passwordBox.Focus();
                e.Handled = true;
            }
        }

        private void passwordBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enter tuşuna basıldığında giriş yapma
            if (e.KeyChar == KEY_ENTER)
            {
                DoLogin(usernameBox.Text, passwordBox.Text);
                e.Handled = true;
            }
        }
    }
}
