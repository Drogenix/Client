using Model;
using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ObservableCollection<InfoCard> cards;
        public ObservableCollection<InfoCard> Cards
        {
            get
            { return cards; }
            set
            {
                cards = value;
            }
        }
        public InfoCard SelectedItem { get; set; }

        public int SelectedIndex { get; set; }


        public MainWindow()
        {
            InitializeComponent();
        }

        //Кнопки управления окном

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void ButtonMaximized_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow.WindowState != WindowState.Maximized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
            else Application.Current.MainWindow.WindowState = WindowState.Normal;

        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DragMove();
        }

        //Обработка событий кнопок CRUD-операций

        private void GetDataButton_Click(object sender, RoutedEventArgs e)
        {
            HttpGet();
        }

        private async void Create(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                if (InfoTb.Text != "" & PictureUrlTb.Text != "")
                {
                    InfoCard card = new InfoCard(InfoTb.Text, PictureUrlTb.Text);
                    Cards.Add(card);

                    await HttpCreate(card);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Delete(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            try
            {
                int index = ListView.SelectedIndex;
                Cards.RemoveAt(index);
                await HttpDelete(index);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void Edit(object sender, RoutedEventArgs e)
        {
            try
            {
                if (EditTextb.Text != "")
                {
                    Cards[SelectedIndex].Info = EditTextb.Text;
                    await HttpPut(SelectedIndex, Cards[SelectedIndex]);
                    EditPanel.IsEnabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void EditEnable(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            EditPanel.IsEnabled = true;
            SelectedIndex = ListView.SelectedIndex;
        }

        //Работа с API

        private async Task HttpPut(int index, InfoCard card)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize<InfoCard>(card), Encoding.UTF8, "application/json");
                string requestString = "https://localhost:7123/api/infocards/edit/" + index;
                await client.PutAsync(requestString, httpContent);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async Task HttpDelete(int index)
        {
            try
            {
                HttpClient client = new HttpClient();
                string requestString = "https://localhost:7123/api/infocards/delete/" + index;
                await client.DeleteAsync(requestString);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void HttpGet()
        {
            try
            {
                HttpClient client = new HttpClient();
                string requestString = "https://localhost:7123/api/infocards/";
                Cards = await client.GetFromJsonAsync<ObservableCollection<InfoCard>>(requestString);
                DataContext = this;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task HttpCreate(InfoCard card)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpContent httpContent = new StringContent(JsonSerializer.Serialize<InfoCard>(card), Encoding.UTF8, "application/json");
                string requestString = "https://localhost:7123/api/infocards/create/";
                await client.PostAsync(requestString, httpContent);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
