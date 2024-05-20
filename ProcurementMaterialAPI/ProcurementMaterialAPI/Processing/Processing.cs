using System;
using System.Collections.Generic;
using System.Linq;

namespace ProcurementMaterialAPI.ModelDB
{
    public class MaterialGroup
    {
        public string Group { get; set; }
        public DateTime Date { get; set; }
        public double CostWithoutInternal { get; set; }
        public double StartCost { get; set; }
        public double EndCost { get; set; }
        public string BE { get; set; }
    }

    public class TransformedMaterialGroup
    {
        public string Group { get; set; }
        public DateTime Date { get; set; }
        public double AverageCost { get; set; }
        public double Last12MonthsCost { get; set; }
        public double Last12MonthsAverageCost { get; set; }
        public double TurnoverByGroup { get; set; }
    }

    public class Processing
    {
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
    }
}
