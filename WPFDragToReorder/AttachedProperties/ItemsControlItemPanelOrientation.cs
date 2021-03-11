using System.Windows;
using System.Windows.Controls;

namespace WPFDragToReorder.AttachedProperties
{
	public static partial class ItemsControlSet
	{
		public static Orientation GetItemPanelOrientation(DependencyObject obj)
		{
			return (Orientation)obj.GetValue(ItemPanelOrientationProperty);
		}

		public static void SetItemPanelOrientation(DependencyObject obj, Orientation value)
		{
			obj.SetValue(ItemPanelOrientationProperty, value);
		}

		// Using a DependencyProperty as the backing store for ItemPanelOrientation.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty ItemPanelOrientationProperty =
			DependencyProperty.RegisterAttached("ItemPanelOrientation", typeof(Orientation), typeof(ItemsControl), new PropertyMetadata(Orientation.Vertical));


	}
}
