using Cerberus.Shared.Entities;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.Events
{
	public class EntityPositionChangedEvent : PubSubEvent<EntityPositionChangedArgs>
	{
	}

	public class EntityPositionChangedArgs
	{
		public IEntity Entity { get; set; }
	}

}
