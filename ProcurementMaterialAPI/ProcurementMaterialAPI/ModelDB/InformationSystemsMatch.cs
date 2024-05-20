using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ProcurementMaterialAPI.ModelDB
{
    public class InformationSystemsMatch
    {
        /// <summary>
        /// Identity (Счётчик)
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Номенклатура|Номер / Материал
        /// </summary>
        public string MaterialName { get; set; }
		/// <summary>
		/// Наимен. материала \ Номенклатура|Наименование
		/// </summary>
		public string MaterialNom { get; set; }
		/// <summary>
		/// Базовая ЕИ \ Номенклатура|ЕИ
		/// </summary>
		[StringLength(3)]
        public string BEI { get; set; }
        /// <summary>
        /// Цех|Наименование / Цех
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Группа ном.|Код	/ Вид материала	
        /// </summary>
        public string GroupMaterialCode { get; set; }
        /// <summary>
        /// Группа ном.|Наименование / Наимен. ВидаМатер
        /// </summary>
        public string GroupMaterialName { get; set; }
        /// <summary>
        /// Подгруппа ном.|Код	/ Группа материалов
        /// </summary>
        public string SubGroupMaterialCode { get; set; }
        /// <summary>
        /// Подгруппа ном.|Наименование / НаименГруппыМтр	
        /// </summary>
        public string SubGroupMaterialName { get; set; }
        /// <summary>
        /// Кол-во расхода / КолОтпБезВнутр
        /// </summary>
        public int CountOutgo { get; set; }
        /// <summary>
        /// Сумма расхода / СтоимОтпБезВнутр
        /// </summary>
        public float SumOutgo { get; set; }
        /// <summary>
        /// Кол-во на конец	/ Остаток на конец
        /// </summary>
        public int CountEnd { get; set; }
        /// <summary>
        /// Сумма на конец / Стоимость на конец
        /// </summary>
        public float SumEnd { get; set; }



        ///Прочее 
        
        ///<summary>
        /// Код Цеха КИС
        /// </summary>
        public string? DepartmentCode { get; set; }



    }
}
