using BrenkaloWebStoreApi.Dtos;
using BrenkaloWebStoreApi.Models;
using BrenkaloWebStoreApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BrenkaloWebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts([FromHeader(Name = "Accept-Language")] string? language = "en")
        {
            try
            {
                int languageId = MapToLanguageId(language);

                var products = await _productService.GetAllProductsAsync(languageId);
                return Ok(products);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id, [FromHeader(Name = "Accept-Language")] string? language = "en")
        {
            try
            {
                int languageId = MapToLanguageId(language);

                var product = await _productService.GetProductByIdAsync(id, languageId);

                if (product == null)
                {
                    return BadRequest("Product not found.");
                }

                return Ok(product);
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        // Get all reviews for a product
        [HttpGet("reviews/{productId}")]
        public async Task<ActionResult<IEnumerable<ProductReviewDto>>> GetProductReviews(int productId)
        {
            var reviews = await _productService.GetProductReviewsAsync(productId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound();
            }

            var reviewsDto = reviews.Select(r => new ProductReviewDto
            {
                ProductId = r.ProductId,
                UserId = r.UserId,
                Username = r.Username,
                Stars = r.Stars,
                ReviewText = r.ReviewText
            }).ToList();

            return Ok(reviewsDto);
        }

        // Create a new product review
        [HttpPost("reviews/{productId}")]
        public async Task<ActionResult> CreateProductReview(int productId, ProductReviewDto reviewDto)
        {
            reviewDto.ProductId = productId; 
            var result = await _productService.CreateProductReviewAsync(reviewDto);
            if (!result)
            {
                return BadRequest("Could not create review");
            }

            return CreatedAtAction(nameof(GetProductReviews), new { productId }, reviewDto);
        }

        // Update an existing product review
        [HttpPut("reviews/{reviewId}")]
        public async Task<ActionResult> UpdateProductReview(int reviewId, ProductReviewDto reviewDto)
        {
            var result = await _productService.UpdateProductReviewAsync(reviewId, reviewDto);
            if (!result)
            {
                return NotFound();
            }

            return NoContent(); 
        }

        [NonAction]
        public static int MapToLanguageId  (string language)
        {
            int languageId = language?.ToLower() switch
            {
                "sl" => 2, // Slovenian
                "en" => 1, // English
                _ => 1 // Default to English
            };

            return languageId;
        }
    }
}
