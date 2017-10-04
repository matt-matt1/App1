using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace App1.Models
{
	[Table("settings")]
	class SettingsTbl
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		[MaxLength(250), Unique]
		public string Key { get; set; }
		[MaxLength(250)]
		public string Value { get; set; }
		public string Type { get; set; }
	}
}
