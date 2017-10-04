using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App1.Models
{
	[Table("slide")]
	public class Slide
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(250), Unique]
		public string Name { get; set; }
		[MaxLength(250), Unique]
		public string ImgSrc { get; set; }
		public int StartPosX { get; set; }
		[MaxLength(250), Unique]
		public string Caption1 { get; set; }
		[MaxLength(250), Unique]
		public string Caption2 { get; set; }
		[MaxLength(250), Unique]
		public string Goto { get; set; }
	}
}
