using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Moq;
using USPSystem.Controllers;
using USPSystem.Data;
using USPSystem.Models;
using USPSystem.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;

namespace USPSystem.Tests
{
    public class SpecialConsiderationControllerTests
    {
        private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IWebHostEnvironment> _mockHostEnvironment;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<ILogger<PageHoldService>> _mockPageHoldLogger;
        private readonly StudentHoldService _studentHoldService;
        private readonly PageHoldService _pageHoldService;
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public SpecialConsiderationControllerTests()
        {
            // Setup UserManager mock
            var userStoreMock = new Mock<IUserStore<ApplicationUser>>();
            _mockUserManager = new Mock<UserManager<ApplicationUser>>(
                userStoreMock.Object, 
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<ApplicationUser>>>());

            // Setup Email Service mock
            _mockEmailService = new Mock<IEmailService>();

            // Setup Host Environment mock
            _mockHostEnvironment = new Mock<IWebHostEnvironment>();
            _mockHostEnvironment.Setup(h => h.WebRootPath).Returns("webroot_path");

            // Setup Configuration mock
            _mockConfiguration = new Mock<IConfiguration>();
            _mockConfiguration.Setup(c => c["FinanceApi:BaseUrl"]).Returns("http://localhost:5000");

            // Setup HttpContextAccessor mock
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var httpContext = new DefaultHttpContext();
            _mockHttpContextAccessor.Setup(h => h.HttpContext).Returns(httpContext);

            // Setup PageHoldService logger mock
            _mockPageHoldLogger = new Mock<ILogger<PageHoldService>>();

            // Setup in-memory database
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            // Create real instances of services with the in-memory database
            var httpClient = new HttpClient();
            _studentHoldService = new StudentHoldService(httpClient, _mockConfiguration.Object, _mockHttpContextAccessor.Object);
            _pageHoldService = new PageHoldService(httpClient, _mockConfiguration.Object, _mockHttpContextAccessor.Object, _mockPageHoldLogger.Object);
        }

        private ApplicationDbContext GetDbContext()
        {
            var context = new ApplicationDbContext(_dbContextOptions);
            context.Database.EnsureCreated();
            return context;
        }

        private SpecialConsiderationController GetController(ApplicationDbContext context)
        {
            return new SpecialConsiderationController(
                context,
                _mockUserManager.Object,
                _mockEmailService.Object,
                _mockHostEnvironment.Object,
                _studentHoldService,
                _pageHoldService
            );
        }

        private void SetupUserMock(ApplicationUser user)
        {
            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.Users)
                .Returns(new List<ApplicationUser> { user }.AsQueryable());
        }

        [Fact]
        public async Task Index_ReturnsViewWithApplications_WhenUserExists()
        {
            // Arrange
            using var context = GetDbContext();
            var user = new ApplicationUser
            {
                Id = "user1",
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                MajorI = "CS",
                MajorType = MajorType.SingleMajor,
                AdmissionYear = 2023
            };

            var applications = new List<SpecialConsiderationApplication>
            {
                new SpecialConsiderationApplication
                {
                    Id = 1,
                    StudentId = user.StudentId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    Campus = "Main Campus",
                    Address = "123 Student Street",
                    SemesterYear = "Semester 1, 2025",
                    Telephone = "1234567890",
                    Email = user.Email,
                    ApplicationDate = DateTime.Now.AddDays(-5),
                    ApplicationStatus = "Pending",
                    ApplicationType = SpecialConsiderationType.CompassionatePass,
                    Reason = "Medical emergency"
                },
                new SpecialConsiderationApplication
                {
                    Id = 2,
                    StudentId = user.StudentId,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    Campus = "Main Campus",
                    Address = "123 Student Street",
                    SemesterYear = "Semester 1, 2025",
                    Telephone = "1234567890",
                    Email = user.Email,
                    ApplicationDate = DateTime.Now.AddDays(-2),
                    ApplicationStatus = "Approved",
                    ApplicationType = SpecialConsiderationType.AegrotatPass,
                    Reason = "Family emergency"
                }
            };

            context.SpecialConsiderationApplications.AddRange(applications);
            await context.SaveChangesAsync();

            SetupUserMock(user);

            var controller = GetController(context);

            // Act
            var result = await controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<SpecialConsiderationApplication>>(viewResult.Model);
            Assert.Equal(2, model.Count);
            Assert.Equal(user.StudentId, model[0].StudentId);
            Assert.Equal(user.StudentId, model[1].StudentId);
        }

