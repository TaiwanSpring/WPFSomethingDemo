using WPFDragToReorder.Enum;
using System.Windows;

namespace WPFDragToReorder.AttachedProperties
{
	public static partial class ItemsControlSet
	{
		public static DragStatusEnum GetDragStatus(DependencyObject obj)
		{
			return (DragStatusEnum)obj.GetValue(DragStatusProperty);
		}

		public static void SetDragStatus(DependencyObject obj, DragStatusEnum value)
		{
			obj.SetValue(DragStatusProperty, value);
		}

		// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DragStatusProperty =
			DependencyProperty.RegisterAttached("DragStatus", typeof(DragStatusEnum), typeof(ItemsControlSet), new PropertyMetadata(DragStatusEnum.None));


	}
}
