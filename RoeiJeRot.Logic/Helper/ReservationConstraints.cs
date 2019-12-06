using System;
using System.Collections.Generic;
using System.Text;
using RoeiJeRot.Logic.Services;
using static Innovative.SolarCalculator.SolarTimes;

namespace RoeiJeRot.Logic.Helper
{
    /// <summary>
    /// A message with a boolean to indicate a reservation is valid and a string to indicate why or why not
    /// </summary>
    public class ReservationConstraintsMessage
    {
        public bool IsValid { get; private set; }
        public string Reason { get; private set; }

        public ReservationConstraintsMessage(bool isValid, string reason)
        {
            IsValid = isValid;
            Reason = reason;
        }
    }

    public static class ReservationConstraints
    {
        /// <summary>
        /// A function to check if a reservation is valid
        /// </summary>
        /// <param name="date">Starting DateTime of reservation</param>
        /// <param name="duration">Duration of the reservation</param>
        /// <param name="reservationService">Reservation service to check if there are more than 2 reservations already</param>
        /// <param name="accountId">The account to check</param>
        /// <returns>A message to indicate if a reservation is valid with info why or why not</returns>
        public static ReservationConstraintsMessage IsValid(DateTime date, TimeSpan duration, IReservationService reservationService, int accountId)
        {
            if (date < DateTime.Now || date + duration < DateTime.Now) return new ReservationConstraintsMessage(false, "Reservatie is in het verleden");
            if (duration > TimeSpan.FromHours(2)) return new ReservationConstraintsMessage(false, "Reservatie is te lang (max 2 uur)");
            if (reservationService.GetFutureReservations(accountId).Count >= 2) return new ReservationConstraintsMessage(false, "U heeft al teveel reservaties geplaatst voor de toekomst");

            if (!DayChecker.IsDay(date, duration)) return new ReservationConstraintsMessage(false, "Reservaties kunnen alleen tijdens de dag geplaatst worden");

            return new ReservationConstraintsMessage(true, "All is fine");
        }
    }
}
