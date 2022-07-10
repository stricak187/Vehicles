namespace Vehicles.Models
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MakeId { get; set; }
        public virtual VehicleMake Make { get; set; }
    }
}
