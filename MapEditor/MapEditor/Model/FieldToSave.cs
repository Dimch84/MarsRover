using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Model
{
    public class FieldToSave
    {
        public int n;
        public int m;
        public int[,] map;

        public FieldToSave() { }

        public FieldToSave(int n, int m, ObservableCollection<Tile> Tiles)
        {
            this.n = n;
            this.m = m;
            map = new int[n,m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    map[i, j] = Tiles[i * n + j].Id;
                }
            }
        }
    }
}
