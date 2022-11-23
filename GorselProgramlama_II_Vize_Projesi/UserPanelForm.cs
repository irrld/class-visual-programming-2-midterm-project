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
using System.Drawing.Printing;

namespace GorselProgramlama_II_Vize_Projesi
{
    public partial class UserPanelForm : Form
    {
        public static string foodsXmlPath = Application.StartupPath + @"\urunler.xml";
        public UserPanelForm()
        {
            InitializeComponent();
        }
        class Food
        {
            public string name { get; set; }
            public int price { get; set; }
            public int listIndex { get; set; }
        }
        class Item
        {
            public Food food { get; set; }
            public int amount { get; set; }
        }

        private List<Food> foods = new List<Food>();
        private List<Item> items = new List<Item>();

        private void UserPanelForm_Load(object sender, EventArgs e)
        {
            // Tüm ürünleri yüklüyoruz, bunlar foods listesine eklenecek ve daha sonra buradan kontrol edilecek
            XmlDocument documentFoods = new XmlDocument();
            documentFoods.Load(foodsXmlPath);

            int index = 0;
            foreach (var child in documentFoods.GetElementsByTagName("foods")[0].ChildNodes)
            {
                XmlElement element = (XmlElement)child;
                string name = element.GetElementsByTagName("name")[0].InnerText;
                int price = int.Parse(element.GetElementsByTagName("price")[0].InnerText);
                foodBox.Items.Add(name + " (" + price.ToString("C") + ")");

                var food = new Food();
                food.name = name;
                food.price = price;
                food.listIndex = index;

                foods.Add(food);
                index++;
            }
            CalculateOutput();
        }

        private void CalculateOutput()
        {
            // RichTextBox'u temizliyoruz
            richTextBox1.ResetText();
            richTextBox1.Clear();

            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("ÜRÜNLER\n");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
            int sum = 0;
            foreach (var item in items)
            {
                int price = item.food.price * item.amount;
                richTextBox1.AppendText(item.food.name + " (" + item.amount + " adet): " + price.ToString("C") + "\n");
                sum += price;
            }
            richTextBox1.AppendText("\n\n\n\n");
            richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
            richTextBox1.AppendText("TOPLAM FİYAT: " + sum.ToString("C"));
        }

        private void addFoodButton_Click(object sender, EventArgs e)
        {
            // Ön kontroller
            if (string.IsNullOrEmpty(foodBox.SelectedItem.ToString()))
            {
                MessageBox.Show("Lütfen bir ürün seçiniz!");
                return;
            }
            if (amountBox.Value <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir miktar seçiniz!");
                return;
            }

            // Önce listede zaten olup olmadığına bakıyoruz
            Item foundItem = null;
            foreach (var item in items)
            {
                if (item.food.listIndex == foodBox.SelectedIndex)
                {
                    foundItem = item;
                    break;
                }
            }
      
            if (foundItem != null) // Eğer bu ürün zaten ekliyse
            {
                // Ürünün miktarına seçili miktarı ekliyoruz, miktarı arttırmış oluyoruz kısacası
                foundItem.amount += (int)amountBox.Value;
            } else // Ürün ekli değilse
            {
                // Dosyadan okuyoruz
                XmlDocument documentFoods = new XmlDocument();
                documentFoods.Load(foodsXmlPath);

                // Tüm food elemanlarını kontrol ediyoruz
                foreach (var food in foods)
                {
                    if (food.listIndex == foodBox.SelectedIndex)  // Seçili ürünü bulduysak
                    {
                        // Ürünler listesine bu ürünü ekliyoruz
                        Item item = new Item();
                        item.food = food;
                        item.amount = (int) amountBox.Value;
                        items.Add(item);
                        break;
                    }
                }
            }
            amountBox.Value = 1; // Miktar kısmını sıfırlıyoruz
            CalculateOutput(); // RichTextBox'u güncelliyoruz
        }

        private void UserPanelForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void removeFoodButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(foodBox.SelectedItem.ToString()))
            {
                MessageBox.Show("Lütfen bir ürün seçiniz!");
                return;
            }
            if (amountBox.Value <= 0)
            {
                MessageBox.Show("Lütfen geçerli bir miktar seçiniz!");
                return;
            }
            bool found = false;
            foreach(var item in items)
            {
                if (item.food.listIndex == foodBox.SelectedIndex) {
                    item.amount -= (int) amountBox.Value;
                    if (item.amount <= 0)
                    {
                        items.Remove(item);
                    }
                    found = true;
                    break;
                }
            }
            if (!found)
            {

                MessageBox.Show("Bu ürün zaten ekli değil!");
            } else
            {
                CalculateOutput();
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            // Temizle butonu
            items.Clear(); // Eklenen ürünleri temizliyoruz
            CalculateOutput(); // RichTextBox'u güncelliyoruz
        }

        // Yazdırma işlemleri
        private void printButton_Click(object sender, EventArgs e)
        {
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        int linesPrinted = 0;
        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int x = e.MarginBounds.Left;
            int y = e.MarginBounds.Top;
            Brush brush = new SolidBrush(richTextBox1.ForeColor);

            string[] lines = richTextBox1.Text.Split('\n');
            int i = 0;
            foreach (string line in lines)
            {
                lines[i++] = line.TrimEnd('\r');
            }

            while (linesPrinted < lines.Length)
            {
                e.Graphics.DrawString(lines[linesPrinted++],
                    richTextBox1.Font, brush, x, y);
                y += 15;
                if (y >= e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }
            e.HasMorePages = false;
        }
    }
}