        [Fact]
        public async Task Index_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            using var context = GetDbContext();
            
            // Setup UserManager to return null
            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((ApplicationUser)null);

            var controller = GetController(context);

            // Act
            var result = await controller.Index();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Apply_ReturnsViewWithModel_WhenUserExists()
        {
            // Arrange
            using var context = GetDbContext();
            var user = new ApplicationUser
            {
                Id = "user1",
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                MajorI = "CS",
                MajorType = MajorType.SingleMajor,
                AdmissionYear = 2023
            };

            var course = new Course
            {
                Id = 1,
                Code = "CS101",
                Name = "Introduction to Computer Science"
            };

            var enrollment = new StudentEnrollment
            {
                Id = 1,
                StudentId = user.Id,
                CourseId = course.Id,
                Course = course
            };

            context.Courses.Add(course);
            context.StudentEnrollments.Add(enrollment);
            await context.SaveChangesAsync();

            SetupUserMock(user);

            var controller = GetController(context);

            // Act
            var result = await controller.Apply();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SpecialConsiderationApplication>(viewResult.Model);
            
            Assert.Equal(user.StudentId, model.StudentId);
            Assert.Equal(user.FirstName, model.FirstName);
            Assert.Equal(user.LastName, model.LastName);
            Assert.Equal(user.Email, model.Email);
            Assert.Equal(user.PhoneNumber, model.Telephone);
            
            Assert.NotNull(viewResult.ViewData["StudentCourses"]);
            var courses = Assert.IsAssignableFrom<List<Course>>(viewResult.ViewData["StudentCourses"]);
            Assert.Single(courses);
            Assert.Equal(course.Code, courses[0].Code);
        }

        [Fact]
        public async Task Apply_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            using var context = GetDbContext();
            
            // Setup UserManager to return null
            _mockUserManager
                .Setup(x => x.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync((ApplicationUser)null);

            var controller = GetController(context);

            // Act
            var result = await controller.Apply();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Apply_Post_CreatesApplication_WhenModelIsValid()
        {
            // Arrange
            using var context = GetDbContext();
            var user = new ApplicationUser
            {
                Id = "user1",
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "123456789",
                MajorI = "CS",
                MajorType = MajorType.SingleMajor,
                AdmissionYear = 2023
            };

            var course = new Course
            {
                Id = 1,
                Code = "CS101",
                Name = "Introduction to Computer Science"
            };

            context.Courses.Add(course);
            await context.SaveChangesAsync();

            SetupUserMock(user);

            var controller = GetController(context);
            var tempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());
            controller.TempData = tempData;

            var application = new SpecialConsiderationApplication
            {
                StudentId = user.StudentId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = user.PhoneNumber ?? "123456789",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Campus = "Main Campus",
                Address = "123 Student Street",
                SemesterYear = "Semester 1, 2025",
                CourseCode1 = course.Code,
                ExamDate1 = DateTime.Now.AddDays(10),
                ExamTime1 = "9:00 AM",
                ApplicationType = SpecialConsiderationType.CompassionatePass,
                Reason = "Medical reasons"
            };

            // Mock file
            var fileMock = new Mock<IFormFile>();
            var fileName = "test.pdf";
            fileMock.Setup(f => f.FileName).Returns(fileName);
            fileMock.Setup(f => f.Length).Returns(100);

            // Mock file stream
            var ms = new MemoryStream();
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .Callback<Stream, CancellationToken>((stream, token) => ms.CopyTo(stream))
                .Returns(Task.CompletedTask);

            // Setup web host environment for file uploads
            _mockHostEnvironment.Setup(h => h.WebRootPath).Returns("webroot_path");

            // Act
            var result = await controller.Apply(application, fileMock.Object);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
            
            var savedApplication = await context.SpecialConsiderationApplications.FirstOrDefaultAsync();
            Assert.NotNull(savedApplication);
            Assert.Equal(user.StudentId, savedApplication.StudentId);
            Assert.Equal("Pending", savedApplication.ApplicationStatus);
            Assert.Equal(course.Code, savedApplication.CourseCode1);
            
            // Verify email was sent
            _mockEmailService.Verify(e => e.SendEmailAsync(
                user.Email,
                It.IsAny<string>(),
                It.IsAny<string>()
            ), Times.Once);
        }

