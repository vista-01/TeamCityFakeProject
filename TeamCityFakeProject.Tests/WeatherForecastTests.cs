namespace TeamCityFakeProject.Tests
{
    public class WeatherForecastTests
    {
        [Fact]
        public void TemperatureF_ConvertsFromCelsiusCorrectly_WhenTemperatureIsZero()
        {
            // Arrange
            var weatherForecast = new WeatherForecast
            {
                TemperatureC = 0
            };

            // Act
            var temperatureF = weatherForecast.TemperatureF;

            // Assert
            Assert.Equal(32, temperatureF);
        }

        [Theory]
        [InlineData(0, 32)]
        [InlineData(100, 211)]
        [InlineData(-40, -39)]
        [InlineData(25, 76)]
        [InlineData(-20, -3)]
        [InlineData(55, 130)]
        public void TemperatureF_ConvertsFromCelsiusCorrectly_WithVariousTemperatures(int celsius, int expectedFahrenheit)
        {
            // Arrange
            var weatherForecast = new WeatherForecast
            {
                TemperatureC = celsius
            };

            // Act
            var actualFahrenheit = weatherForecast.TemperatureF;

            // Assert
            Assert.Equal(expectedFahrenheit, actualFahrenheit);
        }

        [Fact]
        public void Date_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedDate = new DateOnly(2025, 12, 7);
            var weatherForecast = new WeatherForecast();

            // Act
            weatherForecast.Date = expectedDate;

            // Assert
            Assert.Equal(expectedDate, weatherForecast.Date);
        }

        [Fact]
        public void Summary_CanBeSetAndRetrieved()
        {
            // Arrange
            var expectedSummary = "Warm";
            var weatherForecast = new WeatherForecast();

            // Act
            weatherForecast.Summary = expectedSummary;

            // Assert
            Assert.Equal(expectedSummary, weatherForecast.Summary);
        }

        [Fact]
        public void Summary_CanBeNull()
        {
            // Arrange & Act
            var weatherForecast = new WeatherForecast
            {
                Summary = null
            };

            // Assert
            Assert.Null(weatherForecast.Summary);
        }

        [Fact]
        public void WeatherForecast_CanBeInitializedWithObjectInitializer()
        {
            // Arrange
            var expectedDate = new DateOnly(2025, 12, 7);
            var expectedTemperatureC = 25;
            var expectedSummary = "Warm";

            // Act
            var weatherForecast = new WeatherForecast
            {
                Date = expectedDate,
                TemperatureC = expectedTemperatureC,
                Summary = expectedSummary
            };

            // Assert
            Assert.Equal(expectedDate, weatherForecast.Date);
            Assert.Equal(expectedTemperatureC, weatherForecast.TemperatureC);
            Assert.Equal(expectedSummary, weatherForecast.Summary);
            Assert.Equal(76, weatherForecast.TemperatureF);
        }
    }
}
