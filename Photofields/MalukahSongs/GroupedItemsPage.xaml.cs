using Callisto.Controls;
using MalukahSongs.Data;
using MalukahSongs.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента страницы сгруппированных элементов задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234231

namespace MalukahSongs
{
    /// <summary>
    /// Страница, на которой отображается сгруппированная коллекция элементов.
    /// </summary>
    public sealed partial class GroupedItemsPage : MalukahSongs.Common.LayoutAwarePage
    {
        public GroupedItemsPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Заполняет страницу содержимым, передаваемым в процессе навигации. Также предоставляется любое сохраненное состояние
        /// при повторном создании страницы из предыдущего сеанса.
        /// </summary>
        /// <param name="navigationParameter">Значение параметра, передаваемое
        /// <see cref="Frame.Navigate(Type, Object)"/> при первоначальном запросе этой страницы.
        /// </param>
        /// <param name="pageState">Словарь состояния, сохраненного данной страницей в ходе предыдущего
        /// сеанса. Это значение будет равно NULL при первом посещении страницы.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // TODO: Создание соответствующей модели данных для области проблемы, чтобы заменить пример данных
            var sampleDataGroups = SampleDataSource.GetGroups((String)navigationParameter);
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Вызывается при нажатии заголовка группы.
        /// </summary>
        /// <param name="sender">Объект Button, используемый в качестве заголовка выбранной группы.</param>
        /// <param name="e">Данные о событии, описывающие, каким образом было инициировано нажатие.</param>
        void Header_Click(object sender, RoutedEventArgs e)
        {
            // Определение группы, представляемой экземпляром Button
            var group = (sender as FrameworkElement).DataContext;
        }

        /// <summary>
        /// Вызывается при нажатии элемента внутри группы.
        /// </summary>
        /// <param name="sender">Объект GridView (или ListView, если приложение прикреплено),
        /// в котором отображается нажатый элемент.</param>
        /// <param name="e">Данные о событии, описывающие нажатый элемент.</param>
        async void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            // Переход к соответствующей странице назначения и настройка новой страницы
            // путем передачи необходимой информации в виде параметра навигации
            //var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            //this.Frame.Navigate(typeof(ItemDetailPage), itemId);

            //WebBrowserTask web = new WebBrowserTask();
            await Launcher.LaunchUriAsync(new Uri("http://500px.com/photo/"+((ItemViewModel)e.ClickedItem).Id.ToString()));
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                try
                {
                    App.ViewModel.LoadData();
                    App.ViewModel.DataLoad += new MainViewModel.DataLoadEventHandler(this.DataLoaded);
                }
                catch { };
            };
        }

        private void DataLoaded(object sender, EventArgs e)
        {
            try
            {
                this.Loading.IsIndeterminate = false;
                this.Loading.Visibility = Visibility.Collapsed;

                this.itemGridView.ItemsSource = App.ViewModel.Items;
                this.itemListView.ItemsSource = App.ViewModel.Items;
            }
            catch
            {
                this.Loading.IsIndeterminate = false;
                this.Loading.Visibility = Visibility.Collapsed;
            };
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested += Settings_CommandsRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SettingsPane.GetForCurrentView().CommandsRequested -= Settings_CommandsRequested;
        }

        void Settings_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            try
            {
                var viewAboutPage = new SettingsCommand("", "About", cmd =>
                {
                    //(Window.Current.Content as Frame).Navigate(typeof(AboutPage));
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new About();
                    settingsFlyout.HeaderText = "About";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutPage);

                var viewAboutMalukahPage = new SettingsCommand("", "About Malukah", cmd =>
                {
                    var settingsFlyout = new SettingsFlyout();
                    settingsFlyout.Content = new AboutMalukah();
                    settingsFlyout.HeaderText = "About Malukah";

                    settingsFlyout.IsOpen = true;
                });
                args.Request.ApplicationCommands.Add(viewAboutMalukahPage);
            }
            catch { };
        }


    }
}
