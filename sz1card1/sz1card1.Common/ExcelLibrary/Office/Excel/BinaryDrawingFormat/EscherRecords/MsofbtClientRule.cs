using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace sz1card1.Common.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtClientRule : EscherRecord
	{
		public MsofbtClientRule(EscherRecord record) : base(record) { }

		public MsofbtClientRule()
		{
			this.Type = EscherRecordType.MsofbtClientRule;
		}

	}
}
