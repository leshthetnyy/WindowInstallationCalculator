using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        // переменные цен за квадратный метр для различных элементов
        double WindowPriceMeter = 2000.00;
        double BalconyPriceMeter = 1500.00;
        double DoorPriceMeter = 1800.00;
        double TotalCost = 0;

        public Form1()
        {
            InitializeComponent();
            comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; // запрет ввода в comboBox
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // ввод только цифр и одной запятой в textBox
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true; 
            }
            TextBox textBox = sender as TextBox;
            if (e.KeyChar == ',')
            {
                if (textBox.Text.Contains(","))
                {
                    e.Handled = true; 
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // проверка: выбрана ли услгуа
            if (comboBox1.Text != "")
            {
                // проверка: введены ли значения
                if (textBox1.Text != "" && textBox1.Text != "")
                {
                   
                        // берем значения, которые ввел пользователь
                        double WidthMeter = Convert.ToDouble(textBox1.Text);
                        double HeightMeter = Convert.ToDouble(textBox2.Text);

                        // проверка: ввыбран ли элемент "окно"
                        if (comboBox1.SelectedIndex == 0)
                        {
                            // проверка: ввыбран ли тип окна
                            if (radioButton1.Checked || radioButton2.Checked || radioButton3.Checked || radioButton4.Checked || radioButton5.Checked)
                            {
                                double CostTypeWindow = 0; // данная переменная будет хранить, стоимость за тип окна

                                // проверка: какой выбран тип окна
                                if (radioButton1.Checked)
                                {
                                    CostTypeWindow = 1000.00; // глухое
                                }
                                else if (radioButton2.Checked)
                                {
                                    CostTypeWindow = 3400.50; // поворотное
                                }
                                else if (radioButton3.Checked)
                                {
                                    CostTypeWindow = 2560.00; // откидное
                                }
                                else if (radioButton4.Checked)
                                {
                                    CostTypeWindow = 7900.90; // фрамужное
                                }
                                else if (radioButton5.Checked)
                                {
                                    CostTypeWindow = 6210.50; // раздвижное
                                }

                                TotalCost = ((WidthMeter * HeightMeter) * WindowPriceMeter) + CostTypeWindow;
                                label6.Text = "Итого за окно: " + Convert.ToString(TotalCost) + " рублей.";

                            }
                            else
                            {
                                MessageBox.Show("Не выбран тип окна!\nВыберите тип окна!");
                            }
                        }

                        if (comboBox1.SelectedIndex == 1)
                        {
                            TotalCost = ((WidthMeter * HeightMeter) * BalconyPriceMeter);
                            label6.Text = "Итого за балкон: " + Convert.ToString(TotalCost) + " рублей.";

                        }
                        if (comboBox1.SelectedIndex == 2)
                        {
                            TotalCost = ((WidthMeter * HeightMeter) * DoorPriceMeter);
                            label6.Text = "Итого за дверь: " + Convert.ToString(TotalCost) + " рублей.";
                        }
                        }         
                else
                {
                    MessageBox.Show("Значения не введены!\nВведите все значеня!");
                }
            }
            else
            {
                MessageBox.Show("Услуга не выбрана!\nВыберите услугу!");
            }

            // если все верно, то можно сформировать квитанцию
            if (comboBox1.Text != "" && label6.Text != "" && textBox1.Text != "" && textBox2.Text != "")
            {
                button2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // вывод панели выбора типа окна
            if (comboBox1.SelectedIndex == 0)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // создаем объект приложения Word
            Word.Application app = new Word.Application();

            // создаем новый документ на основе шаблона
            Word.Document doc = app.Documents.Add(@"C:\Users\anton\Desktop\Project\WindowsFormsApp1\WindowsFormsApp1\bin\Debug\Квитанции\ШаблонКвитанции.docx");

            string UniqueNumber = Guid.NewGuid().ToString(); // уникальный номер
            string CurrentDate = DateTime.Now.ToString("ddMMyyyy"); // дата (только цифры для названия документа)
            string CurrentDateTry = DateTime.Now.ToString("dd.MM.yyyy"); // дата (для отображения в документе)

            // Вставляем данные в определенные поля в документе
            doc.Bookmarks["Закладка_1"].Range.Text = UniqueNumber;
            doc.Bookmarks["Закладка_2"].Range.Text = Convert.ToString(CurrentDateTry);
            doc.Bookmarks["Закладка_3"].Range.Text = Convert.ToString(comboBox1.Text);
            doc.Bookmarks["Закладка_4"].Range.Text = Convert.ToString(TotalCost) + " рублей";

            //Сохраняем документ с заданным именем
            string NewNameDoc = UniqueNumber + "_" + CurrentDate + "_" + Convert.ToString(TotalCost)+"рублей";
            string NameDoc = NewNameDoc + ".docx";
            object PathDoc = @"C:\Users\anton\Desktop\Project\WindowsFormsApp1\WindowsFormsApp1\bin\Debug\Квитанции\" + NameDoc;
            doc.SaveAs2(PathDoc);
            string PathDocString = Convert.ToString(PathDoc);
            app.Documents.Open(PathDocString);
        }
    }
}
