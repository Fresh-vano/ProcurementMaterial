using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.ModelDB;
using System.Collections.Generic;
using System.Linq;
using ProcurementMaterialAPI.DTOs;
using Microsoft.EntityFrameworkCore;

namespace ProcurementMaterialAPI.Controllers
{

	[ApiController]
	[Route("[controller]")]
	public class ChartController : ControllerBase
	{
		private readonly MaterialDbContext _context;

		public ChartController(MaterialDbContext context)
		{
			_context = context;
		}

		[HttpGet]
		[Route("GetUniqueGroup")]
		public IActionResult GetUniqueGroup()
		{
			var uniqueMaterials = _context.InformationSystemsMatch
				.Select(d => d.GroupMaterialName)
				.Distinct()
				.ToList();

			if (!uniqueMaterials.Any())
				return NotFound();

			return Ok(uniqueMaterials);
		}

		[HttpGet]
        [Route("GetUniqueMaterials")]
        public IActionResult GetUniqueMaterials()
        {
            var uniqueMaterials = _context.Dok_SF
                .Select(d => d.material)
                .Distinct()
                .ToList();

            if (!uniqueMaterials.Any())
                return NotFound();

            return Ok(uniqueMaterials);
        }

        [HttpGet]
        [Route("GetUniqueINNsByMaterial")]
        public IActionResult GetUniqueINNsByMaterial(string request)
        {
            var uniqueINNs = _context.Dok_SF
                .Where(d => d.material == request)
                .Select(d => d.INN)
                .Distinct().Take(50)
				.ToList();

            if (!uniqueINNs.Any())
                return NotFound();

            return Ok(uniqueINNs);
        }

        // Define the MaterialRequest class
        public class MaterialRequest
        {
            public string Material { get; set; }
        }

		[HttpPost]
		[Route("3GenerateCostOverTimeChartJson")]
		public IActionResult GenerateCostOverTimeChartJson([FromBody] MaterialRequest material)
		{
			var chartData = new
			{
				labels = _context.Dok_SF
					.Where(d => d.material == material.Material)
					.Select(d => d.date_budat.ToString("yyyy-MM-dd"))
					.ToList(),
				datasets = new[]
				{
					new
					{
						label = "Цена",
						data = _context.Dok_SF
							.Where(d => d.material == material.Material)
							.Select(d => Math.Round(d.cost / d.quan, 3))
							.ToList(),
						backgroundColor = _context.Dok_SF
							.Where(d => d.material == material.Material)
							.Select(d => Math.Round(d.cost / d.quan, 3))
							.ToList().Select(_ => $"rgba({new Random().Next(0, 255)}, {new Random().Next(0, 255)}, {new Random().Next(0, 255)}, 0.8)").ToList()

					}
				}
			};

			return Ok(new { bar = chartData });
		}

		[HttpPost]
		[Route("4GenerateSupplierCostComparisonChartJson")]
		public IActionResult GenerateSupplierCostComparisonChartJson([FromBody] SupplierCostRequest request)
		{
			var suppliers = _context.Dok_SF
				.Where(d => request.INNs.Contains(d.INN) && d.material == request.Material)
				.Select(d => d.INN)
				.Distinct()
				.ToList();

			if (!suppliers.Any())
				return NotFound();

			var chartData = new
			{
				labels = suppliers,
				datasets = new[]
				{
					new
					{
						label = "Стоимость",
						data = suppliers.Select(supplier => _context.Dok_SF
							.Where(d => d.INN == supplier && d.material == request.Material)
							.Select(d => Math.Round(d.cost / d.quan, 3))
							.Average())
							.ToList(),
						backgroundColor = suppliers.Select(_ => $"rgba({new Random().Next(0, 255)}, {new Random().Next(0, 255)}, {new Random().Next(0, 255)}, 0.8)").ToList()
					}
				}
			};

			return Ok(new { bar = chartData });
		}

		[HttpPost]
		[Route("7GenerateSupplierQuantityComparisonChartJson")]
		public IActionResult GenerateSupplierQuantityComparisonChartJson([FromBody] MaterialRequest material)
		{
			var suppliers = _context.Dok_SF
				.Where(d => d.material == material.Material)
				.Select(d => d.INN)
				.Distinct()
				.ToList();

			if (!suppliers.Any())
				return NotFound();

			var chartData = new
			{
				labels = suppliers,
				datasets = new[]
				{
					new
					{
						label = "Количество",
						data = suppliers.Select(supplier => _context.Dok_SF
							.Where(d => d.INN == supplier && d.material == material.Material)
							.Select(d => d.quan)
							.Average())
							.ToList(),
						backgroundColor = suppliers.Select(_ => $"rgba({new Random().Next(0, 255)}, {new Random().Next(0, 255)}, {new Random().Next(0, 255)}, 0.8)").ToList()
					}
				}
			};

			return Ok(new { bar = chartData });
		}

