using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;
using TDV.Auditing;
using TDV.Auditing.Dto;
using TDV.Authorization;
using TDV.Web.Areas.Portal.Models.AuditLogs;
using TDV.Web.Controllers;

namespace TDV.Web.Areas.Portal.Controllers
{
    [Area("Portal")]
    [DisableAuditing]
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_AuditLogs)]
    public class AuditLogsController : TDVControllerBase
    {
        private readonly IAuditLogAppService _auditLogAppService;

        public AuditLogsController(IAuditLogAppService auditLogAppService)
        {
            _auditLogAppService = auditLogAppService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<PartialViewResult> EntityChangeDetailModal(EntityChangeListDto entityChangeListDto)
        {
            var output = await _auditLogAppService.GetEntityPropertyChanges(entityChangeListDto.Id);

            var viewModel = new EntityChangeDetailModalViewModel(output, entityChangeListDto);

            return PartialView("_EntityChangeDetailModal", viewModel);
        }
    }
}