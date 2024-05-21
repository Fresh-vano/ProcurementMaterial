using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProcurementMaterialAPI.Context;
using ProcurementMaterialAPI.ModelDB;
using System.Linq;
using System.Threading.Tasks;

namespace ProcurementMaterialAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TableController : ControllerBase
	{
		private readonly MaterialDbContext _context;

		public TableController(MaterialDbContext context)
		{
			_context = context;
		}

		[HttpGet("material")]
		public async Task<IActionResult> GetMaterialData()
		{
			var result = await _context.InformationSystemsMatch
				.Select(m => new
				{
					id = m.Id,
					MaterialName = m.MaterialName,
					MaterialNom = m.MaterialNom,
					GroupMaterialCode = m.GroupMaterialCode,
					GroupMaterialName = m.GroupMaterialName,
					SubGroupMaterialCode = m.SubGroupMaterialCode,
					SubGroupMaterialName = m.SubGroupMaterialName,
					CountOutgo = m.CountOutgo,
					SumOutgo = m.SumOutgo,
					CountEnd = m.CountEnd,
					SumEnd = m.SumEnd,
					Date = m.Date
				}).Take(100)
				.ToListAsync();

			var columns = new object[]
			{
				new { field = "id", headerName = "ID", width = 70 },
				new { field = "materialName", headerName = "Номер", width = 130 },
				new { field = "materialNom", headerName = "Наименование", width = 130 },
				new { field = "groupMaterialCode", headerName = "Группа Материала", width = 130 },
				new { field = "groupMaterialName", headerName = "Наименование Группы Материалов", width = 250 },
				new { field = "subGroupMaterialCode", headerName = "Подгруппа Материала", width = 130 },
				new { field = "subGroupMaterialName", headerName = "Наименование Подгруппы Материалов", width = 250 },
				new { field = "countOutgo", headerName = "Количество Расхода", width = 130, type = "number" },
				new { field = "sumOutgo", headerName = "Сумма Расхода", width = 130, type = "number" },
				new { field = "countEnd", headerName = "Количество на Конец", width = 130, type = "number" },
				new { field = "sumEnd", headerName = "Сумма на Конец", width = 130, type = "number" },
				new { field = "date", headerName = "Дата", width = 130, type = "Date" }
			};

			return Ok(new { columns, rows = result });
		}

		[HttpGet("sf")]
		public async Task<IActionResult> GetSupplierData()
		{
			var result = await _context.Dok_SF
				.GroupBy(d => new { d.buisnes_number, d.INN })
				.Select(g => new
				{
					id = g.First().id,
					be = g.Key.buisnes_number,
					inn = g.Key.INN,
					quanSumm = g.Sum(d => d.quan),
					costMax = g.Max(d => Math.Round(d.cost / d.quan, 3)),
					costMin = g.Min(d => Math.Round(d.cost / d.quan, 3))
				}).Take(100)
				.ToListAsync();

			var columns = new object[]
			{
				new { field = "id", headerName = "ID", width = 100 },
				new { field = "be", headerName = "BE", width = 250 },
				new { field = "inn", headerName = "ИНН", width = 250 },
				new { field = "quanSumm", headerName = "Сумма Количество Материалов", width = 200, type = "number" },
				new { field = "costMax", headerName = "Цена MAX", width = 130, type = "number" },
				new { field = "costMin", headerName = "Цена MIN", width = 130, type = "number" }
			};

			return Ok(new { columns, rows = result });
		}
	}
}
