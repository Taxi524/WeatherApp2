
using WeatherApp.Services;

namespace WeatherApp;

public partial class WeatherPage : ContentPage
{
	public List<Models.List> WeatherList;
	private double latitude;
	private double longitude;
	public WeatherPage()
	{
		InitializeComponent();
		WeatherList = new List<Models.List>();
	}

	protected async override void OnAppearing()
	{
		base.OnAppearing();
		await GetLocation();
		await GetWeatherDataByLocation(latitude, longitude);
	}
	public async Task GetLocation()
	{
		var location = await Geolocation.GetLocationAsync();
		if (location != null)
		{
            latitude = location.Latitude;
            longitude = location.Longitude;
        }
	}

	public async void TapLocation_Tapped(object sender, EventArgs e) 
	{
		await GetLocation();
		var result = await ApiService.GetWeather(latitude, longitude);
		UpdateUI(result);
	}

	private async void ImageButton_Clicked(object sender, EventArgs e)
	{
		var response = await DisplayPromptAsync(title: "", message: "", placeholder: "Search weather by city", accept: "Search", cancel: "Cancel");
		if (response != null) 
		{
			await GetWeatherDataByCity(response);
			var result = await ApiService.GetWeather(latitude, longitude);
			UpdateUI(result);
		}
	}

    public async Task GetWeatherDataByLocation(double lat,double lon)
    {
        var result = await ApiService.GetWeather(lat,lon);
        UpdateUI(result);
    }

    public async Task GetWeatherDataByCity(string city)
	{
		var result = await ApiService.GetWeatherByCity(city);
        UpdateUI(result);
    }


    public void UpdateUI(dynamic result)
	{
		foreach(var item in result.list)
		{
			WeatherList.Add(item);
		}
        CvWeather.ItemsSource = WeatherList;

        lblCity.Text = result.city.name;
        weatherDescription.Text = result.list[0].weather[0].description;
        LblTemperature.Text = result.list[0].main.tempearatue + "¢XC";
        LblHumidity.Text = result.list[0].main.humidity + "%";
        LblWind.Text = result.list[0].wind.speed + "m/s";
        weatherIcon.Source = result.list[0].weather[0].fullIcon;
    }
}