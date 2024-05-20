namespace ProcurementMaterialAPI.ModelDB
{
    public class InformationSystems
    {
        /// <summary>
        /// Identity
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Name Information System (SAP, KIS)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Comment
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Plant
        /// </summary>
        public string DepartmentName { get; set; }

        public string DepartmentCode { get; set; }

    }
}
