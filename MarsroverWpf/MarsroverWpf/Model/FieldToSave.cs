using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsroverWpf.Model
{
    public class FieldToSave
    {
        public int n;
        public int m;
        public int[,] data;

        public FieldToSave() { }

        public FieldToSave(int n, int m, ObservableCollection<Tile> Tiles)
        {
            this.n = n;
            this.m = m;
            data = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    data[i, j] = Tiles[i * n + j].Id;
                }
            }
        }
    }
}
