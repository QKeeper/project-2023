using AutoMapper;
using yaDirectParser.Models;

namespace yaDirectParser
{
    /// <summary>
    /// Настройки маппинга всех сущностей приложения
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// В конструкторе настроим соответствие сущностей при маппинге
        /// </summary>
        public MappingProfile()
        {
            CreateMap<Ad, AdViewModel>()
                .ForMember(viewModel => viewModel.Text,
                    opt => opt.MapFrom(src => src.TextAd.Text))
                .ForMember(viewModel => viewModel.Title,
                    opt => opt.MapFrom(src => src.TextAd.Title));
        }
    }
}