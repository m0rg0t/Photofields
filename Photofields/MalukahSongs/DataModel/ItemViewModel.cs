using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MalukahSongs.DataModel
{
    public class ItemViewModel : INotifyPropertyChanged
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value != _name)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        private string _id;
        public string Id
        {
            get
            {
                return _id;
            }
            set
            {
                if (value != _id)
                {
                    _id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value != _description)
                {
                    _description = value;
                    NotifyPropertyChanged("Description");
                }
            }
        }


        private string _image_url;
        public string Image_url
        {
            get
            {
                return _image_url;
            }
            set
            {
                if (value != _image_url)
                {
                    _image_url = value;
                    NotifyPropertyChanged("Image_url");
                }
            }
        }

        private BitmapImage bi;
        public BitmapImage Image
        {
            get {
                return bi;
            }
            set {
                //var stream = RandomAccessStreamReference.CreateFromUri(new Uri(_Waveform_url));
                bi = value;
                //bi.UriSource = new Uri(_Waveform_url);
                NotifyPropertyChanged("Image");
            }
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
