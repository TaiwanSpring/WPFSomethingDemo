using System;
using System.Windows;
using System.Windows.Media;

namespace WPFDragToReorder.AttachedProperties
{
	public static partial class ItemsControlSet
	{
		public static Brush GetDropTargetLineBrush(DependencyObject obj)
		{
			return (Brush)obj.GetValue(DropTargetLineBrushProperty);
		}

		public static void SetDropTargetLineBrush(DependencyObject obj, Brush value)
		{
			obj.SetValue(DropTargetLineBrushProperty, value);
		}

		// Using a DependencyProperty as the backing store for DropTargetLineBrush.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DropTargetLineBrushProperty =
			DependencyProperty.RegisterAttached("DropTargetLineBrush", typeof(Brush), typeof(ItemsControlSet), new PropertyMetadata(Brushes.Red, OnDropTargetLineBrushValueChanged));

		private static void OnDropTargetLineBrushValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (lineAdorner != null)
			{
				if (e.NewValue is Brush brush)
				{
					lineAdorner.UpdateBrush(brush, GetDropTargetLineThickness(d));
				}
			}
		}
	}
}
