using Microsoft.Xna.Framework;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.Events
{
	public class TileSelectedEvent : PubSubEvent<TileSelectedArgs>
	{
		
	}

	public class TileSelectedArgs
	{
		public Point Tile { get; set; }
	}
}
