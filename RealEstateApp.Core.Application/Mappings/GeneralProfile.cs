﻿using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.Improvement;
using RealEstateApp.Core.Application.ViewModels.Property;
using RealEstateApp.Core.Application.ViewModels.PropertyType;
using RealEstateApp.Core.Application.ViewModels.SaleType;
using RealEstateApp.Core.Application.ViewModels.User;
using RealEstateApp.Core.Domain.Entities;

namespace RealEstateApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {

            #region ImprovementProfile
            CreateMap<Improvement, ImprovementViewModel>()
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Improvement, SaveImprovementViewModel>()
            .ReverseMap()
            .ForMember(x => x.Created, opt => opt.Ignore())
            .ForMember(x => x.CreatedBy, opt => opt.Ignore())
            .ForMember(x => x.LastModified, opt => opt.Ignore())
            .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion

            #region SaleTypeProfile
            CreateMap<SaleType, SaleTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<SaleType, SaveSaleTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
            #endregion

            #region PropertyTypeProfile
            CreateMap<PropertyType, PropertyTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<PropertyType, SavePropertyTypeViewModel>()
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());
			#endregion

			#region Propery
            CreateMap<Property, PropertyViewModel>()
                .ReverseMap()
				.ForMember(x => x.Created, opt => opt.Ignore())
				.ForMember(x => x.CreatedBy, opt => opt.Ignore())
				.ForMember(x => x.LastModified, opt => opt.Ignore())
				.ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
                .ForMember(x=>x.Description, opt=>opt.Ignore())
                .ForMember(x=>x.AgentId, opt=>opt.Ignore());

            CreateMap<Property, SavePropertyViewModel>()
                .ForMember(x => x.file1, opt => opt.Ignore())
                .ForMember(x => x.ImgUrl1, opt => opt.Ignore())
                .ForMember(x => x.file2, opt => opt.Ignore())
                .ForMember(x => x.ImgUrl2, opt => opt.Ignore())
                .ForMember(x => x.file3, opt => opt.Ignore())
                .ForMember(x => x.ImgUrl3, opt => opt.Ignore())
                .ForMember(x => x.file4, opt => opt.Ignore())
                .ForMember(x => x.ImgUrl4, opt => opt.Ignore())
                .ForMember(x => x.ImprovementId, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore());

            CreateMap<Property, PropertyDetailsViewModel>()
				.ForMember(x => x.ImgUrl1, opt => opt.Ignore())
				.ForMember(x => x.ImgUrl2, opt => opt.Ignore())
				.ForMember(x => x.ImgUrl3, opt => opt.Ignore())
				.ForMember(x => x.ImgUrl4, opt => opt.Ignore())
				.ForMember(x => x.AgentName, opt => opt.Ignore())
				.ForMember(x => x.AgentPhoneNumber, opt => opt.Ignore())
				.ForMember(x => x.AgentEmail, opt => opt.Ignore())
				.ForMember(x => x.AgentImgUrl, opt => opt.Ignore())
				.ReverseMap()
                .ForMember(x => x.Created, opt => opt.Ignore())
                .ForMember(x => x.CreatedBy, opt => opt.Ignore())
                .ForMember(x => x.LastModified, opt => opt.Ignore())
                .ForMember(x => x.LastModifiedBy, opt => opt.Ignore())
				.ForMember(x => x.AgentId, opt => opt.Ignore());

			#endregion



			#region UserProfile
			CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RegisterRequest, SaveUserViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ForgotPasswordRequest, ForgotPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ResetPasswordRequest, ResetPasswordViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();
            #endregion

        }
    }
}