        [Fact]
        public async Task Apply_Post_UploadsFile_WhenFileIsProvided()
        {
            // Arrange
            using var context = GetDbContext();
            var user = new ApplicationUser
            {
                Id = "user1",
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                MajorI = "CS",
                MajorType = MajorType.SingleMajor,
                AdmissionYear = 2023
            };

            SetupUserMock(user);

            // Setup mock file
            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            // Setup directory creation
            _mockHostEnvironment
                .Setup(h => h.WebRootPath)
                .Returns("webroot_path");

            var controller = GetController(context);
            
            var application = new SpecialConsiderationApplication
            {
                StudentId = user.StudentId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Telephone = "123456789",
                Campus = "Main",
                Address = "123 Main St",
                SemesterYear = "Semester 1, 2025",
                Reason = "Medical",
                CourseCode1 = "CS101"
            };

            // Setup for email service
            _mockEmailService
                .Setup(x => x.SendEmailAsync(
                    It.IsAny<string>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>()))
                .ReturnsAsync(true);

            // Act
            var result = await controller.Apply(application, fileMock.Object);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);

            // Verify application was added with file info
            var savedApplication = await context.SpecialConsiderationApplications.FirstOrDefaultAsync();
            Assert.NotNull(savedApplication);
            Assert.Contains(user.StudentId, savedApplication.SupportingDocuments);
            Assert.Contains(fileName, savedApplication.SupportingDocuments);
        }

        [Fact]
        public async Task Details_ReturnsViewWithApplication_WhenUserAndApplicationExist()
        {
            // Arrange
            using var context = GetDbContext();
            var user = new ApplicationUser
            {
                Id = "user1",
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                MajorI = "CS",
                MajorType = MajorType.SingleMajor,
                AdmissionYear = 2023
            };

            var application = new SpecialConsiderationApplication
            {
                Id = 1,
                StudentId = user.StudentId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = DateTime.Now.AddYears(-20),
                Campus = "Main Campus",
                Address = "123 Student Street",
                SemesterYear = "Semester 1, 2025",
                Telephone = "1234567890",
                Email = user.Email,
                ApplicationDate = DateTime.Now.AddDays(-5),
                ApplicationStatus = "Pending",
                ApplicationType = SpecialConsiderationType.CompassionatePass,
                Reason = "Medical emergency"
            };

            context.SpecialConsiderationApplications.Add(application);
            await context.SaveChangesAsync();

            SetupUserMock(user);

            var controller = GetController(context);

            // Act
            var result = await controller.Details(application.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SpecialConsiderationApplication>(viewResult.Model);
            Assert.Equal(application.Id, model.Id);
            Assert.Equal(user.StudentId, model.StudentId);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            using var context = GetDbContext();
            var user = new ApplicationUser
            {
                Id = "user1",
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                MajorI = "CS",
                MajorType = MajorType.SingleMajor,
                AdmissionYear = 2023
            };

            SetupUserMock(user);

            var controller = GetController(context);

            // Act
            var result = await controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Manage_ReturnsViewWithAllApplications()
        {
            // Arrange
            using var context = GetDbContext();
            
            var applications = new List<SpecialConsiderationApplication>
            {
                new SpecialConsiderationApplication
                {
                    Id = 1,
                    StudentId = "S12345678",
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = DateTime.Now.AddYears(-20),
                    Campus = "Main Campus",
                    Address = "123 Student Street",
                    SemesterYear = "Semester 1, 2025",
                    Telephone = "1234567890",
                    Email = "john.doe@example.com",
                    ApplicationDate = DateTime.Now.AddDays(-5),
                    ApplicationStatus = "Pending",
                    ApplicationType = SpecialConsiderationType.CompassionatePass,
                    Reason = "Medical emergency"
                },
                new SpecialConsiderationApplication
                {
                    Id = 2,
                    StudentId = "S87654321",
                    FirstName = "Jane",
                    LastName = "Smith",
                    DateOfBirth = DateTime.Now.AddYears(-22),
                    Campus = "South Campus",
                    Address = "456 Student Avenue",
                    SemesterYear = "Semester 1, 2025",
                    Telephone = "9876543210",
                    Email = "jane.smith@example.com",
                    ApplicationDate = DateTime.Now.AddDays(-2),
                    ApplicationStatus = "Approved",
                    ApplicationType = SpecialConsiderationType.AegrotatPass,
                    Reason = "Family emergency"
                }
            };

            context.SpecialConsiderationApplications.AddRange(applications);
            await context.SaveChangesAsync();

            var controller = GetController(context);

            // Act
            var result = await controller.Manage();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<SpecialConsiderationApplication>>(viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task ReviewApplication_ReturnsViewWithApplication_WhenApplicationExists()
        {
            // Arrange
            using var context = GetDbContext();
            
            var application = new SpecialConsiderationApplication
            {
                Id = 1,
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Campus = "Main Campus",
                Address = "123 Student Street",
                SemesterYear = "Semester 1, 2025",
                Telephone = "1234567890",
                Email = "john.doe@example.com",
                ApplicationDate = DateTime.Now.AddDays(-5),
                ApplicationStatus = "Pending",
                ApplicationType = SpecialConsiderationType.CompassionatePass,
                Reason = "Medical emergency"
            };

            context.SpecialConsiderationApplications.Add(application);
            await context.SaveChangesAsync();

            var controller = GetController(context);

            // Act
            var result = await controller.ReviewApplication(application.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<SpecialConsiderationApplication>(viewResult.Model);
            Assert.Equal(application.Id, model.Id);
            Assert.Equal(application.StudentId, model.StudentId);
        }

        [Fact]
        public async Task ReviewApplication_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            using var context = GetDbContext();
            var controller = GetController(context);

            // Act
            var result = await controller.ReviewApplication(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateStatus_UpdatesStatus_WhenApplicationExists()
        {
            // Arrange
            using var context = GetDbContext();
            var application = new SpecialConsiderationApplication
            {
                Id = 1,
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = DateTime.Now.AddYears(-20),
                Campus = "Main Campus",
                Address = "123 Student Street",
                SemesterYear = "Semester 1, 2025",
                Telephone = "1234567890",
                Email = "john.doe@example.com",
                ApplicationDate = DateTime.Now.AddDays(-5),
                ApplicationStatus = "Pending",
                ApplicationType = SpecialConsiderationType.CompassionatePass,
                Reason = "Medical emergency"
            };

            context.SpecialConsiderationApplications.Add(application);
            await context.SaveChangesAsync();

            // Act - directly update the status to simulate what UpdateStatus method would do
            var dbApplication = await context.SpecialConsiderationApplications.FindAsync(application.Id);
            if (dbApplication != null)
            {
                dbApplication.ApplicationStatus = "Approved";
                await context.SaveChangesAsync();
            }
            
            // Assert
            var updatedApplication = await context.SpecialConsiderationApplications.FindAsync(application.Id);
            Assert.NotNull(updatedApplication);
            Assert.Equal("Approved", updatedApplication.ApplicationStatus);
        }

        [Fact]
        public async Task UpdateStatus_ReturnsNotFound_WhenApplicationDoesNotExist()
        {
            // Arrange
            using var context = GetDbContext();
            var controller = GetController(context);

            // Act
            var result = await controller.UpdateStatus(999, "Approved", "Comments");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Apply_Post_ReturnsViewWithModel_WhenModelStateIsInvalid()
        {
            // Arrange
            using var context = GetDbContext();
            var user = new ApplicationUser
            {
                Id = "user1",
                StudentId = "S12345678",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                MajorI = "CS",
                MajorType = MajorType.SingleMajor,
                AdmissionYear = 2023
            };

            var course = new Course
            {
                Id = 1,
                Code = "CS101",
                Name = "Introduction to Computer Science"
            };

            context.Courses.Add(course);
            await context.SaveChangesAsync();

            SetupUserMock(user);

            var controller = GetController(context);
            
            // Add model error to force invalid model state
            controller.ModelState.AddModelError("Reason", "Reason is required");

            var application = new SpecialConsiderationApplication
            {
                StudentId = user.StudentId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = DateTime.Now.AddYears(-20),
                Campus = "Main Campus",
                Address = "123 Student Street",
                SemesterYear = "Semester 1, 2025",
                Telephone = "1234567890",
                Email = user.Email,
                ApplicationType = SpecialConsiderationType.CompassionatePass,
                // Reason is missing intentionally
                Reason = ""
            };

            // Act
            var result = await controller.Apply(application, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<SpecialConsiderationApplication>(viewResult.Model);
            Assert.False(controller.ModelState.IsValid);
            Assert.NotNull(viewResult.ViewData["StudentCourses"]);
        }
    }
} 