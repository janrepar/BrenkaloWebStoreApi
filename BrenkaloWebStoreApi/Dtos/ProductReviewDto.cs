namespace BrenkaloWebStoreApi.Dtos
{
    public class ProductReviewDto
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = null!;
        public int Stars { get; set; }
        public string? ReviewText { get; set; }
    }
}
