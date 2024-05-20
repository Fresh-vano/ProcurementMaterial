namespace ProcurementMaterialAPI.ModelDB
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string UserName { get; set; }

        public string UserShortName { get; set; }
        /// <summary>
        /// Role (1 - manager, 2 - purchaser, 3 - report_group)
        /// </summary>
        public int UserRole { get; set; }

    }
}
