using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PES.WebAPI.Tests
{
    public class ProgramTest
    {
        private readonly HttpClient _httpClient;
        private readonly WebApplicationFactory<Program> _application;
        public ProgramTest()
        {
            _application = new WebApplicationFactory<Program>();
            _httpClient = _application.CreateClient();
        }
    }
}
