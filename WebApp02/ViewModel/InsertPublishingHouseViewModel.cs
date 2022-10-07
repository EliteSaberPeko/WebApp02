using WebApp02.Models;
namespace WebApp02.ViewModel
{
    public class InsertPublishingHouseViewModel
    {
        public PublishingHouse PublishingHouse { get; set; }
        public IEnumerable<PublishingHouse>? AllPublishingHouses { get; set; }
    }
}
