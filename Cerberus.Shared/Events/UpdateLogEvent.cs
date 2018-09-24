using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cerberus.Shared.Events
{
	public class UpdateLogEvent : PubSubEvent<UpdateLogArgs>
	{
	}

	public class UpdateLogArgs
	{
		public string Text { get; set; }
	}
}
