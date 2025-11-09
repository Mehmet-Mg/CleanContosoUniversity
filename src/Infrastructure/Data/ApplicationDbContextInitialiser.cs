using CleanContosoUniversity.Domain.Constants;
using CleanContosoUniversity.Domain.Entities;
using CleanContosoUniversity.Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;

namespace CleanContosoUniversity.Infrastructure.Data;

public static class InitialiserExtensions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();

        await initialiser.InitialiseAsync();
        await initialiser.SeedAsync();
    }
}

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            // See https://jasontaylor.dev/ef-core-database-initialisation-strategies
            await _context.Database.EnsureDeletedAsync();
            await _context.Database.EnsureCreatedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        // Default roles
        var administratorRole = new IdentityRole(Roles.Administrator);

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { FirstName = "Admin", LastName = "User", UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            if (!string.IsNullOrWhiteSpace(administratorRole.Name))
            {
                await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
            }
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
                {
                    new TodoItem { Title = "Make a todo list 📃" },
                    new TodoItem { Title = "Check off the first item ✅" },
                    new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                    new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                }
            });

            await _context.SaveChangesAsync();
        }

        // Look for any students.
        if (_context.Students.Any())
        {
            return;   // DB has been seeded
        }

        var alexander = new Student
        {
            FirstMidName = "Carson",
            LastName = "Alexander",
            EnrollmentDate = DateTime.Parse("2016-09-01")
        };

        var alonso = new Student
        {
            FirstMidName = "Meredith",
            LastName = "Alonso",
            EnrollmentDate = DateTime.Parse("2018-09-01")
        };

        var anand = new Student
        {
            FirstMidName = "Arturo",
            LastName = "Anand",
            EnrollmentDate = DateTime.Parse("2019-09-01")
        };

        var barzdukas = new Student
        {
            FirstMidName = "Gytis",
            LastName = "Barzdukas",
            EnrollmentDate = DateTime.Parse("2018-09-01")
        };

        var li = new Student
        {
            FirstMidName = "Yan",
            LastName = "Li",
            EnrollmentDate = DateTime.Parse("2018-09-01")
        };

        var justice = new Student
        {
            FirstMidName = "Peggy",
            LastName = "Justice",
            EnrollmentDate = DateTime.Parse("2017-09-01")
        };

        var norman = new Student
        {
            FirstMidName = "Laura",
            LastName = "Norman",
            EnrollmentDate = DateTime.Parse("2019-09-01")
        };

        var olivetto = new Student
        {
            FirstMidName = "Nino",
            LastName = "Olivetto",
            EnrollmentDate = DateTime.Parse("2011-09-01")
        };

        var students = new Student[]
        {
                alexander,
                alonso,
                anand,
                barzdukas,
                li,
                justice,
                norman,
                olivetto
        };

        _context.AddRange(students);

        var abercrombie = new Instructor
        {
            FirstMidName = "Kim",
            LastName = "Abercrombie",
            HireDate = DateTime.Parse("1995-03-11")
        };

        var fakhouri = new Instructor
        {
            FirstMidName = "Fadi",
            LastName = "Fakhouri",
            HireDate = DateTime.Parse("2002-07-06")
        };

        var harui = new Instructor
        {
            FirstMidName = "Roger",
            LastName = "Harui",
            HireDate = DateTime.Parse("1998-07-01")
        };

        var kapoor = new Instructor
        {
            FirstMidName = "Candace",
            LastName = "Kapoor",
            HireDate = DateTime.Parse("2001-01-15")
        };

        var zheng = new Instructor
        {
            FirstMidName = "Roger",
            LastName = "Zheng",
            HireDate = DateTime.Parse("2004-02-12")
        };

        var instructors = new Instructor[]
        {
                abercrombie,
                fakhouri,
                harui,
                kapoor,
                zheng
        };

        _context.AddRange(instructors);

        var officeAssignments = new OfficeAssignment[]
        {
                new OfficeAssignment {
                    Instructor = fakhouri,
                    Location = "Smith 17" },
                new OfficeAssignment {
                    Instructor = harui,
                    Location = "Gowan 27" },
                new OfficeAssignment {
                    Instructor = kapoor,
                    Location = "Thompson 304" }
        };

        _context.AddRange(officeAssignments);

        var english = new Department
        {
            Name = "English",
            Budget = 350000,
            StartDate = DateTime.Parse("2007-09-01"),
            Administrator = abercrombie
        };

        var mathematics = new Department
        {
            Name = "Mathematics",
            Budget = 100000,
            StartDate = DateTime.Parse("2007-09-01"),
            Administrator = fakhouri
        };

        var engineering = new Department
        {
            Name = "Engineering",
            Budget = 350000,
            StartDate = DateTime.Parse("2007-09-01"),
            Administrator = harui
        };

        var economics = new Department
        {
            Name = "Economics",
            Budget = 100000,
            StartDate = DateTime.Parse("2007-09-01"),
            Administrator = kapoor
        };

        var departments = new Department[]
        {
                english,
                mathematics,
                engineering,
                economics
        };

        _context.AddRange(departments);

        var chemistry = new Course
        {
            CourseID = 1050,
            Title = "Chemistry",
            Credits = 3,
            Department = engineering,
            Instructors = new List<Instructor> { kapoor, harui }
        };

        var microeconomics = new Course
        {
            CourseID = 4022,
            Title = "Microeconomics",
            Credits = 3,
            Department = economics,
            Instructors = new List<Instructor> { zheng }
        };

        var macroeconmics = new Course
        {
            CourseID = 4041,
            Title = "Macroeconomics",
            Credits = 3,
            Department = economics,
            Instructors = new List<Instructor> { zheng }
        };

        var calculus = new Course
        {
            CourseID = 1045,
            Title = "Calculus",
            Credits = 4,
            Department = mathematics,
            Instructors = new List<Instructor> { fakhouri }
        };

        var trigonometry = new Course
        {
            CourseID = 3141,
            Title = "Trigonometry",
            Credits = 4,
            Department = mathematics,
            Instructors = new List<Instructor> { harui }
        };

        var composition = new Course
        {
            CourseID = 2021,
            Title = "Composition",
            Credits = 3,
            Department = english,
            Instructors = new List<Instructor> { abercrombie }
        };

        var literature = new Course
        {
            CourseID = 2042,
            Title = "Literature",
            Credits = 4,
            Department = english,
            Instructors = new List<Instructor> { abercrombie }
        };

        var courses = new Course[]
        {
                chemistry,
                microeconomics,
                macroeconmics,
                calculus,
                trigonometry,
                composition,
                literature
        };

        _context.AddRange(courses);

        var enrollments = new Enrollment[]
        {
                new Enrollment {
                    Student = alexander,
                    Course = chemistry,
                    Grade = Grade.A
                },
                new Enrollment {
                    Student = alexander,
                    Course = microeconomics,
                    Grade = Grade.C
                },
                new Enrollment {
                    Student = alexander,
                    Course = macroeconmics,
                    Grade = Grade.B
                },
                new Enrollment {
                    Student = alonso,
                    Course = calculus,
                    Grade = Grade.B
                },
                new Enrollment {
                    Student = alonso,
                    Course = trigonometry,
                    Grade = Grade.B
                },
                new Enrollment {
                    Student = alonso,
                    Course = composition,
                    Grade = Grade.B
                },
                new Enrollment {
                    Student = anand,
                    Course = chemistry
                },
                new Enrollment {
                    Student = anand,
                    Course = microeconomics,
                    Grade = Grade.B
                },
                new Enrollment {
                    Student = barzdukas,
                    Course = chemistry,
                    Grade = Grade.B
                },
                new Enrollment {
                    Student = li,
                    Course = composition,
                    Grade = Grade.B
                },
                new Enrollment {
                    Student = justice,
                    Course = literature,
                    Grade = Grade.B
                }
        };

        _context.AddRange(enrollments);
        _context.SaveChanges();
    }
}
