namespace RoeiJeRot.View.Wpf.ViewModels
{
    public class BoatTypeViewModel
    {
        public int Id { get; set; }
        public int PossiblePassengers { get; set; }
        public int RequiredLevel { get; set; }
        public string Name { get; set; }
        public string HasCommanderSeat { get; set; }
        public string Status { get; set; }
    }
}