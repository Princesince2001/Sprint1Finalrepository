using AutoMapper;
using LXP.Common.Entities;
using LXP.Common.ViewModels;
using LXP.Core.IServices;
using LXP.Data.IRepository;
using LXP.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LXP.Core.Services
{
    public  class ProfileService:IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private Mapper _learnerProfileMapper;

        public ProfileService( IProfileRepository profileRepository)
        {
            
            this._profileRepository = profileRepository;
            var _configCategory = new MapperConfiguration(cfg => cfg.CreateMap<LearnerProfile, GetProfileViewModel>().ReverseMap());
            _learnerProfileMapper = new Mapper(_configCategory);

        }

        public async Task<List<GetProfileViewModel>> GetAllLearnerProfile()
        {
            List<GetProfileViewModel> learnerProfile = _learnerProfileMapper.Map<List<LearnerProfile>, List<GetProfileViewModel>>(await _profileRepository.GetAllLearnerProfile());
            return learnerProfile;
        }

        public LearnerProfile GetLearnerProfileById(string id) {

            return _profileRepository.GetLearnerprofileDetailsByLearnerprofileId(Guid.Parse(id));
        
        }

    }
}
