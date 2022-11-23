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
    public partial class RemoveUserForm : Form
    {
        public static char KEY_ENTER = (char)13;
        public static string usersXmlPath = Application.StartupPath + @"\users.xml";
        public RemoveUserForm()
        {
            InitializeComponent();
        }

        private void removeUserButton_Click(object sender, EventArgs e)
        {
            RemoveUser(usernameBox.Text);
        }

        private void RemoveUser(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Kullanıcı adı boş geçilemez!");
                return;
            }
            XmlDocument documentUsers = new XmlDocument();
            documentUsers.Load(usersXmlPath);

            bool userFound = false;
            var users = documentUsers.GetElementsByTagName("users")[0];
            foreach (var child in users.ChildNodes)
            {
                XmlElement element = (XmlElement)child;
                var foundUsername = element.GetElementsByTagName("username")[0].InnerText;
                if (username == foundUsername)
                {
                    users.RemoveChild(element);
                    userFound = true;
                    break;
                }
            }
            if (!userFound)
            {
                MessageBox.Show("Böyle bir kullanıcı bulunamadı!");
                return;
            }
            // Kaydediyoruz
            documentUsers.Save(usersXmlPath);
            MessageBox.Show("Kullanıcı başarıyla silindi!");
        }

        private void usernameBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == KEY_ENTER)
            {
                e.Handled = true;
                RemoveUser(usernameBox.Text);
            }
        }
    }
}
