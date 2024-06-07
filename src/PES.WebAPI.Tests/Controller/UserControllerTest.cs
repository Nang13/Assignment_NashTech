using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.DTOs.User;
using PES.Domain.Entities.Model;
using PES.Domain.Tests;
using PES.Infrastructure.Common;
using PES.Presentation.Controllers.V1;

namespace PES.WebAPI.Tests.Controller
{
    public class UserControllerTest : SetUpTest
    {
        private readonly UserController _userController;

        public UserControllerTest()
        {
            _userController = new UserController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Delete_WithValidId_ReturnsOkResult()
        {
            var mock = _fixture.Build<ApplicationUser>().Create();
            _userServiceMock.Setup(x => x.DisableUser(mock.Id)).ReturnsAsync(mock);

            var result = await _userController.Delete(mock.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okObjectResult.Value);

            _userServiceMock.Verify(x => x.DisableUser(mock.Id), Times.Once);
        }


        [Fact]
        public async Task EnableUser_WithValidId_ReturnsOkResult()
        {
            var mock = _fixture.Build<ApplicationUser>().Create();
            _userServiceMock.Setup(x => x.EnableUser(mock.Id)).ReturnsAsync(mock);

            var result = await _userController.EnableUser(mock.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okObjectResult.Value);

            _userServiceMock.Verify(x => x.EnableUser(mock.Id), Times.Once);

        }

        [Fact]
        public async Task Update_WithValidRequest_ReturnsOkResult()
        {

            var request = _fixture.Build<UpdateUseRequest>().Create();
            var response = _fixture.Build<ApplicationUser>().Create();

            _userServiceMock.Setup(x => x.UpdateUser(response.Id, request)).ReturnsAsync
            (response);

            var result = await _userController.Update(response
            .Id, request);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualUser = Assert.IsType<ApplicationUser>(okObjectResult.Value);

            Assert.Equal(response, actualUser);
            _userServiceMock.Verify(x => x.UpdateUser(response.Id, request), Times.Once);


        }


        [Fact]
        public async Task Get_WithValidId_ReturnsOkResult()
        {

            var mock = _fixture.Build<ApplicationUser>().Create();

            _userServiceMock.Setup(x => x.GetUser(mock.Id)).ReturnsAsync(mock);

            var result = await _userController.Get(mock.Id);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualUser = Assert.IsType<ApplicationUser>(okObjectResult.Value);

            Assert.Equal(mock, actualUser);
            _userServiceMock.Verify(x => x.GetUser(mock.Id), Times.Once);


        }

        [Fact]
        public async Task Get_WithValidFilterAndPagination_ReturnsOkResult()
        {
            var request = _fixture.Build<GetProductRequest>().Create();
            var response = _fixture.Build<Pagination<UserDTO>>().Create();

            _userServiceMock.Setup(x => x.GetUsers(request)).ReturnsAsync(response);

            var result = await _userController.Get(request.Filter, request.PageNumber, request.PageSize);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualUsers = Assert.IsType<Pagination<UserDTO>>(okObjectResult.Value);

            Assert.Equal(response, actualUsers);
            _userServiceMock.Verify(x => x.GetUsers(It.IsAny<GetProductRequest>()), Times.Once);
        }




    }
}