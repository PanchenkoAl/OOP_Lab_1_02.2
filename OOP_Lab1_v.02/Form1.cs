using System.Text;

namespace OOP_Lab1_v._02
{
    public partial class Form1 : Form
    {

        const int border = 200;
        public Cell[,] table = new Cell[border,border];
        public int _COLUMNS = 0;
        public int _ROWS = 0;
        
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < border; i++)
                for (int j = 0; j < border; j++)
                    table[i, j] = new Cell();

        }

        private void DataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Update the balance column whenever the value of any cell changes.
            string temp = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            addNewValue(temp, dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _COLUMNS = 5; _ROWS = 5;
            Cell[,] table = new Cell[border,border];
            char c = 'A';
            string temp = null;
            for (int i = 0; i < _COLUMNS; i++)
            {
                temp += c;
                dataGridView1.Columns.Add(Name, temp);
                c++;
                temp = null;
            }
            for (int i = 0; i < _ROWS; i++)
            {
                dataGridView1.Rows.Add();
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            }
            dataGridView1.AllowUserToAddRows = false;
        }

        public void addNewValue(string exp, int row, int col)
        {
            
            table[row, col].cValue = exp;

            Parser parser = new Parser();
            string rr;
            rr = parser.ParseExpression(exp, table, row, col);
            

            dataGridView1[col, row].Value = rr;
            UpdateCells();
        }

        public void UpdateCells()
        {
            for(int i = 0; i < dataGridView1.RowCount; i ++)
            {
                for(int j = 0; j < dataGridView1.ColumnCount; j ++)
                {
                    Parser parser = new Parser();
                    if (table[i, j].cValue != null)
                    {
                        string rr = parser.ParseExpression(table[i, j].cValue, table,i,j);
                        dataGridView1[j, i].Value = rr;
                    }
                }
            }
        }


        private void button7_Click(object sender, EventArgs e)
        {
            string temp = (string)textBox1.Text;
            addNewValue(temp, dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);   
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        public void SaveData()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Текстовый документ (*.txt)|*.txt|Все файлы (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.Unicode);
                try
                {
                    List<int> col_n = new List<int>();
                    foreach (DataGridViewColumn col in dataGridView1.Columns)
                        if (col.Visible)
                        {
                            //sw.Write(col.HeaderText + "\t");
                            col_n.Add(col.Index);
                        }
                    //sw.WriteLine();
                    int x = dataGridView1.RowCount;
                    if (dataGridView1.AllowUserToAddRows) x--;

                    for (int i = 0; i < x; i++)
                    {
                        for (int y = 0; y < col_n.Count; y++)
                        {
                            if (y != col_n.Count - 1)
                            {
                                if (table[i, col_n[y]].cValue != null)
                                    sw.Write(table[i, col_n[y]].cValue + " ");
                                else
                                    sw.Write("%" + " ");
                            }
                            else
                            {
                                if (table[i, col_n[y]].cValue != null)
                                    sw.Write(table[i, col_n[y]].cValue);
                                else
                                    sw.Write("%");
                            }
                        }
                        sw.Write("\n");
                    }
                    sw.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CreateNewElement newElement = new CreateNewElement();
            newElement.AddColumn(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CreateNewElement newElement = new CreateNewElement();
            newElement.AddRow(dataGridView1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.RowCount > 2)
            {
                for (int i = 0; i < dataGridView1.ColumnCount; i++)
                {
                    addNewValue("0", dataGridView1.RowCount - 1, i);
                }
                //dataGridView1.Refresh();
                dataGridView1.Rows.RemoveAt(dataGridView1.RowCount - 1);
            }
            else
                MessageBox.Show("Unable to delete last two rows");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool columnIsEmpty = true;
            int columns = dataGridView1.ColumnCount;
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (dataGridView1.Rows[i].Cells[columns - 1].Value != null)
                {
                    columnIsEmpty = false;
                }
            }

            if (dataGridView1.Columns.Count <= 2)
            {

                return;
            }
            if (!columnIsEmpty)
            {
                MessageBox.Show("Column is not Empty");
                return;
            }
            else
            {
                for (int i = 0; i < dataGridView1.RowCount; i++)
                {
                    //addNewValue("0", i, columns - 1);
                }
                dataGridView1.Columns.RemoveAt(columns - 1);
                columns--;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                CreateNewElement NewElement = new CreateNewElement();
                StreamReader rd = new StreamReader(openFileDialog.FileName);
                string header = rd.ReadLine();

                int i = 0;
                while (header != null)
                {
                    string[] col = System.Text.RegularExpressions.Regex.Split(header, " ");
                    if (col.Length > dataGridView1.ColumnCount)
                    {
                        while (col.Length > dataGridView1.ColumnCount)
                        {
                            NewElement.AddColumn(dataGridView1);
                        }
                    }
                    for (int j = 0; j < col.Length; j++)
                    {
                        if(col[j] != "%")
                            addNewValue(col[j], i, j);
                    }
                    header = rd.ReadLine();
                    i++;
                    if (i >= dataGridView1.RowCount && header != null)
                    {
                        NewElement.AddRow(dataGridView1);
                    }
                }
                return;
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox1.Text = table[dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex].cValue;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string temp = (string)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (temp != null)
                addNewValue(temp, dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string temp = (string)textBox1.Text;
                if (temp != null)
                    addNewValue(temp, dataGridView1.CurrentCell.RowIndex, dataGridView1.CurrentCell.ColumnIndex);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure want to clear table?", "Before Clearing", MessageBoxButtons.YesNo);
            if(result == DialogResult.Yes)
            for(int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                for(int j = 0; j < dataGridView1.Rows.Count; j ++)
                {
                    table[i, j].cValue = null;
                    dataGridView1[i, j].Value = null;
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to save table before quitting?", "Quit App", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                SaveData();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Лабораторна робота 1. Автор - студент 2го курсу, групи К-25 Панченко Олександр Вадимович." + '\n' + "Для введення адреси клітинок використовуйте літери верхнього регістру та цифри." + '\n' + "Підтримуються операції степеня, множення, ділення, суми, різниці, інкременту, декременту, унарного мінусу (^, *, /, +, -, inc(), dec()), -(Xn)");
        }
    }
}