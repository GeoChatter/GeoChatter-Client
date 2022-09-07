using GeoChatter.Helpers;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows.Forms;

namespace GeoChatter.Core.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public static class PropertyGridHelpers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="width"></param>
        public static void SetLabelColumnWidth(PropertyGrid grid, int width)
        {
            GCUtils.ThrowIfNull(grid, nameof(grid));

            // get the grid view
            Control view = (Control)grid.GetType().BaseType.GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(grid);

            // set label width
            FieldInfo fi = view.GetType().GetField("labelWidth", BindingFlags.Instance | BindingFlags.NonPublic);
            fi.SetValue(view, width);

            // refresh
            view.Invalidate();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="width"></param>
        public static void SetLabelColumnWidth1([NotNull] this PropertyGrid grid, int width)
        {
            FieldInfo fi = grid.GetType().GetField("gridView", BindingFlags.Instance | BindingFlags.NonPublic);
            object view = fi.GetValue(grid);
            MethodInfo mi = view.GetType().GetMethod("MoveSplitterTo", BindingFlags.Instance | BindingFlags.NonPublic);

            mi.Invoke(view, new object[] { width });
        }

    }
}
