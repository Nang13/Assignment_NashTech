using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using PES.Domain.DTOs.CategoryDTO;
using PES.Domain.Entities.Model;
using PES.Domain.Tests;
using PES.Presentation.Controllers.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PES.Presentation.Tests.Controller
{
    [Collection("CategoryIntergrationTest")]


    public class CategoryIntergrationTest :SetUpTest, IClassFixture<WebApplicationFactory<Program>> 
    {
       
        private readonly HttpClient _client;
        public CategoryIntergrationTest(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Index_WhenCalled_ReturnsApplicationForm()
        {
            var response = await _client.GetAsync("/api/v1/Category");
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");


            var responseContent = await response.Content.ReadAsStringAsync();
            var invoices = JsonSerializer.Deserialize<List<CategoryResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            invoices.Should().NotBeNull();
            //invoices.Should().HaveCount(1);
        }

        [Theory]
        [InlineData("78afaccf-33f3-4010-a362-812affb31876")]
        public  async Task GetCategoryById_ReturnSuceesCorrectData(string categoryId)
        {
            var response = await _client.GetAsync($"api/v1/Category/{categoryId}");
            Console.WriteLine(response);
            response.EnsureSuccessStatusCode();
            response.Content.Headers.ContentType.Should().NotBeNull();
            response.Content.Headers.ContentType!.ToString().Should().Be("application/json; charset=utf-8");
            var responseContent = await response.Content.ReadAsStringAsync();
            var categories = JsonSerializer.Deserialize<List<CategoryResponse>>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            categories.Should().NotBeNull();
        }


        //? Add new Category
        [Fact]
        public async Task AddNewCategory_ReturnSuccess()
        {
            var mock = _fixture.Build<AddNewCategoryRequest>()
                    .With(x => x.CategoryName, "Test1")
                    .With(x => x.CategoryDescription, "Breads & Bakery is Main Category")
                    .With(x => x.CategoryMain, "Test1")
                    .Create();

            mock.CategoryParentId = null;
            var jsonContent = JsonSerializer.Serialize(mock);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/v1/Category", stringContent);
            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType;
            contentType.Should().NotBeNull();
            contentType!.ToString().Should().Be("application/json; charset=utf-8");

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ResponseObject>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            responseObject.Should().NotBeNull();

        }

        [Fact]
        public async Task AddNewSubCategory_ReturnSuccess()
        {
            var mock = _fixture.Build<AddNewCategoryRequest>()
                   .With(x => x.CategoryName, "Test1")
                   .With(x => x.CategoryDescription, "Breads & Bakery is Main Category")
                   .With(x => x.CategoryParentId, Guid.Parse("8c33a800-d955-49ae-8c9b-972017fe59ca"))
                   .Create();

            mock.CategoryParentId = null;
            var jsonContent = JsonSerializer.Serialize(mock);
            var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("api/v1/Category", stringContent);
            response.EnsureSuccessStatusCode();

            var contentType = response.Content.Headers.ContentType;
            contentType.Should().NotBeNull();
            contentType!.ToString().Should().Be("application/json; charset=utf-8");

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<ResponseObject>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            responseObject.Should().NotBeNull();
        }








    }
}
