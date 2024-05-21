namespace ProcurementMaterialAPI.DTOs
{
	public class SupplierCostRequest
	{
		public string Material {  get; set; }

		public List<string> INNs { get; set; }
	}
}
