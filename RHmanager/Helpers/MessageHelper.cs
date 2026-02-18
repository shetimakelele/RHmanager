namespace RHmanager.Helpers
{
    /// <summary>
    /// Helper pour gérer l'affichage des messages (succès/erreur)
    /// </summary>
    public class MessageHelper
    {
        public string Message { get; private set; } = string.Empty;
        public bool IsError { get; private set; } = false;
        public bool HasMessage => !string.IsNullOrEmpty(Message);

        /// <summary>
        /// Affiche un message de succès
        /// </summary>
        public void ShowSuccess(string message)
        {
            Message = message;
            IsError = false;
        }

        /// <summary>
        /// Affiche un message d'erreur
        /// </summary>
        public void ShowError(string message)
        {
            Message = message;
            IsError = true;
        }

        /// <summary>
        /// Affiche un message (succès ou erreur selon le paramètre)
        /// </summary>
        public void Show(string message, bool isError = false)
        {
            Message = message;
            IsError = isError;
        }

        /// <summary>
        /// Efface le message
        /// </summary>
        public void Clear()
        {
            Message = string.Empty;
            IsError = false;
        }

        /// <summary>
        /// Retourne la classe CSS Bootstrap pour l'alerte
        /// </summary>
        public string GetAlertClass()
        {
            return IsError ? "alert-danger" : "alert-success";
        }
    }
}