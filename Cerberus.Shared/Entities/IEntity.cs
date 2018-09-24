using Cerberus.Shared.DataModels;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.Entities
{
	public interface IEntity
	{
		EntityType EntityType { get; }
		CharacterAttributes Attributes { get; set; }
		Point GetPosition();
		void SetPosition(Point point);
		Guid Id { get; }
	}
}
