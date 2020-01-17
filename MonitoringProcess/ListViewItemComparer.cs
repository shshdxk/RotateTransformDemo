using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace MonitoringProcess
{
    class ListViewItemComparer:IComparer
    {
        private bool sort_b;
        Type _t = typeof(string);

        private int col;

        public ListViewItemComparer()
        {
            col = 0;
        }

        public ListViewItemComparer(int column, bool sort, Type t)
        {
            col = column;
            sort_b = sort;
            _t = t;
        }

        public int Compare(object x, object y)
        {
            if (_t == typeof(string))
            {
                if (sort_b)
                {
                    return String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);
                }
                else
                {
                    return String.Compare(((ListViewItem)y).SubItems[col].Text, ((ListViewItem)x).SubItems[col].Text);
                }
            }
            else if (_t == typeof(int))
            {
                if (sort_b)
                {
                    return Int32.Parse(((ListViewItem)x).SubItems[col].Text) - Int32.Parse(((ListViewItem)y).SubItems[col].Text);
                }
                else
                {
                    return Int32.Parse(((ListViewItem)y).SubItems[col].Text) - Int32.Parse(((ListViewItem)x).SubItems[col].Text);
                }
            }
            else {
                return 0;
            }
            
        }
    }
}
