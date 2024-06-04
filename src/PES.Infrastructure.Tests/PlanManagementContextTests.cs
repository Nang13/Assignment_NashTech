using AutoFixture;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PES.Domain.Entities.Model;
using PES.Domain.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.Infrastructure.Tests
{
    public class PlanManagementContextTests : SetUpTest ,IDisposable
    {
        [Fact]
        public async Task PlantManagementContext_CategoryDbSetShouldReturnCorrectData()
        {
            var mockData = _fixture.Build<Category>().Without(x => x.Products).CreateMany(10).ToList();
            await _context.Categories.AddRangeAsync(mockData);
            await _context.SaveChangesAsync();

            var result = await _context.Categories.ToListAsync();
            result.Should().BeEquivalentTo(mockData);
        }
    }
}
