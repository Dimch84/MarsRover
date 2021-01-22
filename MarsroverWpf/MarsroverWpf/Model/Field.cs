using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MarsroverWpf.Model
{
	public class Field : INotifyPropertyChanged
	{
		private int n;
		private int m;
		public ObservableCollection<Tile> Tiles { get; set; } = new ObservableCollection<Tile>();

		private Dictionary<string, int> multipliers = new Dictionary<string, int>()
		{
			{ "UP", 1 },
			{ "DOWN", 2 },
			{ "LEFT", 3 },
			{ "RIGHT", 4 },
			{ "UPUP", 5 },
			{ "DOWNDOWN", 5 },
			{ "LEFTLEFT", 6 },
			{ "RIGHTRIGHT", 6 },
			{ "UPLEFT", 7 },
			{ "RIGHTDOWN", 7 },
			{ "UPRIGHT", 8 },
			{ "LEFTDOWN", 8 },
			{ "DOWNLEFT", 9 },
			{ "RIGHTUP", 9 },
			{ "DOWNRIGHT", 10 },
			{ "LEFTUP", 10 }
		};

		public Field(int m, int n)
		{
			this.n = n;
			this.m = m;
			for (int i = 0; i < n * m; i++)
			{
				Tiles.Add(new Tile { Id = 1 });
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
					Tiles.Add(new Tile { Id = loadedField.data[i, j] });
				}
			}
		}

		public void ApplyPath(string pathJson, int startX, int startY)
        {
			resetIds();
            PathResponse pathResponse = JsonConvert.DeserializeObject<PathResponse>(pathJson);
			string lastCmd = "";
            foreach (string cmd in pathResponse.cmds)
            {
                switch (cmd)
                {
                    case "RIGHT":
                        Tiles[startY * N + startX].Id += multipliers[lastCmd + "RIGHT"] * 6;
						lastCmd = "RIGHT";
                        startX += 1;
                        break;
                    case "LEFT":
                        Tiles[startY * N + startX].Id += multipliers[lastCmd + "LEFT"] * 6;
						lastCmd = "LEFT";
						startX -= 1;
                        break;
                    case "UP":
                        Tiles[startY * N + startX].Id += multipliers[lastCmd + "UP"] * 6;
						lastCmd = "UP";
						startY -= 1;
                        break;
                    case "DOWN":
                        Tiles[startY * N + startX].Id += multipliers[lastCmd + "DOWN"] * 6;
						lastCmd = "DOWN";
						startY += 1;
                        break;
                }
            }
			switch (lastCmd)
			{
				case "RIGHT":
					Tiles[startY * N + startX].Id += multipliers["LEFT"] * 6;
					break;
				case "LEFT":
					Tiles[startY * N + startX].Id += multipliers["RIGHT"] * 6;
					break;
				case "UP":
					Tiles[startY * N + startX].Id += multipliers["DOWN"] * 6;
					break;
				case "DOWN":
					Tiles[startY * N + startX].Id += multipliers["UP"] * 6;
					break;
			}
		}

		private void resetIds()
        {
			foreach(Tile t in Tiles)
            {
				t.Id %= 6;
            }
        }

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
