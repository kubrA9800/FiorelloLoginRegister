namespace FiorelloBackend.Models
{
    public class HomeSay: BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
    }
}
