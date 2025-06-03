using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public class MinigameTemplateService
    {
        private readonly UnitOfWork _unitOfWork;
        public MinigameTemplateService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MinigameTemplate?> GetTemplateByIdAsync(string templateId)
        {
            var template = await _unitOfWork.MinigameTemplateRepo
                .GetByIdAsync(templateId);

            return template;
        }
    }
}
