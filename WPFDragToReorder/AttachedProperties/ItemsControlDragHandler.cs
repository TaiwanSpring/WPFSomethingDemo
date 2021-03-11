using WPFDragToReorder.Interface;
using System.Windows;

namespace WPFDragToReorder.AttachedProperties
{
	public static partial class ItemsControlSet
	{
		public static IDropHandler GetDragHandler(DependencyObject obj)
		{
			return (IDropHandler)obj.GetValue(DragHandlerProperty);
		}

		public static void SetDragHandler(DependencyObject obj, IDropHandler value)
		{
			obj.SetValue(DragHandlerProperty, value);
		}

		// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DragHandlerProperty =
			DependencyProperty.RegisterAttached("DragHandler", typeof(IDropHandler), typeof(ItemsControlSet));
	}
}
