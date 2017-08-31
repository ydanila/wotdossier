using WotDossier.Domain.Entities;

namespace WotDossier.Dal.Mappings
{
    /// <summary>
    /// Represents map class for <see cref="TankRankedBattlesStatisticEntity"/>.
    /// </summary>
    public class TankRankedBattlesStatisticEntityMapping : ClassMapBase<TankRankedBattlesStatisticEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TankRankedBattlesStatisticEntityMapping"/> class.
        /// </summary>
        public TankRankedBattlesStatisticEntityMapping()
        {
            Map(v => v.UId);
            Map(v => v.Updated);
            Map(v => v.Raw).CustomSqlType("BinaryBlob");
            Map(v => v.TankId).ReadOnly();
            Map(v => v.TankUId);
            Map(v => v.Version);
            Map(v => v.BattlesCount);

            Map(v => v.Rev);

            References(v => v.TankIdObject).Column(Column(v => v.TankId)).Insert();
        }
    }
}
