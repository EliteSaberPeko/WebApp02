using WebApp02.Models;
namespace WebApp02.ViewModel
{
    public class InsertPublishingHouseViewModel
    {
        public PublishingHouse PublishingHouse { get; set; }
        public IEnumerable<PublishingHouse> ListPublishingHouses { get; set; } = new List<PublishingHouse>();
        public PageViewModel Page { get; set; }
    }
}
