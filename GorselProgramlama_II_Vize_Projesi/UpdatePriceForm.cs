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
    public partial class UpdatePriceForm : Form
    {
        public static string foodsXmlPath = Application.StartupPath + @"\urunler.xml";
        public UpdatePriceForm()
        {
            InitializeComponent();
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(foodBox.Text))
            {
                MessageBox.Show("Lütfen bir ürün seçiniz!");
                return;
            }

            int newPrice;
            if (!int.TryParse(newPriceBox.Text, out newPrice))
            {
                MessageBox.Show("Lütfen geçerli bir sayı giriniz!");
                return;
            }


            XmlDocument documentFoods = new XmlDocument();
            documentFoods.Load(foodsXmlPath);

            bool updated = false;
            foreach (var child in documentFoods.GetElementsByTagName("foods")[0].ChildNodes)
            {
                XmlElement element = (XmlElement)child;
                string name = element.GetElementsByTagName("name")[0].InnerText;
                if (name == foodBox.Text)
                {
                    updated = true;
                    element.GetElementsByTagName("price")[0].InnerText = newPrice.ToString();
                    break;
                }
            }
            if (updated)
            {
                documentFoods.Save(foodsXmlPath);
                MessageBox.Show("Ürün güncellendi!");
            } else
            {
                MessageBox.Show("Ürün bulunamadı!");
            }
        }

        private void UpdatePriceForm_Load(object sender, EventArgs e)
        {
            XmlDocument documentFoods = new XmlDocument();
            documentFoods.Load(foodsXmlPath);

            foreach (var child in documentFoods.GetElementsByTagName("foods")[0].ChildNodes)
            {
                XmlElement element = (XmlElement)child;
                string name = element.GetElementsByTagName("name")[0].InnerText;
                foodBox.Items.Add(name);
            }
        }

        private void foodBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            XmlDocument documentFoods = new XmlDocument();
            documentFoods.Load(foodsXmlPath);

            foreach (var child in documentFoods.GetElementsByTagName("foods")[0].ChildNodes)
            {
                XmlElement element = (XmlElement)child;
                string name = element.GetElementsByTagName("name")[0].InnerText;
                string price = element.GetElementsByTagName("price")[0].InnerText;
                if (name == foodBox.SelectedItem.ToString())
                {
                    newPriceBox.Text = price;
                    break;
                }
            }
        }
    }
}
