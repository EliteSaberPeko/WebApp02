namespace WebApp02.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public decimal Sum { get; set; }
        public DateTime Date { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
