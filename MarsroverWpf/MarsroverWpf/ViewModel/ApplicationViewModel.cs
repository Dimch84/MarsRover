using MarsroverWpf.Model;
using MarsroverWpf.ViewModel.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MarsroverWpf.ViewModel
{
    public class ApplicationViewModel
    {
        private Field field = new Field(5, 5);
        private string serviceUrl;
        private int mapId = 0;
        private int startX = 0;
        private int startY = 0;
        private int finishX = 0;
        private int finishY = 0;
        private string pathString = "path";

        public string ServiceUrl
        {
            get => serviceUrl;
            set
            {
                serviceUrl = value;
                OnPropertyChanged("ServiceUrl");
            }
        }
        public int MapId
        {
            get => mapId;
            set
            {
                mapId = value;
                OnPropertyChanged("MapId");
            }
        }
        public int StartX
        {
            get => startX;
            set
            {
                startX = value;
                OnPropertyChanged("StartX");
            }
        }
        public int StartY
        {
            get => startY;
            set
            {
                startY = value;
                OnPropertyChanged("StartY");
            }
        }
        public int FinishX
        {
            get => finishX;
            set
            {
                finishX = value;
                OnPropertyChanged("FinishX");
            }
        }
        public int FinishY
        {
            get => finishY;
            set
            {
                finishY = value;
                OnPropertyChanged("FinishY");
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

        public string PathString
        {
            get => pathString;
            set
            {
                pathString = value;
                OnPropertyChanged("PathString");
            }
        }

        #region Commands

        private RelayCommand loadMapCommand;
        public RelayCommand LoadMapCommand
        {
            get
            {
                return loadMapCommand ??
                       (loadMapCommand = new RelayCommand(obj =>
                       {
                           HttpClient client = new HttpClient();
                           try
                           {
                               client.BaseAddress = new Uri(serviceUrl);
                               client.DefaultRequestHeaders.Accept.Clear();
                               client.DefaultRequestHeaders.Accept.Add(
                                   new MediaTypeWithQualityHeaderValue("application/json"));
                           }
                           catch (Exception e)
                           {
                               MessageBox.Show("Error!");
                           }
                           try
                           {
                               var response = client.GetStringAsync(serviceUrl + "/map/" + mapId.ToString());
                               string fieldString = response.Result;
                               field.LoadFromString(fieldString);
                               OnPropertyChanged("Field");
                           }
                           catch (Exception e)
                           {
                               MessageBox.Show("Error!");
                           }
                       }));
            }
        }

        private RelayCommand findPathCommand;
        public RelayCommand FindPathCommand
        {
            get
            {
                return findPathCommand ??
                       (findPathCommand = new RelayCommand(obj =>
                       {
                           var request = WebRequest.Create(serviceUrl + "/path");

                           request.ContentType = "application/json";
                           request.Method = "GET";

                           var type = request.GetType();
                           var currentMethod = type.GetProperty("CurrentMethod", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(request);

                           var methodType = currentMethod.GetType();
                           methodType.GetField("ContentBodyNotAllowed", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(currentMethod, false);

                           PathRequest pathRequest = new PathRequest(mapId, startX, startY, finishX, finishY);

                           using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                           {
                               streamWriter.Write(JsonConvert.SerializeObject(pathRequest));
                           }
                           HttpWebResponse response = null;
                           try
                           {
                               response = (HttpWebResponse)request.GetResponse();
                           }
                           catch (Exception e)
                           {
                               MessageBox.Show("Error occured during sending path request");
                               return;
                           }
                           var encoding = ASCIIEncoding.ASCII;
                           string responseText = "";
                           using (var reader = new StreamReader(response.GetResponseStream(), encoding))
                           {
                               responseText = reader.ReadToEnd();
                           }
                           field.ApplyPath(responseText, startX, startY);

                       }));
            }
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
