using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using challenge.Models;
using code_challenge.Tests.Integration;
using CodeChallenge.Models;

using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using challenge.DTO;

namespace CodeCodeChallenge.Tests.Integration
{
    [TestClass]
    public class ReportingStructureControllerTests
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
        public void ReportingStructure_TreeRoot()
        {
            var exp = new ReportingStructure()
            {
                numberOfReports = 4,
                Manager = new Employee()
                {
                    EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                    FirstName = "John",
                    LastName = "Lennon",
                    Position = "Development Manager",
                    Department = "Engineering",
                    DirectReports = new List<Employee>()
                    {
                        new Employee() {
                            EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3"
                        }, new Employee() {
                            EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                            DirectReports = new List<Employee>()
                            {
                                new Employee() {
                                     EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309",
                                },new Employee() {
                                    EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                                }
                            }
                        }
                    }
                }
            };
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{exp.Manager.EmployeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var report = response.DeserializeContent<ReportingStructure>();

            Assert.IsNotNull(report);
            Assert.IsNotNull(report.numberOfReports);
            Assert.IsNotNull(report.Manager);
            Assert.AreEqual(exp.Manager.FirstName, report.Manager.FirstName);
            Assert.AreEqual(exp.Manager.LastName, report.Manager.LastName);
            Assert.AreEqual(exp.Manager.Department, report.Manager.Department);
            Assert.AreEqual(exp.Manager.Position, report.Manager.Position);

            Assert.AreEqual(exp.Manager.DirectReports[0].EmployeeId, report.Manager.DirectReports[0].EmployeeId);
            Assert.AreEqual(exp.Manager.DirectReports[1].EmployeeId, report.Manager.DirectReports[1].EmployeeId);
            Assert.AreEqual(exp.Manager.DirectReports[1].DirectReports[0].EmployeeId, report.Manager.DirectReports[1].DirectReports[0].EmployeeId);
            Assert.AreEqual(exp.Manager.DirectReports[1].DirectReports[1].EmployeeId, report.Manager.DirectReports[1].DirectReports[1].EmployeeId);

            Assert.AreEqual(exp.numberOfReports, report.numberOfReports);
        }

        [TestMethod]
        public void ReportingStructure_TreeLeaf()
        {
            var exp = new ReportingStructure()
            {
                numberOfReports = 0,
                Manager = new Employee()
                {
                    EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                    FirstName = "George",
                    LastName = "Harrison",
                    Position = "Developer III",
                    Department = "Engineering",
                    DirectReports = new List<Employee>()
                }
            };
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{exp.Manager.EmployeeId}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var report = response.DeserializeContent<ReportingStructure>();

            Assert.IsNotNull(report);
            Assert.IsNotNull(report.numberOfReports);
            Assert.IsNotNull(report.Manager);

            Assert.AreEqual(exp.Manager.FirstName, report.Manager.FirstName);
            Assert.AreEqual(exp.Manager.LastName, report.Manager.LastName);
            Assert.AreEqual(exp.Manager.Department, report.Manager.Department);
            Assert.AreEqual(exp.Manager.Position, report.Manager.Position);

            Assert.AreEqual(0, report.Manager.DirectReports.Count);

            Assert.AreEqual(exp.numberOfReports, report.numberOfReports);
        }

        [TestMethod]
        public void ReportingStructure_Returns_NotFound()
        {
            String Invalid_ID = "0";
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{Invalid_ID}");
            var response = getRequestTask.Result;

            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }

        [TestMethod]
        public void ReportingStructure_New_Manager_Addition()
        {
            // Arrange
            var newManager = new EmployeeDto()
            {
                FirstName = "Elvis",
                LastName = "Presley",
                Position = "Chief Operations Officer",
                Department = "Management",
                DirectReports = new List<String> { "16a596ae-edd3-4847-99fe-c4518e82c86f" }
            };

            var expectedResponse = new ReportingStructure()
            {
                numberOfReports = 5,
                Manager = new Employee()
                {
                    FirstName = "Elvis",
                    LastName = "Presley",
                    Position = "Chief Operations Officer",
                    Department = "Management",
                    DirectReports = new List<Employee>()
                    {
                        new Employee()
                        {
                            EmployeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f",
                            DirectReports = new List<Employee>() {
                                new Employee() {
                                    EmployeeId = "b7839309-3348-463b-a7e3-5de1c168beb3",

                                },new Employee() {
                                    EmployeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f",
                                    DirectReports = new List<Employee>()
                                    {
                                        new Employee() {
                                            EmployeeId = "62c1084e-6e34-4630-93fd-9153afb65309",
                                        },new Employee() {
                                            EmployeeId = "c0c2293d-16bd-4603-8e08-638a9d18b22c",
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

             var newManagerContent = new JsonSerialization().ToJson(newManager);
            
            // Execute Create Employee for Manager
            var postRequestTask = _httpClient.PostAsync("api/employee",
                new StringContent(newManagerContent, Encoding.UTF8, "application/json"));
            var managerCreationResponse = postRequestTask.Result;
            
            // Assert Manager Creation
            Assert.AreEqual(HttpStatusCode.Created, managerCreationResponse.StatusCode);
            
            // Deserialize Creation Response to use ID 
            var manager = managerCreationResponse.DeserializeContent<Employee>();
            
            // Execute Reporting Structure for Manager
            var getRequestTask = _httpClient.GetAsync($"api/reporting-structure/{manager.EmployeeId}");
            var response = getRequestTask.Result;
            
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            
            var report = response.DeserializeContent<ReportingStructure>();
            Assert.IsNotNull(report);
            Assert.IsNotNull(report.numberOfReports);
            Assert.IsNotNull(report.Manager);
            
            // First Level of Tree
            Assert.AreEqual(expectedResponse.numberOfReports, report.numberOfReports);
            Assert.AreEqual(expectedResponse.Manager.FirstName, report.Manager.FirstName);
            Assert.AreEqual(expectedResponse.Manager.LastName, report.Manager.LastName);
            Assert.AreEqual(expectedResponse.Manager.Department, report.Manager.Department);
            Assert.AreEqual(expectedResponse.Manager.Position, report.Manager.Position);
            
            // Second Level of Tree {John Lennon}
            Assert.AreEqual(expectedResponse.Manager.DirectReports[0].EmployeeId, 
                report.Manager.DirectReports[0].EmployeeId);

            // Third Level of Tree {Paul McCartney , Ringo Starr}
            Assert.AreEqual(expectedResponse.Manager
                    .DirectReports[0]
                    .DirectReports[0].EmployeeId,
                report.Manager
                    .DirectReports[0]
                    .DirectReports[0].EmployeeId);
            
            Assert.AreEqual(expectedResponse.Manager
                    .DirectReports[0]
                    .DirectReports[1].EmployeeId, 
                report.Manager
                    .DirectReports[0]
                    .DirectReports[1].EmployeeId);
            
            // Fourth Level of Tree {Pete Best , George Harrison}
            Assert.AreEqual(expectedResponse.Manager
                    .DirectReports[0]
                    .DirectReports[1]
                    .DirectReports[0].EmployeeId,
                report.Manager
                    .DirectReports[0]
                    .DirectReports[1]
                    .DirectReports[0].EmployeeId);
            Assert.AreEqual(expectedResponse.Manager
                    .DirectReports[0]
                    .DirectReports[1]
                    .DirectReports[1].EmployeeId,
                report.Manager
                    .DirectReports[0]
                    .DirectReports[1]
                    .DirectReports[1].EmployeeId);
        }
    }
}