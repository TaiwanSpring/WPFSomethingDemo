using System.Windows;
using System.Windows.Controls;
using WPFDragToReorder.Enum;

namespace WPFDragToReorder.DataStruct
{
	public struct DragInfo
	{
		public ItemsControl ItemsControl { get; set; }
		public DependencyObject ItemContainer { get; set; }
		public object Item { get; set; }
		public int OriginIndex { get; set; }
		public int TargetIndex { get; set; }
		public DropPositionEnum DropPosition { get; set; }
	}
}
