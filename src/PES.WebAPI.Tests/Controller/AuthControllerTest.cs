using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PES.Domain.DTOs.User;
using PES.Domain.Tests;
using PES.Presentation.Controllers.V1;
using static PES.Domain.DTOs.User.RegisterRequest;
using LoginRequest = PES.Domain.DTOs.User.LoginRequest;
using RegisterRequest = PES.Domain.DTOs.User.RegisterRequest;

namespace PES.WebAPI.Tests.Controller
{
    public class AuthControllerTest : SetUpTest
    {
        private readonly AuthController _authController;

        public AuthControllerTest()
        {
            _authController = new AuthController(_userServiceMock.Object);
        }

        [Fact]
        public async Task Register_WithValidRequest_ReturnsOkResult()
        {

            var request = _fixture.Build<RegisterRequest>().Create();
            var response = _fixture.Build<AuthDTO>().Create();

            _userServiceMock.Setup(x => x.Register(request)).ReturnsAsync(response);

            var result = await _authController.Register(request);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<AuthDTO>(okObjectResult.Value);

            Assert.Equal(response, actualResponse);
            _userServiceMock.Verify(x => x.Register(request), Times.Once);
        }


        [Fact]
        public async Task Login_WithValidRequest_ReturnsOkResult()
        {
            var request = _fixture.Build<LoginRequest>().Create();
            var response = _fixture.Build<AuthDTO>().Create();

            _userServiceMock.Setup(x => x.Login(request)).ReturnsAsync(response);

            var result = await _authController.Login(request);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<AuthDTO>(okObjectResult.Value);

            Assert.Equal(response, actualResponse);
            _userServiceMock.Verify(x => x.Login(request), Times.Once);

        }


        [Fact]
        public async Task ForgetPassword_WithValidEmail_ReturnsOkResult()
        {
        
            var mock = _fixture.Build<string>().Create();

            _userServiceMock.Setup(x => x.ForgetPassword(It.IsAny<string>())).ReturnsAsync(mock);

            var result = await _authController.ForgetPasswrod(It.IsAny<string>());

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<string>(okObjectResult.Value);

            Assert.Equal(mock, actualResponse);
            _userServiceMock.Verify(x => x.ForgetPassword(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task ChangePassword_WithValidRequest_ReturnsOkResult()
        {
            var emailMock = _fixture.Build<string>().Create();
            var ChangePasswordRequestMock = _fixture.Build<ChangePasswordRequest>().Create();

            _userServiceMock.Setup(x => x.ChangePassword(ChangePasswordRequestMock, emailMock)).ReturnsAsync(true);

            var result = await _authController.ChangPassword(emailMock, ChangePasswordRequestMock);


            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<string>(okObjectResult.Value);

            Assert.Equal("Change password Successfully", actualResponse);
            _userServiceMock.Verify(x => x.ChangePassword(ChangePasswordRequestMock, emailMock), Times.Once);

        }

        [Fact]
        public async Task ConfirmEmail_WithValidEmail_ReturnsOkResult()
        {
            var mock = _fixture.Build<string>().Create();
            _userServiceMock.Setup(x => x.ForgetPassword(It.IsAny<string>())).ReturnsAsync(mock);

            var result = await _authController.ForgetPasswrod(It.IsAny<string>());

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var actualResponse = Assert.IsType<string>(okObjectResult.Value);

            Assert.Equal(mock, actualResponse);
            _userServiceMock.Verify(x => x.ForgetPassword(It.IsAny<string>()), Times.Once);
        }


    }
}