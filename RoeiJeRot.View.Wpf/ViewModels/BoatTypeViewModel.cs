namespace RoeiJeRot.View.Wpf.ViewModels
{
    public class BoatTypeViewModel
    {
        public int Id { get; set; }
        public int PossiblePassengers { get; set; }
        public int RequiredLevel { get; set; }
        public string Name { get; set; }
        protected bool HasCommanderSeat { get; set; }
    }
}