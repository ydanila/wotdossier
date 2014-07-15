using System;
using System.Collections.Generic;

namespace WotDossier.Domain.Entities
{
	/// <summary>
	/// Object representation for table 'Player'.
	/// </summary>
	[Serializable]
	public class PlayerEntity : EntityBase
	{	
		/// <summary>
		/// Gets/Sets the field "Name".
		/// </summary>
		public virtual string Name	{get; set; }

        /// <summary>
        /// Gets or sets the server.
        /// </summary>
        public virtual string Server { get; set; }
		
		/// <summary>
		/// Gets/Sets the field "Creaded".
		/// </summary>
		public virtual DateTime Creaded	{get; set; }
		
		/// <summary>
		/// Gets/Sets the field "PlayerId".
		/// </summary>
		public virtual int PlayerId	{get; set; }
		
		#region Collections
		
		private IList<TankEntity> _tankEntities;
		/// <summary>
		/// Gets/Sets the <see cref="TankEntity"/> collection.
		/// </summary>
        public virtual IList<TankEntity> TankEntities
        {
            get
            {
                return _tankEntities ?? (_tankEntities = new List<TankEntity>());
            }
            set { _tankEntities = value; }
        }
		
		#endregion Collections
		
	}
}

