using Hybrid.Repositories.Base;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel;
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
        Task<GetAllMinigameResponse> GetMinigameOfTeacherAsync(string teacherId);
        Task<GetMiniGameResponse?> GetMiniGameAsync(string miniGameId);
        Task<AddMiniGameResponse> AddMiniGameAsync<T>(AddMiniGameRequest<T> request, string fileExtention) where T : MinigameModels;
        Task<UpdateMinigameResponse> UpdateMiniGameAsync<T>(UpdateMinigameRequest<T> request, string fileExtention) where T : MinigameModels;
    }

    public class MiniGameService : IMiniGameService
    {
        private readonly UnitOfWork _unitOfWork;
        public MiniGameService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Func_Get MiniGame
        /// Created By: TuanCA
        /// Created Date: 27/5/2025
        /// Updated By: X
        /// Updated Date: X
        /// </summary>
        public async Task<GetAllMinigameResponse> GetMinigameOfTeacherAsync(string teacherId)
        {
            var response = new GetAllMinigameResponse()
            {
                Minigames = new List<GetAllMinigameModel>()
            };
            var teacher = await _unitOfWork.TeacherRepo.GetByIdAsync(teacherId);

            if (teacher == null)
            {
                return response;
            }

            var minigames = await _unitOfWork.MiniGameRepo.GetMinigamesOfTeacherAsync(teacherId);
            response.Minigames = minigames.Select(x => x.ToGetAllMinigameModel()).ToList();

            return response;
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

            var miniGame = request.ToMiniGame();
            miniGame.MinigameId = _unitOfWork.MiniGameRepo.GenerateId(miniGame);
            miniGame.ThumbnailImage = $"{miniGame.MinigameId}_thumbnail{fileExtention}";
            miniGame.DataText = SerializeQuestions(request.GameData);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                await _unitOfWork.MiniGameRepo.CreateAsync(miniGame);
                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                throw;
                await _unitOfWork.RollbackTransactionAsync();
                response.Message = "Failed to create Minigame";
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
            if (miniGame.TemplateId != request.TemplateId)
            {
                response.Message = $"Invalid templateId. (Expected: {miniGame.TemplateId}) - (Actual: {request.TemplateId})";
                return response;
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Set request data to miniGame
                miniGame.MinigameName = request.MinigameName;
                miniGame.Duration = request.Duration;
                miniGame.DataText = SerializeQuestions(request.GameData);
                miniGame.ThumbnailImage = $"{miniGame.MinigameId}_thumbnail{fileExtention}";
                await _unitOfWork.MiniGameRepo.UpdateAsync(miniGame);

                await _unitOfWork.CommitTransactionAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync();
                response.Message = "Failed to update Minigame";
                return response;
            }
            var updatedMinigame = await _unitOfWork.MiniGameRepo.GetFirstWithIncludeAsync(
                x => x.MinigameId == miniGame.MinigameId,
                x => x.Template
            );

            response.IsSuccess = true;
            response.Message = "Updated minigame successfully.";
            response.Model = updatedMinigame!.ToAddMiniGameResponseModel();
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

        private string SerializeQuestionsJson<T>(T question)
        {
            return JsonSerializer.Serialize(question);
        }

    }
}
