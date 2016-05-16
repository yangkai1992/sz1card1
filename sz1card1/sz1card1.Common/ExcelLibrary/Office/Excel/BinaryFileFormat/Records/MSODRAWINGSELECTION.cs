using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace sz1card1.Common.ExcelLibrary.BinaryFileFormat
{
	public partial class MSODRAWINGSELECTION : MSOCONTAINER
	{
		public MSODRAWINGSELECTION(Record record) : base(record) { }

		public MSODRAWINGSELECTION()
		{
			this.Type = RecordType.MSODRAWINGSELECTION;
		}

	}
}
