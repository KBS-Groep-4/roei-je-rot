namespace RoeiJeRot.View.Wpf.ViewModels
{
    public class ReservationViewModel
    {
        public int Id { get; set; }
        public int BoatTypeName { get; set; }
        public int Status { get; set; }
        public bool Reserved { get; set; }
        public string ReservationDate { get; set; }
        public string Duration { get; set; }
        public int ReservedByUserId { get; set; }
        public int ReservedBoatId { get; set; }
    }
}