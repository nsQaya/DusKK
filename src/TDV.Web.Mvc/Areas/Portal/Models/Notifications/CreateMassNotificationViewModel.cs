using System.Collections.Generic;
using Abp.Notifications;

namespace TDV.Web.Areas.Portal.Models.Notifications
{
    public class CreateMassNotificationViewModel
    {
        public List<string> TargetNotifiers { get; set; }
    
        public NotificationSeverity Severity { get; set; }
    }
}