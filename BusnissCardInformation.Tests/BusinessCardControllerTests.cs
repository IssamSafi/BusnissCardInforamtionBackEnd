using BusnissCardInforamtion.Controllers;
using BusnissCardInforamtion.Model;
using BusnissCardInforamtion.Repository;
using BusnissCardInforamtion.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusnissCardInformation.Tests
{
    public class BusinessCardControllerTests
    {

        private readonly Mock<IBusinessCardRepository> _mockRepository;
        private readonly Mock<IBusinessCardServices> _mockServices;
        private readonly BusinessCardController _controller;

        public BusinessCardControllerTests()
        {
            _mockRepository = new Mock<IBusinessCardRepository>();
            _mockServices = new Mock<IBusinessCardServices>();
            _controller = new BusinessCardController(_mockRepository.Object, _mockServices.Object);
        }

        [Fact]
        public void GetBusinessCards_ReturnsOkResult_WithListOfBusinessCards()
        {
            
            var businessCards = new List<BusinessCard>
        {
            new BusinessCard { Id = 1, Name = "Issam Safi" },
            new BusinessCard { Id = 2, Name = "Issam Safi" }
        };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(businessCards);

            var result = _controller.GetBusinessCards();

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCards = Assert.IsType<List<BusinessCard>>(okResult.Value);
            Assert.Equal(2, returnedCards.Count);
        }

        [Fact]
        public void CreateBusinessCard_ReturnsCreatedResult_WhenSuccessful()
        {
            
            var businessCard = new BusinessCard { Name = "Issam Safi" }; 
            var createdCard = new BusinessCard { Id = 1, Name = "Issam Safi" }; 
            _mockServices.Setup(service => service.Create(businessCard)).Returns(createdCard);

            var result = _controller.CreateBusinessCard(businessCard);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetBusinessCardsByid", createdResult.ActionName);
            Assert.Equal(1, ((BusinessCard)createdResult.Value).Id); 
        }

        [Fact]
        public void CreateBusinessCardReturnsCreatedResult_WhenSuccessful()
        {
            
            var businessCard = new BusinessCard { Name = "Issam Safi" }; 
            var createdCard = new BusinessCard { Id = 1, Name = "Issam Safi" }; 
            _mockServices.Setup(service => service.Create(businessCard)).Returns(createdCard);

            
            var result = _controller.CreateBusinessCard(businessCard);

            
            var createdResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetBusinessCardsByid", createdResult.ActionName);
            Assert.Equal(1, ((BusinessCard)createdResult.Value).Id); 
        }

        [Fact]
        public void DeleteBusinessCard_ReturnsNoContent_WhenSuccessful()
        {
            int idToDelete = 1;
            _mockRepository.Setup(repo => repo.Delete(idToDelete)).Verifiable();

            
            var result = _controller.DeleteBusinessCard(idToDelete);

            Assert.IsType<NoContentResult>(result);
            _mockRepository.Verify(repo => repo.Delete(idToDelete), Times.Once);
        }

        [Fact]
        public void FilterBusinessCards_ReturnsOkResult_WithFilteredCards()
        {
            
            var filteredCards = new List<BusinessCard>
            {
                new BusinessCard { Id = 1, Name = "Issam Safi" }
            };
            _mockRepository.Setup(repo => repo.Filter("Issam ", null, null, null, null)).Returns(filteredCards);

            var result = _controller.FilterBusinessCards(name: "Issam ");

            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCards = Assert.IsType<List<BusinessCard>>(okResult.Value);
            Assert.Single(returnedCards);
        }

        [Fact]
        public void ImportFromCsv_ReturnsNoContent_WhenFileIsValid()
        {
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(10); 

            
            var result = _controller.ImportFromCsv(mockFile.Object);

            
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void ImportFromCsv_ReturnsBadRequest_WhenFileIsEmpty()
        {
            
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0); 

           
            var result = _controller.ImportFromCsv(mockFile.Object);

            
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void ExportToXml_ReturnsFileResult_WithValidData()
        {
          
            var businessCards = new List<BusinessCard>
            {
                new BusinessCard { Id = 1, Name = "Issam Safi" }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(businessCards);
            _mockServices.Setup(service => service.ExportToXml(businessCards)).Returns("<BusinessCards></BusinessCards>");

           
            var result = _controller.ExportToXml();

            
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/xml", fileResult.ContentType);
            Assert.Equal("BusinessCards.xml", fileResult.FileDownloadName);
        }

        [Fact]
        public void ExportToCsv_ReturnsFileResult_WithValidData()
        {
          
            var businessCards = new List<BusinessCard>
            {
                new BusinessCard { Id = 1, Name = "Issam Safi" }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(businessCards);
            _mockServices.Setup(service => service.ExportToCsv(businessCards)).Returns("Id,Name\n1,Issam Safi");

            
            var result = _controller.ExportToCsv();

            
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("text/csv", fileResult.ContentType);
            Assert.Equal("BusinessCards.csv", fileResult.FileDownloadName);
        }
    }
}
