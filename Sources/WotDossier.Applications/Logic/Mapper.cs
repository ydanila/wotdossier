using System.Dynamic;
using WotDossier.Applications.ViewModel;
using WotDossier.Domain;
using WotDossier.Domain.Entities;
using WotDossier.Domain.Interfaces;

namespace WotDossier.Applications.Logic
{
    public class Mapper
    {
        static Mapper()
        {
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<IRandomBattlesAchievements, IRandomBattlesAchievements>();
                cfg.CreateMap<IHistoricalBattlesAchievements, IHistoricalBattlesAchievements>();
                cfg.CreateMap<ITeamBattlesAchievements, ITeamBattlesAchievements>();
                cfg.CreateMap<IFortAchievements, IFortAchievements>();
                cfg.CreateMap<IClanBattlesAchievements, IClanBattlesAchievements>();
                cfg.CreateMap<ExpandoObject, Map>();
                cfg.CreateMap<FavoritePlayerEntity, ListItem<int>>()
                    .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                    .ForMember(x => x.Value, opt => opt.MapFrom(x => x.Name));
            });
        }

        public static void Map<TSource, TTarget>(TSource source, TTarget target)
        {
            AutoMapper.Mapper.Map(source, target);
        }

        public static void Map<T>(T source, T target)
        {
            AutoMapper.Mapper.Map(source, target);
        }

        public static TDestination Map<TSource, TDestination>(TSource source)
        {
           return AutoMapper.Mapper.Map<TDestination>(source);
        }
    }
}
