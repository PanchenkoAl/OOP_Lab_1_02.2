using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Lab1_v._02
{
    internal class CreateNewElement
    {
        const int b = 65;
        const int c = 26;
        char letter = 'A';
        char firstLetter = 'A';
        int firstLetterN = 1;
        string temp;
        int r;
        public void AddColumn(DataGridView dgv)
        {
            DataGridViewColumn newColumn = (DataGridViewColumn)dgv.Columns[0].Clone();
            dgv.Columns.Add(newColumn);
            string sHeader = null;
            if(dgv.ColumnCount <= 26)
            {
                sHeader += (char)(64 + dgv.ColumnCount);
            }
            else
            {
                int mod = dgv.ColumnCount / 26;
                int ost = dgv.ColumnCount % 26;
                
                sHeader += (char)(mod + 64);
                sHeader += (char)(ost + 65);
            }
            dgv.Columns[dgv.ColumnCount - 1].HeaderCell.Value = sHeader;
        }



        public int AddRow(DataGridView dgv)
        {
            DataGridViewRow newRow = (DataGridViewRow)dgv.Rows[0].Clone();
            dgv.Rows.Add(newRow);
            dgv.Rows[dgv.RowCount - 1].HeaderCell.Value = (dgv.RowCount - 1).ToString();
            return (0);
        }
    }
}

