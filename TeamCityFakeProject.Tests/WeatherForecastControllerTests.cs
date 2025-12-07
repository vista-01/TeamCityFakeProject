using Microsoft.Extensions.Logging;
using Moq;
using TeamCityFakeProject.Controllers;

namespace TeamCityFakeProject.Tests
{
    public class WeatherForecastControllerTests
    {
        private readonly Mock<ILogger<WeatherForecastController>> _mockLogger;
        private readonly WeatherForecastController _controller;

        public WeatherForecastControllerTests()
        {
            _mockLogger = new Mock<ILogger<WeatherForecastController>>();
            _controller = new WeatherForecastController(_mockLogger.Object);
        }

        [Fact]
        public void Get_ReturnsCollectionOfWeatherForecasts()
        {
            // Act
            var result = _controller.Get();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<WeatherForecast>>(result);
        }

        [Fact]
        public void Get_ReturnsFiveForecasts()
        {
            // Act
            var result = _controller.Get();
            var forecasts = result.ToList();

            // Assert
            Assert.Equal(5, forecasts.Count);
        }

        [Fact]
        public void Get_AllForecastsHaveFutureDates()
        {
            // Arrange
            var today = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = _controller.Get();
            var forecasts = result.ToList();

            // Assert
            Assert.All(forecasts, forecast =>
            {
                Assert.True(forecast.Date > today, $"Forecast date {forecast.Date} should be after today {today}");
            });
        }

        [Fact]
        public void Get_AllForecastsHaveValidSummaries()
        {
            // Arrange
            var validSummaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

            // Act
            var result = _controller.Get();
            var forecasts = result.ToList();

            // Assert
            Assert.All(forecasts, forecast =>
            {
                Assert.NotNull(forecast.Summary);
                Assert.Contains(forecast.Summary, validSummaries);
            });
        }

        [Fact]
        public void Get_AllForecastsHaveTemperaturesInValidRange()
        {
            // Act
            var result = _controller.Get();
            var forecasts = result.ToList();

            // Assert
            Assert.All(forecasts, forecast =>
            {
                Assert.InRange(forecast.TemperatureC, -20, 54);
            });
        }

        [Fact]
        public void Get_ForecastDatesAreSequential()
        {
            // Act
            var result = _controller.Get();
            var forecasts = result.ToList();

            // Assert
            for (int i = 0; i < forecasts.Count - 1; i++)
            {
                Assert.True(forecasts[i + 1].Date > forecasts[i].Date, 
                    "Forecast dates should be in ascending order");
            }
        }

        [Fact]
        public void Get_TemperatureFIsCalculatedCorrectly()
        {
            // Act
            var result = _controller.Get();
            var forecasts = result.ToList();

            // Assert
            Assert.All(forecasts, forecast =>
            {
                int expectedTempF = 32 + (int)(forecast.TemperatureC / 0.5556);
                Assert.Equal(expectedTempF, forecast.TemperatureF);
            });
        }

        [Fact]
        public void Constructor_AcceptsLogger()
        {
            // Arrange
            var mockLogger = new Mock<ILogger<WeatherForecastController>>();

            // Act
            var controller = new WeatherForecastController(mockLogger.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public void Get_ReturnsNewDataOnEachCall()
        {
            // Act
            var firstResult = _controller.Get().ToList();
            var secondResult = _controller.Get().ToList();

            // Assert - At least one property should be different due to randomness
            bool hasDifference = false;
            for (int i = 0; i < firstResult.Count; i++)
            {
                if (firstResult[i].TemperatureC != secondResult[i].TemperatureC ||
                    firstResult[i].Summary != secondResult[i].Summary)
                {
                    hasDifference = true;
                    break;
                }
            }

            Assert.True(hasDifference, "Multiple calls should produce different random data");
        }
    }
}
