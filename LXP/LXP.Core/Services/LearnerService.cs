
using LXP.Data.Repository;
using System;
using LXP.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LXP.Data.IRepository;
using System.Threading.Tasks;
using LXP.Common.ViewModels;
using LXP.Data;
using LXP.Core.IServices;
using LXP.Common.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using LXP.Common.Utils;
using System.Net.Http;


namespace LXP.Core.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly ILearnerRepository _learnerRepository;
        private readonly IProfileRepository _profileRepository;
        private Mapper _learnerMapper;  //Mapper1

        public LearnerService(ILearnerRepository learnerRepository, IProfileRepository profileRepository)
        {
            this._learnerRepository = learnerRepository;
            this._profileRepository = profileRepository;
            var _configCategory = new MapperConfiguration(cfg => cfg.CreateMap<Learner, GetLearnerViewModel>().ReverseMap());//mapper 2
            _learnerMapper = new Mapper(_configCategory);// mapper 3

        }

        public async Task<bool> LearnerRegistration(RegisterUserViewModel registerUserViewModel)
        {
            bool isLearnerExists = await _learnerRepository.AnyLearnerByEmail(registerUserViewModel.Email);
            if (!isLearnerExists)
            {
                Learner newlearner = new Learner()
                {
                    LearnerId = Guid.NewGuid(),
                    Email = registerUserViewModel.Email,
                    Password = SHA256Encrypt.ComputePasswordToSha256Hash(registerUserViewModel.Password),
                    Role = registerUserViewModel.Role,
                    UnblockRequest = false,
                    AccountStatus = true,
                    UserLastLogin = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    CreatedBy = $"{registerUserViewModel.FirstName} {registerUserViewModel.LastName}",
                    ModifiedAt = DateTime.Now,
                    ModifiedBy = $"{registerUserViewModel.FirstName} {registerUserViewModel.LastName}"
                };
                _learnerRepository.AddLearner(newlearner);
                Learner learner = _learnerRepository.GetLearnerByLearnerEmail(newlearner.Email);
                LearnerProfile profile = new LearnerProfile()
                {
                    ProfileId = Guid.NewGuid(),
                    FirstName = registerUserViewModel.FirstName,
                    LastName = registerUserViewModel.LastName,
                    Dob = DateOnly.ParseExact(registerUserViewModel.Dob, "yyyy-MM-dd", null),
                    Gender = registerUserViewModel.Gender,
                    ContactNumber = registerUserViewModel.ContactNumber,
                    Stream = registerUserViewModel.Stream,
                    CreatedAt = DateTime.Now,
                    CreatedBy = $"{registerUserViewModel.FirstName} {registerUserViewModel.LastName}",
                    LearnerId = learner.LearnerId,
                    ModifiedBy = $"{registerUserViewModel.FirstName} {registerUserViewModel.LastName}",
                };
                _profileRepository.AddProfile(profile);
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<List<GetLearnerViewModel>> GetAllLearner()
        {
            List<GetLearnerViewModel> learner = _learnerMapper.Map<List<Learner>, List<GetLearnerViewModel>>(await _learnerRepository.GetAllLearner()); //mapper 4
            return learner;
        }

        //public void UpdateAllLearner(Learner learner)
        //{
        //    if (learner == null)
        //    {
        //        throw new ArgumentNullException(nameof(learner));
        //    }

        //    _learnerRepository.UpdateAllLearner(learner);
        //}


        public Learner GetLearnerById(string id)
        {

            return _learnerRepository.GetLearnerDetailsByLearnerId(Guid.Parse(id));

        }
    }
    }

