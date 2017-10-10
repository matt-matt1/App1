using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace App1.Models
{
	[Table("pages")]
	public class MyPage
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(250), Unique]
		public string Title { get; set; }
		//[tinyint]
		public bool InGrid { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }
		[MaxLength(10)]
		public string Icon { get; set; }
		[MaxLength(10)]
		public string Unicode { get; set; }
		public bool InMenu { get; set; }
		//[MaxLength(250), Unique]
		public string TargetEvent { get; set; }
		[MaxLength(250)]
		public string Type { get; set; }
	}
}
