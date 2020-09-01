using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace JWLibrary.Core.NetFramework.DataStructure
{
    public static class StoneCircleBindingSort
    {
        public static void SortAscending<T, P>(this BindingList<T> bindingList, Func<T, P> sortProperty)
        {
            bindingList.Sort(null, (a, b) => ((IComparable<P>)sortProperty(a)).CompareTo(sortProperty(b)));
        }
        public static void SortDescending<T, P>(this BindingList<T> bindingList, Func<T, P> sortProperty)
        {
            bindingList.Sort(null, (a, b) => ((IComparable<P>)sortProperty(b)).CompareTo(sortProperty(a)));
        }
        public static void Sort<T>(this BindingList<T> bindingList)
        {
            bindingList.Sort(null, null);
        }
        public static void Sort<T>(this BindingList<T> bindingList, IComparer<T> comparer)
        {
            bindingList.Sort(comparer, null);
        }
        public static void Sort<T>(this BindingList<T> bindingList, Comparison<T> comparison)
        {
            bindingList.Sort(null, comparison);
        }
        private static void Sort<T>(this BindingList<T> bindingList, IComparer<T> p_Comparer, Comparison<T> p_Comparison)
        {
            //Extract items and sort separately
            List<T> sortList = new List<T>();
            bindingList.ForEach(item => sortList.Add(item));//Extension method for this call
            if (p_Comparison == null)
            {
                sortList.Sort(p_Comparer);
            }//if
            else
            {
                sortList.Sort(p_Comparison);
            }//else

            //Disable notifications, rebuild, and re-enable notifications
            bool oldRaise = bindingList.RaiseListChangedEvents;
            bindingList.RaiseListChangedEvents = false;
            try
            {
                bindingList.Clear();
                sortList.ForEach(item => bindingList.Add(item));
            }
            finally
            {
                bindingList.RaiseListChangedEvents = oldRaise;
                bindingList.ResetBindings();
            }

        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (T item in source)
            {
                action(item);
            }
        }
    }
}
