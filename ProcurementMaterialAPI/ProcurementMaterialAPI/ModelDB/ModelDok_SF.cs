using Microsoft.Identity.Client;
using System.Security.Cryptography.X509Certificates;

namespace ProcurementMaterialAPI.ModelDB
{
    public class ModelDok_SF
    {
        public int buisnes_number {  get; set; }

        public int buisnes_consignee { get; set; }

        public int fact_number { get; set; }

        public int fact_pos { get; set; }

        public string material { get; set; }

        public string material_name {  get; set; }

        public string material_type { get; set; }

        public DateOnly date_budat { get; set; }

        public string material_group {  get; set; }

        public string material_group_name { get; set; }

        public char  EI {  get; set; }

        public string INN { get; set; }

        public decimal quan {  get; set; }

        public decimal cost { get; set; }
        
        public string Direction {  get; set; }

        // фигня ниже не нужна особо
        public string Department { get; set; }

        public string normalization {  get; set; }



    }
}
