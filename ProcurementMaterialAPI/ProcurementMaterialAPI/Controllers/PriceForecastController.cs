using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProcurementMaterialAPI.Controllers
{
    [Authorize(Roles = "Purchaser")]
    [ApiController]
    [Route("[controller]")]
    public class PriceForecastController : ControllerBase
    {
        private readonly MaterialDbContext _context;
        private const decimal Alpha = 0.7m; // Коэффициент сглаживания

        public PriceForecastController(MaterialDbContext context)
        {
            _context = context;
        }

        // Метод для получения данных по материалам и расчета прогнозируемой средней цены
        [HttpGet("forecast")]
        public async Task<IActionResult> GetPriceForecastByINNAndMaterial(string inn, string material)
        {
            var oneYearAgo = DateOnly.FromDateTime(DateTime.Now.AddYears(-1));

            // Получаем данные за последний год по ИНН и наименованию материала, группируя по дате
            var materialPrices = await _context.Dok_SF
                .Where(m => m.INN == inn && m.material == material)
                .GroupBy(m => m.date_budat)
                .Select(g => new
                {
                    date_budat = g.Key,
                    cost = g.Average(m => m.cost / m.quan) // Берем среднюю цену за день
                })
                .OrderBy(m => m.date_budat)
                .ToListAsync();

            // Проверяем, есть ли данные для данного ИНН и материала
            if (materialPrices == null || !materialPrices.Any())
            {
                return NotFound("Данные для указанного ИНН и материала не найдены.");
            }

            List<decimal> forecastedPrices = new List<decimal>();

            // Инициализируем первое значение F(1) = A(1)
            decimal previousForecast = (decimal)materialPrices[0].cost;
            forecastedPrices.Add(previousForecast);

            // Выполняем расчет прогноза для каждого дня по формуле
            for (int i = 1; i < materialPrices.Count; i++)
            {
                decimal currentPrice = (decimal)materialPrices[i].cost;
                decimal forecast = Alpha * currentPrice + (1 - Alpha) * previousForecast;
                forecastedPrices.Add(forecast);
                previousForecast = forecast; // Обновляем предыдущее прогнозируемое значение
            }

            // Рассчитываем среднюю прогнозируемую цену на месяц вперед
            var forecastForNextMonth = forecastedPrices.Last();

            // Формируем ответ с данными
            var result = new
            {
                LastForecast = Math.Round(forecastForNextMonth, 2).ToString("F2"),
                Date = materialPrices.Last().date_budat.AddMonths(1).ToString("MM-yyyy"),
			};

            return Ok(result);
        }
    }
}
