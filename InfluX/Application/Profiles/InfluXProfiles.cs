using Application.DTOs;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Domain.Entities;

namespace Application.Profiles
{
    public class InfluXProfiles : Profile
    {
        public InfluXProfiles()
        {
            // Identity
            CreateMap<ApplicationUser, IdentityUserDto>()
                .ForMember(d => d.PhoneNumber, m => m.MapFrom(s => s.PhoneNumber));

            // Profiles
            CreateMap<UserProfile, UserProfileDto>().ReverseMap();
            CreateMap<UserProfile, UserProfileCreateDto>().ReverseMap();
            CreateMap<UserProfile, UserProfileUpdateDto>().ReverseMap();

            CreateMap<InfluencerProfile, InfluencerProfileDto>().ReverseMap();
            CreateMap<InfluencerProfile, InfluencerProfileCreateDto>().ReverseMap();
            CreateMap<InfluencerProfile, InfluencerProfileUpdateDto>().ReverseMap();

            CreateMap<SocialAccount, SocialAccountDto>().ReverseMap();
            CreateMap<SocialAccount, SocialAccountCreateDto>().ReverseMap();
            CreateMap<SocialAccount, SocialAccountUpdateDto>().ReverseMap();

            CreateMap<Niche, NicheDto>().ReverseMap();
            CreateMap<Niche, NicheCreateDto>().ReverseMap();
            CreateMap<Niche, NicheUpdateDto>().ReverseMap();

            CreateMap<UserNiche, UserNicheDto>().ReverseMap();
            CreateMap<UserNiche, UserNicheCreateDto>().ReverseMap();

            CreateMap<UserKeyWord, UserKeyWordDto>().ReverseMap();
            CreateMap<UserKeyWord, UserKeyWordCreateDto>().ReverseMap();

            CreateMap<VerificationRequest, VerificationRequestDto>().ReverseMap();
            CreateMap<VerificationRequest, VerificationRequestCreateDto>().ReverseMap();
            CreateMap<VerificationRequest, VerificationRequestUpdateDto>().ReverseMap();

            CreateMap<ServiceListing, ServiceListingDto>().ReverseMap();
            CreateMap<ServiceListing, ServiceListingCreateDto>().ReverseMap();
            CreateMap<ServiceListing, ServiceListingUpdateDto>().ReverseMap();

            CreateMap<ServicePricingOption, ServicePricingOptionDto>().ReverseMap();
            CreateMap<ServicePricingOption, ServicePricingOptionCreateDto>().ReverseMap();
            CreateMap<ServicePricingOption, ServicePricingOptionUpdateDto>().ReverseMap();

            CreateMap<InfluencerMedia, InfluencerMediaDto>().ReverseMap();
            CreateMap<InfluencerMedia, InfluencerMediaCreateDto>().ReverseMap();
            CreateMap<InfluencerMedia, InfluencerMediaUpdateDto>().ReverseMap();

            CreateMap<InfluencerAsset, InfluencerAssetDto>().ReverseMap();
            CreateMap<InfluencerAsset, InfluencerAssetCreateDto>().ReverseMap();
            CreateMap<InfluencerAsset, InfluencerAssetUpdateDto>().ReverseMap();
        }
    }
}
