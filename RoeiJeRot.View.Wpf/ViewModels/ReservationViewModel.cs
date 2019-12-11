namespace RoeiJeRot.View.Wpf.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public string ReservationDate { get; set; }
        public string Duration { get; set; }
        public string ReservedByUserId { get; set; }
        public int ReservedBoatId { get; set; }
    }
}