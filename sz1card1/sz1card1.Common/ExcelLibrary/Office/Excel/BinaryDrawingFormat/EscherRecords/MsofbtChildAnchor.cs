using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace sz1card1.Common.ExcelLibrary.BinaryDrawingFormat
{
	public partial class MsofbtChildAnchor : EscherRecord
	{
		public MsofbtChildAnchor(EscherRecord record) : base(record) { }

		public MsofbtChildAnchor()
		{
			this.Type = EscherRecordType.MsofbtChildAnchor;
		}

	}
}
