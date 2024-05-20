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
        public DateOnly date_budat { get; set; }
        public string material_grpup {  get; set; }
        public char  EI {  get; set; }

    }
}
