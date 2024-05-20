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
        [StringLength(40)]
        public string MaterialName { get; set; }
        /// <summary>
        /// Наимен. материала
        /// </summary>
        [StringLength(12)]
        public string MaterialNomSap { get; set; }
        /// <summary>
        /// Номенклатура|Наименование
        /// </summary>
        [StringLength(15)]
        public string MaterialNomKis { get; set; }
        /// <summary>
        /// Базовая ЕИ
        /// </summary>
        [StringLength(3)]
        public string SapBEI { get; set; }
        /// <summary>
        /// Номенклатура|ЕИ
        /// </summary>
        [StringLength(3)]
        public string KisBEI { get; set; }
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
        [StringLength(3)]
        public float SumOutgo { get; set; }
        /// <summary>
        /// Кол-во на конец	/ Остаток на конец
        /// </summary>
        public int CountEnd { get; set; }
        /// <summary>
        /// Сумма на конец / Стоимость на конец
        /// </summary>
        [StringLength(3)]
        public float SumEnd { get; set; }



        ///Прочее
        public string DepartmentCode { get; set; }



    }
}
