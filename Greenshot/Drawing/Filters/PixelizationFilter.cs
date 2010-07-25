﻿/*
 * Greenshot - a free and open source screenshot tool
 * Copyright (C) 2007-2010  Thomas Braun, Jens Klingen, Robin Krom
 * 
 * For more information see: http://getgreenshot.org/
 * The Greenshot project is hosted on Sourceforge: http://sourceforge.net/projects/greenshot/
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 1 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.Serialization;

using Greenshot.Configuration;
using Greenshot.Drawing.Fields;
using Greenshot.Helpers;

namespace Greenshot.Drawing.Filters {
	[Serializable()] 
	public class PixelizationFilter : AbstractFilter {
				
		public PixelizationFilter(DrawableContainer parent) : base(parent) {
			AddField(FieldFactory.CreateField(FieldType.PIXEL_SIZE));
		}
		
		public override void Apply(Graphics graphics, Bitmap applyBitmap, Rectangle rect, RenderMode renderMode) {
			applyRect = IntersectRectangle(applyBitmap.Size, rect);
			
			int pixelSize = GetFieldValueAsInt(FieldType.PIXEL_SIZE);
			if(pixelSize <= 1 || applyRect.Width == 0 || applyRect.Height == 0) {
				// Nothing to do
				return;
			}
			if(applyRect.Width < pixelSize) pixelSize = applyRect.Width;
			if(applyRect.Height < pixelSize) pixelSize = applyRect.Height;
		
			using (BitmapBuffer bbbDest = new BitmapBuffer(applyBitmap, applyRect)) {
				bbbDest.Lock();
				using(BitmapBuffer bbbSrc = new BitmapBuffer(applyBitmap, applyRect)) {
					bbbSrc.Lock();
					List<Color> colors = new List<Color>();
					int halbPixelSize = pixelSize/2;
					for(int y=-halbPixelSize;y<bbbSrc.Height+halbPixelSize; y=y+pixelSize) {
						for(int x=-halbPixelSize;x<=bbbSrc.Width+halbPixelSize; x=x+pixelSize) {
							colors.Clear();
							for(int yy=y;yy<y+pixelSize;yy++) {
								if (yy >=0 && yy < bbbSrc.Height) {
									for(int xx=x;xx<x+pixelSize;xx++) {
										colors.Add(bbbSrc.GetColorAt(xx,yy));
									}
								}
							}
							Color currentAvgColor = Colors.Mix(colors);
							for(int yy=y;yy<=y+pixelSize;yy++) {
								if (yy >=0 && yy < bbbSrc.Height) {
									for(int xx=x;xx<=x+pixelSize;xx++) {
										bbbDest.SetColorAt(xx, yy, currentAvgColor);
									}
								}
							}
						}
					}
				}
				bbbDest.DrawTo(graphics, applyRect.Location);
			}
		}
	}
}