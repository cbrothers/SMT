﻿#region

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace SMT.ZoomControl
{
    public delegate void ContentSizeChangedHandler(object sender, Size newSize);

    public class ZoomContentPresenter : ContentPresenter
    {
        private Size _contentSize;

        public event ContentSizeChangedHandler ContentSizeChanged;

        public Size ContentSize
        {
            get
            {
                return _contentSize;
            }
            private set
            {
                if(value == _contentSize)
                    return;

                _contentSize = value;
                if(ContentSizeChanged != null)
                    ContentSizeChanged(this, _contentSize);
            }
        }

        protected override Size ArrangeOverride(Size arrangeBounds)
        {
            var child = VisualChildrenCount > 0
                ? VisualTreeHelper.GetChild(this, 0) as UIElement
                : null;
            if(child == null)
                return arrangeBounds;

            //set the ContentSize
            ContentSize = child.DesiredSize;
            child.Arrange(new Rect(child.DesiredSize));

            return arrangeBounds;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            base.MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));
            var max = 1000000000;
            var x = double.IsInfinity(constraint.Width) ? max : constraint.Width;
            var y = double.IsInfinity(constraint.Height) ? max : constraint.Height;
            return new Size(x, y);
        }
    }
}