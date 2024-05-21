using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.ModelDB;
using System.Collections.Generic;
using System.Linq;
using ProcurementMaterialAPI.DTOs;

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

        [HttpPost]
        [Route("GetUniqueMaterials")]
        public IActionResult GetUniqueMaterials()
        {
            var uniqueMaterials = _context.Dok_SF
                .Select(d => d.material)
                .Distinct()
                .ToList();

            if (!uniqueMaterials.Any())
                return NotFound();

            return Ok(new { materials = uniqueMaterials });
        }

        [HttpPost]
        [Route("GetUniqueINNsByMaterial")]
        public IActionResult GetUniqueINNsByMaterial([FromBody] MaterialRequest request)
        {
            var uniqueINNs = _context.Dok_SF
                .Where(d => d.material == request.Material)
                .Select(d => d.INN)
                .Distinct()
                .ToList();

            if (!uniqueINNs.Any())
                return NotFound();

            return Ok(new { INNs = uniqueINNs });
        }

        // Define the MaterialRequest class
        public class MaterialRequest
        {
            public string Material { get; set; }
        }

        [HttpPost]
		[Route("3GenerateCostOverTimeChartJson")]
		public string GenerateCostOverTimeChartJson(List<ModelDok_SF> data)
		{
			var chartData = new
			{
				labels = data.Select(d => d.date_budat.ToString("yyyy-MM-dd")),
				datasets = new[]
				{
			new { label = "Cost over time", data = data.Select(d => d.cost) }
		}
			};

			return JsonSerializer.Serialize(new { bar = chartData });
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
							.Select(d => d.cost)
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
		public string GenerateSupplierQuantityComparisonChartJson(List<ModelDok_SF> data)
		{
			var suppliers = data.Select(d => d.INN).Distinct().ToList();

			var chartData = new
			{
				labels = suppliers,
				datasets = suppliers.Select(supplier => new
				{
					label = $"Supplier {supplier}",
					data = data.Where(d => d.INN == supplier).Select(d => d.quan).Average()
				})
			};

			return JsonSerializer.Serialize(new { bar = chartData });
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
}