		[HttpPost]
		[Route("GeneratePerformanceChartJson")]
		public async Task<IActionResult> GeneratePerformanceChartJson([FromBody] PerformanceRequest request)
		{
			var inventoryData = _context.Dok_SF
				.Where(d => d.material == request.Material && request.INNs.Contains(d.INN))
				.GroupBy(d => new { d.buisnes_number, d.date_budat })
				.Select(g => new
				{
					BE = g.Key.buisnes_number,
					Date = g.Key.date_budat.ToString("yyyy-MM-dd"),
					Quantity = g.Sum(d => d.quan)
				})
				.ToList();

			var periodData = _context.InformationSystemsMatch
				.Where(d => d.MaterialName.Contains(request.Material))
				.GroupBy(d => new { d.BE, d.Date })
				.Select(g => new
				{
					BE = g.Key.BE,
					Date = g.Key.Date.ToString("yyyy-MM-dd"),
					Quantity = g.Sum(d => d.CountEnd)
				})
				.ToList();

			var chartData = new
			{
				labels = periodData.Select(d => d.Date).Distinct().ToList(),
				datasets = new object[]
				{
			new
			{
				label = "Поступления МТР на БЕ",
				data = inventoryData.Select(d => d.Quantity).ToList(),
				backgroundColor = "rgba(75, 192, 192, 0.6)"
			},
			new
			{
				label = "Запас МТР на БЕ",
				data = periodData.Select(d => d.Quantity).ToList(),
				backgroundColor = "rgba(153, 102, 255, 0.6)"
			}
				}
			};

			return Ok(new { chart = chartData });
		}

		[HttpGet]
		[Route("GenerateReceiptsAndStocksChartJson")]
		public async Task<IActionResult> GenerateReceiptsAndStocksChartJson()
		{
			var now = DateOnly.FromDateTime(DateTime.Now);
			var oneYearAgo = now.AddYears(-2);

			var inventoryData = await _context.Dok_SF
				.Where(d => d.date_budat >= oneYearAgo)
				.GroupBy(d => new { d.buisnes_number, Month = d.date_budat.Month, Year = d.date_budat.Year })
				.Select(g => new
				{
					BE = g.Key.buisnes_number,
					Date = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
					Quantity = g.Sum(d => d.cost)
				})
				.ToListAsync();

			var periodData = await _context.InformationSystemsMatch
				.Where(d => d.Date >= oneYearAgo)
				.GroupBy(d => new { d.BE, Month = d.Date.Month, Year = d.Date.Year })
				.Select(g => new
				{
					BE = g.Key.BE,
					Date = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
					Quantity = g.Sum(d => d.SumEnd)
				})
				.ToListAsync();

			var allDates = inventoryData.Select(d => d.Date)
				.Union(periodData.Select(d => d.Date))
				.Distinct()
				.OrderBy(date => date)
				.ToList();

			var receiptsData = allDates.Select(date => inventoryData
				.Where(d => d.Date == date)
				.Sum(d => d.Quantity))
				.ToList();

			var stocksData = allDates.Select(date => periodData
				.Where(d => d.Date == date)
				.Sum(d => d.Quantity))
				.ToList();

			var chartData = new
			{
				labels = allDates,
				datasets = new object[]
				{
					new
					{
						label = "Поступления",
						data = receiptsData,
						backgroundColor = "rgba(75, 192, 192, 0.6)"
					},
					new
					{
						label = "Запасы",
						data = stocksData,
						backgroundColor = "rgba(153, 102, 255, 0.6)"
					}
				}
			};

			return Ok(new { chart = chartData });
		}

		[HttpGet]
		[Route("GenerateMonthlyExpensesChartJson")]
		public async Task<IActionResult> GenerateMonthlyExpensesChartJson()
		{
			var expensesData = _context.Dok_SF
				.GroupBy(d => new { d.date_budat.Year, d.date_budat.Month })
				.Select(g => new
				{
					Date = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
					Expense = g.Sum(d => d.cost) // Assuming 'quan' is quantity and 'price' is unit price
				})
				.ToList();

			var chartData = new
			{
				labels = expensesData.Select(d => d.Date).Distinct().ToList(),
				datasets = new object[]
				{
			new
			{
				label = "Суммарные траты",
				data = expensesData.Select(d => d.Expense).ToList(),
				backgroundColor = "rgba(255, 99, 132, 0.6)"
			}
				}
			};

			return Ok(new { chart = chartData });
		}

