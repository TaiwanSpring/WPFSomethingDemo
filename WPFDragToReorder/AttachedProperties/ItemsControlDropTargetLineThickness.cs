using System;
using System.Windows;

namespace WPFDragToReorder.AttachedProperties
{
	public static partial class ItemsControlSet
	{
		public static double GetDropTargetLineThickness(DependencyObject obj)
		{
			return (double)obj.GetValue(DropTargetLineThicknessProperty);
		}

		public static void SetDropTargetLineThickness(DependencyObject obj, double value)
		{
			obj.SetValue(DropTargetLineThicknessProperty, value);
		}

		// Using a DependencyProperty as the backing store for DropTargetLineBrush.  This enables animation, styling, binding, etc...
		public static readonly DependencyProperty DropTargetLineThicknessProperty =
			DependencyProperty.RegisterAttached("DropTargetLineThickness", typeof(double), typeof(ItemsControlSet), new PropertyMetadata(1d, OnDropTargetLineThicknessValueChanged));

		private static void OnDropTargetLineThicknessValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (lineAdorner != null)
			{
				if (e.NewValue is double value)
				{
					lineAdorner.UpdateBrush(GetDropTargetLineBrush(d), value);
				}
			}
		}
	}
}
