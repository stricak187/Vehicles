using System.Collections.Generic;

namespace Vehicles.Models
{
    public class VehicleMake
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<VehicleModel> Models { get; set; }
    }
}
