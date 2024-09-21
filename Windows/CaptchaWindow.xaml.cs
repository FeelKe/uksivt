using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Timers;

namespace WpfApp17.Windows
{
    public partial class CaptchaWindow : Window
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private bool isTimerRunning = true;
        private System.Timers.Timer captchaTimer;
        public bool IsCaptchaValid = false;

        public CaptchaWindow()
        {
            InitializeComponent();
            StartCaptchaTimer(); 
            LoadCaptchaImage(); 
        }

        private void StartCaptchaTimer()
        {
            captchaTimer = new System.Timers.Timer(10000); 
            captchaTimer.Elapsed += async (sender, e) => await ReloadCaptcha(); 
            captchaTimer.AutoReset = true; 
            captchaTimer.Start();
        }

        private void RestartCaptchaTimer()
        {
            captchaTimer.Stop(); 
            captchaTimer.Start();
        }

        private async Task ReloadCaptcha()
        {
            await Dispatcher.Invoke(async () => await LoadCaptchaImage()); 
        }
        private async Task LoadCaptchaImage()
        {
            try
            {
                var response = await httpClient.GetAsync("http://localhost:5056/api/captcha/image");
                response.EnsureSuccessStatusCode();
                var imageBytes = await response.Content.ReadAsByteArrayAsync();
                var bitmap = new BitmapImage();
                using (var stream = new MemoryStream(imageBytes))
                {
                    bitmap.BeginInit();
                    bitmap.StreamSource = stream;
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                }
                CaptchaImage.Source = bitmap;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}");
            }
        }
        private async void VerifyClick(object sender, RoutedEventArgs e)
        {
            var captchaInput = CaptchaTextBox.Text;

            try
            {
                var response = await httpClient.PostAsJsonAsync("http://localhost:5056/api/captcha/verify", captchaInput);

                if (response.IsSuccessStatusCode)
                {
                    IsCaptchaValid = true;
                    MessageBox.Show("Капча верна!");
                    this.Close(); 
                }
                else
                {
                    IsCaptchaValid = false;
                    MessageBox.Show("Неверная капча, попробуйте снова.");
                    await LoadCaptchaImage(); 
                    RestartCaptchaTimer(); 
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }
    }
}
