namespace RadisCacheImplementation.Data
{
    public class EmployeeInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // This specifies that the Id property is an identity column
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public string EmployeeCode { get; set; }
        public string? Name { get; set; }
        public string? phone { get; set; }

        public EmployeeInfo()
        {
            // Generate EmployeeCode automatically
            EmployeeCode = GenerateEmployeeCode();
        }

        private string GenerateEmployeeCode()
        {
            // Logic to generate employee code (e.g., using current timestamp)
            return "EMP" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
