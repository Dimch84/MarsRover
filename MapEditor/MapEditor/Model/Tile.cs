using MapEditor.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MapEditor.Model
{
	public class Tile : INotifyPropertyChanged
	{
		private int id;
		private ImageBrush texture;
		private string textureName;
		public static int SelectedId = 1;

		private static Dictionary<int, ImageBrush> textures = new Dictionary<int, ImageBrush>()
		{
			{1, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Rocks.png")))},
			{2, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Fall.png")))},
			{3, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Sand.png")))},
			{4, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSand.png")))},
			{5, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Ground.png")))},
			{6, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGround.png")))}
		};

		private static Dictionary<int, string> textureNames = new Dictionary<int, string>()
		{
			{1, "Rocks" },
			{2, "Fall" },
			{3, "Sand" },
			{4, "Shadowed Sand" },
			{5, "Ground" },
			{6, "Shadowed Ground" }
		};


		public int Id
		{
			get => id;
			set
			{
				if (!textures.ContainsKey(value))
					throw new ArgumentException("Wrong tile id");
				id = value;
				Texture = textures[id];
				TextureName = textureNames[id];
				OnPropertyChanged("Id");
			}
		}
		public ImageBrush Texture
		{
			get => texture;
			set
			{
				texture = value;
				OnPropertyChanged("Texture");
			}
		}

		public string TextureName
		{
			get => textureName;
			set
			{
				textureName = value;
				OnPropertyChanged("TextureName");
			}
		}

		private RelayCommand changeCommand;
		public RelayCommand ChangeCommand
		{
			get
			{
				return changeCommand ??
					   (changeCommand = new RelayCommand(obj =>
					   {
							if (Mouse.LeftButton == MouseButtonState.Pressed && texture != textures[SelectedId])
							{
							   Id = SelectedId;
							}
					   }));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
