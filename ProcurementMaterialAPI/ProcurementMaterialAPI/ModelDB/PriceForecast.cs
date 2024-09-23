using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcurementMaterialAPI.ModelDB
{
    public class PriceForecast
    {
        private readonly List<ModelDok_SF> _materialData;
        private readonly List<InformationSystemsMatch> _systemMatchData;
        private readonly double _alpha; // Коэффициент сглаживания

        public PriceForecast(List<ModelDok_SF> materialData, List<InformationSystemsMatch> systemMatchData, double alpha = 0.7)
        {
            _materialData = materialData;
            _systemMatchData = systemMatchData;
            _alpha = alpha;
        }

        // Метод для расчета экспоненциального сглаживания
        public double CalculateForecast(List<double> prices)
        {
            if (prices == null || prices.Count == 0)
                throw new ArgumentException("Нет данных для расчета прогноза.");

            // Инициализация начального прогноза как первого значения
            double forecast = prices[0];

            // Применение формулы экспоненциального сглаживания для каждого дня
            for (int i = 1; i < prices.Count; i++)
            {
                forecast = _alpha * prices[i] + (1 - _alpha) * forecast;
            }

            return forecast;
        }

        // Метод для получения данных о ценах материалов из ModelDok_SF
        public List<double> GetMaterialPricesByINN(string inn)
        {
            // Фильтрация данных по ИНН и возвращение списка цен
            return _materialData
                .Where(m => m.INN == inn)
                .OrderBy(m => m.date_budat)
                .Select(m => m.cost)
                .ToList();
        }

        // Основной метод для выполнения прогноза по конкретному материалу и ИНН
        public double GetPriceForecast(string inn)
        {
            // Получаем цены для указанного ИНН
            var prices = GetMaterialPricesByINN(inn);

            // Рассчитываем прогнозируемую цену
            return CalculateForecast(prices);
        }
    }
}
