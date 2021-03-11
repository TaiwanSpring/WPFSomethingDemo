using WPFDragToReorder.DataStruct;
using System;
using System.Collections.Generic;
using System.Text;

namespace WPFDragToReorder.Interface
{
	public interface IDropHandler
	{
		void OnDrag(DragInfo dragInfo);
	}
}
