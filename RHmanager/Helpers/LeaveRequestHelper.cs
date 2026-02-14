using RHmanager.Enums;

namespace RHmanager.Helpers
{
    /// <summary>
    /// Helper pour le formatage et l'affichage des demandes de congés
    /// </summary>
    public static class LeaveRequestHelper
    {
        /// <summary>
        /// Retourne la classe CSS Bootstrap du badge selon le statut
        /// </summary>
        /// <param name="status">Statut de la demande</param>
        /// <returns>Classes CSS Bootstrap</returns>
        public static string GetStatusBadgeClass(LeaveRequestStatus status)
        {
            return status switch
            {
                LeaveRequestStatus.Pending => "bg-warning text-dark",
                LeaveRequestStatus.Approved => "bg-success",
                LeaveRequestStatus.Rejected => "bg-danger",
                _ => "bg-secondary"
            };
        }

        /// <summary>
        /// Retourne le texte français du statut
        /// </summary>
        /// <param name="status">Statut de la demande</param>
        /// <returns>Libellé en français</returns>
        public static string GetStatusText(LeaveRequestStatus status)
        {
            return status switch
            {
                LeaveRequestStatus.Pending => "En attente",
                LeaveRequestStatus.Approved => "Approuvée",
                LeaveRequestStatus.Rejected => "Rejetée",
                _ => "Inconnu"
            };
        }

        /// <summary>
        /// Retourne l'icône Bootstrap correspondant au statut
        /// </summary>
        /// <param name="status">Statut de la demande</param>
        /// <returns>Classe d'icône Bootstrap</returns>
        public static string GetStatusIcon(LeaveRequestStatus status)
        {
            return status switch
            {
                LeaveRequestStatus.Pending => "bi-clock-history",
                LeaveRequestStatus.Approved => "bi-check-circle-fill",
                LeaveRequestStatus.Rejected => "bi-x-circle-fill",
                _ => "bi-question-circle"
            };
        }

        /// <summary>
        /// Formate une période de congés (date début - date fin)
        /// </summary>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>Chaîne formatée "dd/MM/yyyy - dd/MM/yyyy"</returns>
        public static string FormatLeavePeriod(DateTime startDate, DateTime endDate)
        {
            return $"{startDate:dd/MM/yyyy} - {endDate:dd/MM/yyyy}";
        }

        /// <summary>
        /// Formate le nombre de jours (singulier/pluriel)
        /// </summary>
        /// <param name="numberOfDays">Nombre de jours</param>
        /// <returns>"X jour" ou "X jours"</returns>
        public static string FormatDaysCount(int numberOfDays)
        {
            return numberOfDays > 1 ? $"{numberOfDays} jours" : $"{numberOfDays} jour";
        }
    }
}
