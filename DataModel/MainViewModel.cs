using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MalukahSongs.DataModel

{
    [Windows.Foundation.Metadata.WebHostHidden]
    public class MainViewModel : MalukahSongs.Common.BindableBase
    {
        Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings; 

        public MainViewModel()
        {
            this._items = new ObservableCollection<ItemViewModel>();

            try
            {
                string json = (string)roamingSettings.Values["tracks"];
                _items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);

            }
            catch { };
        }

        public delegate void DataLoadEventHandler(object sender, EventArgs e);
        public event DataLoadEventHandler DataLoad;
        protected virtual void OnDataLoad(EventArgs e)
        {

            if (DataLoad != null)
            {
                //this.NotifyPropertyChanged("Items");
                DataLoad(this, e);
            };
        }

        private ObservableCollection<ItemViewModel> _items = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> Items
        {
            get { return this._items; }
            
        }

        private ObservableCollection<ItemViewModel> _topItem = new ObservableCollection<ItemViewModel>();
        public ObservableCollection<ItemViewModel> TopItems
        {
            get { return this._topItem; }
        }


        private async void LoadImages()
        {
            try
            {
                List<BitmapImage> ImagesList = new List<BitmapImage>();

                foreach (var item in this.Items)
                {
                    var httpClient = new HttpClient();
                    var contentBytes = await httpClient.GetByteArrayAsync(item.Waveform_url);
                    var ims = new InMemoryRandomAccessStream();
                    var dataWriter = new DataWriter(ims);
                    dataWriter.WriteBytes(contentBytes);
                    await dataWriter.StoreAsync();
                    ims.Seek(0);

                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(ims);
                    ImagesList.Add(bitmap);
                };

                for (var i = 0; i < Items.Count(); i++)
                {
                    Items[i].Image = ImagesList[i];
                };
            }
            catch { };
            this.IsDataLoaded = true;
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            try
            {
                string json = "";

                json = await MakeWebRequestForSoundcloud();

                try
                {
                    roamingSettings.Values["tracks"] = json;
                }
                catch { };

                _items = JsonConvert.DeserializeObject<ObservableCollection<ItemViewModel>>(json);

                this.LoadImages();
            }
            catch { };
            
            NotifyPropertyChanged("IsDataLoaded");
            NotifyPropertyChanged("Items");

            App.ViewModel.OnDataLoad(EventArgs.Empty);
        }

        public async Task<string> MakeWebRequestForSoundcloud()
        {
                HttpClient http = new System.Net.Http.HttpClient();
                HttpResponseMessage response = await http.GetAsync("http://api.soundcloud.com/users/malukah/tracks.json?client_id=c210a3efbb3d75200118f6bf24d71ee0");
                return await response.Content.ReadAsStringAsync();
            
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
