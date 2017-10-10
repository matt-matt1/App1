using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace App1
{
	public class RouteMap : Map
	{
		public List<Position> RouteCoordinates { get; set; }

		public RouteMap()
		{
			RouteCoordinates = new List<Position>();
		}
	}
}
