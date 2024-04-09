namespace RealEstateApp.Core.Application.ViewModels.Dashboard
{
    public class AdminDashboardViewModel
    {
        public int TotalActiveAgents { get; set; }
        public int TotalActiveAdmins { get; set; }
        public int TotalActiveDevs { get; set; }
        public int TotalInactiveAgents { get; set; }
        public int TotalInactiveAdmins { get; set; }
        public int TotalInactiveDevs { get; set; }
        public int TotalPropertiesRegistered { get; set; }
    }
}