		[HttpPost]
		[Route("GenerateReceiptsAndStocksChartJson")]
		public async Task<IActionResult> GenerateReceiptsAndStocksChartJson([FromBody] GroupMaterialName materialName)
		{
			var now = DateOnly.FromDateTime(DateTime.Now);
			var oneYearAgo = now.AddYears(-2);

			var code = _context.InformationSystemsMatch.FirstOrDefault(x => x.GroupMaterialName == materialName.MaterialName).GroupMaterialCode;

			var inventoryData = await _context.Dok_SF
				.Where(d => d.date_budat >= oneYearAgo && d.material_type == code)
				.GroupBy(d => new { d.buisnes_number, Month = d.date_budat.Month, Year = d.date_budat.Year })
				.Select(g => new
				{
					BE = g.Key.buisnes_number,
					Date = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
					Quantity = g.Sum(d => d.quan)
				})
				.ToListAsync();

			var periodData = await _context.InformationSystemsMatch
				.Where(d => d.Date >= oneYearAgo && d.GroupMaterialName == materialName.MaterialName)
				.GroupBy(d => new { d.BE, Month = d.Date.Month, Year = d.Date.Year })
				.Select(g => new
				{
					BE = g.Key.BE,
					Date = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("yyyy-MM"),
					Quantity = g.Sum(d => d.CountEnd)
				})
				.ToListAsync();

			var allDates = inventoryData.Select(d => d.Date)
				.Union(periodData.Select(d => d.Date))
				.Distinct()
				.OrderBy(date => date)
				.ToList();

			var receiptsData = allDates.Select(date => inventoryData
				.Where(d => d.Date == date)
				.Sum(d => d.Quantity))
				.ToList();

			var stocksData = allDates.Select(date => periodData
				.Where(d => d.Date == date)
				.Sum(d => d.Quantity))
				.ToList();

			var chartData = new
			{
				labels = allDates,
				datasets = new object[]
				{
					new
					{
						label = "Поступления",
						data = receiptsData,
						backgroundColor = "rgba(75, 192, 192, 0.6)"
					},
					new
					{
						label = "Запасы",
						data = stocksData,
						backgroundColor = "rgba(153, 102, 255, 0.6)"
					}
				}
			};

			return Ok(new { chart = chartData });
		}


		[HttpPost]
		[Route("8GenerateMaterialTypeCostChartJson")]
		public string GenerateMaterialTypeCostChartJson(List<ModelDok_SF> data)
		{
			var materialTypes = data.Select(d => d.material_type).Distinct().ToList();

			var chartData = new
			{
				labels = materialTypes,
				datasets = materialTypes.Select(materialType => new
				{
					label = materialType,
					data = data.Where(d => d.material_type == materialType).Select(d => d.cost)
				})
			};

			return JsonSerializer.Serialize(new { bar = chartData });
		}

		[HttpPost]
		[Route("9GenerateMaterialCostOverTimeChartJson")]
		public string GenerateMaterialCostOverTimeChartJson(List<ModelDok_SF> data)
		{
			var groupedData = data.GroupBy(d => d.material);

			var chartData = new
			{
				labels = data.Select(d => d.date_budat.ToString("yyyy-MM-dd")).Distinct(),
				datasets = groupedData.Select(g => new
				{
					label = g.Key,
					data = g.OrderBy(d => d.date_budat).Select(d => d.cost).ToList()
				})
			};

			return JsonSerializer.Serialize(new { bar = chartData });
		}

		[HttpPost]
		[Route("10GenerateMaterialQuantityByBEChartJson")]
		public string GenerateMaterialQuantityByBEChartJson(List<ModelDok_SF> data)
		{
			var bes = data.Select(d => d.Direction).Distinct().ToList();

			var chartData = new
			{
				labels = data.Select(d => d.material).Distinct(),
				datasets = bes.Select(be => new
				{
					label = be,
					data = data.Where(d => d.Direction == be).GroupBy(d => d.material)
							   .Select(g => g.Sum(d => d.quan)).ToList()
				})
			};

			return JsonSerializer.Serialize(new { bar = chartData });
		}
	}

	public class PerformanceRequest
	{
		public string Material { get; set; }
		public List<string> INNs { get; set; }
	}

	public class MaterialRequest
	{
		public string Material { get; set; }
	}

	public class GroupMaterialName
	{
		public string MaterialName { get; set; }
	}
}
