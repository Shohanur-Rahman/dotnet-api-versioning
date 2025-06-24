
using AutoMapper;
using My.ApiVersioningExample.Core.Users.DTOs.Request;
using My.ApiVersioningExample.Core.Users.DTOs.Response;
using My.ApiVersioningExample.Core.Users.Entities;

namespace My.ApiVersioningExample.Services.Configurations
{
	/// <summary>
	/// AutoMapper profile for configuring object-to-object mappings related to user entities.
	/// </summary>
	public class MapperProfie : Profile
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MapperProfie"/> class and defines the mapping configurations.
		/// </summary>
		public MapperProfie()
		{
			#region Users Mapping

			CreateMap<UserCreateRequest, DbUser>().ReverseMap();
			CreateMap<UserUpdateRequest, DbUser>()
				.ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
				.ForMember(dest => dest.PasswordSalt, opt => opt.Ignore())
				.ForMember(dest => dest.PhotoUrl, opt => opt.Ignore())
				.ReverseMap();
			CreateMap<PhotoUrlRequest, DbUser>().ReverseMap();
			CreateMap<UserDetailResponse, DbUser>().ReverseMap();

			#endregion
		}
	}

}
