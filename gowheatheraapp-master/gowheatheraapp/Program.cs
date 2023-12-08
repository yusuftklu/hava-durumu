using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    static async Task Main()
    {
        // İstanbul, İzmir ve Ankara için hava durumu bilgilerini al
        await GetWeatherData("istanbul");
        await GetWeatherData("izmir");
        await GetWeatherData("ankara");

        Console.ReadLine();
    }

    static async Task GetWeatherData(string city)
    {
        // HttpClient sınıfı, HTTP istekleri göndermek için kullanılır.
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // API'ye gönderilecek istek URL'si
                string apiUrl = $"https://goweather.herokuapp.com/weather/{city}";

                // API'ye GET isteği gönder ve cevabı al
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                // Başarılı bir cevap alındıysa devam et
                if (response.IsSuccessStatusCode)
                {
                    // Cevaptan JSON verisini oku
                    string json = await response.Content.ReadAsStringAsync();

                    // JSON verisini C# nesnelerine dönüştür
                    WeatherData weatherData = JsonConvert.DeserializeObject<WeatherData>(json);

                    // Hava durumu bilgilerini konsola yazdır
                    Console.WriteLine($"{city.ToUpper()} Weather Today:");
                    Console.WriteLine($"Temperature: {weatherData.Temperature}");
                    Console.WriteLine($"Wind: {weatherData.Wind}");
                    Console.WriteLine($"Description: {weatherData.Description}"); // Description alanı burada yazdırılıyor
                    Console.WriteLine();

                    // 3 günlük tahmini hava durumu bilgilerini konsola yazdır

                    // Başlangıç tarihi
                    DateTime startDate = DateTime.Today;

                    foreach (var forecast in weatherData.Forecast)
                    {
                        // Her bir tahmin için tarih bilgisini ekleyerek yazdır
                        Console.WriteLine($"Date: {startDate.ToShortDateString()}");
                        Console.WriteLine($"Temperature: {forecast.Temperature}");
                        Console.WriteLine($"Wind: {forecast.Wind}");
                        Console.WriteLine($"Description: {forecast.Description}"); // Description alanı burada yazdırılıyor
                        Console.WriteLine();

                        // Bir sonraki günün tarihini belirle
                        startDate = startDate.AddDays(1);
                    }
                }
                else
                {
                    // Başarısız bir HTTP durumu alındıysa hata mesajını yazdır
                    Console.WriteLine($"Failed to retrieve data for {city}. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda hata mesajını yazdır
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}

// Hava durumu bilgilerini tutan sınıf
class WeatherData
{
    public string Temperature { get; set; }
    public string Wind { get; set; }
    public string Description { get; set; } // JSON verisindeki "description" alanı buraya eklendi
    public ForecastData[] Forecast { get; set; }
}

// Tahmini hava durumu bilgilerini tutan sınıf
class ForecastData
{
    public string Temperature { get; set; }
    public string Wind { get; set; }
    public string Description { get; set; } // JSON verisindeki "description" alanı buraya eklendi
}
