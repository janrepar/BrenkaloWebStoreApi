namespace BrenkaloWebStoreApi.Dtos
{
    public class ProductReviewDto
    {
        public string Username { get; set; } = null!;
        public int Stars { get; set; }
        public string ReviewText { get; set; } = null!;
        public DateTime? CreatedAt { get; set; }
    }
}
