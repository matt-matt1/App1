using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Forms;
using App1.iOS;
using Xamarin.Forms.Maps.iOS;
using MapKit;
using Xamarin.Forms.Platform.iOS;
using CoreLocation;
using ObjCRuntime;
using App1;

[assembly: ExportRenderer(typeof(RouteMap), typeof(CustomMapRenderer))]
namespace App1.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		MKPolylineRenderer polylineRenderer;

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				var nativeMap = Control as MKMapView;
				if (nativeMap != null)
				{
					nativeMap.RemoveOverlays(nativeMap.Overlays);
					nativeMap.OverlayRenderer = null;
					polylineRenderer = null;
				}
			}

			if (e.NewElement != null)
			{
				var formsMap = (RouteMap)e.NewElement;
				var nativeMap = Control as MKMapView;
				nativeMap.OverlayRenderer = GetOverlayRenderer;

				CLLocationCoordinate2D[] coords = new CLLocationCoordinate2D[formsMap.RouteCoordinates.Count];
				int index = 0;
				foreach (var position in formsMap.RouteCoordinates)
				{
					coords[index] = new CLLocationCoordinate2D(position.Latitude, position.Longitude);
					index++;
				}

				var routeOverlay = MKPolyline.FromCoordinates(coords);
				nativeMap.AddOverlay(routeOverlay);
			}
		}

		MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlayWrapper)
		{
			if (polylineRenderer == null && !Equals(overlayWrapper, null))
			{
				var overlay = Runtime.GetNSObject(overlayWrapper.Handle) as IMKOverlay;
				polylineRenderer = new MKPolylineRenderer(overlay as MKPolyline)
				{
					FillColor = UIColor.Blue,
					StrokeColor = UIColor.Red,
					LineWidth = 3,
					Alpha = 0.4f
				};
			}
			return polylineRenderer;
		}
	}
}