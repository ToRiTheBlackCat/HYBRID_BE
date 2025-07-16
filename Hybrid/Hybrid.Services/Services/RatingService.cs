using Hybrid.Repositories.Base;
using Hybrid.Repositories.Models;
using Hybrid.Services.Helpers;
using Hybrid.Services.ViewModel.Rating;
using Microsoft.EntityFrameworkCore.Migrations.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Services.Services
{
    public interface IRatingService
    {
        Task<List<GetAllRatingResponse>?> GetAllRatingByMinigameId(string minigameId);
        Task<(bool, string)> CreateRating(CreateRatingRequest request);
        Task<double> GetScoreOfMinigame(string minigameId);

    }
    public class RatingService : IRatingService
    {
        private readonly UnitOfWork _unitOfWork;
        public RatingService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(bool, string)> CreateRating(CreateRatingRequest request)
        {
            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var foundRating = await _unitOfWork.RatingRepo.GetExistedRating(request.MinigameId, request.StudentId);
                if (foundRating != null)
                {
                    return (false, "This student already rating this minigame!");
                }

                var mappedRating = request.Map_CreateRatingRequest_To_Rating();
                await _unitOfWork.RatingRepo.CreateAsync(mappedRating);
                await _unitOfWork.CommitTransactionAsync();

                return (true, "Rating successfully");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                Console.WriteLine(ex.Message);
                return (false, "Rating fail");
            }
        }

        public async Task<List<GetAllRatingResponse>?> GetAllRatingByMinigameId(string minigameId)
        {
            var ratingList = await _unitOfWork.RatingRepo.GetAllByMinigameId(minigameId);
            if (ratingList == null)
            {
                return null;
            }

            var mappedList = ratingList.Map_ListRating_To_ListGetAllRatingResponse();

            return mappedList;
        }

        public async Task<double> GetScoreOfMinigame(string minigameId)
        {
            var ratingScore = new double();
            var ratingCount = 0;

            var ratingList = await _unitOfWork.RatingRepo.GetAllByMinigameId(minigameId);
            if (ratingList == null || ratingList.Count == 0)
            {
                return ratingScore;
            }

            foreach (var rating in ratingList)
            {
                ratingScore += rating.Score;
                ratingCount++;
            }
            ratingScore /= ratingCount;

            return ratingScore;

        }
    }
}
