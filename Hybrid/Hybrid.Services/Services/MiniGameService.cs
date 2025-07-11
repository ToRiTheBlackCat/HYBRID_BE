﻿using Hybrid.Repositories.Base;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel.Minigames;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Hybrid.Services.Services
{
    public interface IMiniGameService
    {
        Task<List<GetMinigameTemplatesModel>> GetMinigameTemplatesAsync();
        Task<List<GetAllMinigameModel>> GetTopMinigamesAsync(int count);
        Task<GetAllMinigameResponse> GetMinigameOfCourseAsync(string courseId, GetAllMinigameRequest request);
        Task<GetAllMinigameResponse> GetMinigameOfTeacherAsync(string teacherId, GetAllMinigameRequest request);
        Task<GetMiniGameResponse?> GetMiniGameAsync(string miniGameId);
        Task<AddMiniGameResponse> AddMiniGameAsync<T>(AddMiniGameRequest<T> request, string fileExtention) where T : MinigameModels;
        Task<UpdateMinigameResponse> UpdateMiniGameAsync<T>(UpdateMinigameRequest<T> request, string fileExtention) where T : MinigameModels;
        Task<DeleteMinigameResponse> DeleteMiniGameAsync(string minigameId);
        Task<Dictionary<string, int>> AnalyzeMinigames();
    }

    public class MiniGameService : IMiniGameService
    {
        private readonly UnitOfWork _unitOfWork;
        public MiniGameService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Func_Get MiniGame Templates
        /// Created By: TuanCA
        /// Created Date: 03/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<List<GetMinigameTemplatesModel>> GetMinigameTemplatesAsync()
        {
            var minigames = await _unitOfWork.MinigameTemplateRepo.GetAllAsync();
            var result = minigames
                .OrderBy(x => int.Parse(x.TemplateId.Substring(2).Trim()))
                .Select(x => x.ToGetMinigameTemplateModel())
                .ToList();

            return result;
        }

        /// <summary>
        /// Func_Get Top Minigames
        /// Created By: TuanCA
        /// Created Date: 16/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<List<GetAllMinigameModel>> GetTopMinigamesAsync(int count)
        {
            var minigames = await _unitOfWork.MiniGameRepo
                .GetAllWithIncludeAsync(
                    x => x.Ratings,
                    x => x.Teacher,
                    x => x.Template,
                    x => x.StudentAccomplisments
                );
            var result = minigames
                .Where(x => x.Ratings.Any())
                //.OrderByDescending(x => x.Ratings.Average(r => r.Score))
                //.ThenByDescending(x => x.StudentAccomplisments.Count)
                .Select(x => x.ToGetAllMinigameModel())
                .OrderByDescending(x => (x.RatingScore ?? 0) * Math.Log(1 + x.ParticipantsCount))
                .Take(count)
                .ToList();

            return result;
        }

        /// <summary>
        /// Func_Get MiniGame Of course
        /// Created By: TuanCA
        /// Created Date: 03/06/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<GetAllMinigameResponse> GetMinigameOfCourseAsync(string courseId, GetAllMinigameRequest request)
        {
            var minigames = await _unitOfWork.MiniGameRepo.GetMinigamesOfCourseAsync(courseId, request.TemplateId, request.MinigameName);
            //minigames = minigames.OrderBy(x => int.Parse(x.MinigameId.Substring(2))).ToList();
            var result = GetAllMinigameResponse.ToResponse(minigames, request.PageSize, request.PageNum);

            return result;
        }

        /// <summary>
        /// Func_Get MiniGame of teacher
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<GetAllMinigameResponse> GetMinigameOfTeacherAsync(string teacherId, GetAllMinigameRequest request)
        {
            var teacher = await _unitOfWork.TeacherRepo.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                return new GetAllMinigameResponse();
            }

            var minigames = await _unitOfWork.MiniGameRepo.GetMinigamesOfTeacherAsync(teacherId, request.TemplateId, request.MinigameName);
            //minigames = minigames.OrderBy(x => int.Parse(x.MinigameId.Substring(2))).ToList();
            var result = GetAllMinigameResponse.ToResponse(minigames, request.PageSize, request.PageNum);

            return result;
        }


        /// <summary>
        /// Func_Get MiniGame
        /// Created By: TuanCA
        /// Created Date: 26/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<GetMiniGameResponse?> GetMiniGameAsync(string miniGameId)
        {
            var miniGame = await _unitOfWork.MiniGameRepo.GetFirstWithIncludeAsync(
                x => x.MinigameId == miniGameId,
                [
                    x => x.Teacher,
                    x => x.Template,
                    x => x.Ratings,
                ]
            );

            if (miniGame == null)
            {
                return null;
            }

            var response = miniGame.ToGetMiniGameResponse();
            return response;
        }

        /// <summary>
        /// Func_Add minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<AddMiniGameResponse> AddMiniGameAsync<T>(AddMiniGameRequest<T> request, string fileExtention)
            where T : MinigameModels
        {
            var response = new AddMiniGameResponse();
            response.IsSuccess = false;

            // Check valid template
            var template = _unitOfWork.MinigameTemplateRepo.GetById(request.TemplateId);
            if (template == null)
            {
                response.Message = $"Error: Invalid templateId. (Input: {request.TemplateId})";
                return response;
            }

            // Check valid teacherrId
            var teacher = await _unitOfWork.TeacherRepo.GetByIdAsync(request.TeacherId);
            if (teacher == null)
            {
                response.Message = $"Error: Teacher not found. (Input: {request.TeacherId})";
                return response;
            }

            // Check valid CourseId
            var course = await _unitOfWork.CourseRepo.GetByIdAsync(request.CourseId);
            if (course == null)
            {
                response.Message = $"Error: Course not found. (Input: {request.CourseId})";
                return response;
            }

            // Set up the MiniGame object for adding to the database
            var miniGame = request.ToMiniGame();
            miniGame.MinigameId = _unitOfWork.MiniGameRepo.GenerateId(miniGame);
            miniGame.ThumbnailImage = $"users/{miniGame.MinigameId}_thumbnail{fileExtention}";

            if (request.GameData is IEnumerable<IMinigameWithPicture> minigames)
            {
                var minigameId = miniGame.MinigameId.Trim();
                int counter = 0;
                foreach (var question in minigames)
                {
                    if (question.Image != null)
                    {
                        question.ImagePath = $"users/{minigameId}_img{counter++}{Path.GetExtension(question.Image.FileName)}";
                    }
                    else
                    {
                        question.ImagePath = ""; // Ensure ImagePath is set even if no image is provided
                    }
                }
            }

            // Serialize the questions into XML format
            miniGame.DataText = SerializeQuestions(request.GameData);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.MiniGameRepo.CreateAsync(miniGame);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                //throw;
                await _unitOfWork.RollbackTransactionAsync();
                response.Message = "Failed to create Minigame. Error: " + ex.Message;
                return response;
            }
            var addedMinigame = await _unitOfWork.MiniGameRepo.GetFirstWithIncludeAsync(
                x => x.MinigameId == miniGame.MinigameId,
                x => x.Template
            );

            response.IsSuccess = true;
            response.Message = "Created minigame successfully.";
            response.Model = addedMinigame!.ToAddMiniGameResponseModel();
            return response;
        }


        /// <summary>
        /// Func_Update minigame
        /// Created By: TuanCA
        /// Created Date: 03/6/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<UpdateMinigameResponse> UpdateMiniGameAsync<T>(UpdateMinigameRequest<T> request, string fileExtention)
            where T : MinigameModels
        {
            var response = new UpdateMinigameResponse();

            // Find Minigame by Id
            var miniGame = await _unitOfWork.MiniGameRepo.GetByIdAsync(request.MinigameId);
            if (miniGame == null)
            {
                response.Message = "Minigame not found.";
                return response;
            }

            // Checking templateId
            if (!miniGame.TemplateId.Trim().Equals(request.TemplateId))
            {
                response.Message = $"Invalid templateId. (Expected: {miniGame.TemplateId}) - (Actual: {request.TemplateId})";
                return response;
            }

            // Checking TeacherId
            if (!miniGame.TeacherId.Trim().Equals(request.GetTeacherId()))
            {
                response.Message = $"Invalid TeacherId. (Expected: {miniGame.TeacherId}) - (Actual: {request.GetTeacherId()})";
                return response;
            }

            // Update MiniGame properties
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                miniGame.MinigameName = request.MinigameName;
                miniGame.Duration = request.Duration;

                // Set the path for the images if the minigame has pictures
                if (request.GameData is IEnumerable<IMinigameWithPicture> minigames)
                {
                    var minigameId = miniGame.MinigameId.Trim();
                    int counter = 0;
                    foreach (var question in minigames)
                    {
                        if (question.Image != null)
                        {
                            question.ImagePath = $"users/{minigameId}_img{counter++}{Path.GetExtension(question.Image.FileName)}";
                        }
                        else
                        {
                            question.ImagePath = string.Empty; // Ensure ImagePath is set even if no image is provided
                        }
                    }
                }

                miniGame.DataText = SerializeQuestions(request.GameData);
                miniGame.ThumbnailImage = $"users/{miniGame.MinigameId.Trim()}_thumbnail{fileExtention}";
                await _unitOfWork.MiniGameRepo.UpdateAsync(miniGame);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                response.Message = "Failed to update Minigame";
                return response;
            }
            miniGame = await _unitOfWork.MiniGameRepo.GetFirstWithIncludeAsync(
                x => x.MinigameId == miniGame.MinigameId,
                x => x.Template
            );

            response.IsSuccess = true;
            response.Message = "Updated minigame successfully.";
            response.Model = miniGame!.ToAddMiniGameResponseModel();
            return response;
        }

        /// <summary>
        /// Serialize The list of questions into XML format
        /// </summary>
        /// <param name="questions"></param>
        /// <returns></returns>
        private string SerializeQuestions<T>(List<T> questions)
        {
            var wrapper = new QuestionList<T>
            {
                Questions = questions
            };

            var serializer = new XmlSerializer(typeof(QuestionList<T>));

            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
            };
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty); // remove default xmlns declarations

            using var stringWriter = new StringWriter();
            using var xmlWriter = XmlWriter.Create(stringWriter, settings);
            serializer.Serialize(xmlWriter, wrapper, namespaces);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Serialize Question Json Version
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="question"></param>
        /// <returns></returns>
        private string SerializeQuestionsJson<T>(T question)
        {
            return JsonSerializer.Serialize(question);
        }

        /// <summary>
        /// Func_Delete minigame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<DeleteMinigameResponse> DeleteMiniGameAsync(string minigameId)
        {
            var result = new DeleteMinigameResponse()
            {
                IsSuccess = false,
                Message = ""
            };
            var minigame = _unitOfWork.MiniGameRepo.GetById(minigameId);

            if (minigame == null)
            {
                result.Message = "Minigame not found.";
                return result;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.MiniGameRepo.RemoveAsync(minigame);
                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();

                result.Minigame = minigame;
                result.IsSuccess = true;
                result.Message = "Delete minigame successfully.";
            }
            catch (Exception)
            {
                result.Message = "Delete minigame failed";
            }

            return result;
        }
        //AnalyzeMinigamesResponse
        public async Task<Dictionary<string, int>> AnalyzeMinigames()
        {
            var result = await _unitOfWork.MiniGameRepo.GetMinigamesCountByTemplateNameAsync();
            return result;
        }
    }
}
