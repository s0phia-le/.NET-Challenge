using challenge.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Net;
using System.Net.Http;
using System.Text;
using challenge.DTO;
using CodeCodeChallenge.Tests.Integration.Helpers;
using CodeCodeChallenge.Tests.Integration.Extensions;

namespace code_challenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer(WebHost.CreateDefaultBuilder()
                .UseStartup<TestServerStartup>()
                .UseEnvironment("Development"));

            _httpClient = _testServer.CreateClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public void CreateCompensation_Returns_Created()
        {
            // Arrange
            var expectedFName = "John";
            var expectedLName = "Lennon";
            var expectedPosition = "Development Manager";
            var expectedDepartment = "Engineering";
            var expectedEffectiveDate = new DateTime();
            var expectedSalary = 75000.00m;
            var compensation = new CompensationDto()
            {
                EmployeeID = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                EffectiveDate = expectedEffectiveDate,
                Salary = 75000.00m
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
               new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<Compensation>();
            Assert.IsNotNull(newCompensation.Employee);
            Assert.AreEqual(expectedFName,newCompensation.Employee.FirstName);
            Assert.AreEqual(expectedLName, newCompensation.Employee.LastName);
            Assert.AreEqual(expectedDepartment,newCompensation.Employee.Department);
            Assert.AreEqual(expectedPosition,newCompensation.Employee.Position);
            Assert.AreEqual(expectedEffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(expectedSalary,newCompensation.Salary);
        }
        
        [TestMethod]
        public void CreateCompensation_NonExistentEmployee_Returns_NotFound()
        {
            // Arrange
            var compensation = new CompensationDto()
            {
                EmployeeID = "Invalid_ID",
                EffectiveDate = new DateTime(),
                Salary = 75000.00m
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
        
        [TestMethod]
        public void CreateCompensation_MissingFields_Returns_NotNull_Created()
        {
            // Arrange
            var compensation = new CompensationDto()
            {
                EmployeeID = "16a596ae-edd3-4847-99fe-c4518e82c86f"
            };

            var requestContent = new JsonSerialization().ToJson(compensation);

            // Execute
            var postRequestTask = _httpClient.PostAsync("api/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json"));
            var response = postRequestTask.Result;

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            
            var newCompensation = response.DeserializeContent<Compensation>();
            
            Assert.AreEqual(compensation.EmployeeID, newCompensation.Employee.EmployeeId);
            Assert.IsNotNull( newCompensation.Salary);
            Assert.IsNotNull(newCompensation.EffectiveDate);
        }
        
        [TestMethod]
        public void GetCompensationById_Returns_Ok()
        {
            // Arrange
            var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            var expectedFName = "John";
            var expectedLName = "Lennon";
            var expectedPosition = "Development Manager";
            var expectedDepartment = "Engineering";
            var expectedEffectiveDate = new DateTime();
            var expectedSalary = 75000.00m;
            
            var compensation = new CompensationDto()
            {
                EmployeeID = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                EffectiveDate = expectedEffectiveDate,
                Salary = 75000.00m
            };
        
            var requestContent = new JsonSerialization().ToJson(compensation);
        
            // Execute
            _httpClient.PostAsync("api/compensation",
                new StringContent(requestContent, Encoding.UTF8, "application/json")).Wait();
            // var postResponse = postRequestTask.Result;
            // var postCompensation = postResponse.DeserializeContent<Compensation>();

            var getRequestTask = _httpClient.GetAsync($"api/compensation/{employeeId}");
            var response = getRequestTask.Result;
        
            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        
            var newCompensation = response.DeserializeContent<Compensation>();
            
            Assert.IsNotNull(newCompensation);
            Assert.IsNotNull(newCompensation.Employee);
            Assert.IsNotNull(newCompensation.Salary);
            Assert.IsNotNull(newCompensation.EffectiveDate);
            
            // Employee Relation
            Assert.AreEqual(expectedFName,newCompensation.Employee.FirstName);
            Assert.AreEqual(expectedLName, newCompensation.Employee.LastName);
            Assert.AreEqual(expectedDepartment,newCompensation.Employee.Department);
            Assert.AreEqual(expectedPosition,newCompensation.Employee.Position);
            
            // Compensation Details
            Assert.AreEqual(expectedEffectiveDate, newCompensation.EffectiveDate);
            Assert.AreEqual(expectedSalary,newCompensation.Salary);
        }
        
        [TestMethod]
        public void GetCompensationById_Returns_NotFound()
        {
            // Arrange
            var invalidID = "Invalid_ID";

            var getRequestTask = _httpClient.GetAsync($"api/compensation/{invalidID}");
            var response = getRequestTask.Result;
        
            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}