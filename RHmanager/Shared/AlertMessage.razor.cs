using Microsoft.AspNetCore.Components;
using RHmanager.Helpers;

namespace RHmanager.Shared
{
    public partial class AlertMessage
    {
        [Parameter]
        public MessageHelper MessageHelper { get; set; } = new();
    }
}