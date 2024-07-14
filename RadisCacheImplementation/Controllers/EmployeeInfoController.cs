namespace RadisCacheImplementation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeInfoController : ControllerBase
    {
        private readonly databaseContext _context;
        private readonly IDistributedCache _cache;

        public EmployeeInfoController(databaseContext context, IDistributedCache cache)
        {
            _context = context;
            _cache = cache;
        }

        [HttpPost]
        public IActionResult Post()
        {
            // Generate sample data
            var random = new Random();
            var employeeInfo = new EmployeeInfo
            {
                EmployeeCode = "EMP" + random.Next(1000, 9999), // Generate a random employee code
                Name = "Employee " + random.Next(1, 100), // Generate a random name
                phone = "123-456-7890" // Sample phone number
            };

            // Insert the generated data into the database
            _context.EmployeeInfos.Add(employeeInfo);
            _context.SaveChanges();

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            // Try to retrieve employee data from cache
            var cachedEmployees = await _cache.GetStringAsync("Employee");

            if (!string.IsNullOrEmpty(cachedEmployees))
            {
                // If employee data found in cache, return it
                var employees = JsonSerializer.Deserialize<List<EmployeeInfo>>(cachedEmployees);
                return Ok(employees);
            }
            else
            {
                // If employee data not found in cache, retrieve it from the database
                var employeesFromDb = await _context.EmployeeInfos.ToListAsync();

                // Serialize the employee data to JSON
                var jsonEmployees = JsonSerializer.Serialize(employeesFromDb);

                // Store employee data in cache with a sliding expiration of 5 minutes
                await _cache.SetStringAsync("Employee", jsonEmployees, CacheConfiguration.EmployeeCacheOptions);


                return Ok(employeesFromDb);
            }
        }

        [HttpGet("{employeeCode}")]
        public async Task<IActionResult> GetByEmployeeCode(string employeeCode)
        {
            // Try to retrieve employee data from cache
            var cachedEmployee = await _cache.GetStringAsync($"Employee_{employeeCode}");

            if (cachedEmployee != null)
            {
                // If employee data found in cache, return it
                var employee = JsonSerializer.Deserialize<EmployeeInfo>(cachedEmployee);
                return Ok(employee);
            }
            else
            {
                // If employee data not found in cache, retrieve it from the database
                var employeeFromDb = await _context.EmployeeInfos.FirstOrDefaultAsync(e => e.EmployeeCode == employeeCode);

                if (employeeFromDb == null)
                {
                    return NotFound(); // Return 404 Not Found if employee with the specified code is not found
                }

                // Serialize the employee data to JSON
                var jsonEmployee = JsonSerializer.Serialize(employeeFromDb);

                // Store employee data in cache with a sliding expiration of 5 minutes
                await _cache.SetStringAsync($"Employee_{employeeCode}", jsonEmployee, CacheConfiguration.EmployeeCacheOptions);

                return Ok(employeeFromDb);
            }
        }

        [HttpGet("searchById/{id}")]
        public async Task<IActionResult> SearchEmployeeById(int id)
        {
            // Try to retrieve employee data from cache
            var cachedEmployee = await _cache.GetStringAsync($"Employee_{id}");

            if (cachedEmployee != null)
            {
                // If employee data found in cache, return it
                var employee = JsonSerializer.Deserialize<EmployeeInfo>(cachedEmployee);
                return Ok(employee);
            }
            else
            {
                // If employee data not found in cache, retrieve it from the database
                var employeeFromDb = await _context.EmployeeInfos.FindAsync(id);

                if (employeeFromDb == null)
                {
                    return NotFound(); // Return 404 Not Found if employee with the specified ID is not found
                }

                // Serialize the employee data to JSON
                var jsonEmployee = JsonSerializer.Serialize(employeeFromDb);

                // Store employee data in cache with a sliding expiration of 5 minutes
                await _cache.SetStringAsync($"Employee_{id}", jsonEmployee, CacheConfiguration.EmployeeCacheOptions);

                return Ok(employeeFromDb);
            }
        }

    }
}
