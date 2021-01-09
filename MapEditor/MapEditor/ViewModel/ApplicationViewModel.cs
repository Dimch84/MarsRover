using MapEditor.Model;
using MapEditor.ViewModel.Commands;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Media;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using System.IO;

namespace MapEditor.ViewModel
{
	public class ApplicationViewModel : INotifyPropertyChanged
	{
		private Tile selectedTile;
		private int n = 5;
		private int m = 5;
        private Field field;

		public ObservableCollection<Tile> Tiles{ get; set; }

        #region Commands

        private RelayCommand setSizeCommand;
        public RelayCommand SetSizeCommand
        {
            get
            {
                return setSizeCommand ??
                       (setSizeCommand = new RelayCommand(obj =>
                       {
                           if (n > 1 && n < 101 && m > 1 && m < 101)
                           {
                               Field = new Field(n, m);
                               OnPropertyChanged("N");
                               OnPropertyChanged("M");
                           }
                           else
                           {
                               MessageBox.Show("Field size should be larger than 1 and less than 101");
                           }
                       }));
            }
        }

        //// команда удаления
        //private RelayCommand removeCommand;
        //public RelayCommand RemoveCommand
        //{
        //	get
        //	{
        //		return removeCommand ??
        //			   (removeCommand = new RelayCommand(obj =>
        //			   {
        //				   Phone phone = obj as Phone;
        //				   if (phone != null)
        //				   {
        //					   Phones.Remove(phone);
        //				   }
        //			   },
        //				   (obj) => Phones.Count > 0));
        //	}
        //}

        private RelayCommand saveCommand;
        public RelayCommand SaveCommand
        {
            get
            {
                return saveCommand ??
                       (saveCommand = new RelayCommand(obj =>
                       {
                           SaveFileDialog saveFileDialog = new SaveFileDialog();
                           saveFileDialog.Filter = "JSON (*.json)|*.json";
                           if (saveFileDialog.ShowDialog() == true)
                               File.WriteAllText(saveFileDialog.FileName, Field.SaveString());
                       }));
            }
        }

        private RelayCommand loadCommand;
        public RelayCommand LoadCommand
        {
            get
            {
                return loadCommand ??
                       (loadCommand = new RelayCommand(obj =>
                       {
                           OpenFileDialog openFileDialog = new OpenFileDialog();
                           openFileDialog.Filter = "JSON (*.json)|*.json";
                           if (openFileDialog.ShowDialog() == true)
                           {
                               string loadedString = File.ReadAllText(openFileDialog.FileName);
                               Field.LoadFromString(loadedString);
                               n = Field.N;
                               m = Field.M;
                               OnPropertyChanged("N");
                               OnPropertyChanged("M");
                           }
                       }));
            }
        }

        #endregion

        public Tile SelectedTile
		{
			get => selectedTile;
			set
			{
				selectedTile = value;
                Tile.SelectedId = value.Id;
				OnPropertyChanged("SelectedTile");
			}
		}
        public int N
        {
            get => n;
            set
            {
                n = value;
            }
        }

        public int M
        {
            get => m;
            set
            {
                m = value;
            }
        }

        public Field Field
        {
            get => field;
            set
            {
                field = value;
                OnPropertyChanged("Field");
            }
        }

        public ApplicationViewModel()
		{
			Tiles = new ObservableCollection<Tile>
			{
				new Tile { Id = 1 },
				new Tile { Id = 2 },
				new Tile { Id = 3 },
				new Tile { Id = 4 },
				new Tile { Id = 5 },
				new Tile { Id = 6 },
			};
            field = new Field(n, m);
		}


		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
