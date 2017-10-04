/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App1
{
	public class Drawer : BoxView
	{
		volatile bool isOpen;
		public static readonly BindableProperty BlinkProperty = BindableProperty.Create<Drawer, bool>(w => w.Blink, default(bool), BindingMode.OneWay);

		public bool Blink
		{
			get
			{
				return (bool)GetValue(BlinkProperty);
			}
			set
			{
				SetValue(BlinkProperty, value);
			}
		}

		public static readonly BindableProperty BlinkDurationProperty = BindableProperty.Create<Drawer, uint>(w => w.BlinkDuration, 500, BindingMode.OneWay);

		public uint BlinkDuration
		{
			get
			{
				return (uint)GetValue(BlinkDurationProperty);
			}
			set
			{
				SetValue(BlinkDurationProperty, value);
			}
		}

		protected override void OnPropertyChanged(string propertyName)
		{
			base.OnPropertyChanged(propertyName);

			if (propertyName == BlinkProperty.PropertyName)
			{
				SetBlinking(Blink);
			}
			if (propertyName == BlinkDurationProperty.PropertyName)
			{
				if (isOpen)
				{
					SetBlinking(false);
					SetBlinking(Blink);
				}
			}
		}

		void SetBlinking(bool shouldBlink)
		{
			if (shouldBlink && !isOpen)
			{
				isOpen = true;

				var blinkAnimation = new Animation(((d) => {
					Opacity = d;
				}), 0f, 1f, Easing.SinInOut);

				this.Animate("BlinkingBoxViewBlink", blinkAnimation, length: BlinkDuration, repeat: () => isOpen);
			}
			else if (!shouldBlink && isOpen)
			{
				isOpen = false;
			}
		}

	}
}
*/