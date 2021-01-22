using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MarsroverWpf.Model
{
	public class Tile : INotifyPropertyChanged
	{
		private int id;
		private ImageBrush texture;
		public static int SelectedId = 1;

		private static Dictionary<int, ImageBrush> textures = new Dictionary<int, ImageBrush>()
		{
			{1, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Rocks.png")))},
			{2, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Fall.png")))},
			{3, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Sand.png")))},
			{4, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSand.png")))},
			{5, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/Ground.png")))},
			{6, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGround.png")))},
			{9, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandUp.png")))},
			{10, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandUp.png")))},
			{11, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundUp.png")))},
			{12, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundUp.png")))},
			{15, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandDown.png")))},
			{16, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandDown.png")))},
			{17, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundDown.png")))},
			{18, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundDown.png")))},
			{21, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandLeft.png")))},
			{22, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandLeft.png")))},
			{23, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundLeft.png")))},
			{24, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundLeft.png")))},
			{27, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandRight.png")))},
			{28, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandRight.png")))},
			{29, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundRight.png")))},
			{30, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundRight.png")))},
			{33, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandUpDown.png")))},
			{34, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandUpDown.png")))},
			{35, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundUpDown.png")))},
			{36, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundUpDown.png")))},
			{39, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandLeftRight.png")))},
			{40, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandLeftRight.png")))},
			{41, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundLeftRight.png")))},
			{42, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundLeftRight.png")))},
			{45, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandUpLeft.png")))},
			{46, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandUpLeft.png")))},
			{47, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundUpLeft.png")))},
			{48, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundUpLeft.png")))},
			{51, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandUpRight.png")))},
			{52, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandUpRight.png")))},
			{53, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundUpRight.png")))},
			{54, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundUpRight.png")))},
			{57, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandDownLeft.png")))},
			{58, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandDownLeft.png")))},
			{59, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundDownLeft.png")))},
			{60, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundDownLeft.png")))},
			{63, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/SandDownRight.png")))},
			{64, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedSandDownRight.png")))},
			{65, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/GroundDownRight.png")))},
			{66, new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Images/ShadowedGroundDownRight.png")))}
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

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged([CallerMemberName] string prop = "")
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
		}
	}
}
