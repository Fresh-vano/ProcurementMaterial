using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcurementMaterialAPI.ModelDB
{
    // Класс для представления группы материалов
    public class MaterialGroup
    {
        public string Group { get; set; }
        public DateTime Date { get; set; }
        public double CostWithoutInternal { get; set; }
        public double StartCost { get; set; }
        public double EndCost { get; set; }
        public string BE { get; set; }
    }

    // Класс для представления преобразованной группы материалов
    public class TransformedMaterialGroup
    {
        public string Group { get; set; }
        public DateTime Date { get; set; }
        public double AverageCost { get; set; }
        public double Last12MonthsCost { get; set; }
        public double Last12MonthsAverageCost { get; set; }
        public double TurnoverByGroup { get; set; }
    }

    // Класс для представления материала
    public class Material
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double QuantityWithoutInternal { get; set; }
        public double CostWithoutInternal { get; set; }
        public double EndCost { get; set; }
        public double AverageCost { get; set; }
        public double AverageStock { get; set; }
        public double QuantityReceivedWithoutInternal { get; set; }
        public double CostReceivedWithoutInternal { get; set; }
        public double StartStock { get; set; }
        public double EndStock { get; set; }
        public double StartCost { get; set; }
        public string BE { get; set; }
        public double NewCounter { get; set; }
        public double DaysWithStock { get; set; }
    }

    // Класс для представления преобразованного материала
    public class TransformedMaterial
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public double TurnoverByAmount { get; set; }
        public double TurnoverByQuantity { get; set; }
        public double StockMonthsWithStock12 { get; set; }
        public double Usage12MonthsQuantity { get; set; }
        public double Usage12MonthsCost { get; set; }
        public double SumAverageCost12Months { get; set; }
        public double SumAverageStock12Months { get; set; }
        public double Days12Months { get; set; }
    }

    // Класс для обработки данных
    public class Processing
    {
        // Метод для преобразования данных группы материалов
        public List<TransformedMaterialGroup> TransformOborGr(List<MaterialGroup> materialGroups)
        {
            var transformedGroups = new List<TransformedMaterialGroup>();

            foreach (var group in materialGroups.GroupBy(g => g.Group))
            {
                var rollingCostWithoutInternal = new List<double>();
                var rollingAverageCost = new List<double>();

                foreach (var item in group)
                {
                    rollingCostWithoutInternal.Add(item.CostWithoutInternal);
                    rollingAverageCost.Add((item.StartCost + item.EndCost) / 2);

                    var rollingCostSum = rollingCostWithoutInternal.TakeLast(12).Sum();
                    var rollingAverageCostSum = rollingAverageCost.TakeLast(12).Sum();

                    var turnoverByGroup = rollingAverageCostSum / 12 / rollingCostSum * 365;

                    transformedGroups.Add(new TransformedMaterialGroup
                    {
                        Group = item.Group,
                        Date = item.Date,
                        AverageCost = (item.StartCost + item.EndCost) / 2,
                        Last12MonthsCost = rollingCostSum,
                        Last12MonthsAverageCost = rollingAverageCostSum,
                        TurnoverByGroup = turnoverByGroup
                    });
                }
            }

            return transformedGroups;
        }

        // Метод для преобразования данных материалов
        public List<TransformedMaterial> TransformObor(List<Material> materials)
        {
            var transformedMaterials = new List<TransformedMaterial>();

            var groupedMaterials = materials
                .GroupBy(m => new { m.Name, m.Date })
                .Select(g => new
                {
                    g.Key.Name,
                    g.Key.Date,
                    QuantityWithoutInternal = g.Sum(x => x.QuantityWithoutInternal),
                    CostWithoutInternal = g.Sum(x => x.CostWithoutInternal),
                    EndCost = g.Sum(x => x.EndCost),
                    AverageCost = g.Sum(x => x.AverageCost),
                    AverageStock = g.Sum(x => x.AverageStock),
                    QuantityReceivedWithoutInternal = g.Sum(x => x.QuantityReceivedWithoutInternal),
                    CostReceivedWithoutInternal = g.Sum(x => x.CostReceivedWithoutInternal),
                    StartStock = g.Sum(x => x.StartStock),
                    EndStock = g.Sum(x => x.EndStock),
                    StartCost = g.Sum(x => x.StartCost),
                    BE = g.First().BE,
                    NewCounter = g.Sum(x => x.NewCounter),
                    DaysWithStock = g.Sum(x => x.DaysWithStock)
                }).ToList();

            foreach (var materialGroup in groupedMaterials.GroupBy(m => m.Name))
            {
                var rollingQuantityWithoutInternal = new List<double>();
                var rollingCostWithoutInternal = new List<double>();
                var rollingAverageCost = new List<double>();
                var rollingAverageStock = new List<double>();
                var rollingNewCounter = new List<double>();
                var rollingDaysWithStock = new List<double>();

                foreach (var item in materialGroup)
                {
                    rollingQuantityWithoutInternal.Add(item.QuantityWithoutInternal);
                    rollingCostWithoutInternal.Add(item.CostWithoutInternal);
                    rollingAverageCost.Add(item.AverageCost);
                    rollingAverageStock.Add(item.AverageStock);
                    rollingNewCounter.Add(item.NewCounter);
                    rollingDaysWithStock.Add(item.DaysWithStock);

                    var usage12MonthsQuantity = rollingQuantityWithoutInternal.TakeLast(12).Sum();
                    var usage12MonthsCost = rollingCostWithoutInternal.TakeLast(12).Sum();
                    var sumAverageCost12Months = rollingAverageCost.TakeLast(12).Sum();
                    var sumAverageStock12Months = rollingAverageStock.TakeLast(12).Sum();
                    var stockMonthsWithStock12 = rollingNewCounter.TakeLast(12).Sum();
                    var days12Months = rollingDaysWithStock.TakeLast(12).Sum();

                    var turnoverByAmount = sumAverageCost12Months / stockMonthsWithStock12 / usage12MonthsCost * days12Months;
                    var turnoverByQuantity = sumAverageStock12Months / stockMonthsWithStock12 / usage12MonthsQuantity * days12Months;

                    transformedMaterials.Add(new TransformedMaterial
                    {
                        Name = item.Name,
                        Date = item.Date,
                        TurnoverByAmount = turnoverByAmount,
                        TurnoverByQuantity = turnoverByQuantity,
                        StockMonthsWithStock12 = stockMonthsWithStock12,
                        Usage12MonthsQuantity = usage12MonthsQuantity,
                        Usage12MonthsCost = usage12MonthsCost,
                        SumAverageCost12Months = sumAverageCost12Months,
                        SumAverageStock12Months = sumAverageStock12Months,
                        Days12Months = days12Months
                    });
                }
            }

            return transformedMaterials;
        }
    }
}