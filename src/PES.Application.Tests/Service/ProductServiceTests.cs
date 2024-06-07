using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using PES.Application.IService;
using PES.Application.Service;
using PES.Domain.DTOs.ProductDTO;
using PES.Domain.Entities.Model;
using PES.Domain.Tests;

namespace PES.Application.Tests.Service
{
    public class ProductServiceTests : SetUpTest
    {
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _productService = new ProductService(_unitOfWorkMock.Object, _claimServiceMock.Object, _categoryServiceMock.Object,_redisMock.Object);
        }

        [Fact]
        public async Task AddNewProduct_ReturnProductResponse()
        {
            var mock = _fixture.Build<AddNewProductRequest>().Create();
            mock.MainImage = mock.ListImages.FirstOrDefault();
            mock.ListImages.RemoveAt(0);
            Guid productId = Guid.NewGuid();
            Product product = mock.ToDTO();
            product.Id = productId;

            _unitOfWorkMock.Setup(x => x.ProductRepository.AddAsync(product)).ReturnsAsync(product);
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync());

        }

        [Fact]
        public async Task AddInconsequentialImage_ShouldAddImagesCorrectly()
        {
            var mock = _fixture.Build<string>().CreateMany(2).ToList();
            var productId = Guid.NewGuid();

            _unitOfWorkMock
              .Setup(repo => repo.ProductImageRepository.AddRangeAsync(It.IsAny<List<ProductImage>>()))
              .Returns(Task.CompletedTask)
              .Verifiable();

            await _productService.AddInconsequentialImage(mock, productId);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.ProductImageRepository.AddRangeAsync(It.Is<List<ProductImage>>(productImages =>
                productImages.Count() == 2
            )), Times.Once);

           
                

            _unitOfWorkMock.Verify(uow => uow.ProductImageRepository, Times.Once);

        }

        [Fact]
        public async Task AddNutrionInformation_ShouldAddNutritionInfoCorrectly()
        {
            // Arrange
            var mock = _fixture.Build<NutrionInforrmationRequest>().Create();
            var productId = Guid.NewGuid();
            var nutrionData = mock.MapDTO(productId);

            _unitOfWorkMock.Setup(x => x.NutrionInfoRepository.AddAsync(nutrionData));
            _unitOfWorkMock.Setup(uow => uow.SaveChangeAsync());

            // Act
            await _productService.AddNutrionInformation(mock, productId);

            // Assert
            _unitOfWorkMock.Verify(repo => repo.NutrionInfoRepository.AddAsync(It.Is<NutritionInformation>(nutrition =>
                nutrition.ProductId == productId &&
                nutrition.Calories == nutrition.Calories &&
                nutrition.Protein == nutrition.Protein
            )), Times.Once);

            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }

        [Fact]
        public async Task AddImportantInformation_ShouldAddNutritionInfoCorrectly()
        {

            var mock = _fixture.Build<ImportantImformationRequest>().Create();
            var productId = Guid.NewGuid();
            var information = mock.MapperDTO(productId);

            _unitOfWorkMock.Setup(x => x.ImportantInfoRepository.AddAsync(information));
            _unitOfWorkMock.Setup(x => x.SaveChangeAsync());

            await _productService.AddImportantInformation(mock, productId);


            _unitOfWorkMock.Verify(repo => repo.ImportantInfoRepository.AddAsync(It.Is<ImportantInformation>(nutrition =>
               nutrition.ProductId == productId &&
               nutrition.Ingredients == nutrition.Ingredients &&
               nutrition.Directions == nutrition.Directions
           )), Times.Once);

            _unitOfWorkMock.Verify(uow => uow.SaveChangeAsync(), Times.Once);
        }


      


    }
}