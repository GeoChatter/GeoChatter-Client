using GeoChatter.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;

namespace GeoChatter.Core.Extensions
{
    //public static class FormExtensions
    //{
    //    public static TResult InvokeEx<TForm, TResult>(this TForm control,
    //                                               Func<TForm, TResult> func)
    //      where TForm : Form
    //    {
    //        return control.InvokeRequired
    //                ? (TResult)control.Invoke(func, control)
    //                : func(control);
    //    }

    //    public static void InvokeEx<TForm>(this TForm control,
    //                                          Action<TForm> func)
    //      where TForm : Form
    //    {
    //        control.InvokeEx(c => { func(c); return c; });
    //    }

    //    public static void InvokeEx<TForm>(this TForm control, Action action)
    //      where TForm : Form
    //    {
    //        control.InvokeEx(c => action());
    //    }
    //}
    /// <summary>
    /// Extension methods for <see cref="Control"/>s
    /// </summary>
    public static class ControlExtensions
    {
        /// <summary>
        /// Invoke <paramref name="func"/> with <paramref name="control"/> thread safe.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="control"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static TResult InvokeDefault<TControl, TResult>([NotNull] this TControl control,
                                                   Func<TControl, TResult> func)
          where TControl : Control
        {
            GCUtils.ThrowIfNull(func, nameof(func), "Invokable can't be null.");

            return control.InvokeRequired
                    ? (TResult)control.Invoke(func, control)
                    : func(control);
        }

        /// <summary>
        /// Invoke <paramref name="func"/> with <paramref name="control"/> thread safe.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="control"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static void InvokeDefault<TControl>([NotNull] this TControl control,
                                              Action<TControl> func)
          where TControl : Control
        {
            GCUtils.ThrowIfNull(func, nameof(func), "Invokable can't be null.");
            control.InvokeDefault(c => { func(c); return c; });
        }

        /// <summary>
        /// Invoke <paramref name="action"/> with <paramref name="control"/> thread safe.
        /// </summary>
        /// <typeparam name="TControl"></typeparam>
        /// <param name="control"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static void InvokeDefault<TControl>([NotNull] this TControl control, Action action)
          where TControl : Control
        {
            GCUtils.ThrowIfNull(action, nameof(action), "Invokable can't be null.");
            control.InvokeDefault(c => action());
        }
    }
}
