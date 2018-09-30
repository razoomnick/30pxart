using Svg;

namespace Patterns.Svg
{
    public class SvgKnittingFilter: BaseSvgFilter
    {
        protected override void AddElementDefinition(SvgGroup group)
        {
            var pathLeft = new SvgPath();
            var strPathLeft = "M0 0 c 0 -1 1 -1 2 0 l1 1 c 1 1 2 2.6 2 4 l0 6.8 c 0 1 -1 1 -2 0 l-1 -1 c -1 -1 -2 -2.6 -2 -4 l0 -6.8 z";
            pathLeft.PathData = SvgPathBuilder.Parse(strPathLeft);
            group.Children.Add(pathLeft);
            var pathRight = new SvgPath();
            var strPathRight = "M8 1 c -1 1 -2 2.6 -2 4 l0 6.8 c 0 1 1 1 2 0 l1 -1 c 1 -1 2 -2.6 2 -4 l0 -6.8 c 0 -1 -1 -1 -2 0 l-1 1 z";
            pathRight.PathData = SvgPathBuilder.Parse(strPathRight);
            group.Children.Add(pathRight);
        }

        protected override bool UseBackgroundForMajorColor
        {
            get { return false; }
        }
    }
}
