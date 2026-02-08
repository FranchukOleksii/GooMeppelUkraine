namespace GooMeppelUkraine.Web.Models
{
    public class HomeVm
    {
        public List<Stat> Stats { get; set; } = new();
        public List<TeamMember> Team { get; set; } = new();
        public List<Partner> Partners { get; set; } = new();
    }
}
