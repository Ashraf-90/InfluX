using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles
{
    public class InfluXProfiles : Profile
    {
        public InfluXProfiles()
        {
            // Identity
            CreateMap<ApplicationUser, IdentityUserDto>()
                .ForMember(d => d.UserId, m => m.MapFrom(s => s.Id))
                .ForMember(d => d.Id, m => m.MapFrom(s => s.Id)); // CommonDto.Id

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
            CreateMap<UserNiche, UserNicheUpdateDto>().ReverseMap();

            CreateMap<UserKeyWord, UserKeyWordDto>().ReverseMap();
            CreateMap<UserKeyWord, UserKeyWordCreateDto>().ReverseMap();
            CreateMap<UserKeyWord, UserKeyWordUpdateDto>().ReverseMap();

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

            CreateMap<BrandProfile, BrandProfileDto>().ReverseMap();
            CreateMap<BrandProfile, BrandProfileCreateDto>().ReverseMap();
            CreateMap<BrandProfile, BrandProfileUpdateDto>().ReverseMap();

            CreateMap<AgencyProfile, AgencyProfileDto>().ReverseMap();
            CreateMap<AgencyProfile, AgencyProfileCreateDto>().ReverseMap();
            CreateMap<AgencyProfile, AgencyProfileUpdateDto>().ReverseMap();

            CreateMap<AgencyClient, AgencyClientDto>().ReverseMap();
            CreateMap<AgencyClient, AgencyClientCreateDto>().ReverseMap();
            CreateMap<AgencyClient, AgencyClientUpdateDto>().ReverseMap();

            CreateMap<InfluencerBusiness, InfluencerBusinessDto>().ReverseMap();
            CreateMap<InfluencerBusiness, InfluencerBusinessCreateDto>().ReverseMap();
            CreateMap<InfluencerBusiness, InfluencerBusinessUpdateDto>().ReverseMap();


            CreateMap<Campaign, CampaignDto>().ReverseMap();
            CreateMap<Campaign, CampaignCreateDto>().ReverseMap();
            CreateMap<Campaign, CampaignUpdateDto>().ReverseMap();

            CreateMap<CampaignRequirement, CampaignRequirementDto>().ReverseMap();
            CreateMap<CampaignRequirement, CampaignRequirementCreateDto>().ReverseMap();
            CreateMap<CampaignRequirement, CampaignRequirementUpdateDto>().ReverseMap();

            CreateMap<CampaignInvite, CampaignInviteDto>().ReverseMap();
            CreateMap<CampaignInvite, CampaignInviteCreateDto>().ReverseMap();
            CreateMap<CampaignInvite, CampaignInviteUpdateDto>().ReverseMap();


            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, OrderCreateDto>().ReverseMap();
            CreateMap<Order, OrderUpdateDto>().ReverseMap();

            CreateMap<OrderDeliverable, OrderDeliverableDto>().ReverseMap();
            CreateMap<OrderDeliverable, OrderDeliverableCreateDto>().ReverseMap();
            CreateMap<OrderDeliverable, OrderDeliverableUpdateDto>().ReverseMap();

            CreateMap<OrderApproval, OrderApprovalDto>().ReverseMap();
            CreateMap<OrderApproval, OrderApprovalCreateDto>().ReverseMap();
            CreateMap<OrderApproval, OrderApprovalUpdateDto>().ReverseMap();

            CreateMap<Dispute, DisputeDto>().ReverseMap();
            CreateMap<Dispute, DisputeCreateDto>().ReverseMap();
            CreateMap<Dispute, DisputeUpdateDto>().ReverseMap();
        }
    }
}

