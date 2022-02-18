using Microsoft.AspNetCore.Mvc;

namespace Lesson1.Controllers
{
    [ApiController]
    [Route("/")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ValueHolder _holder;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ValueHolder holder)
        {
            _logger = logger;
            _holder = holder;
        }


        [HttpPost, Route("SaveData/{dd}/{temp}")] //Save data sending parameters dd= date to add(2022-02-18), temp = temperature to add(30) to insert or update if data exists on that date
        public string SaveData([FromRoute] DateTime dd, int temp)
        {

            WeatherForecast newWeather = new WeatherForecast() { Date = dd, TemperatureC = temp };

            int updated = 0;
            string status = "inserted";
            foreach (var Temper in _holder.Values)
            {
                if (dd == Temper.Date)
                {
                    Temper.TemperatureC = temp;
                    updated = 1;
                    status = "updated";
                }
            }
            if (updated == 0)
            {
                _holder.Values.Add(newWeather);
                status = "inserted";
            }

            return $"Temp is {status} to {temp} on {dd.ToString()}";
        }

        [HttpGet, Route("LoadData/{dateFrom}/{dateTo}")] // load data between 2 dates including them datefrom(2022-02-01) and dateTo(2022-02-19)
        public string LoadData([FromRoute] DateTime dateFrom, DateTime dateTo)
        {
            string ReturnData = "";
            foreach (var Temper in _holder.Values)
            {
                if (dateFrom <= Temper.Date && dateTo >= Temper.Date)
                {
                    ReturnData += $"Temperature for {Temper.Date.ToString()} is {Temper.TemperatureC.ToString()} \n";
                }
            }
            return ReturnData;
        }


        [HttpDelete, Route("DeleteData/{dateFrom}/{dateTo}")] // deletes data between 2 parameters datefrom(2022-02-01) and dateTo(2022-02-19)
        public string DeleteData([FromRoute] DateTime dateFrom, DateTime dateTo)
        {
            string ReturnData = "";
            foreach (var Temper in _holder.Values)
            {
                if (dateFrom <= Temper.Date && dateTo >= Temper.Date)
                {
                    ReturnData += $"Temperature for {Temper.Date.ToString()} was {Temper.TemperatureC.ToString()} and deleted \n";

                }
            }
            _holder.Values.RemoveAll(x => x.Date >= dateFrom && x.Date <= dateTo);
            return ReturnData;
        }
    }
}