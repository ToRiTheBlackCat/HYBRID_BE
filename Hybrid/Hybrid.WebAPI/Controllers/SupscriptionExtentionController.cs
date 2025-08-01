﻿using Hybrid.Repositories.Models;
using Hybrid.Services.Services;
using Hybrid.Services.ViewModel.SupscriptionExtention;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hybrid.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupscriptionExtentionController : ControllerBase
    {
        private readonly ISupscriptionExtentionService _extentionService;
        public SupscriptionExtentionController(ISupscriptionExtentionService extentionService)
        {
            _extentionService = extentionService;
        }

        /// <summary>
        /// API_CreateSupscriptionExtentionOfStudent
        /// SupscriptionExtentionOrder
        /// X
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: TriNHM
        /// Updated Date: 20/6/2025
        /// </summary>
        [HttpPost("create-student")]
        [Authorize]
        public async Task<ActionResult> CreateSupscriptionExtentionOfStudent([FromBody] SupscriptionExtentionOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message) = await _extentionService.CreateSupscriptionExtentionOrder_Student(request);
            return Ok(new
            {
                isSuccess,
                message,
            });
        }

        /// <summary>
        /// API_CreateSupscriptionExtentionOfTeacher
        /// SupscriptionExtentionOrder
        /// X
        /// Created By: TriNHM
        /// Created Date: 21/5/2025
        /// Updated By: TriNHM
        /// Updated Date: 20/6/2025
        /// </summary>
        [HttpPost("create-teacher")]
        [Authorize]
        public async Task<ActionResult> CreateSupscriptionExtentionOfTeacher([FromBody] SupscriptionExtentionOrderRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (isSuccess, message) = await _extentionService.CreateSupscriptionExtentionOrder_Teacher(request);
            return Ok(new
            {
                isSuccess,
                message,
            });
        }
    }
}
