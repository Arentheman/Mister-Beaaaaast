using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Задание_9
{
    public partial class Form1 : Form
    {
        int imgNum = 12;
        int currec = 0, Rec = 0;
        int tickcount = 0;
        private List<int> usedNumbers = new List<int>();
        private List<int> ShowedImages = new List<int>();
        public string Nickname = "Гость";
        enum Names
        {
            банан,
            киви,
            яблоко,
            апельсин,
            кресло,
            ключ,
            бита,
            весы,
            топор,
            дерево,
            машина,
            ноутбук,
            календарь,
            текст
        }
        public Form1()
        {
            InitializeComponent();
            LoadDataFromFile();
            comboBox1.SelectedIndex = 0;
        }
        private void LoadDataFromFile()
        {
            if (File.Exists("dataE.txt") && File.Exists("dataM.txt") && File.Exists("dataH.txt"))
            {
                string[] Easy = File.ReadAllLines("dataE.txt");
                string[] Medium = File.ReadAllLines("dataM.txt");
                string[] Hard = File.ReadAllLines("dataH.txt");
                foreach (string line in Easy)
                {
                    string[] parts = line.Split(',');
                    dataGridView1.Rows.Add(parts);
                }
                foreach (string line in Medium)
                {
                    string[] parts = line.Split(',');
                    dataGridView2.Rows.Add(parts);
                }
                foreach (string line in Hard)
                {
                    string[] parts = line.Split(',');
                    dataGridView3.Rows.Add(parts);
                }
            }
        }
        bool but1 = false;
        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            button1.Enabled = false;
            button2.Enabled = false;
            timer1.Enabled = true;
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    {
                        imgNum = 13;
                        timer1.Interval = 1500;
                    }
                    break;
                case 1:
                    {
                        imgNum = 14;
                        timer1.Interval = 1000;
                    }
                    break;
                case 2:
                    {
                        imgNum = 15;
                        timer1.Interval = 500;
                    }
                    break;
            }
            usedNumbers.Clear();
            but1 = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> Words = new List<string>();
            List<string> Answer = new List<string>();
            string difficulty = comboBox1.SelectedItem.ToString();
            string filepath = "dataE.txt";
            if (but1 == false || richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("Перед тем как ответить, сначала запустите игру, а потом заполните поле");
            }
            foreach (var item in ShowedImages)
            {
                Words.Add((Names)item - 1+"");
            }
            char[] delimiterChars = { '\n', '\r', ' ', ',', '.', ':', '!', '?', '(', ')', '[', ']', '{', '}', '@', '"', '#', '№', '$', '%', '^', '&', '*', '-', '_', '+', '=', '`', '~', '|', '/', '\\', ';', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', '\'', '<', '>' };
            string[] WrittenWords = richTextBox1.Text.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
            foreach (string recordline in WrittenWords)
            {
                Answer.Add(recordline.Split('\t')[0].Trim());
            }
            currec = ScoreCounting(Words, Answer);
            Rec = Convert.ToInt32(label7.Text);
            if (currec > Rec)
                label7.Text = currec.ToString();
            label8.Text = currec.ToString();
            if (label4.Text != "Гость")
            {
                DataGridViewRow existingRow = null;
                switch (difficulty)
                {
                    case "Лёгкая":
                        existingRow = dataGridView1.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(row =>row.Cells[0].Value != null && row.Cells[0].Value.ToString() == label4.Text);
                        break;
                    case "Средняя":
                        filepath = "dataMtxt";
                        existingRow = dataGridView2.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(row => row.Cells[0].Value != null && row.Cells[0].Value.ToString() == label4.Text);
                        break;
                    case "Сложная":
                        filepath = "dataH.txt";
                        existingRow = dataGridView3.Rows
                .Cast<DataGridViewRow>()
                .FirstOrDefault(row => row.Cells[0].Value != null && row.Cells[0].Value.ToString() == label4.Text);
                        break;
                }
                
                if (existingRow != null)
                {
                    int existingScore = int.Parse(existingRow.Cells[1].Value.ToString());
                    if (currec > existingScore)
                    {
                        // Обновляем рекорд пользователя
                        existingRow.Cells[1].Value = currec;
                        UpdateRecordInFile(label4.Text, currec, filepath);
                    }
                }
                else
                {
                    // Добавляем новую строку в DataGridView и записываем данные в файл
                    string data = label4.Text + "," + currec;
                    dataGridView1.Rows.Add(data.Split(','));

                    using (StreamWriter sw = File.AppendText(filepath))
                    {
                        sw.WriteLine(data);
                    }
                }
            }
        }
        private void UpdateRecordInFile(string name, int score, string filepath)
        {
            List<string> lines = File.ReadAllLines(filepath).ToList();

            for (int i = 0; i < lines.Count; i++)
            {
                string[] parts = lines[i].Split(',');
                if (parts.Length >= 2 && parts[0] == name)
                {
                    lines[i] = name + "," + score.ToString();
                    break;
                }
            }
            File.WriteAllLines(filepath, lines);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Form2 autorization = new Form2();
            autorization.NameEntered += Form2_NameEntered;
            autorization.ShowDialog();
            string playerName = label4.Text;

            // Находим соответствующий рекорд во втором столбце DataGridView
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == playerName)
                {
                    label7.Text = row.Cells[1].Value.ToString();
                    break;
                }
            }
        }
        private void Form2_NameEntered(object sender, NameEnteredEventArgs e)
        {
            label4.Text = e.Name;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tickcount++;
            Random random = new Random();

            int x;
            do
            {
                x = random.Next(1, imgNum);
            } while (usedNumbers.Contains(x));

            usedNumbers.Add(x);

            pictureBox1.ImageLocation = string.Format("..\\..\\..\\images\\img{0}.jpg", tickcount == imgNum ? 15:x);

            if (usedNumbers.Count == imgNum-1)
            {
                ShowedImages.Clear();
                if (ShowedImages.Count == 0)
                {
                    ShowedImages.AddRange(usedNumbers.ToArray());
                }
                usedNumbers.Clear();
            }
            if (tickcount == imgNum)
            {
                tickcount = 0;
                comboBox1.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                timer1.Stop();
            }
        }
        public int ScoreCounting(List<string> list1, List<string> list2)
        {
            int res = 0;
            foreach (var item in list2)
            {
                if (list2.IndexOf(item) != list2.LastIndexOf(item))
                {
                    MessageBox.Show("Слова не должны совпадать!"); // Если элементы в list2 повторяются, возвращаем текущий результат res
                    return res;
                }
            }
            if (list1.Count != list2.Count)
            {
                while (list2.Count < list1.Count)
                {
                    list2.Add("smth");
                }

                if (list2.Count > list1.Count)
                {
                    MessageBox.Show("Cлов введено больше чем показано!"); // Если список Answer стал больше списка Words, возвращаем 0
                }
            }
            for (int i = 0; i < list1.Count; i++)
            {
                if (list1[i] == list2[i])
                {
                    res += 2; // Если элементы совпадают, добавляем 2 к результату
                }
                else if (list1.Contains(list2[i]))
                {
                    res += 1; // Если элемент из list2 присутствует в list1, добавляем 1 к результату
                }
            }
            return res;
        }
    }
}
