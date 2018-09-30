using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using Svg;

namespace Patterns.Svg
{
    public abstract class BaseSvgFilter: ISvgFilter
    {
        private int scale = 12;

        public Stream ConvertToSvg(Bitmap bitmap)
        {
            var document = CreateSvgDocument(bitmap);
            var defs = GetDefinitions();
            document.Children.Add(defs);
            var background = GetBackground(document, bitmap);
            document.Children.Add(background);
            AddElements(bitmap, document);
            return GetStream(document);
        }

        private SvgDefinitionList GetDefinitions()
        {
            var defs = new SvgDefinitionList();
            var group = new SvgGroup();
            group.ID = "element";
            defs.Children.Add(group);
            AddElementDefinition(group);
            return defs;
        }

        private SvgDocument CreateSvgDocument(Bitmap bitmap)
        {
            var document = new SvgDocument
            {
                Width = bitmap.Width*scale,
                Height = bitmap.Height*scale
            };
            return document;
        }

        private static Stream GetStream(SvgDocument document)
        {
            var s = new MemoryStream();
            document.Write(s);
            s.Seek(0, SeekOrigin.Begin);
            return s;
        }

        protected virtual void AddElements(Bitmap bitmap, SvgDocument document)
        {
            var groups = GetElementGroups(bitmap);
            AddElementGroups(document, groups);
        }

        private static void AddElementGroups(SvgElement parent, Dictionary<Color, SvgGroup> groups)
        {
            int i = 1;
            foreach (var group in groups)
            {
                group.Value.ID = "color" + i;
                parent.Children.Add(group.Value);
                i++;
            }
        }

        private Dictionary<Color, SvgGroup> GetElementGroups(Bitmap bitmap)
        {
            var groups = new Dictionary<Color, SvgGroup>();

            var majorColor = GetMajorColor(bitmap);

            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    var color = bitmap.GetPixel(x, y);
                    if (color != majorColor || !UseBackgroundForMajorColor)
                    {
                        if (!groups.ContainsKey(color))
                        {
                            groups.Add(color, new SvgGroup());
                        }
                        var group = groups[color];
                        var element = GetSvgElement(x, y, color);
                        if (element != null)
                        {
                            group.Children.Add(element);
                        }
                    }
                }
            }
            return groups;
        }

        protected virtual SvgElement GetSvgElement(int x, int y, Color color)
        {
            var use = new SvgUse
            {
                X = x*scale,
                Y = y*scale,
                ReferencedElement = new Uri("#element", UriKind.Relative),
                Fill = new SvgColourServer(color)
            };
            return use;
        }

        protected Color GetMajorColor(Bitmap bitmap)
        {
            var colors = new Dictionary<Color, int>();
            for (var x = 0; x < bitmap.Width; x++)
            {
                for (var y = 0; y < bitmap.Height; y++)
                {
                    var color = bitmap.GetPixel(x, y);
                    if (!colors.ContainsKey(color))
                    {
                        colors.Add(color, 0);
                    }
                    colors[color]++;
                }
            }
            var majorColor = colors.OrderByDescending(p => p.Value).First().Key;
            return majorColor;
        }

        protected virtual SvgElement GetBackground(SvgDocument document, Bitmap bitmap)
        {
            var background = new SvgRectangle();
            background.Width = document.Width;
            background.Height = document.Height;
            background.Fill = new SvgColourServer(Color.Black);
            return background;
        }

        protected abstract void AddElementDefinition(SvgGroup group);

        protected abstract Boolean UseBackgroundForMajorColor { get; }
    }
}
