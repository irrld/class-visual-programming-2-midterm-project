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
    public partial class AddUserForm : Form
    {
        public static string usersXmlPath = Application.StartupPath + @"\users.xml";
        public AddUserForm()
        {
            InitializeComponent();
        }

        private void addUserButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(usernameBox.Text))
            {
                MessageBox.Show("Kullanıcı adı boş geçilemez!");
                return;
            }
            if (string.IsNullOrEmpty(passwordBox.Text))
            {
                MessageBox.Show("Şifre boş geçilemez!");
                return;
            }
            if (passwordBox.Text != passwordConfirmBox.Text)
            {
                MessageBox.Show("Şifreler eşleşmiyor!");
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
                if (usernameBox.Text == foundUsername)
                {
                    // böyle bir kullanıcı var, geri dön
                    userFound = true;
                    break;
                }
            }
            if (userFound)
            {
                MessageBox.Show("Böyle bir kullanıcı zaten bulunuyor!");
                return;
            }
            // Useri oluşturuyoruz
            var user = documentUsers.CreateElement("user");
            // Username ve passwordu oluşturuyoruz
            var username = documentUsers.CreateElement("username");
            var password = documentUsers.CreateElement("password");
            // Elementlerin içerlerine bilgilerini giriyoruz
            username.InnerText = usernameBox.Text;
            password.InnerText = passwordBox.Text;

            // Username ve password'u userin içine ekliyoruz
            user.AppendChild(username);
            user.AppendChild(password);

            // User'i users'e ekliyoruz
            users.AppendChild(user);
            // Kaydediyoruz
            documentUsers.Save(usersXmlPath);
            MessageBox.Show("Kullanıcı başarıyla eklendi!");
        }
    }
}
