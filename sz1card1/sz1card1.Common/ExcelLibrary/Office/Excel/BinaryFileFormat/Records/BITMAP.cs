using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace sz1card1.Common.ExcelLibrary.BinaryFileFormat
{
	public partial class BITMAP : Record
	{
		public BITMAP(Record record) : base(record) { }

		public BITMAP()
		{
			this.Type = RecordType.BITMAP;
		}

	}
}
