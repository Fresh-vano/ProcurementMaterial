using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcurementMaterialAPI.ModelDB
{
    public class MaterialGroup // определение класса MaterialGroup, который будет представлять группу материалов
    {
        public string Group { get; set; } // определение свойства Group, которое представляет название группы материалов
        public DateTime Date { get; set; } // определение свойства Date, которое содержит дату
        public double CostWithoutInternal { get; set; } // определение свойства CostWithoutInternal, которое представляет стоимость без внутренних затрат
        public double StartCost { get; set; } // определение свойства StartCost, которое содержит начальную стоимость
        public double EndCost { get; set; } // определение свойства EndCost, которое содержит конечную стоимость
        public string BE { get; set; } // определение свойства BE, которое представляет бизнес-единицу
    }

    public class TransformedMaterialGroup // определение класса TransformedMaterialGroup, который будет представлять преобразованную группу материалов
    {
        public string Group { get; set; } // определение свойства Group, которое представляет название группы материалов
        public DateTime Date { get; set; } // определение свойства Date, которое содержит дату
        public double AverageCost { get; set; } // определение свойства AverageCost, которое представляет среднюю стоимость
        public double Last12MonthsCost { get; set; } // определение свойства Last12MonthsCost, которое содержит стоимость за последние 12 месяцев
        public double Last12MonthsAverageCost { get; set; } // определение свойства Last12MonthsAverageCost, которое представляет среднюю стоимость за последние 12 месяцев
        public double TurnoverByGroup { get; set; } // определение свойства TurnoverByGroup, которое представляет оборачиваемость по группе
    }

    public class Processing
    {
        public List<TransformedMaterialGroup> TransformOborGr(List<MaterialGroup> materialGroups) // определение метода TransformOborGr, который принимает
                                                                                                  // список объектов MaterialGroup в качестве входных данных
                                                                                                  // и возвращает список объектов TransformedMaterialGroup
        {
            var transformedGroups = new List<TransformedMaterialGroup>(); // создается новый список transformedGroups, который будет содержать преобразованные группы материалов

            foreach (var group in materialGroups.GroupBy(g => g.Group)) // происходит итерация по группам материалов. materialGroups разбивается на группы с одинаковым значением
                                                                        // свойства Group с помощью метода GroupBy
            {
                var rollingCostWithoutInternal = new List<double>(); // создается новый список rollingCostWithoutInternal, который будет использоваться для хранения суммы стоимости без внутренних расходов
                var rollingAverageCost = new List<double>(); // создается новый список rollingAverageCost, который будет использоваться для хранения средней стоимости

                foreach (var item in group) // происходит итерация по каждому элементу в группе
                {
                    rollingCostWithoutInternal.Add(item.CostWithoutInternal); // значения стоимости без внутренних расходов добавляется в соответствующие списки для каждого элемента в группе
                    rollingAverageCost.Add((item.StartCost + item.EndCost) / 2); // значения средней стоимости добавляются в соответствующие списки для каждого элемента в группе

                    var rollingCostSum = rollingCostWithoutInternal.TakeLast(12).Sum(); // вычисляются суммы последних 12 значений стоимости без внутренних расходов
                    var rollingAverageCostSum = rollingAverageCost.TakeLast(12).Sum(); // вычисляются суммы последних 12 значений средней стоимости

                    var turnoverByGroup = rollingAverageCostSum / 12 / rollingCostSum * 365; // вычисляется оборачиваемость по группе как отношение средней стоимости за последние 12 месяцев
                                                                                             // к стоимости без внутренних расходов, умноженное на 365

                    transformedGroups.Add(new TransformedMaterialGroup // создается новый объект TransformedMaterialGroup и добавляется в список transformedGroups
                                                                       // с заполненными данными о группе материалов
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

            return transformedGroups; // возвращается список преобразованных групп материалов
        }
    }
}