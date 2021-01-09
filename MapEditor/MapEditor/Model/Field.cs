using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MapEditor.Model
{
	public class Field : INotifyPropertyChanged
	{
		private int n;
		private int m;
		public ObservableCollection<Tile> Tiles { get; set; } = new ObservableCollection<Tile>();

		public Field(int m, int n)
        {
			this.n = n;
			this.m = m;
			for (int i = 0; i < n; i++)
            {
				for (int j = 0; j < m; j++)
				{
					Tiles.Add(new Tile { Id = 1 });
				}
			}
        }

		public int N
		{
			get => n;
			set
			{
				if (value < 2 || value > 100)
					throw new ArgumentException("Введите число от 2 до 100");
				else
				{
					n = value;
					OnPropertyChanged("N");
				}
			}
		}

		public int M
		{
			get => m;
			set
			{
				if (value < 2 || value > 100)
					throw new ArgumentException("Введите число от 2 до 100");
				else
				{
					m = value;
					OnPropertyChanged("M");
				}
			}
		}

		public string SaveString()
        {
			FieldToSave field = new FieldToSave(n, m, Tiles);
			return JsonConvert.SerializeObject(field);
        }

		public void LoadFromString(string s)
        {
			FieldToSave loadedField = JsonConvert.DeserializeObject<FieldToSave>(s);
			N = loadedField.n;
			M = loadedField.m;
			Tiles.Clear();
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < m; j++)
				{
					Tiles.Add(new Tile { Id = loadedField.map[i, j] });
				}
			}
		}

        public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
