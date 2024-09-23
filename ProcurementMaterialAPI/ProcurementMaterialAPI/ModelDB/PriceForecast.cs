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
            if (materialData == null)
                throw new ArgumentNullException(nameof(materialData), "Данные о материалах не могут быть null.");

            if (systemMatchData == null)
                throw new ArgumentNullException(nameof(systemMatchData), "Данные системного соответствия не могут быть null.");

            if (alpha <= 0 || alpha >= 1)
                throw new ArgumentOutOfRangeException(nameof(alpha), "Коэффициент сглаживания должен быть в диапазоне (0, 1).");

            _materialData = materialData;
            _systemMatchData = systemMatchData;
            _alpha = alpha;
        }

        // Метод для расчета экспоненциального сглаживания
        public decimal CalculateForecast(List<decimal> prices)
        {
            if (prices == null || prices.Count == 0)
                throw new ArgumentException("Нет данных для расчета прогноза.");

            decimal forecast = prices[0];

            for (int i = 1; i < prices.Count; i++)
            {
                forecast = (decimal)_alpha * prices[i] + (1 - (decimal)_alpha) * forecast;
            }

            return forecast;
        }

        // Метод для получения данных о ценах материалов из ModelDok_SF
        public List<decimal> GetMaterialPricesByINN(string inn)
        {
            if (string.IsNullOrWhiteSpace(inn))
                throw new ArgumentException("ИНН не может быть пустым или состоять только из пробелов.", nameof(inn));

            return _materialData
                .Where(m => m.INN == inn && m.cost > 0)
                .OrderBy(m => m.date_budat)
                .Select(m => Convert.ToDecimal(m.cost))
                .ToList();
        }

        // Основной метод для выполнения прогноза по конкретному материалу и ИНН
        public decimal GetPriceForecast(string inn)
        {
            var prices = GetMaterialPricesByINN(inn);

            if (prices.Count == 0)
                throw new InvalidOperationException("Нет доступных цен для указанного ИНН.");

            return CalculateForecast(prices);
        }

        // Дополнительный метод, который может использовать данные из InformationSystemsMatch
        // (Пример использования поля _systemMatchData)
        public decimal CalculateDemandForecast(string materialName)
        {
            if (string.IsNullOrWhiteSpace(materialName))
                throw new ArgumentException("Название материала не может быть пустым или состоять только из пробелов.", nameof(materialName));

            // Предположим, что мы хотим прогнозировать спрос на материал на основе его исторического потребления
            var consumptionData = _systemMatchData
                .Where(m => m.MaterialName == materialName && m.CountOutgo.HasValue)
                .OrderBy(m => m.Date)
                .Select(m => m.CountOutgo.Value)
                .ToList();

            if (consumptionData.Count == 0)
                throw new InvalidOperationException("Нет данных о потреблении для указанного материала.");

            // Используем экспоненциальное сглаживание для прогнозирования спроса
            decimal forecast = consumptionData[0];

            for (int i = 1; i < consumptionData.Count; i++)
            {
                forecast = (decimal)_alpha * consumptionData[i] + (1 - (decimal)_alpha) * forecast;
            }

            return forecast;
        }
    }
}